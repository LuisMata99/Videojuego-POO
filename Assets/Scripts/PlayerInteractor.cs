using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Configuración del Escáner Espacial")]
    [SerializeField] private Transform puntoOrigenRayo; // Sustituye a interactionPoint
    [SerializeField] private float distanciaInteraccion = 2f;
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
        Vector3 centroEsfera = puntoOrigenRayo.position + (puntoOrigenRayo.forward * (distanciaInteraccion / 2f));
        float radioSuperposicion = distanciaInteraccion / 2f;

        // POR QUÉ: OverlapSphere omite el cálculo de trayectorias direccionales. Detecta geometrías incluso si 
        // el colisionador del jugador ya está penetrando la malla del objetivo, anulando el error 'Inside Collider Ignore'.
        Collider[] impactos = Physics.OverlapSphere(centroEsfera, radioSuperposicion, capaInteractuable);

        if (impactos.Length > 0)
        {
            Collider objetoDetectado = impactos[0];

            // Extracción de componentes (Desacoplamiento)
            FeedbackVisual nuevoFeedbackMat = objetoDetectado.GetComponent<FeedbackVisual>();
            FeedbackVisualInteractuable nuevoFeedbackUI = objetoDetectado.GetComponent<FeedbackVisualInteractuable>();
            IInteractable nuevoInteractuable = objetoDetectado.GetComponent<IInteractable>();

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
            #if UNITY_EDITOR
            Debug.LogWarning("El jugador ya tiene un objeto en mano.");
            #endif
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
        Vector3 centroEsfera = puntoOrigenRayo.position + (puntoOrigenRayo.forward * (distanciaInteraccion / 2f));
        float radioSuperposicion = distanciaInteraccion / 2f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centroEsfera, radioSuperposicion);
    }
}