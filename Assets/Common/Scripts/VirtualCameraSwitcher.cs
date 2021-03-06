using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Cinemachine;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode, RequireComponent(typeof(EditingBlockSnapping))]
public class VirtualCameraSwitcher : MonoBehaviour
{
    class OverlapObservable
    {
        bool overlap = false;
        float enterTime = -1;
        float exitTime = -1;

        public bool Overlap
        {
            get => overlap;
            set => SetOverlap(value);
        }

        public bool GetOverlap() => overlap;

        public bool GetOverlapWithDelay(float delay) => overlap || (Time.time - exitTime < delay);

        public bool SetOverlap(bool value)
        {
            if (value != overlap)
            {
                overlap = value;

                if (overlap)
                    enterTime = Time.time;
                else
                    exitTime = Time.time;

                return true;
            }

            return false;
        }
    }

    static List<VirtualCameraSwitcher> instances = new List<VirtualCameraSwitcher>();
    static CinemachineVirtualCamera[] vcams;
    static CinemachineVirtualCamera currentVcam;
    static CinemachineVirtualCamera defaultVcam;
    static int defaultVcamPriority = 10;
    static Transform follow;
    static void FindVcams(bool force = false)
    {
        if (vcams != null && force == false)
            return;

        vcams = FindObjectsOfType<CinemachineVirtualCamera>();

        // The default vcam is the first vcam that is not controlled 
        // by a VirtualCameraSwitcher instance.
        defaultVcam = vcams
            .Where(vcam => instances.All(switcher => switcher.vcam != vcam))
            .OrderBy(vcam => vcam.transform.GetSiblingIndex())
            .FirstOrDefault();
    }

    static int updatePriorityFrame = -1;
    static int updatePriorityCount = 0;
    public static void UpdatePriority(bool force = false)
    {
        if (force == false && Time.frameCount % 10 != 0)
            return;

        bool alreadyUpdated = updatePriorityFrame == Time.frameCount;
        if (force == false && alreadyUpdated)
            return;

        updatePriorityFrame = Time.frameCount;

        FindVcams(force);

        follow = instances
            .Where(item => item.vcam != null)
            .Select(item => item.vcam.Follow)
            .Where(item => item != null)
            .FirstOrDefault();

        if (follow == null)
            return;

        // Compute the maximum priority for each vcam.
        var vcamPriority = new Dictionary<CinemachineVirtualCamera, int>();
        foreach (var instance in instances)
        {
            // Ignore null vcam, important!
            if (instance.vcam == null)
                continue;

            instance.UpdateOverlap(follow.position);

            int priority = instance.Overlaps() ?
                instance.onEnterPriority :
                defaultVcamPriority - 2;

            if (vcamPriority.ContainsKey(instance.vcam))
            {
                var current = vcamPriority[instance.vcam];
                vcamPriority[instance.vcam] = Mathf.Max(current, priority);
            }
            else
            {
                vcamPriority.Add(instance.vcam, priority);
            }
        }

        // NOTE: THIS IS AN HACK!
        // Since Cinemachine do not always take vcam priority into consideration 
        // (a bug), we have to update regulary the priority with a different number
        // (to trigger Cinemachine brain inner update).
        int forceCinemachineUpdateOffset = updatePriorityCount % 2;

        // Update each vcam with its previously computed priority.
        foreach (var entry in vcamPriority)
        {
            var vcam = entry.Key;
            int priority = entry.Value;
            vcam.Priority = priority + forceCinemachineUpdateOffset;
        }

        if (defaultVcam != null)
            defaultVcam.Priority = defaultVcamPriority + forceCinemachineUpdateOffset;

        var newVcam = vcams.OrderBy(vcam => vcam.Priority).LastOrDefault();
        if (newVcam != currentVcam)
            Player.BroadcastAll("OnSwitchCamera", newVcam);
        currentVcam = newVcam;

        updatePriorityCount++;
    }

    public CinemachineVirtualCamera vcam;
    public int onEnterPriority = 20;
    public float exitDelay = 0f;
    public Vector3 safeMargin = Vector3.one * 0.5f;

    public Bounds Bounds => new Bounds(transform.position, transform.localScale + safeMargin);

    OverlapObservable overlap = new OverlapObservable();

    void UpdateName()
    {
        var str = vcam != null
            ? Regex.Split(vcam.name, @"\W").Last()
            : "NO-CAM";
        gameObject.name = $"SwitchTo [{str} : {onEnterPriority}]";
    }

    void UpdateOverlap(Vector3 point)
    {
        overlap.SetOverlap(Bounds.Contains(point));
    }

    public bool Overlaps()
    {
#if UNITY_EDITOR
        if (Application.isPlaying == false)
            return overlap.GetOverlap();
#endif
        return overlap.GetOverlapWithDelay(exitDelay);
    }

    void OnEnable()
    {
        instances.Add(this);
    }

    void OnDisable()
    {
        instances.Remove(this);
    }

    void FixedUpdate()
    {
        UpdatePriority();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            UpdatePriority();
            UpdateName();
        }
#endif
    }

    void OnValidate()
    {
        UpdateName();
    }

    public Color gizmoColor = Color.yellow;
    static bool drawGizmos = true;
    void DrawGizmos()
    {
        Gizmos.color = gizmoColor;
        var bounds = Bounds;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
    void OnDrawGizmos()
    {
        if (drawGizmos)
            DrawGizmos();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        var bounds = Bounds;
        GizmosUtils.WithAlpha(0.2f, () => Gizmos.DrawCube(bounds.center, bounds.size));
    }

#if UNITY_EDITOR
    [CanEditMultipleObjects, CustomEditor(typeof(VirtualCameraSwitcher))]
    class MyEditor : Editor
    {
        VirtualCameraSwitcher Target => target as VirtualCameraSwitcher;
        IEnumerable<VirtualCameraSwitcher> Targets => targets.Cast<VirtualCameraSwitcher>();

        void Draw(string propName)
        {   
            EditorUtils.ChangeCheck(
                () => EditorGUILayout.PropertyField(serializedObject.FindProperty(propName)),
                () => {
                    Type type = typeof(VirtualCameraSwitcher);
                    var prop  = type.GetProperty(propName);
                    var field = type.GetField(propName);
                    foreach (var item in Targets)
                    {
                        field?.SetValue(item, field.GetValue(Target));
                        prop?.SetValue(item, prop.GetValue(Target));
                    }
                }
            );
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField($"{vcams?.Length.ToString() ?? "(?)"} vcams, {instances.Count} switchers.");

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Follow (info)", follow, typeof(Transform), true);
            EditorGUILayout.ObjectField("Current Vcam (info)", currentVcam, typeof(CinemachineVirtualCamera), true);
            EditorGUILayout.ObjectField("Default Vcam (info)", defaultVcam, typeof(CinemachineVirtualCamera), true);
            GUI.enabled = true;

            Draw("vcam");
            Draw("onEnterPriority");
            Draw("exitDelay");
            Draw("safeMargin");

            EditorUtils.ChangeCheck(
                () => drawGizmos = EditorGUILayout.Toggle("Draw Gizmos", drawGizmos),
                () => EditorUtils.SetDirty(FindObjectsOfType<VirtualCameraSwitcher>()));

            Draw("gizmoColor");
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Manual Update Priority"))
            {
                UpdatePriority(true);
            }
        }
    }
#endif
}
