using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unj_TimeStop : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        var timeAttack = FindObjectOfType<unj_TimeAttackLevelManager>();
        timeAttack.timePaused = true;
    }

}
