using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kev_TimeStop : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        var timeAttack = FindObjectOfType<Kev_TimeAttackLevelManager>();
        timeAttack.timePaused = true;
    }

}