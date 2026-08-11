using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private float frecuenciaRespiracion = 2f;
    [SerializeField] private float amplitudRespiracion = 0.03f;
    [SerializeField] private float frecuenciaCaminata = 14f;
    [SerializeField] private float amplitudCaminata = 0.08f;

    private Vector3 posicionInicialLocal;

    void Start()
    {
        posicionInicialLocal = transform.localPosition;
    }

    void Update()
    {
        // POR QUÉ: Detectamos el input directamente para evitar la dependencia de .linearVelocity con MovePosition.
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");
        bool estaCaminando = Mathf.Abs(inputHorizontal) > 0.1f || Mathf.Abs(inputVertical) > 0.1f;

        if (estaCaminando)
        {
            float desplazamientoPaso = Mathf.Sin(Time.time * frecuenciaCaminata) * amplitudCaminata;
            transform.localPosition = posicionInicialLocal + new Vector3(0f, Mathf.Abs(desplazamientoPaso), 0f);
        }
        else
        {
            float desplazamientoRespiracion = Mathf.Sin(Time.time * frecuenciaRespiracion) * amplitudRespiracion;
            transform.localPosition = posicionInicialLocal + new Vector3(0f, desplazamientoRespiracion, 0f);
        }
    }
}