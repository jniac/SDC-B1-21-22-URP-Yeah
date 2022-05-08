using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kev_TimeBonus : MonoBehaviour
{
    public float timeBonus = 5f;

    void OnTriggerEnter(Collider other)
    {
        Destroy (gameObject);
        qng_TimeAttackLevelManager manager = FindObjectOfType<qng_TimeAttackLevelManager>();
        manager.remainingTime += timeBonus;
    }
}
