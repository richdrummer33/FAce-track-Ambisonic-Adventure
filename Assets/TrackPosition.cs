using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPosition : MonoBehaviour
{
    Transform toTrack;
    Vector3 lastPos;

    private void OnEnable()
    {
        toTrack = GameObject.FindGameObjectWithTag("Player").transform;
        lastPos = toTrack.position + Vector3.up * 0.25f;
        transform.position = lastPos;
    }

    void Update()
    {
        transform.position += toTrack.position - lastPos;
        lastPos = toTrack.position;
    }
}
