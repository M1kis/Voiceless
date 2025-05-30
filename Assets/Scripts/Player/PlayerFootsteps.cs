using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioSource footstepsAudioSource;
    public AudioSource tiredAudioSource;

    public AudioClip footsteps;

    public float walkVolume = 0.3f;
    public float runVolume = 0.7f;
    public float walkInterval = 0.5f;
    public float runInterval = 0.3f;

    private CharacterController characterController;
    private float nextFootstepTime = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleFootsteps();
    }

    void HandleFootsteps()
    {
        // Verificar si el jugador está en el suelo
        if (!characterController.isGrounded)
        {
            StopAllFootstepSounds();
            return;
        }

        // Comprobar si el jugador está moviéndose con WASD
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        if (!isMoving)
        {
            StopAllFootstepSounds();
            return;
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float interval = isRunning ? runInterval : walkInterval;
        footstepsAudioSource.volume = isRunning ? runVolume : walkVolume;

        if (Time.time >= nextFootstepTime)
        {
            footstepsAudioSource.PlayOneShot(footsteps);
            nextFootstepTime = Time.time + interval;
        }

        if (isRunning)
        {
            if (!tiredAudioSource.isPlaying)
            {
                tiredAudioSource.Play();
            }
        }
        else
        {
            tiredAudioSource.Stop();
        }
    }

    // Método para detener todos los sonidos de pasos cuando el jugador no se mueve
    void StopAllFootstepSounds()
    {
        footstepsAudioSource.Stop();
        tiredAudioSource.Stop();
    }

}
