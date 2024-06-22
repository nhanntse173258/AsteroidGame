using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip background;
    public AudioClip sfxExplode;
    public AudioClip sfxStarCollect;
    public AudioClip sfxThrust;
    public AudioClip sfxLaser;

    public void Start()
    {
        audioSource.clip = background;
        audioSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
