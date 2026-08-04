using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private KeyCode dropKey = KeyCode.G;

    // Configuración de interacción
    [SerializeField] private float interactionRadius = 0.5f; // Radio de la interacción
    [SerializeField] private float maxInteractionDistance = 2f; // Distancia máxima a la que se puede interactuar
    [SerializeField] private Transform interactionPoint;  // Punto de origen de la interacción
    [SerializeField] private LayerMask interactableLayer; // Filtra qué objetos se pueden detectar dentro de la zona de interacción

    // Configuración de equimamiento
    [SerializeField] private Transform puntoDeAgarre; // Variables para implementar la lógica de las herramientas
    public GameObject objetoEnMano { get; private set; }
    private PlayerMovement playerMovement; //Almacena la referencia del script de movimiento
   

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
        Vector3 direction = transform.forward;
        Vector3 origin = interactionPoint.position - (direction * 0.5f) ;

        if (Physics.SphereCast(origin, interactionRadius, direction, out RaycastHit hitInfo, maxInteractionDistance, interactableLayer))
        {
            if (hitInfo.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact(this);
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

    // Dibuja el cilindro de detección en la vista 'Scene' de Unity para depurar visualmente la orientación y el alcance.
    private void OnDrawGizmos()
    {
        if (interactionPoint == null) return;

        // Vector frontal calculado exactamente igual que en el SphereCast
        Vector3 direction = transform.forward;
        Vector3 origin = interactionPoint.position - (direction * 0.5f);

        // Pintamos el rayo de rojo
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, direction * maxInteractionDistance);

        // Pintamos la esfera en el punto final para ver el grosor real del impacto
        Gizmos.DrawWireSphere(origin + (direction * maxInteractionDistance), interactionRadius);
    }
}
