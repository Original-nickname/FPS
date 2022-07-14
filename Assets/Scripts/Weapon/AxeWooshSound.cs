using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AxeWooshSound : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] wooshSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlayWooshSound()
    {
        audioSource.clip = wooshSound[Random.Range(0, wooshSound.Length)];
        audioSource.Play();
    }

}
