using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxeWooshSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] wooshSound;


    void PlayWooshSound()
    {
        audioSource.clip = wooshSound[Random.Range(0, wooshSound.Length)];
        audioSource.Play();
    }



}
