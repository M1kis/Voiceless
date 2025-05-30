using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActivateItems : MonoBehaviour
{
    [Header("Asignaciones")]
    public GameObject uiImage;               // Imagen del jugador a activar
    public AudioSource levelMusic;
    public AudioSource heart;

    [Header("Configuración")]
    public float fadeDuration = 2f;   
    public float targetVolume = 0.20f;
    public float targetVolumeHearth = 0.20f;

    private bool hasActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasActivated) return;

        if (other.CompareTag("Player"))
        {
            hasActivated = true;

            if (uiImage != null)
                EnableAllImages(uiImage);

            if (levelMusic != null)
            {
                heart.Play();
                levelMusic.Play();
                levelMusic.volume = 0f;
                heart.volume = 0f;
                StartCoroutine(FadeInMusic());
            }

            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
        }
    }

    private System.Collections.IEnumerator FadeInMusic()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            levelMusic.volume = Mathf.Lerp(0f, targetVolume, elapsed / fadeDuration);
            heart.volume = Mathf.Lerp(0f, targetVolumeHearth, elapsed / fadeDuration);
            yield return null;
        }

        levelMusic.volume = targetVolume;
        heart.volume = targetVolumeHearth;
    }

    void EnableAllImages(GameObject target)
    {
        Image[] images = target.GetComponentsInChildren<Image>(true);
        foreach (Image img in images)
        {
            img.enabled = true;
        }
    }

}
