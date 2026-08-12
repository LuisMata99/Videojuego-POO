using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float velocidadRotacion = 15f;

    // Referencia al componente Animator de tu personaje
    [SerializeField] private Animator anim;

    private Rigidbody rb;
    private Vector3 direccionMovimiento;
    private Transform transformCamara;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (Camera.main != null)
        {
            transformCamara = Camera.main.transform; // Caché inicial
        }
    }

    void Update()
    {
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");

        if (transformCamara != null)
        {
            Vector3 forwardCam = transformCamara.forward;
            Vector3 rightCam = transformCamara.right;

            // Se aplanan los vectores
            forwardCam.y = 0f;
            rightCam.y = 0f;

            forwardCam.Normalize();
            rightCam.Normalize();

            direccionMovimiento = (forwardCam * inputVertical + rightCam * inputHorizontal).normalized;
        }

        // Se utiliza sqrMagnitude por rendimiento, evitando el cálculo de raíz cuadrada de .magnitude
        bool estaMoviendose = direccionMovimiento.sqrMagnitude > 0.01f;

        if (anim != null)
        {
            anim.SetBool("isWalking", estaMoviendose);
        }
    }

    void FixedUpdate()
    {
        // 1. Movimiento
        rb.MovePosition(rb.position + direccionMovimiento * velocidad * Time.fixedDeltaTime);

        // 2. Rotación (Solo rotar si realmente se está aplicando input)
        if (direccionMovimiento.sqrMagnitude > 0.01f)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
            Quaternion rotacionSuavizada = Quaternion.Slerp(rb.rotation, rotacionObjetivo, velocidadRotacion * Time.fixedDeltaTime);

            rb.MoveRotation(rotacionSuavizada);
        }
        else
        {
            // Detener cualquier velocidad angular física que intente hacerlo girar quieto
            rb.angularVelocity = Vector3.zero;
        }
    }
}