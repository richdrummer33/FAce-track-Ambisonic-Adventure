using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggeredSound : MonoBehaviour
{
    AudioSource source;
    public UnityEvent OnCollideEvent;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        source.Play();
        OnCollideEvent?.Invoke();
    }
}
