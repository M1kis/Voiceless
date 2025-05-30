using UnityEngine;

public class InteractableObjectRayCast : MonoBehaviour
{
    [Header("UI Text")]
    public GameObject interactionText;

    [Header("Interaction")]
    public KeyCode interactionKey = KeyCode.E;
    public float interactDistance = 3f; // Distancia máxima para interactuar

    public Camera cam;
    private GameObject currentInteractable;

    private void Start()
    {
        cam = Camera.main;
        if (interactionText != null)
            interactionText.SetActive(false);
    }

    private void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                if (interactionText != null && currentInteractable != hit.collider.gameObject)
                    interactionText.SetActive(true);

                currentInteractable = hit.collider.gameObject;

                if (Input.GetKeyDown(interactionKey))
                {
                    interactionText.SetActive(false);
                    Interact();
                }

                return; // Salimos para evitar desactivar si aún estamos mirando
            }
        }

        currentInteractable = null;
        if (interactionText != null)
            interactionText.SetActive(false);
    }

    private void Interact()
    {
        if (currentInteractable != null)
        {
            IInteractable interactable = currentInteractable.GetComponent<IInteractable>();
            if (interactable != null)
                interactable.Interact();
        }
    }
}
