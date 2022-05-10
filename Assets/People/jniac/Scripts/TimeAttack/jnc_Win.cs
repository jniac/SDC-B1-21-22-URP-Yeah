using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jnc_Win : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.tag == "Player")
        {
            var levelManager = FindObjectOfType<jnc_TimeAttackLevelManager>();
            levelManager.Win();
        }
    }
}
