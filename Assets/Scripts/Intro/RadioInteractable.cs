using UnityEngine;

public class RadioInteractable : MonoBehaviour, IInteractable
{

    [SerializeField]
    private AudioSource voiceNote;

    [SerializeField]
    private GameObject subtitles;

    public bool end = false;


    public void Interact()
    {
        FindObjectOfType<IntroManager>().InteractuarConRadio();
        subtitles.SetActive(true);
        voiceNote.Play();

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
}