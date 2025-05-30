using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EntityAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator entityAnimator;
    public Transform player;
    public LayerMask obstacleLayer;

    [Header("Patrol Settings")]
    public float patrolSpeed = 2.0f;
    public float runSpeed = 5.0f;
    public float patrolRange = 10.0f;

    [Header("Detection Settings")]
    public float coneDetectionAngle = 45f;
    public float coneDetectionRange = 10f;
    public float smallSphereDetectionRange = 3f;
    public float largeSphereDetectionRange = 15f;

    [Header("Audio")]
    public AudioSource playerDetectedAudio;
    public AudioSource playerEscapedAudio;
    public AudioSource maze_Sound;
    public AudioClip maze_ambience;
    public AudioClip chase_music;

    private bool isPatrolling = true;
    public bool isChasing = false;
    private bool wasChasing = false;
    private Vector3 randomPatrolPoint;

    public float fadeDuration = 0.5f;
    private bool isTransitioning = false;

    void Start()
    {
        GenerateRandomPatrolPoint();
    }

    void Update()
    {
        if (isPatrolling)
        {
            Patrol();
        }

        if (isChasing)
        {
            ChasePlayer();
        }

        CheckForPlayer();
        UpdateAnimations();

        if (isChasing && !wasChasing)
        {
            playerDetectedAudio.Play(); // Sonido de alerta
            StartCoroutine(TransitionAudio(chase_music));
        }

        else if (!isChasing && wasChasing)
        {
            playerEscapedAudio.Play(); // Sonido de escape
            StartCoroutine(TransitionAudio(maze_ambience));
        }

        wasChasing = isChasing;
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, randomPatrolPoint) < 1.0f)
        {
            GenerateRandomPatrolPoint();
        }

        agent.speed = patrolSpeed;
        agent.SetDestination(randomPatrolPoint);
    }

    void GenerateRandomPatrolPoint()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * patrolRange;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, patrolRange, NavMesh.AllAreas);
        randomPatrolPoint = hit.position;
    }

    void ChasePlayer()
    {
        agent.speed = runSpeed;
        agent.SetDestination(player.position);
    }

    void CheckForPlayer()
    {
        if (isChasing)
        {
            // Si el jugador sale del rango grande, dejar de perseguir
            if (Vector3.Distance(transform.position, player.position) > largeSphereDetectionRange)
            {
                StopChasing();
            }
        }
        else
        {
            // Si el jugador entra en el cono o en la esfera pequeña, comenzar a perseguir
            if (IsPlayerInConeDetection() || IsPlayerInSmallSphereDetection())
            {
                StartChasing();
            }
        }
    }

    bool IsPlayerInConeDetection()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Verificar si el jugador está dentro del ángulo del cono
        if (angleToPlayer < coneDetectionAngle / 2)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Verificar si el jugador está dentro del rango del cono
            if (distanceToPlayer < coneDetectionRange)
            {
                // Verificar si no hay obstáculos entre el enemigo y el jugador
                RaycastHit hit;
                if (!Physics.SphereCast(transform.position, 0.5f, directionToPlayer, out hit, distanceToPlayer, obstacleLayer))
                {
                    return true;
                }
            }
        }

        return false;
    }

    bool IsPlayerInSmallSphereDetection()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < smallSphereDetectionRange)
        {
            if (!Physics.Linecast(transform.position, player.position, obstacleLayer))
            {
                return true;
            }
        }

        return false;
    }

    void StartChasing()
    {
        isPatrolling = false;
        isChasing = true;
    }

    void StopChasing()
    {
        isPatrolling = true;
        isChasing = false;
    }

    void UpdateAnimations()
    {
        entityAnimator.SetBool("isPatrolling", isPatrolling);
        entityAnimator.SetBool("isChasing", isChasing);
    }

    // Dibujar los rangos de detección en el editor
    private void OnDrawGizmosSelected()
    {
        // Cono de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, coneDetectionRange);
        Vector3 coneDirection = Quaternion.Euler(0, coneDetectionAngle / 2, 0) * transform.forward * coneDetectionRange;
        Gizmos.DrawLine(transform.position, transform.position + coneDirection);
        coneDirection = Quaternion.Euler(0, -coneDetectionAngle / 2, 0) * transform.forward * coneDetectionRange;
        Gizmos.DrawLine(transform.position, transform.position + coneDirection);

        // Esfera pequeña
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, smallSphereDetectionRange);

        // Esfera grande
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, largeSphereDetectionRange);
    }

    // Corrutina para el fade suave
    IEnumerator TransitionAudio(AudioClip newClip)
    {
        if (isTransitioning) yield break;

        isTransitioning = true;

        float startVolume = maze_Sound.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            maze_Sound.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        // Cambiar el clip y reiniciar el volumen
        maze_Sound.Stop();
        maze_Sound.clip = newClip;
        maze_Sound.loop = true;
        maze_Sound.Play();

        // Fade In: Subir volumen gradualmente al original
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            maze_Sound.volume = Mathf.Lerp(0f, startVolume, timer / fadeDuration);
            yield return null;
        }

        isTransitioning = false;

    }


}