using UnityEngine;

public class Torch : MonoBehaviour, IInteractable
{
    [SerializeField]
    private PlayerController playercontroller;

    public void Interact()
    {
        playercontroller.torch_equiped = true;

        // Notificar al PlayerInteraction que este objeto ha sido destruido
        InteractableObject playerInteraction = FindObjectOfType<InteractableObject>();
        if (playerInteraction != null)
        {
            playerInteraction.ClearInteractable();
        }

        Destroy(gameObject); // La antorcha desaparece
    }
}
