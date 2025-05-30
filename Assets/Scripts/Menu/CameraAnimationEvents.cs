using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimationEvents : MonoBehaviour
{
    public GameObject interfaz;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (interfaz != null)
            interfaz.SetActive(false);
    }

    public void ShowObject()
    {
        if (interfaz != null)
        {
            interfaz.SetActive(true);
        }
    }
}
