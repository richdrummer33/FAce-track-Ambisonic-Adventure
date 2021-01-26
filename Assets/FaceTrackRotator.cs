using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceTrackRotator : MonoBehaviour
{
    [SerializeField] ARFace face = null;
    [SerializeField] ARSessionOrigin origin = null;
    [SerializeField] ARFaceManager manager;
    [Range(0, 10)]
    [SerializeField] int smoothingFactor = 3; // Number of averages - this is actually a minimum*, which is approached when head is rotating fast
    [Range(0f,10f)]
    [SerializeField] float velocityMultiplier = 3.75f; // Increases the delta in # aves when moving vs when still
    [SerializeField] float maxSmoothingMultiplier = 5; // Clamps the # of averages at a maximum value (approaches or reaches max value when head is very still)
    
    [SerializeField] float yRotMultiplier = 1.33f;
    [SerializeField] float xRotMultiplier = 1.33f;

    [Header("Debug")]
    [SerializeField] Vector3 eulerAnglesRelativeToCamera;

    Quaternion lastRot;
    List<Vector3> smoothedEulerRots = new List<Vector3>();
    List<Quaternion> smoothedRots = new List<Quaternion>();


    private void Start()
    {
        manager.facesChanged += FaceDetected;
    }

    void FaceDetected(ARFacesChangedEventArgs args)
    {
        if (args.added.Count > 0 && !face)
        {
            face = args.added[0];
        }
        if(args.removed.Count > 0)
        {
            if (face == args.removed[0])
                face = null;
        }
    }

    private void Update()
    {
        if (face)
        {
            Vector3 faceEulers = face.transform.eulerAngles;

            faceEulers.x = -faceEulers.x; // Flip/invert (since it comes inverted)

            Quaternion faceRot = Quaternion.Euler(faceEulers);

            if (smoothingFactor == 0)
                transform.rotation = RightCoordToUnityCord(face.transform.rotation);
            else
                transform.rotation = SmoothRotations(smoothingFactor, RightCoordToUnityCord(face.transform.rotation));  // Quaternion.Lerp(lastRot, faceRot, Time.deltaTime * 1f / smoothingFactor);
        }
    }

    private Quaternion RightCoordToUnityCord(Quaternion q)
    {
        return new Quaternion(-q.x, q.y, q.z, q.w);
    }

    Quaternion SmoothRotations(int numAves, Quaternion newRot)
    {
        float velocityFactor = 1f - Mathf.Clamp(Quaternion.Angle(lastRot, newRot) / 45f, 0f, 1f);
        velocityFactor *= this.velocityMultiplier;

        int numVelocityAdaptedAves = (int)Mathf.Clamp(Mathf.RoundToInt(numAves* (1f + velocityFactor)), 0f, maxSmoothingMultiplier * numAves);

        Debug.Log("D: " + Quaternion.Angle(lastRot, newRot) + "\n" + "V: " + velocityFactor + "\n" + "nV: " + numVelocityAdaptedAves);

        lastRot = newRot;

        // Holds the amount of rotations which need to be averaged.
        int addAmount = 0;

        // Represents the additive quaternion
        Quaternion summedRotation = Quaternion.identity;

        // The eventual result
        Quaternion averageRotation = Quaternion.identity;

        // Grow the list as newRots are added, up to length defined by numAves
        if (smoothedRots.Count < numAves * maxSmoothingMultiplier)
            smoothedRots.Add(newRot);
        else
        {
            smoothedRots.RemoveAt(0);
            smoothedRots.Add(newRot);
        }

        for (int i = smoothedRots.Count - 1; i > smoothedRots.Count - numVelocityAdaptedAves - 1; i--)
        {
            //Temporary values
            float w;
            float x;
            float y;
            float z;

            //Amount of separate rotational values so far
            addAmount++;

            float addDet = 1.0f / (float)addAmount;
            summedRotation.w += smoothedRots[i].w;
            w = summedRotation.w * addDet;
            summedRotation.x += smoothedRots[i].x * xRotMultiplier;
            x = summedRotation.x * addDet;
            summedRotation.y += smoothedRots[i].y * yRotMultiplier;
            y = summedRotation.y * addDet;
            summedRotation.z += smoothedRots[i].z;
            z = summedRotation.z * addDet;

            //Normalize
            float D = 1.0f / (w * w + x * x + y * y + z * z);
            w *= D;
            x *= D;
            y *= D;
            z *= D;

            //The result is valid right away, without first going through the entire array.
            averageRotation = new Quaternion(x, y, z, w);
        }

        return averageRotation;
    }

    Vector3 NewAngleSmoothed(int numAves, Vector3 newAngle)
    {
        Vector3 summedAngle = new Vector3();

        if (smoothedEulerRots.Count < numAves)
            smoothedEulerRots.Add(newAngle);
        else
        {
            smoothedEulerRots.RemoveAt(0);
            smoothedEulerRots.Add(newAngle);
        }

        foreach (Vector3 angle in smoothedEulerRots)
        {
            summedAngle += angle;
        }

        summedAngle /= numAves;

        return summedAngle;
    }
}

