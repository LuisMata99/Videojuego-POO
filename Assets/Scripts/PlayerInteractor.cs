using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] // Permite modificar las variables privadas desde el inspector sin necesidad de cambiar el código
    private float interactionRadius; // Radio de la interacción

    [SerializeField]
    private Transform interactionPoint;  // Punto de origen de la interacción

    [SerializeField]
    private LayerMask interactableLayer; // Filtra qué objetos se pueden detectar dentro de la zona de interacción

    private PlayerMovement playerMovement; //Almacena la referencia del script de movimiento

    // -------------------------------------------------------------------------------
    public Transform puntoDeAgarre; // Variables para implementar la lógica de las herramientas
    public GameObject objetoEnMano;
    // -------------------------------------------------------------------------------

    private void Awake()
    {
        // Se obtiene y se guarda la referencia en memoria una sola vez al instanciar el Prefab
        playerMovement = GetComponent<PlayerMovement>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Evalúa que la interacción se lleve a cabo si
        {                               // la tecla 'E' está presionada
            TryInteract();
        }

        if (Input.GetKeyDown(KeyCode.G) && objetoEnMano != null) // Si se presiona la G y el jugador tiene un objeto en mano
        {
            objetoEnMano.GetComponent<HerramientaBase>().Soltar(transform.forward); // Se llama al método soltar
            objetoEnMano = null; // Se actualiza para evitar errores como que el jugador no pueda recoger un objeto si sus manos están vacías
        }

    }

    private void TryInteract()
    {
        Collider[] colliders =  Physics.OverlapSphere(interactionPoint.position, interactionRadius, interactableLayer);
        /* Se genera en la interacción un volumen de detección que permite optimizar el rendimiento al no calcular colisiones
         * sin sentido*/

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact(this);
                break;
            }
            
        }
    }
}
