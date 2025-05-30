using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Header("UI Text")]
    public GameObject interactionText;

    [Header("Interaction Key")]
    public KeyCode interactionKey = KeyCode.E;

    private bool isNearInteractable = false;
    private GameObject currentInteractable;

    private void Start()
    {
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isNearInteractable && Input.GetKeyDown(interactionKey))
        {
            interactionText.SetActive(false);
            Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            isNearInteractable = true;
            currentInteractable = other.gameObject;

            if (interactionText != null)
                interactionText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            isNearInteractable = false;
            currentInteractable = null;

            if (interactionText != null)
                interactionText.gameObject.SetActive(false);
        }
    }

    private void Interact()
    {
        if (currentInteractable != null)
        {
            Collider col = currentInteractable.GetComponent<Collider>();
            if (col != null && col.enabled && col.isTrigger)
            {
                IInteractable interactable = currentInteractable.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }


    public void ClearInteractable()
    {
        isNearInteractable = false;
        currentInteractable = null;
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }
}
