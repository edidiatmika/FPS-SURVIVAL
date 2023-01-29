using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private Transform playerRoot, lookRoot;

    [SerializeField]
    private bool invert;

    [SerializeField]
    private bool canUnlock = true;

    [SerializeField]
    private float sensitivity = 5f;

    [SerializeField]
    private float smoothSteps = 10f;

    [SerializeField]
    private float smoothWeight = 0.4f;

    [SerializeField]
    private float rollAngle = 10f;

    [SerializeField]
    private float rollSpeed = 3f;

    [SerializeField]
    private Vector2 defaultLookLimit = new Vector2(-70f, 80f);


    private Vector2 lookAngles;

    private Vector2 currentMouseLook;
    private Vector2 smoothMove;

    private float currentRollAngles;

    private int lastLookFrame;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LockAndUnlockCursor();
        
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            LookAround();
        }
    }

    void LockAndUnlockCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void LookAround()
    {
        currentMouseLook = new Vector2(Input.GetAxis(MouseAxis.mouseY), Input.GetAxis(MouseAxis.mouseX));

        lookAngles.x += currentMouseLook.x * sensitivity * (invert ? 1f : -1f);
        lookAngles.y += currentMouseLook.y * sensitivity;

        lookAngles.x = Mathf.Clamp(lookAngles.x, defaultLookLimit.x, defaultLookLimit.y);

        currentRollAngles = Mathf.Lerp(currentRollAngles, Input.GetAxisRaw(MouseAxis.mouseX) * rollAngle, Time.deltaTime * rollSpeed);

        lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, currentRollAngles);
        playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);
    }
}
