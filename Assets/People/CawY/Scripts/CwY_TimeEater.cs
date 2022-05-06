using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CwY_TimeEater : MonoBehaviour
{
    public float timeAmount = 1f;

    void OnTriggerEnter(Collider other)
    {
        var timeAttack = FindObjectOfType<CawY_TimeAttackLevelManager>();
        timeAttack.remainingTime -= timeAmount;

        Destroy(gameObject);
    }
}
