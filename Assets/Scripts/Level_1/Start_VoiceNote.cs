using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_VoiceNote : MonoBehaviour, IInteractable
{
    [SerializeField]
    private AudioSource voiceNote;

    [SerializeField]
    private GameObject subtitles;

    public bool end = false;

    private bool isPlaying = false;

    public void Interact()
    {
        subtitles.SetActive(true);
        voiceNote.Play();
        isPlaying = true;

        // Ocultar panel de interacción
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            InteractableObject interactableController = player.GetComponent<InteractableObject>();
            if (interactableController != null)
            {
                interactableController.ClearInteractable();
            }
        }

        // Desactivar el collider que está en trigger (solo si hay uno)
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            if (col.isTrigger)
            {
                col.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (isPlaying && !voiceNote.isPlaying)
        {
            end = true;
            isPlaying = false;
        }
    }
}
