using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lia_TimeGiver : MonoBehaviour
{
    public float timeAmount = 3f;

    void OnTriggerEnter(Collider other)
    {
        var timeAttack = FindObjectOfType<lia_TimeAttackLevelManager>();
        timeAttack.remainingTime += timeAmount;

        Destroy(gameObject);
    }
}
