using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollector : MonoBehaviour
{
    public Material highlightMaterial;
    public float duration = 5f;
    public float fadeDuration = 1f;
    public AudioSource helpMeAudio;
    public AudioSource powerUpAudio;

    // Diccionario para guardar los materiales originales
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();
    private PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            powerUpAudio.Play();
            helpMeAudio.Play();
            StartCoroutine(FadeOutAudio(helpMeAudio, 5f));
            StartCoroutine(HighlightRemainingPoints());
            Destroy(other.gameObject);
        }
    }

    IEnumerator HighlightRemainingPoints()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        List<(GameObject point, float distance)> sortedPoints = new List<(GameObject, float)>();

        Vector3 playerPos = transform.position;

        foreach (GameObject point in points)
        {
            if (point != null && point.activeInHierarchy)
            {
                float dist = Vector3.Distance(playerPos, point.transform.position);
                sortedPoints.Add((point, dist));
            }
        }

        sortedPoints.Sort((a, b) => a.distance.CompareTo(b.distance));

        int total = sortedPoints.Count;
        float durationPerPoint = duration / Mathf.Max(1, total);
        float fadeTime = fadeDuration;
        float visibleTime = 1f;

        for (int i = 0; i < total; i++)
        {
            var (point, _) = sortedPoints[i];

            if (point != null)
            {
                Renderer rend = point.GetComponent<Renderer>();
                if (rend != null)
                {
                    Vector3 dirToPoint = rend.bounds.center - Camera.main.transform.position;
                    Ray ray = new Ray(Camera.main.transform.position, dirToPoint);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, dirToPoint.magnitude))
                    {
                        if (hit.collider.gameObject != point)
                        {
                            // Guardar el material original y habilitar el renderer temporalmente
                            if (!originalMaterials.ContainsKey(rend))
                            {
                                originalMaterials[rend] = rend.material;
                            }

                            // Forzar que sea visible durante el efecto
                            rend.enabled = true;

                            Material matInstance = new Material(highlightMaterial);
                            rend.material = matInstance;

                            StartCoroutine(FadeInAndOutAndRestore(rend, fadeDuration, visibleTime));
                        }
                    }
                }
            }

            yield return new WaitForSeconds(durationPerPoint);
        }

        // Limpiar después de que termine todo el efecto
        StartCoroutine(CleanupAfterDelay(2f));
    }

    IEnumerator FadeInAndOutAndRestore(Renderer rend, float fadeTime, float visibleTime)
    {
        Material mat = rend.material;

        // Empezar invisible
        Color color = mat.color;
        color.a = 0f;
        mat.color = color;

        // FADE IN
        float t = 0f;
        while (t < fadeTime)
        {
            if (rend == null) yield break;

            float alpha = Mathf.Lerp(0f, 1f, t / fadeTime);
            color.a = alpha;
            mat.color = color;
            t += Time.deltaTime;
            yield return null;
        }

        // visibilidad total
        if (rend != null)
        {
            color.a = 1f;
            mat.color = color;
        }

        // Tiempo visible
        yield return new WaitForSeconds(visibleTime);

        // FADE OUT
        t = 0f;
        while (t < fadeTime)
        {
            if (rend == null) yield break;

            float alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            color.a = alpha;
            mat.color = color;
            t += Time.deltaTime;
            yield return null;
        }

        // Restaurar material original
        if (rend != null && originalMaterials.ContainsKey(rend))
        {
            rend.material = originalMaterials[rend];
            originalMaterials.Remove(rend);
        }
    }

    IEnumerator CleanupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Restaurar cualquier material que haya quedado pendiente
        foreach (var kvp in originalMaterials)
        {
            if (kvp.Key != null)
            {
                kvp.Key.material = kvp.Value;
            }
        }
        originalMaterials.Clear();

        // Restaurar la visibilidad correcta basada en la antorcha
        if (playerController != null)
        {
            bool torchActive = playerController.GetAnimator() != null &&
                              playerController.GetAnimator().GetBool("isTorch");

            // Restaurar visibilidad de puntos
            GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
            foreach (GameObject point in points)
            {
                if (point != null)
                {
                    Renderer rend = point.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        rend.enabled = torchActive;
                    }
                }
            }

            // Restaurar visibilidad de powerups
            GameObject[] powerups = GameObject.FindGameObjectsWithTag("PowerUp");
            foreach (GameObject powerup in powerups)
            {
                if (powerup != null)
                {
                    Renderer rend = powerup.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        rend.enabled = torchActive;
                    }
                }
            }
        }
    }

    IEnumerator FadeOutAudio(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}