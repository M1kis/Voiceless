using System.Collections;
using UnityEngine;

public class Door_KeyPad : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject panelKeyPad;
    [SerializeField]
    private AudioSource keyPadAudioSource;
    [SerializeField]
    private GameObject fisicKeyPad;
    [SerializeField]
    private BoxCollider boxCollider;
    [SerializeField]
    private Start_VoiceNote voiceNote;
    [SerializeField]
    private GameObject aviso;
    [SerializeField]
    private TypewriterEffect typewriter;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Interact()
    {
        if (voiceNote.end == true)
        {
            panelKeyPad.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            keyPadAudioSource.Play();
            Time.timeScale = 0f;
            if (boxCollider != null)
                boxCollider.enabled = false;
        }
        else
        {
            if (typewriter != null)
            {
                typewriter.StartTyping("Aún no puedes hacer eso...");
            }
        }
    }

    public void ClosePanelWithDelay(float delay)
    {
        StartCoroutine(ClosePanelCoroutine(delay));
    }

    private IEnumerator ClosePanelCoroutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (fisicKeyPad != null)
        {
            BoxCollider box = fisicKeyPad.GetComponent<BoxCollider>();
            if (box != null)
                box.enabled = true;
        }
    }
}
