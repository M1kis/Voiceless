using UnityEngine;

public class TwoDimensionalAnimationStateController : MonoBehaviour
{
    private Animator animator;

    public float acceleration = 2.0f;
    public float deceleration = 2.0f;
    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;

    private float velocityZ = 0.0f;
    private float velocityX = 0.0f;

    private int velocityZHash;
    private int velocityXHash;

    private bool isRunningForward = false;
    private bool isRunningBackward = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        velocityZHash = Animator.StringToHash("velocityZ");
        velocityXHash = Animator.StringToHash("velocityX");
    }

    void Update()
    {
        ChangeVelocity();
        LockOrResetVelocity();

        animator.SetFloat(velocityZHash, velocityZ);
        animator.SetFloat(velocityXHash, velocityX);
    }

    private void ChangeVelocity()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backwardPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        // Determinar si el personaje está corriendo hacia adelante o hacia atrás
        if (runPressed && forwardPressed)
        {
            isRunningForward = true;
            isRunningBackward = false;
        }
        else if (runPressed && backwardPressed)
        {
            isRunningForward = false;
            isRunningBackward = true;
        }
        else
        {
            isRunningForward = false;
            isRunningBackward = false;
        }

        // Calcular la velocidad máxima actual de manera gradual
        float targetMaxVelocity;

        if (isRunningForward)
        {
            targetMaxVelocity = maximumRunVelocity;
        }
        else if (isRunningBackward)
        {
            targetMaxVelocity = -maximumRunVelocity;
        }
        else if (forwardPressed)
        {
            targetMaxVelocity = maximumWalkVelocity;
        }
        else if (backwardPressed)
        {
            targetMaxVelocity = -maximumWalkVelocity;
        }
        else
        {
            targetMaxVelocity = 0;
        }

        // Manejar transiciones bruscas en el eje Z (adelante/atrás)
        if ((forwardPressed && velocityZ < 0) || (backwardPressed && velocityZ > 0))
        {
            // Cambio de dirección: aumentar la desaceleración
            velocityZ = Mathf.MoveTowards(velocityZ, 0, deceleration * 2 * Time.deltaTime);
        }
        else
        {
            // Ajustar la velocidad actual gradualmente hacia la velocidad objetivo
            velocityZ = Mathf.MoveTowards(velocityZ, targetMaxVelocity, acceleration * Time.deltaTime);
        }

        // Calcular la velocidad máxima lateral (correr o caminar)
        float lateralMaxVelocity = runPressed ? maximumRunVelocity : maximumWalkVelocity;

        // Manejar transiciones bruscas en el eje X (izquierda/derecha)
        if ((leftPressed && velocityX > 0) || (rightPressed && velocityX < 0))
        {
            // Cambio de dirección: aumentar la desaceleración
            velocityX = Mathf.MoveTowards(velocityX, 0, deceleration * 2 * Time.deltaTime);
        }
        else
        {
            // Ajustar la velocidad actual gradualmente hacia la velocidad objetivo
            if (leftPressed)
            {
                velocityX = Mathf.MoveTowards(velocityX, -lateralMaxVelocity, acceleration * Time.deltaTime);
            }
            else if (rightPressed)
            {
                velocityX = Mathf.MoveTowards(velocityX, lateralMaxVelocity, acceleration * Time.deltaTime);
            }
            else
            {
                velocityX = Mathf.MoveTowards(velocityX, 0, deceleration * Time.deltaTime);
            }
        }
    }

    private void LockOrResetVelocity()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (velocityZ > 0)
            {
                velocityZ -= Time.deltaTime * deceleration;
                if (velocityZ < 0) velocityZ = 0;
            }
            else if (velocityZ < 0)
            {
                velocityZ += Time.deltaTime * deceleration;
                if (velocityZ > 0) velocityZ = 0;
            }

            if (velocityX < 0 && velocityX > -0.05f)
            {
                velocityX = 0;
            }
            else if (velocityX > 0 && velocityX < 0.05f)
            {
                velocityX = 0;
            }
        }
    }
}