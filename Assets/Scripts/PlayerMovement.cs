using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;

    // Variable para controlar qué tan rápido gira el personaje
    [SerializeField] private float velocidadRotacion = 15f;

    private Rigidbody rb;
    private Vector3 direccionMovimiento;
    private Transform transformCamara;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transformCamara = Camera.main.transform; // Caché inicial
    }

    void Update()
    {
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");

        Vector3 forwardCam = transformCamara.forward;
        Vector3 rightCam = transformCamara.right;

        // Se aplanan los vectores
        forwardCam.y = 0f;
        rightCam.y = 0f;

        forwardCam.Normalize();
        rightCam.Normalize();

        direccionMovimiento = (forwardCam * inputVertical + rightCam * inputHorizontal).normalized;
    }

    void FixedUpdate()
    {
        // 1. Movimiento
        rb.MovePosition(rb.position + direccionMovimiento * velocidad * Time.fixedDeltaTime);

        // 2. Rotación
        if (direccionMovimiento != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);

            // POR QUÉ: Quaternion.Slerp suaviza la transición entre la rotación actual y el objetivo.
            Quaternion rotacionSuavizada = Quaternion.Slerp(rb.rotation, rotacionObjetivo, velocidadRotacion * Time.fixedDeltaTime);

            rb.MoveRotation(rotacionSuavizada);
        }
    }
}