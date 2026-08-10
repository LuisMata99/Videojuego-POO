using UnityEngine;
using System;

public enum TipoAveria
{
    NoAsignada = 0,
    Fisura = 1,
    FugaFuerte = 2
}

public class TuberiaBase : MonoBehaviour, IInteractable
{
    public static event Action OnCualquierTuberiaReparada;
    public TipoAveria tipoDeAveria;

    private FeedbackVisualInteractuable feedbackVisual;
    private bool isRepaired = false;

    [Header("Efectos Visuales (VFX)")]
    [SerializeField] private GameObject fxFisura;
    [SerializeField] private GameObject fxRotura;

    protected virtual void Awake()
    {
        feedbackVisual = GetComponent<FeedbackVisualInteractuable>();

        // Previene fallos silenciosos en el Level Design, garantizando que el objeto inicie con un estado lógico válido
        if (tipoDeAveria == TipoAveria.NoAsignada)
        {
            Debug.LogError($"ERROR LÓGICO: La tubería '{gameObject.name}' no tiene una avería asignada.", this);
        }
    }

    private void Start()
    {
        // Acoplamiento visual al estado inicial para asegurar coherencia entre la variable lógica y el renderizado en escena
        if (tipoDeAveria == TipoAveria.Fisura && fxFisura != null)
        {
            fxFisura.SetActive(true);
        }
        else if (tipoDeAveria == TipoAveria.FugaFuerte && fxRotura != null)
        {
            fxRotura.SetActive(true);
        }
    }

    public void Interact(PlayerInteractor interactor)
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

        // Side effect visual: Detenemos la emisión de partículas directamente desactivando los contenedores de agua
        if (fxFisura != null) fxFisura.SetActive(false);
        if (fxRotura != null) fxRotura.SetActive(false);

        Debug.Log("¡Tubería reparada exitosamente!");

        // Invocación del evento global para que el FloodManager registre el avance de la partida
        OnCualquierTuberiaReparada?.Invoke();
    }
}