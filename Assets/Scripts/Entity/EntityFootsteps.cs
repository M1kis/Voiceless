using UnityEngine;

public class EntityFootsteps : MonoBehaviour
{
    public GameObject footStep; // Objeto que contiene el AudioSource
    public float walkPitch = 1.5f; // Pitch para caminar
    public float runPitch = 2.5f;  // Pitch para correr

    private AudioSource audioSource; // Referencia al AudioSource
    private EntityAI entityAI;       // Referencia al script EntityAI

    void Start()
    {
        // Obtener el componente AudioSource del objeto footStep
        if (footStep != null)
        {
            audioSource = footStep.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("No se encontr� un AudioSource en el objeto footStep.");
            }
        }
        else
        {
            Debug.LogError("El objeto footStep no est� asignado.");
        }

        // Obtener la referencia al script EntityAI
        entityAI = GetComponent<EntityAI>();
        if (entityAI == null)
        {
            Debug.LogError("No se encontr� el script EntityAI en el objeto.");
        }
    }

    void Update()
    {
        // Ajustar el pitch en funci�n del estado de la entidad
        if (audioSource != null && entityAI != null)
        {
            if (entityAI.isChasing) // Si est� corriendo
            {
                audioSource.pitch = runPitch;
            }
            else // Si est� caminando
            {
                audioSource.pitch = walkPitch;
            }
        }
    }

    public void ActivarFootSteps()
    {
        if (footStep != null)
        {
            footStep.SetActive(true);
        }
    }
}