using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// player-only class for having the camera follow the robot that we're currently controlling
public class CameraLookComponent : MonoBehaviour
{
    [SerializeField]
    float lookLength;

    private static Transform playerTransform;
    private Transform camTransform;
    private Camera playerCamera;
    private void Awake()
    {
        playerCamera = Camera.main;
        camTransform = playerCamera.transform;
        playerTransform = GetComponent<Transform>();
        Cursor.visible = false;
    }
    private void FixedUpdate()
    {
        CameraFollow();
    }

    public void ChangeCameraFocus(GameObject go)
    {
        playerTransform = go.transform;
    }


    private void CameraFollow()
    {
        float xComponent = ((float)Input.mousePosition.x / Screen.width - 0.5f);
        float yComponent = ((float)Input.mousePosition.y / Screen.height - 0.5f);
        camTransform.position = playerTransform.position + new Vector3(xComponent * lookLength, yComponent * lookLength, -1);
    }
}
