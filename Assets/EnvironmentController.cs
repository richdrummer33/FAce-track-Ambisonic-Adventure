using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public EnvironmentController previousEnvironment;

    private void OnTriggerEnter(Collider other)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);

        if(previousEnvironment)
            previousEnvironment.DisableEvironment();
    }

    public void DisableEvironment()
    {
        StartCoroutine(DisableDelayed());
    }

    IEnumerator DisableDelayed()
    {
        float t = 0f;

        LerpVolume[] vols = GetComponentsInChildren<LerpVolume>();

        foreach (LerpVolume v in vols)
            v.LerpVolDown();

        while (t < 10f)
        {
            t += Time.deltaTime;
            yield return null;
        }

        foreach (Transform child in transform)
            if (child != this.transform)
                child.gameObject.SetActive(false);
    }
}
