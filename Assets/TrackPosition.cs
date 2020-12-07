using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPosition : MonoBehaviour
{
    Transform toTrack;
    Vector3 lastPos;
    public Vector3 offset = Vector3.up * 0.5f;
    Vector3 followPos;
    bool isSetup = false;

    private void OnEnable()
    {
        toTrack = GameObject.FindGameObjectWithTag("Player").transform;
        
        lastPos = toTrack.position;
        followPos = toTrack.position;
        transform.position = toTrack.position;

        isSetup = true;
    }

    private void OnDisable()
    {
        isSetup = false;
    }

    void Update()
    {
        if (isSetup)
        {
            followPos += toTrack.position - lastPos;

            transform.position = followPos + offset;

            lastPos = toTrack.position;
        }
    }
}
