using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorComponent : MonoBehaviour
{
    [SerializeField]
    float cursorRotateRate = 1.0f;
    Transform thistransform;
    Camera camera;
    Animator animators;

    // Start is called before the first frame update
    void Start()
    {
        thistransform = GetComponent<Transform>();
        camera = Camera.main;
        animators = GetComponent<Animator>();
        animators.Play("CrosshairAnimation");

    }

    // Update is called once per frame
    void Update()
    {
        thistransform.Rotate(Vector3.forward, cursorRotateRate);
        Vector3 newPos = camera.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        thistransform.position = newPos;
    }
}