/*

void Update()
{
    if (face)
    {
        Vector3 faceEulers = face.transform.eulerAngles;
        faceEulers.x = -faceEulers.x;
        Quaternion faceRot = Quaternion.Euler(faceEulers);

        var rotationRelativeToCamera = Quaternion.Inverse(origin.camera.transform.rotation) * faceRot; // * face.transform.rotation; // this quaternion represents a face rotation relative to camera

        transform.rotation = rotationRelativeToCamera;
    }
}
*/

/*
 *  private void Update()
    {
        if (face)
        {
            Vector3 faceEulers = face.transform.eulerAngles;

            faceEulers.x = -faceEulers.x; // Flip/invert (since it comes inverted)

            Quaternion faceRot = Quaternion.Euler(faceEulers);
            //Vector3 relativeEulerRot = (Quaternion.Inverse(origin.camera.transform.rotation) * faceRot).eulerAngles; // Relative to fwd diection (probably not needed, but keep anyway)

            Vector3 newSmoothedRot = NewAngleSmoothed(smoothingFactor, faceRot.eulerAngles);

            Vector3 smoothedDeltaEulerRot = newSmoothedRot - lastEulerRot;

            if (init)
            {
                transform.Rotate(smoothedDeltaEulerRot, Space.World);
            }

            transform.rotation = SmoothRotations(smoothingFactor, faceRot);  // Quaternion.Lerp(lastRot, faceRot, Time.deltaTime * 1f / smoothingFactor);

            lastEulerRot = newSmoothedRot;

            lastRot = faceRot;
            //init = true;
        }
    }

*/


/*
 *  foreach (Quaternion singleRotation in smoothedRots)
        {
            if (i < Mathf.Clamp(numVelocityAdaptedAves, 0f, numAves * maxSmoothingMultiplier))
            {
                //Temporary values
                float w;
                float x;
                float y;
                float z;

                //Amount of separate rotational values so far
                addAmount++;

                float addDet = 1.0f / (float)addAmount;
                summedRotation.w += singleRotation.w;
                w = summedRotation.w * addDet;
                summedRotation.x += singleRotation.x;
                x = summedRotation.x * addDet;
                summedRotation.y += singleRotation.y;
                y = summedRotation.y * addDet;
                summedRotation.z += singleRotation.z;
                z = summedRotation.z * addDet;

                //Normalize. Note: experiment to see whether you
                //can skip this step.
                float D = 1.0f / (w * w + x * x + y * y + z * z);
                w *= D;
                x *= D;
                y *= D;
                z *= D;

                //The result is valid right away, without
                //first going through the entire array.
                averageRotation = new Quaternion(x, y, z, w);
            }
            else
                break;

            i++;
        }
 */
