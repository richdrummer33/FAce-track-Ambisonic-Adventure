using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EnableOnCollision : MonoBehaviour
{
    public List<GameObject> toEnable;
    public bool disableOnAwake;

    [Space]
    public UnityEvent OnCollideEvent;

    private void Start()
    {
        if(disableOnAwake)
            foreach (GameObject o in toEnable)
                o.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (GameObject o in toEnable)
            o.SetActive(true);

        OnCollideEvent?.Invoke();
    }
}
