using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMusic : MonoBehaviour
{
    public GameObject insideMusicSource, outsideMusicSource;
    AudioSource insideMusic, outsideMusic;

    void Start()
    {
        insideMusic = insideMusicSource.GetComponent<AudioSource>();
        outsideMusic = outsideMusicSource.GetComponent<AudioSource>();
    }

    void SetOutside(bool outside)
    {
        if (outside) StartCoroutine("PlayOutsideMusic");
        else StartCoroutine("PlayInsideMusic");
    }

    IEnumerator PlayOutsideMusic()
    {
        while (outsideMusic.volume < 1)
        {
            outsideMusic.volume += Time.deltaTime;
            insideMusic.volume -= 0.45f * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator PlayInsideMusic()
    {
        while (insideMusic.volume < 0.4f)
        {
            insideMusic.volume += 0.4f * Time.deltaTime;
            outsideMusic.volume -= Time.deltaTime;
            yield return null;
        }
    }
}
