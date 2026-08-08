using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Configuración del Escáner Espacial")]
    [SerializeField] private Transform puntoOrigenRayo; // Sustituye a interactionPoint
    [SerializeField] private float distanciaInteraccion = 2f;
    [SerializeField] private float radioEsfera = 0.5f;
    [SerializeField] private LayerMask capaInteractuable;

    [Header("Configuración de Equipamiento")]
    [SerializeField] private Transform puntoDeAgarre;
    [SerializeField] private KeyCode teclaInteraccion = KeyCode.E;
    [SerializeField] private KeyCode teclaSoltar = KeyCode.G;

    // Propiedad pública que permite a otros scripts leer qué sostiene el jugador, pero no modificarlo
    public GameObject ObjetoEnMano { get; private set; }

    // Caché de memoria para la detección continua
    private FeedbackVisual feedbackMaterialActual;
    private FeedbackVisualInteractuable feedbackUIActual;
    private IInteractable interactuableActual;

    private void Update()
    {
        EscanearEntorno();
        ProcesarInput();
    }

    /// <summary>
    /// Dispara un SphereCast para detectar objetos en la capa Interactuable.
    /// Extrae los componentes de interfaz, lógica y feedback visual para procesarlos.
    /// </summary>
    private void EscanearEntorno()
    {
        Vector3 origen = puntoOrigenRayo.position - (puntoOrigenRayo.forward * 0.5f);
        bool impacto = Physics.SphereCast(
            origen,
            radioEsfera,
            puntoOrigenRayo.forward,
            out RaycastHit hitInfo,
            distanciaInteraccion,
            capaInteractuable
        );

        if (impacto)
        {
            // Extracción de componentes (Desacoplamiento)
            FeedbackVisual nuevoFeedbackMat = hitInfo.collider.GetComponent<FeedbackVisual>();
            FeedbackVisualInteractuable nuevoFeedbackUI = hitInfo.collider.GetComponent<FeedbackVisualInteractuable>();
            IInteractable nuevoInteractuable = hitInfo.collider.GetComponent<IInteractable>();

            // Si el jugador mira un objeto distinto al del frame anterior
            if (nuevoInteractuable != interactuableActual)
            {
                LimpiarEnfoqueActual();

                // Guardado en caché
                feedbackMaterialActual = nuevoFeedbackMat;
                feedbackUIActual = nuevoFeedbackUI;
                interactuableActual = nuevoInteractuable;

                // Encendido de sistemas visuales
                if (feedbackMaterialActual != null) feedbackMaterialActual.Resaltar();
                if (feedbackUIActual != null) feedbackUIActual.Encender();
            }
        }
        else
        {
            LimpiarEnfoqueActual();
        }
    }

    /// <summary>
    /// Apaga de forma segura todos los sistemas visuales del último objeto mirado
    /// y libera la memoria en caché.
    /// </summary>
    private void LimpiarEnfoqueActual()
    {
        if (feedbackMaterialActual != null) feedbackMaterialActual.Restaurar();
        if (feedbackUIActual != null) feedbackUIActual.Apagar();

        feedbackMaterialActual = null;
        feedbackUIActual = null;
        interactuableActual = null;
    }

    /// <summary>
    /// Escucha el teclado para ejecutar los contratos lógicos de interacción (Recoger/Reparar)
    /// o soltar la herramienta equipada aplicando físicas.
    /// </summary>
    private void ProcesarInput()
    {
        // Acción de Interactuar
        if (Input.GetKeyDown(teclaInteraccion) && interactuableActual != null)
        {
            interactuableActual.Interact(this);
        }

        // Acción de Soltar
        if (Input.GetKeyDown(teclaSoltar) && ObjetoEnMano != null)
        {
            if (ObjetoEnMano.TryGetComponent<HerramientaBase>(out HerramientaBase herramienta))
            {
                herramienta.Soltar(transform.forward);
                RemoverObjeto();
            }
        }
    }

    /// <summary>
    /// Ancla espacialmente un objeto a la mano del jugador y lo guarda en memoria.
    /// Se invoca desde los scripts de las herramientas (ej. LlaveInglesa).
    /// </summary>
    public void EquiparObjeto(GameObject nuevoObjeto)
    {
        if (ObjetoEnMano != null)
        {
            Debug.LogWarning("El jugador ya tiene un objeto en mano.");
            return;
        }

        ObjetoEnMano = nuevoObjeto;
        ObjetoEnMano.transform.SetParent(puntoDeAgarre);
        ObjetoEnMano.transform.localPosition = Vector3.zero;
        ObjetoEnMano.transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// Elimina la referencia de la herramienta sostenida.
    /// </summary>
    public void RemoverObjeto()
    {
        ObjetoEnMano = null;
    }

    // Dibujo de telemetría para el Editor de Unity
    private void OnDrawGizmos()
    {
        if (puntoOrigenRayo == null) return;
        Vector3 origen = puntoOrigenRayo.position - (puntoOrigenRayo.forward * 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origen, puntoOrigenRayo.forward * distanciaInteraccion);
        Gizmos.DrawWireSphere(origen + (puntoOrigenRayo.forward * distanciaInteraccion), radioEsfera);
    }
}