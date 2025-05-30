using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController player;
    private CameraController cameraController;

    void Start()
    {
        // Buscar automáticamente los controladores en la escena
        player = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<CameraController>();

        if (player == null)
            Debug.LogWarning("No se encontró el PlayerController en la escena.");

        if (cameraController == null)
            Debug.LogWarning("No se encontró el CameraController en la escena.");
    }

    public void DisablePlayerControl()
    {
        if (player != null) player.enabled = false;
        if (cameraController != null) cameraController.enabled = false;
    }

    public void EnablePlayerControl()
    {
        if (player != null) player.enabled = true;
        if (cameraController != null) cameraController.enabled = true;

    }
}
