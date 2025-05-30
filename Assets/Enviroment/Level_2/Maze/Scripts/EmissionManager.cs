using UnityEngine;

public class EmissionManager : MonoBehaviour
{
    public Material targetMaterial; // Material que se va a modificar
    public float transitionSpeed = 1.0f; // Velocidad de transición

    private float targetIntensity = -5f; // Intensidad objetivo
    public float currentIntensity; // Intensidad actual
    private bool isIncreasing = true; // Controla la dirección del cambio

    private Color initialEmissionColor; // Color de emisión inicial (base)
    private float initialIntensity = 0f; // Intensidad inicial (0 para el valor base)

    public PlayerController playerController;

    private void Start()
    {
        if (targetMaterial != null)
        {
            // Guarda el color de emisión inicial del material (base)
            initialEmissionColor = targetMaterial.GetColor("_EmissionColor");
            currentIntensity = initialIntensity; // Inicializa la intensidad

            // Aplica el valor inicial al material (-10)
            ApplyEmissionIntensity(targetIntensity);
        }
        else
        {
            Debug.LogError("No se ha asignado un material objetivo.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerController.torch_equiped == true)
        {
            // Cambia la intensidad objetivo al presionar F
            if (isIncreasing)
            {
                targetIntensity = 2f; // Cambia a 2 (aparición)
            }
            else
            {
                targetIntensity = -5f; // Cambia a -10 (desaparición)
            }
            isIncreasing = !isIncreasing; // Invierte la dirección
        }

        // Interpola suavemente la intensidad actual hacia la intensidad objetivo
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * transitionSpeed);

        // Aplica la intensidad al material
        ApplyEmissionIntensity(currentIntensity);
    }

    private void ApplyEmissionIntensity(float intensity)
    {
        if (targetMaterial != null)
        {
            // Calcula el nuevo color de emisión multiplicando el color inicial por la intensidad
            Color newEmissionColor = initialEmissionColor * Mathf.Pow(2, intensity);
            targetMaterial.SetColor("_EmissionColor", newEmissionColor);

            // Asegúrate de que Unity actualice la emisión en tiempo real
            targetMaterial.EnableKeyword("_EMISSION");
        }
    }

    private void OnDisable()
    {
        // Restaura el valor inicial del material cuando el script se desactiva o el juego se detiene
        if (targetMaterial != null)
        {
            ApplyEmissionIntensity(initialIntensity);
        }
    }

    private void OnApplicationQuit()
    {
        // Restaura el valor inicial del material cuando el juego termina
        if (targetMaterial != null)
        {
            ApplyEmissionIntensity(initialIntensity);
        }
    }
}