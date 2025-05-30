using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour
{
    public AudioSource point_sound;
    public PlayerController playerController;

    [Header("Barra de progreso")]
    public RectTransform progressBar;

    private float initialWidth;
    private int totalPoints;
    private int collectedPoints = 0;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        // Contar automáticamente todos los puntos con tag "Point"
        totalPoints = GameObject.FindGameObjectsWithTag("Point").Length;

        // Guardar el ancho original de la barra
        if (progressBar != null)
        {
            initialWidth = progressBar.sizeDelta.x;
        }
    }

    public void CollectPoint()
    {
        point_sound.Play();
        collectedPoints++;

        if (progressBar != null && totalPoints > 0)
        {
            float ratio = 1f - ((float)collectedPoints / totalPoints);
            float newWidth = initialWidth * Mathf.Clamp01(ratio);

            // Aplicar nuevo ancho
            progressBar.sizeDelta = new Vector2(newWidth, progressBar.sizeDelta.y);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Point") && IsTorchAnimationActive())
        {
            CollectPoint();
            Destroy(other.gameObject);
        }
    }

    private bool IsTorchAnimationActive()
    {
        if (playerController == null) return true;
        Animator animator = playerController.GetAnimator();
        return animator != null && animator.GetBool("isTorch");
    }

    public float GetProgress()
    {
        if (totalPoints == 0) return 0f;
        return (float)collectedPoints / totalPoints;
    }
}
