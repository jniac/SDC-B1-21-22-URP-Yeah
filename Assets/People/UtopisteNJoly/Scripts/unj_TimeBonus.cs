using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unj_TimeBonus : MonoBehaviour
{
    public float timeBonus = 3f;

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        unj_TimeAttackLevelManager manager = FindObjectOfType<unj_TimeAttackLevelManager>();
        manager.remainingTime += timeBonus;
    }

}