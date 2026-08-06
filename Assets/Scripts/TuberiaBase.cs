using UnityEngine;

// Se definen los tipos de averías que puede tener una tubería
public enum TipoAveria
{
    FugaFuerte, // Requiere Llave Inglesa
    Fisura      // Requiere Cinta Adhesiva
}

public class TuberiaBase : MonoBehaviour, IInteractable
{
    private FeedbackVisualInteractuable feedbackVisual;

    protected virtual void Awake()
    {
        feedbackVisual = GetComponent < FeedbackVisualInteractuable >();
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

        if (interactor.objetoEnMano == null)
        {
            Debug.Log("Necesitas una herramienta para reparar esto.");
            return;
        }

        if (interactor.objetoEnMano.TryGetComponent<HerramientaBase>(out HerramientaBase herramienta))
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
