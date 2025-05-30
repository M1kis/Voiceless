using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    [Header("Objetos a activar inmediatamente")]
    public List<GameObject> objectsToActivate;

    [Header("Objetos a desactivar inmediatamente")]
    public List<GameObject> objectsToDeactivate;

    [Header("GameObject que aparece tras 10 segundos")]
    public GameObject delayedObject;

    public AudioSource mazeSound;
    public GameObject fade_in;

    private string scene = "Main_Menu";

    [Header("Una sola vez")]
    public bool triggerOnce = true;

    private bool alreadyTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyTriggered && triggerOnce) return;

        if (other.CompareTag("Player"))
        {
            fade_in.SetActive(true);

            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null) obj.SetActive(true);
            }

            foreach (GameObject obj in objectsToDeactivate)
            {
                if (obj != null) obj.SetActive(false);
            }

            if (mazeSound != null)
            {
                StartCoroutine(FadeOutAudio(mazeSound, 3f));
            }

            if (delayedObject != null)
            {
                StartCoroutine(ActivateDelayedObject());
            }

            alreadyTriggered = true;
        }
    }

    private IEnumerator ActivateDelayedObject()
    {
        yield return new WaitForSeconds(5f);
        delayedObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(scene);
    }

    private IEnumerator FadeOutAudio(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}