using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public string requiredKeyID = "1";
    private Animator animator;
    public bool isOpen = false;

    [SerializeField]
    private AudioSource door_sound;


    [SerializeField]
    private GameObject aviso;

    [SerializeField]
    private TypewriterEffect typewriter;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        Debug.Log("Intentando interactuar con puerta");

        if (isOpen) return;

        if (KeyManager.Instance.HasKey(requiredKeyID))
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
                typewriter.StartTyping("Necesitas una llave para abrir esta puerta");
            }
        }
    }
}
