using UnityEngine;

public class HerramientaBase : MonoBehaviour, IInteractable

{
    private Rigidbody rb;
    private Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Llamada a las referencias 
        col = GetComponent<Collider>();
    }

    public void Interact(PlayerInteractor interactor) // Implementación del método para interactuar (Polimorfismo)
    {
        // Guard Clause: Si el jugador ya tiene algo, no se hace nada
        if (interactor.objetoEnMano != null) return;

        interactor.objetoEnMano = this.gameObject; // Asigna el objeto a recoger a la variable del jugador

        // Anulación de Físicas: Se evitan colisiones locas.
        rb.isKinematic = true; // Unity deja de calcular la gravedad y colisiones
        col.enabled = false; // Vuelve a la herramienta un "fantasma" para no colisionar con el jugador

        // Jerarquía y Matemáticas: Se pega a la mano y se centra.
        transform.SetParent(interactor.puntoDeAgarre);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Soltar(Vector3 direccionDelJugador)
    {
        transform.SetParent(null); // Se disvincula el objeto del padre (manos)

        // Se reactivan las físicas
        rb.isKinematic = false;
        col.enabled = true;

        rb.AddForce(direccionDelJugador * 3f, ForceMode.Impulse);
    }
}
