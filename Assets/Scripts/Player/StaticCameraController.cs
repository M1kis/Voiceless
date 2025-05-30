using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCameraController : MonoBehaviour
{
    public Transform cameraPos; 
    public float mouseSensitivity = 100f;

    public float minX = -70f;       
    public float maxX = 70f;
    public float minY = -90f;        
    public float maxY = 90f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Vector3 euler = transform.localRotation.eulerAngles;
        xRotation = euler.x;
        yRotation = euler.y;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, minX, maxX);
        yRotation = Mathf.Clamp(yRotation, minY, maxY);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void LateUpdate()
    {
        if (cameraPos != null)
        {
            transform.position = cameraPos.position;
        }
    }
}
