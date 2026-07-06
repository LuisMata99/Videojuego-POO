using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float velocidad = 5f;

    private Rigidbody _rb;

    private Vector3 _direccionMovimiento;

    private Transform _cameraTransform;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        float calcularHorizontal = Input.GetAxisRaw("Horizontal");
        float calcularVertical = Input.GetAxisRaw("Vertical");

        Vector3 forwardCam = Camera.main.transform.forward;
        Vector3 rightCam = Camera.main.transform.right;

        forwardCam.y = 0f;
        rightCam.y = 0f;

        forwardCam.Normalize();
        rightCam.Normalize();

        _direccionMovimiento = (forwardCam * calcularVertical + rightCam * calcularHorizontal).normalized;
    }

    void FixedUpdate()
    {
        _rb.MovePosition(transform.position + _direccionMovimiento * velocidad * Time.fixedDeltaTime);

        if (_direccionMovimiento != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_direccionMovimiento);
            _rb.MoveRotation(targetRotation);
        }
    }
}
