using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperNote : MonoBehaviour, IInteractable
{
    [SerializeField]
    private AudioSource paperSound;
    [SerializeField]
    private GameObject paperPanel;

    private bool isPanelActive = false;
    private BoxCollider boxCollider;

    // Guardamos los audios pausados para reanudarlos después
    private AudioSource[] pausedAudioSources;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Interact()
    {
        paperPanel.SetActive(true);
        PauseAllAudioExceptPaper();
        paperSound.Play();
        Time.timeScale = 0f;
        isPanelActive = true;

        if (boxCollider != null)
            boxCollider.enabled = false;
    }

    private void Update()
    {
        if (isPanelActive && Input.GetKeyDown(KeyCode.Return))
        {
            ClosePanel();
        }
    }

    private void ClosePanel()
    {
        paperPanel.SetActive(false);
        ResumeAllAudio();
        Time.timeScale = 1f;
        isPanelActive = false;
        StartCoroutine(EnableColliderAfterDelay(2f));
    }

    private IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (boxCollider != null)
            boxCollider.enabled = true;
    }

    private void PauseAllAudioExceptPaper()
    {
        // Busca todos los AudioSource activos en la escena
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();
        // Lista para guardar los que estaban sonando
        List<AudioSource> pausedList = new List<AudioSource>();

        foreach (AudioSource audio in allAudio)
        {
            if (audio != paperSound && audio.isPlaying)
            {
                audio.Pause();
                pausedList.Add(audio);
            }
        }
        pausedAudioSources = pausedList.ToArray();
    }

    private void ResumeAllAudio()
    {
        if (pausedAudioSources == null) return;
        foreach (AudioSource audio in pausedAudioSources)
        {
            if (audio != null)
                audio.UnPause();
        }
        pausedAudioSources = null;
    }
}
