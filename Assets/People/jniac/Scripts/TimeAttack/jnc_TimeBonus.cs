using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jnc_TimeBonus : MonoBehaviour
{
    public float timeBonus = 3f;

    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 180 * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        var levelManage = FindObjectOfType<jnc_TimeAttackLevelManager>();
        levelManage.remainingTime += timeBonus;
    }
}
