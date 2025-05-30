using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Change_Door_Scene : MonoBehaviour, IInteractable
{
    public string requiredKeyID = "1";
    private Animator animator;
    public bool isOpen = false;

    [SerializeField]
    private AudioSource door_sound;


    [SerializeField]
    private Start_VoiceNote voiceNote;

    [SerializeField]
    private GameObject fadeIn;

    [SerializeField]
    private GameObject aviso;

    [SerializeField]
    private TypewriterEffect typewriter;


    private IEnumerator CargarEscenaConRetraso(string nombreEscena)
    {
        yield return new WaitForSeconds(6.0f);
        SceneManager.LoadScene(nombreEscena);
    }

    private IEnumerator CargarFadeIn()
    {
        yield return new WaitForSeconds(1.0f);
        fadeIn.SetActive(true);
    }


    private void Start()
    {
        animator = GetComponent<Animator>();
        typewriter = aviso.GetComponent<TypewriterEffect>();
    }

    public void Interact()
    {
        Debug.Log("Intentando interactuar con puerta");

        if (isOpen) return;

        if (KeyManager.Instance.HasKey(requiredKeyID))
        {
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

                StartCoroutine(CargarFadeIn());
                StartCoroutine(CargarEscenaConRetraso("Level_2"));
            }
            else
            {
                if (typewriter != null)
                {
                    typewriter.StartTyping("Aún no puedes hacer eso...");
                }
            }
        }
        else
        {
            aviso.SetActive(true);

            if (typewriter != null)
            {
                typewriter.StartTyping("Necesitas una llave para abrir esta puerta");
            }
        }
    }
}
