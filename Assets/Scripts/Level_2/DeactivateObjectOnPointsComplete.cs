using System.Collections;
using UnityEngine;

public class DeactivateObjectOnPointsComplete : MonoBehaviour
{
    [Header("Referencia al contador de puntos")]
    public PointCounter pointCounter;

    [Header("Objeto a desactivar (puerta)")]
    public GameObject objectToDeactivate;

    [SerializeField]
    private GameObject aviso;

    [SerializeField]
    private TypewriterEffect typewriter;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip newClip;
    public float fadeDuration = 1.5f;

    private bool hasDeactivated = false;

    void Update()
    {
        if (hasDeactivated || pointCounter == null || objectToDeactivate == null)
            return;

        if (pointCounter.GetProgress() >= 1f)
        {

            objectToDeactivate.SetActive(false);
            hasDeactivated = true;

            aviso.SetActive(true);

            if (typewriter != null)
            {
                typewriter.StartTyping("Una puerta se ha abierto ¡Escapa!");
            }
        }
    }

}
