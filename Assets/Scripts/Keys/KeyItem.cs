using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    [Header("ID de la llave")]
    public string keyID;

    [Header("Audio al recoger")]
    public AudioSource pickupSound;

    public void Interact()
    {
        // Desactivar el mesh y el collider para que "desaparezca"
        GetComponent<Collider>().enabled = false;

        // Si hay un MeshRenderer o algún hijo visual
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null) mesh.enabled = false;

        // También puedes desactivar todos los renderers hijos (opcional)
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        // Registrar la llave
        KeyManager.Instance.CollectKey(keyID);

        // Reproducir sonido
        if (pickupSound != null)
        {
            pickupSound.Play();
            Destroy(gameObject, pickupSound.clip.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}