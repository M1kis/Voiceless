using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyPadUI : MonoBehaviour
{
    [Header("Configuración del KeyPad")]
    public TMP_Text displayText;
    public string correctCode = "456";
    public int maxDigits = 3;

    [Header("Referencias externas")]
    public GameObject panelKeyPad;
    public AudioSource doorSound;
    public AudioSource buttonSound;
    public AudioSource successSound;
    public AudioSource errorSound;
    public Animator doorAnimator;
    public string animatorParameter = "Interacting";

    [Header("Referencia al script Door_KeyPad")]
    public Door_KeyPad doorKeyPad;

    private string currentInput = "";
    private AudioSource[] pausedAudioSources;

    private void Update()
    {
        if (panelKeyPad && Input.GetKeyDown(KeyCode.Return))
        {
            ClosePanel();
        }
    }

    private void OnEnable()
    {
        if (panelKeyPad != null && panelKeyPad.activeSelf)
        {
            PauseAllAudioExceptKeyPad();
            Time.timeScale = 0f;
        }
    }

    private void OnDisable()
    {
        ResumeAllAudio();
        Time.timeScale = 1f;
    }

    public void OnDigitButtonPressed(string digit)
    {
        PlayButtonSound();
        if (currentInput.Length < maxDigits)
        {
            currentInput += digit;
            UpdateDisplay();
        }
    }

    public void OnClearButtonPressed()
    {
        PlayButtonSound();
        currentInput = "";
        UpdateDisplay();
    }

    // Ahora delega el cierre al Door_KeyPad
    private void ClosePanel()
    {
        if (doorKeyPad != null)
        {
            if (panelKeyPad != null)
                panelKeyPad.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            doorKeyPad.ClosePanelWithDelay(2f);
        }
    }

    public void OnEnterButtonPressed()
    {
        if (currentInput == correctCode)
        {
            PlaySuccessSound();
            StartCoroutine(SuccessSequence());
        }
        else
        {
            PlayErrorSound();
            currentInput = "";
            UpdateDisplay();
        }
    }

    private void PlaySuccessSound()
    {
        if (successSound != null)
            successSound.Play();
    }

    private void PlayErrorSound()
    {
        if (errorSound != null)
            errorSound.Play();
    }

    private IEnumerator SuccessSequence()
    {
        if (successSound != null) successSound.Play();
        yield return new WaitForSecondsRealtime(2f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (panelKeyPad != null) panelKeyPad.SetActive(false);
        if (doorSound != null) doorSound.Play();
        if (doorAnimator != null) doorAnimator.SetBool(animatorParameter, true);
    }

    private void UpdateDisplay()
    {
        if (displayText != null)
            displayText.text = currentInput;
    }

    private void PlayButtonSound()
    {
        if (buttonSound != null)
            buttonSound.Play();
    }

    private void PauseAllAudioExceptKeyPad()
    {
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();
        List<AudioSource> pausedList = new List<AudioSource>();

        foreach (AudioSource audio in allAudio)
        {
            if (audio != buttonSound && audio != successSound && audio != errorSound && audio != doorSound && audio.isPlaying)
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
