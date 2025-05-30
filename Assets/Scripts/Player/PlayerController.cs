using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float runSpeed = 10f; 
    public float gravity = 9.81f; 
    private float verticalVelocity = 0f; 
    public GameObject torch;
    public bool torch_equiped = false;
    private CharacterController characterController;
    private Animator animator; 

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

 
        UpdatePointVisibility();
    }

    void Update()
    {
        Move();
        ApplyGravity();
        HandleTorchInput();
    }

    void Move()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        if (move.magnitude < 0.1f)
        {
            move = Vector3.zero;
        }
        move.y = verticalVelocity;
        characterController.Move(move * speed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
    }

    void HandleTorchInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && torch_equiped) // Si se presiona la tecla F
        {
            bool isTorchEquipped = animator.GetBool("isTorch"); // Obtener el estado actual
            isTorchEquipped = !isTorchEquipped; // Alternar el estado
            animator.SetBool("isTorch", isTorchEquipped); // Aplicar el nuevo estado
            torch.SetActive(isTorchEquipped); // Activar o desactivar la antorcha según el estado

            // Actualizar visibilidad de puntos cuando cambie la antorcha
            UpdatePointVisibility();
        }
    }

    // Método para actualizar visibilidad de puntos y powerups
    void UpdatePointVisibility()
    {
        bool torchActive = animator != null && animator.GetBool("isTorch");

        // Actualizar visibilidad de puntos
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        foreach (GameObject point in points)
        {
            if (point != null)
            {
                Renderer rend = point.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.enabled = torchActive;
                }
            }
        }

        // Actualizar visibilidad de powerups
        GameObject[] powerups = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject powerup in powerups)
        {
            if (powerup != null)
            {
                Renderer rend = powerup.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.enabled = torchActive;
                }
            }
        }
    }

    public Animator GetAnimator()
    {
        return animator;
    }
}