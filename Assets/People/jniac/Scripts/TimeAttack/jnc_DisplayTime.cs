using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jnc_DisplayTime : MonoBehaviour
{
    public enum Mode
    {
        RemainingTime,
        ElapsedTime,
    }

    Mode mode = Mode.RemainingTime;

    private TMPro.TextMeshProUGUI text;
    private jnc_TimeAttackLevelManager levelManager;

    void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        levelManager = FindObjectOfType<jnc_TimeAttackLevelManager>();
    }

    void Update()
    {
        if (mode == Mode.RemainingTime)
        {
            text.text = $"{levelManager.remainingTime:f2}s";
        }
        else
        {
            text.text = $"{levelManager.elapsedTime:f2}s";
        }
    }
}
