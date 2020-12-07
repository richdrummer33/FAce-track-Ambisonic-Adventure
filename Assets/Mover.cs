using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
