using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private float delayBeforeHide = 2f;

    private Coroutine typingCoroutine;

    public void StartTyping(string newText)
    {
        gameObject.SetActive(true); // Asegura que esté visible antes de comenzar

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(newText));
    }

    private IEnumerator TypeText(string fullText)
    {
        textComponent.text = "";

        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(delayBeforeHide);
        gameObject.SetActive(false);
    }
}