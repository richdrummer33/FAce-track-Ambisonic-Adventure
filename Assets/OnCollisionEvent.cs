using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionEvent : MonoBehaviour
{
    public UnityEvent CollisionEvent;

    private void OnTriggerEnter(Collider other)
    {
        CollisionEvent?.Invoke();
    }
}
