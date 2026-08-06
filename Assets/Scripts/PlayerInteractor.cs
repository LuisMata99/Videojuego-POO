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

    private IInteractable _interactableEnfocadoActual; /* Variable para no llamar Enfocar() 60 veces por segundo, recuerda lo que se tenía
                                                        * a la vista el frame anterior*/


    private void Awake()
    {
        // Se obtiene y se guarda la referencia en memoria una sola vez al instanciar el Prefab
        playerMovement = GetComponent<PlayerMovement>();
    }


    void Update()
    {
        // 1. Detección continua (El "Qué")
        ActualizarFocoVisual();

        // 2. Escucha de eventos del teclado (El "Cuándo")
        ProcesarEntradaUsuario();
    }

    private void ProcesarEntradaUsuario()
    {
        // Evaluación de la interacción usando la variable configurada en el Inspector
        if (Input.GetKeyDown(interactionKey))
        {
            TryInteract();
        }

        // Evaluar soltar el objeto usando la variable configurada
        if (Input.GetKeyDown(dropKey) && objetoEnMano != null)
        {
            if (objetoEnMano.TryGetComponent<HerramientaBase>(out HerramientaBase herramienta))
            {
                herramienta.Soltar(transform.forward);
                RemoverObjeto();
            }
        }
    }

    private void ActualizarFocoVisual()
    {
        // Calcular un punto de origen desplazado hacia atrás desde un objeto de interacción, creando una dirección frontal.
        Vector3 direction = transform.forward;
        Vector3 origin = interactionPoint.position - (direction * 0.5f);

        // Proyección de las físicas
        bool impacto = Physics.SphereCast(
            origin,
            interactionRadius,
            direction,
            out RaycastHit hitInfo,
            maxInteractionDistance,
            interactableLayer);

        if (impacto)
        {
            // Valida que el objeto tenga el contrato correcto
            if (hitInfo.collider.TryGetComponent<IInteractable>(out IInteractable interactableDetectado))
            {
                // Si es un objeto distinto al que se estaba mirando en el frame anterior
                if (interactableDetectado != _interactableEnfocadoActual)
                {
                    // Se apaga el anterior, se guarda el nuevo, y lo se enciende
                    _interactableEnfocadoActual?.Desenfocar();
                    _interactableEnfocadoActual = interactableDetectado;
                    _interactableEnfocadoActual.Enfocar();
                }
            }
        }
        else
        {
            // 3. Limpieza de estado si el rayo no impacta nada
            if (_interactableEnfocadoActual != null)
            {
                _interactableEnfocadoActual.Desenfocar();
                _interactableEnfocadoActual = null;
            }
        }
    }

    private void TryInteract()
    {
        if (_interactableEnfocadoActual != null)
        {
            _interactableEnfocadoActual.Interact(this);
        }
    }

    public void EquiparObjeto(GameObject nuevoObjeto)
    {
        // Valida que el jugador tenga las manos vacías
        if (objetoEnMano != null)
        {
            Debug.LogWarning("El jugador ya tiene un objeto en mano.");
            return;
        }

        // 1. Asigna la variable en memoria para romper la cláusula de guarda
        objetoEnMano = nuevoObjeto;

        // 2. Ejecuta la lógica visual y espacial en la jerarquía de Unity
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

        // Se pinta el rayo de rojo
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, direction * maxInteractionDistance);

        // Se pinta la esfera en el punto final para ver el grosor real del impacto
        Gizmos.DrawWireSphere(origin + (direction * maxInteractionDistance), interactionRadius);
    }
}
