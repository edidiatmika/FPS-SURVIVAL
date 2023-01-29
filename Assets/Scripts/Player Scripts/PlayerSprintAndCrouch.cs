using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public float sprintSpeed = 10f;
    public float moveSpeed = 5f;
    public float crouchSpeed = 2f;

    private Transform lookRoot;
    private float standHeight = 1.6f;
    private float crouchHeight = 1f;

    private bool isCrouching;

    private PlayerFootsteps playerFootsteps;

    private float sprintVolume = 1f;
    private float crouchVolume = 0.1f;
    private float walkVolumeMin = 0.2f, walkVolumeMax = 0.6f;

    private float walkStepsDistance = 0.4f;
    private float sprintStepsDistance = 0.25f;
    private float crouchStepsDistance = 0.5f;

    private PlayerStats playerStats;

    private float sprintValue = 100f;
    public float sprintTreshold = 10f;


    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        lookRoot = transform.GetChild(0);

        playerFootsteps = GetComponentInChildren<PlayerFootsteps>();

        playerStats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        playerFootsteps.volumeMin = walkVolumeMin;
        playerFootsteps.volumeMax = walkVolumeMax;
        playerFootsteps.stepDistance = walkStepsDistance;
    }

    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()
    {
        if (sprintValue > 0f)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching )
            {
                playerMovement.speed = sprintSpeed;

                playerFootsteps.stepDistance = sprintStepsDistance;
                playerFootsteps.volumeMin = sprintVolume;
                playerFootsteps.volumeMax = sprintVolume;
            }
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift) && !isCrouching)
        {
            playerMovement.speed = moveSpeed;

            playerFootsteps.stepDistance = walkStepsDistance;
            playerFootsteps.volumeMin = walkVolumeMin;
            playerFootsteps.volumeMax = walkVolumeMax;
        }

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            sprintValue -= sprintTreshold * Time.deltaTime;

            if(sprintValue <= 0f)
            {
                sprintValue = 0f;

                playerMovement.speed = moveSpeed;

                playerFootsteps.stepDistance = walkStepsDistance;
                playerFootsteps.volumeMin = walkVolumeMin;
                playerFootsteps.volumeMax = walkVolumeMax;
            }
            playerStats.DisplayStaminaStats(sprintValue);
        }
        else
        {
            if (sprintValue != 100f)
            {
                sprintValue += (sprintTreshold / 2f) * Time.deltaTime;

                playerStats.DisplayStaminaStats(sprintValue);

                if (sprintValue > 100f)
                {
                    sprintValue = 100f;
                }
            }
        }

    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            if (isCrouching)
            {
                lookRoot.localPosition = new Vector3(0f, standHeight, 0f);
                playerMovement.speed = moveSpeed;

                playerFootsteps.stepDistance = walkStepsDistance;
                playerFootsteps.volumeMin = walkVolumeMin;
                playerFootsteps.volumeMax = walkVolumeMax;

                isCrouching = false;
            }
            else
            {
                lookRoot.localPosition = new Vector3(0f, crouchHeight, 0f);
                playerMovement.speed = crouchSpeed;

                playerFootsteps.stepDistance = crouchStepsDistance;
                playerFootsteps.volumeMin = crouchVolume;
                playerFootsteps.volumeMax = crouchVolume;

                isCrouching = true;
            }
        }
    }
}
