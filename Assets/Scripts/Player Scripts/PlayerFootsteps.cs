using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private AudioSource footstepsSounds;

    [SerializeField]
    private AudioClip[] footstepsClips;

    private CharacterController characterController;

    [HideInInspector]
    public float volumeMin, volumeMax;

    private float accumulatedDistance;

    [HideInInspector]
    public float stepDistance;


    void Awake()
    {
        footstepsSounds = GetComponent<AudioSource>();
        characterController = GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        CheckToPlayFootstepsSounds();
    }

    void CheckToPlayFootstepsSounds()
    {
        if (!characterController.isGrounded)
            return;
        if (characterController.velocity.sqrMagnitude > 0)
        {
            accumulatedDistance += Time.deltaTime;
            if(accumulatedDistance > stepDistance)
            {
                footstepsSounds.volume = Random.Range(volumeMin, volumeMax);
                footstepsSounds.clip = footstepsClips[Random.Range(0, footstepsClips.Length)];
                footstepsSounds.Play();

                accumulatedDistance = 0f;
            }
        }
        else
        {
            accumulatedDistance = 0f;
        }
    }
}
