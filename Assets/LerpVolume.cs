using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpVolume : MonoBehaviour
{
    [SerializeField] float duration = 10f;
    AudioSource source;
    float vol;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        vol = source.volume;
        StartCoroutine(LerpVol());
    }
    
    IEnumerator LerpVol()
    {
        while (source.volume < 1f)
        {
            source.volume = source.volume + Time.deltaTime / duration;
            yield return null;
        }
    }

    public void LerpVolDown()
    {
        StopAllCoroutines();
        StartCoroutine(LerpVolDownRoutine());
    }

    IEnumerator LerpVolDownRoutine()
    {
        while (source.volume > 0f)
        {
            source.volume = source.volume - Time.deltaTime / duration / 3f;
            yield return null;
        }
    }

}
