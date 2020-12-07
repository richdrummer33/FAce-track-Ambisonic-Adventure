using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpVolume : MonoBehaviour
{
    [SerializeField] float duration = 10f;
    AudioSource source;
    float vol;

    public enum LerpType { In, Out, OutThenDisable, Both }
    public LerpType type = LerpType.Both;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        vol = source.volume;
        if (type == LerpType.Both || type == LerpType.In)
        {
            source.volume = 0f;
            StartCoroutine(LerpVol());
        }
    }
    
    IEnumerator LerpVol()
    {
        while (source.volume < vol)
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

        if (type == LerpType.OutThenDisable)
            gameObject.SetActive(false);
    }

}
