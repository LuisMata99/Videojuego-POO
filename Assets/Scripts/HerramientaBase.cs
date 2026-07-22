using UnityEngine;

public abstract class HerramientaBase : MonoBehaviour, IInteractable

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

        interactor.EquiparObjeto(this.gameObject); // Manda a llamar al método para equipar un objeto

        // Anulación de Físicas: Se evitan colisiones locas.
        rb.isKinematic = true; // Unity deja de calcular la gravedad y colisiones
        col.enabled = false; // Vuelve a la herramienta un "fantasma" para no colisionar con el jugador
    }

    public void Soltar(Vector3 direccionDelJugador)
    {
        transform.SetParent(null); // Se disvincula el objeto del padre (manos)

        // Se reactivan las físicas
        rb.isKinematic = false;
        col.enabled = true;

        rb.AddForce(direccionDelJugador * 3f, ForceMode.Impulse);
    }

    public abstract bool PuedeReparar(TipoAveria averia);
}
