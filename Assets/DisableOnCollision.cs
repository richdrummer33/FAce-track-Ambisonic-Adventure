using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnCollision : MonoBehaviour
{
    public List<GameObject> toDisable;

    private void OnTriggerEnter(Collider other)
    {
        foreach (GameObject o in toDisable)
            o.SetActive(false);
    }
}
