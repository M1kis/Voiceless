using System.Collections;
using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float blinkSpeed = 0.5f;
    private Coroutine blinkCoroutine;

    void OnEnable()
    {
        if (blinkCoroutine == null)
        {
            blinkCoroutine = StartCoroutine(Blink());
        }
    }

    void OnDisable()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    IEnumerator Blink()
    {
        while (true)
        {
            textMesh.enabled = !textMesh.enabled;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}
