using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jnc_TimeBonus : MonoBehaviour
{
    public float timeBonus = 3f;

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        var levelManage = FindObjectOfType<jnc_TimeAttackLevelManager>();
        levelManage.remainingTime += timeBonus;
    }
}
