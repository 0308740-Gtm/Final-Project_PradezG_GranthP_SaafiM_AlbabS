using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Enemy Audio Clips")]
    public AudioClip callClip;
    public AudioClip hitClip;
    public AudioClip deathClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PlayCall();
    }

    public void PlayCall()
    {
        if (audioSource != null && callClip != null)
        {
            audioSource.PlayOneShot(callClip);
        }
    }

    public void PlayHit()
    {
        if (audioSource != null && hitClip != null)
        {
            audioSource.PlayOneShot(hitClip);
        }
    }

    public void PlayDeath()
    {
        if (audioSource != null && deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
        }
    }
}
