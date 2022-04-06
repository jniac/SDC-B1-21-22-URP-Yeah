using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    public float magnitude = 4f;

    void OnTriggerEnter(Collider other)
    {
        var body = other.attachedRigidbody;
        if (body != null)
        {
            body.velocity += transform.right * magnitude;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.right * magnitude);
        Gizmos.DrawSphere(transform.position + transform.right * magnitude, 0.25f);
    }
}
