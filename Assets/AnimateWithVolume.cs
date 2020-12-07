using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateWithVolume : MonoBehaviour
{
    float timer;

    public   AudioSource source;

    AudioClip clip;

    private float[] sampleData; // bin of data
    public int sampleLength = 50;
    public float sampleDuration = 0.25f; // Length of sample derived from this duration (number in seconds)

    Vector3 startScale;
    Vector3 startPos;
    [SerializeField] float scaleMultiplier = 10f;

    float amplitude;
    [SerializeField] float positionAmplitude = 10f;
    [SerializeField] float positionSpeed = 10f;
    [SerializeField] float period = 2f;

    private void Start()
    {
        StartCoroutine(GetClip());

        startScale = transform.localScale;

        startPos = transform.position;

        //StartCoroutine(LerpPos());
    }

    

    void Update()
    {
        timer = 0f;
        amplitude = 0f;

        if (source.clip && source.isPlaying)
        {
            source.clip.GetData(sampleData, source.timeSamples);

            foreach (float sample in sampleData)
            {
                amplitude += Mathf.Abs(sample);
            }

            amplitude = amplitude / sampleLength;

            transform.localScale = startScale + Vector3.one * Mathf.Sqrt(amplitude) * scaleMultiplier;

            transform.position = Vector3.Lerp(transform.position, randomPosition, Time.deltaTime * positionSpeed * amplitude);
        }
    }

    Vector3 randomPosition;
    IEnumerator LerpPos()
    {
        float t = 0f;

        while (true)
        {
            randomPosition = startPos + new Vector3(Random.Range(-positionAmplitude, positionAmplitude), Random.Range(-positionAmplitude, positionAmplitude), Random.Range(-positionAmplitude, positionAmplitude));
            
            yield return new WaitForSeconds(period + Random.Range(-period / 2f, period / 2f));
        }
    }

    IEnumerator GetClip()
    {
        while (!clip)
        {
            clip = source.clip;
            yield return null;
        }

        timer = 0f;

        float sampleRate = (int)clip.samples / clip.length;

        sampleLength = (int)(sampleRate * sampleDuration);
        sampleData = new float[sampleLength];

        int nyquist = 30000; // 2x fmax for 15kHz spectrum

        int sampleStep = Mathf.FloorToInt(sampleRate * 1 / nyquist); // Round down (conservative approach)


        float[] clipSamples = new float[clip.samples]; // bin of data

        clip.GetData(clipSamples, 0);
    }
}
