using UnityEngine;

// Se definen los tipos de averías que puede tener una tubería
public enum TipoAveria
{
    NoAsignada = 0,
    Fisura = 1,     // Requiere Cinta Adhesiva
    FugaFuerte = 2 // Requiere Llave Inglesa
    
}

public class TuberiaBase : MonoBehaviour, IInteractable
{
    private FeedbackVisualInteractuable feedbackVisual;

    protected virtual void Awake()
    {
        // 1. Obtención de referencias
        feedbackVisual = GetComponent<FeedbackVisualInteractuable>();

        // 2. Cláusula de guarda para la integridad del Level Design
        if (tipoDeAveria == TipoAveria.NoAsignada)
        {
            Debug.LogError($"ERROR LÓGICO: La tubería '{gameObject.name}' no tiene una avería asignada. Selecciona una en el Inspector.", this);
        }
    }

    public TipoAveria tipoDeAveria;

    private bool isRepaired = false;
    [SerializeField] private MeshRenderer renderizadoVisual; // Referencia al componente MeshRender (Encargado de hacer visible el objeto en escena)
    

    public void Interact(PlayerInteractor interactor) // Implementación del método para interactuar (Polimorfismo)
    {
        if (isRepaired)
        {
            Debug.Log("La tubería no necesita reparación");
            return;
        }

        if (interactor.ObjetoEnMano == null)
        {
            Debug.Log("Necesitas una herramienta para reparar esto.");
            return;
        }

        if (interactor.ObjetoEnMano.TryGetComponent<HerramientaBase>(out HerramientaBase herramienta))
        {
            if (herramienta.PuedeReparar(tipoDeAveria))
            {
                RepararTuberia();
            }
            else
            {
                Debug.Log("Herramienta incorrecta para este tipo de avería.");
            }
        }
        else
        {
            Debug.Log("El objeto que sostienes no es una herramienta válida para reparaciones.");
        }

    }
    public virtual void Enfocar()
    {
        if (feedbackVisual != null) feedbackVisual.Encender();
    }

    public virtual void Desenfocar()
    {
        if (feedbackVisual != null) feedbackVisual.Apagar();
    }
    private void RepararTuberia()
    {
        isRepaired = true;
        renderizadoVisual.material.color = Color.blue;
        Debug.Log("¡Tubería reparada exitosamente!");
    }
}
