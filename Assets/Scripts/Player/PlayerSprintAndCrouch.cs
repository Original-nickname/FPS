using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    public float sprintSpeed = 10f;
    public float moveSpeed = 5f;
    public float crouchSpeed = 2f;

    private PlayerMovement playerMovement;

    private Transform lookRoot;
    private float standHeight = 1.6f;
    private float crouchHeight = 1f;

    private bool isCrouching;

    private PlayerFootStep playerFootStep;

    private readonly float sprintVolume = 1f;
    private readonly float crouchVolume = 0.1f;
    private readonly float walkVolumeMin = 0.2f;
    private readonly float walkVolumeMax = 0.6f;

    private readonly float walkStepDistance = 0.4f;
    private readonly float sprintStepDistance = 0.25f;
    private readonly float crouchStepDistance = 0.4f;

    private PlayerStats playerStats;
    private float sprintValue = 100f;
    private float sprintTreshold = 10f;
    private float staminaForJump = 20;

    public float SprintValue 
    {
        get => sprintValue;
    }

    public float StaminaForJump
    {
        get => staminaForJump;
    }

    private void Start()
    {
        playerFootStep.volumeMin = walkVolumeMin;
        playerFootStep.volumeMax = walkVolumeMax;
        playerFootStep.stepDistance = walkStepDistance;
    }

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        lookRoot = transform.GetChild(0);
        playerFootStep = GetComponentInChildren<PlayerFootStep>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        Sprint();
        Crouch();
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sprintValue -= staminaForJump;
            if (sprintValue <= 0)
            {
                sprintValue = 0;
            }
        }
    }
    void Sprint()
    {  
        if (sprintValue > 0)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching)
            {
                playerMovement.speed = sprintSpeed;
                playerFootStep.volumeMin = sprintVolume;
                playerFootStep.volumeMax = sprintVolume;
                playerFootStep.stepDistance = sprintStepDistance;
            }
        }

        if(Input.GetKeyUp(KeyCode.LeftShift) && !isCrouching)
        {
            playerMovement.speed = moveSpeed;
            playerFootStep.volumeMin = walkVolumeMin;
            playerFootStep.volumeMax = walkVolumeMax;
            playerFootStep.stepDistance = walkStepDistance;
        }


        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            sprintValue -= sprintTreshold * Time.deltaTime;
            Debug.Log(sprintValue);
            if (sprintValue <= 0)
            {
                sprintValue = 0;
                playerMovement.speed = moveSpeed;
                playerFootStep.volumeMin = walkVolumeMin;
                playerFootStep.volumeMax = walkVolumeMax;
                playerFootStep.stepDistance = walkStepDistance;
            }

            playerStats.DisplayStaminaStats(sprintValue);
        } 
        else
        {
            if (sprintValue != 100f)
            {
                sprintValue += (sprintTreshold / 2) * Time.deltaTime;
                playerStats.DisplayStaminaStats(sprintValue);
                if(sprintValue > 100)
                {
                    sprintValue = 100;
                }
            }
        }
    }

    void Crouch()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(isCrouching)
            {
                lookRoot.localPosition = new Vector3(0, standHeight, 0);
                playerMovement.speed = moveSpeed;
                isCrouching = false;

                playerFootStep.volumeMin = walkVolumeMin;
                playerFootStep.volumeMax = walkVolumeMax;
                playerFootStep.stepDistance = walkStepDistance;
            }
            else
            {
                lookRoot.localPosition = new Vector3(0, crouchHeight, 0);
                playerMovement.speed = crouchSpeed;
                isCrouching = true;

                playerFootStep.volumeMin = crouchVolume;
                playerFootStep.volumeMax = crouchVolume;
                playerFootStep.stepDistance = crouchStepDistance;
            }
        }
    }
}
