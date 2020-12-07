using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowRotation : MonoBehaviour
{
    [SerializeField] Transform toTrack;

   // RectTransform t;
    Quaternion startRot;

    private void Start()
    {
      //  t = GetComponent<RectTransform>();
        startRot = transform.rotation;
        Debug.Log("startRot " + startRot);
        Debug.Log("toTrack rot " + toTrack.rotation);
    }

    void Update()
    {
        Quaternion deltaRot = toTrack.rotation;

        deltaRot.x = 0f;
        deltaRot.z = -deltaRot.y;
        deltaRot.y = 0f;
        deltaRot.w = deltaRot.w;

        transform.rotation = startRot * deltaRot;
    }
}
