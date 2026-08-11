using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    // Referencia al componente Animator de tu personaje
    public Animator anim;

    void Update()
    {
        // Detectamos el input
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");

        // Si hay input en cualquier dirección, está caminando
        bool estaCaminando = Mathf.Abs(inputHorizontal) > 0.1f || Mathf.Abs(inputVertical) > 0.1f;

        // Le mandamos la señal a la ventana de Animator
        if (anim != null)
        {
            anim.SetBool("isWalking", estaCaminando);
        }
    }
}