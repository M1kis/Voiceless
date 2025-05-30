using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door_NoKey : MonoBehaviour, IInteractable
{
    private Animator animator;
    public bool isOpen = false;

    [SerializeField]
    private AudioSource door_sound;


    [SerializeField]
    private Start_VoiceNote voiceNote;


    [SerializeField]
    private GameObject aviso;

    [SerializeField]
    private TypewriterEffect typewriter;



    private void Start()
    {
        animator = GetComponent<Animator>();
        typewriter = aviso.GetComponent<TypewriterEffect>();
    }

    public void Interact()
    {

        if (isOpen) return;

        if (voiceNote.end == true)
        {
            door_sound.Play();
            animator.SetBool("Interacting", true);
            isOpen = true;

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
        else
        {
            aviso.SetActive(true);

            if (typewriter != null)
            {
                typewriter.StartTyping("Aún no puedes hacer eso...");
            }
        }
    }
}
