using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dld_jnc_TimeDisplay : MonoBehaviour
{
    TMPro.TextMeshProUGUI text;
    dld_TimeAttackLevelManager levelManager;

    void Start()
    {
        levelManager = FindObjectOfType<dld_TimeAttackLevelManager>();
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = $"{levelManager.remainingTime:F2}";
    }
}
