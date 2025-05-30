using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearth : MonoBehaviour
{
    [Header("Configuración del corazón")]
    public AudioSource heartAudio;
    public float chaseVolume = 0.15f;
    public float chasePitch = 1.5f;
    public float transitionSpeed = 1.5f; // Velocidad de transición

    private float initialVolume;
    private float initialPitch;

    void Start()
    {
        if (heartAudio == null)
            heartAudio = GetComponent<AudioSource>();

        initialVolume = 0.095f;
        initialPitch = heartAudio.pitch;
    }

    void Update()
    {
        bool anyChasing = false;
        EntityAI[] allEntities = FindObjectsOfType<EntityAI>();
        foreach (EntityAI entity in allEntities)
        {
            if (entity.isChasing)
            {
                anyChasing = true;
                break;
            }
        }

        if (anyChasing)
        {
            // Subir volumen y pitch gradualmente
            heartAudio.volume = Mathf.MoveTowards(heartAudio.volume, chaseVolume, transitionSpeed * Time.deltaTime);
            heartAudio.pitch = Mathf.MoveTowards(heartAudio.pitch, chasePitch, transitionSpeed * Time.deltaTime);
        }
        else
        {
            // Bajar volumen y pitch gradualmente a los valores iniciales
            heartAudio.volume = Mathf.MoveTowards(heartAudio.volume, initialVolume, transitionSpeed * Time.deltaTime);
            heartAudio.pitch = Mathf.MoveTowards(heartAudio.pitch, initialPitch, transitionSpeed * Time.deltaTime);
        }
    }
}
