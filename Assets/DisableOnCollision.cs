using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnCollision : MonoBehaviour
{
    public List<GameObject> toDisable;
    public Collider col;

    private void OnTriggerEnter(Collider other)
    {
        if (other == col)
        {
            foreach(GameObject o in toDisable)
                o.SetActive(false);
        }
    }
}
