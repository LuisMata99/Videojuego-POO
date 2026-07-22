using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    
    [SerializeField] private KeyCode dropKey = KeyCode.G;

    [SerializeField] // Permite modificar las variables privadas desde el inspector sin necesidad de cambiar el código
    private float interactionRadius; // Radio de la interacción

    [SerializeField]
    private Transform interactionPoint;  // Punto de origen de la interacción

    [SerializeField]
    private LayerMask interactableLayer; // Filtra qué objetos se pueden detectar dentro de la zona de interacción

    private PlayerMovement playerMovement; //Almacena la referencia del script de movimiento

    // -------------------------------------------------------------------------------
    [SerializeField]
    private Transform puntoDeAgarre; // Variables para implementar la lógica de las herramientas

    public GameObject objetoEnMano { get; private set; }
    // -------------------------------------------------------------------------------

    private void Awake()
    {
        // Se obtiene y se guarda la referencia en memoria una sola vez al instanciar el Prefab
        playerMovement = GetComponent<PlayerMovement>();
    }


    void Update()
    {
        // Evaluamos la interacción usando la variable configurada en el Inspector
        if (Input.GetKeyDown(interactionKey))
        {
            TryInteract();
        }

        // Evaluamos soltar el objeto usando la variable configurada
        if (Input.GetKeyDown(dropKey) && objetoEnMano != null)
        {
            if (objetoEnMano.TryGetComponent<HerramientaBase>(out HerramientaBase herramienta))
            {
                herramienta.Soltar(transform.forward);
                RemoverObjeto();
            }
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

    public void EquiparObjeto(GameObject nuevoObjeto)
    {
        if (objetoEnMano != null)
        {
            Debug.LogWarning("El jugador ya tiene un objeto en mano.");
            return;
        }

        objetoEnMano = nuevoObjeto;

        // Emparentar visualmente
        objetoEnMano.transform.SetParent(puntoDeAgarre);
        objetoEnMano.transform.localPosition = Vector3.zero;
        objetoEnMano.transform.localRotation = Quaternion.identity;
    }

    public void RemoverObjeto()
    {
        objetoEnMano = null;
    }
}
