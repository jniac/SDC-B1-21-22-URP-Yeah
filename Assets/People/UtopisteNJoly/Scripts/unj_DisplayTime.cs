using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unj_DisplayTime : MonoBehaviour
{
    private TMPro.TextMeshProUGUI text;
    private unj_TimeAttackLevelManager levelManager;
    
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        levelManager = FindObjectOfType<unj_TimeAttackLevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"time: {levelManager.remainingTime:f2} ";
    }
}
