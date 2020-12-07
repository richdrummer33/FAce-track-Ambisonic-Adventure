using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DisableAudioOnFinish : MonoBehaviour
{
    public UnityEvent EventOnDisable;

    void OnEnable()
    {
        StartCoroutine(DisableOnFinish());
    }

    IEnumerator DisableOnFinish()
    {
        AudioSource source = GetComponent<AudioSource>();

        yield return new WaitForSecondsRealtime(source.clip.length);

        EventOnDisable?.Invoke();
        gameObject.SetActive(false);
    }
}
