using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleAudioClips : MonoBehaviour
{
    public List<AudioClip> clips = new List<AudioClip>();
    public AudioSource aSource;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    public AudioClip GiveClip(int clipIndex)
    {
        return clips[clipIndex];
    }

    public void PlayClip(int clipIndex)
    {
        aSource.PlayOneShot(clips[clipIndex]);
    }

    public void PlayClip(int clipIndex, float volume)
    {
        aSource.PlayOneShot(clips[clipIndex], volume);
    }
}
