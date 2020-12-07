using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnCollision : MonoBehaviour
{
    public List<GameObject> toEnable;
    public Collider col;

    private void OnTriggerEnter(Collider other)
    {
        if (other == col)
        {
            foreach (GameObject o in toEnable)
                o.SetActive(true);
        }
    }
}
