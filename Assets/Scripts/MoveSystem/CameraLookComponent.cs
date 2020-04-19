using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookComponent : MonoBehaviour
{
    [SerializeField]
    float lookLength;

    private Transform playerTransform;
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

    private void CameraFollow()
    {
        float xComponent = ((float)Input.mousePosition.x / Screen.width - 0.5f);
        float yComponent = ((float)Input.mousePosition.y / Screen.height - 0.5f);
        camTransform.position = playerTransform.position + new Vector3(xComponent * lookLength, yComponent * lookLength, -1);
    }
}
