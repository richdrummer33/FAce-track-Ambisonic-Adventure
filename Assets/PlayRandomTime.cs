using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomTime : MonoBehaviour
{
    AudioSource s;

    void Start()
    {
        s = GetComponent<AudioSource>();
        float d = s.clip.length;

        float t = Random.Range(0f, d);

        s.Play();
        s.time = t;
    }

    float timer;

    public float period = 1f;
    float randPeriod = 1f;

    public float pitchRange = 0.25f;
    float targetPitch = 1f;

    private void Update()
    {
        if (timer > randPeriod)
        {
            targetPitch = 1f + Random.Range(1f - pitchRange, 1f + pitchRange);
            s.pitch = targetPitch;
            
            randPeriod = Random.Range(period - period / 2f, period + period / 2f);

            timer = 0f;
        }
    }
}
