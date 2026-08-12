using UnityEngine;
using System;

public enum TipoAveria
{
    NoAsignada = 0,
    Fisura = 1,
    FugaFuerte = 2
}

[RequireComponent(typeof(AudioSource))] // Obliga a Unity a requerir este componente para evitar NullReferenceExceptions
public class TuberiaBase : MonoBehaviour, IInteractable
{
    public static event Action OnCualquierTuberiaReparada;
    public TipoAveria tipoDeAveria;

    private FeedbackVisualInteractuable feedbackVisual;
    private bool isRepaired = false;

    [Header("Efectos Visuales (VFX)")]
    [SerializeField] private GameObject fxFisura;
    [SerializeField] private GameObject fxRotura;

    [Header("Efectos de Sonido (SFX)")]
    [SerializeField] private AudioSource fuenteAudio;
    [SerializeField] private AudioClip sfxFisura;
    [SerializeField] private AudioClip sfxRotura;
    [SerializeField] private AudioClip sfxReparacionExitosa; // Opcional, feedback para el jugador

    protected virtual void Awake()
    {
        feedbackVisual = GetComponent<FeedbackVisualInteractuable>();

        // Asignación automática por si el Level Designer olvida arrastrar el componente en el Inspector
        if (fuenteAudio == null) fuenteAudio = GetComponent<AudioSource>();

        // Previene fallos silenciosos en el Level Design, garantizando que el objeto inicie con un estado lógico válido
        if (tipoDeAveria == TipoAveria.NoAsignada)
        {
            #if UNITY_EDITOR
            Debug.LogError($"ERROR LÓGICO: La tubería '{gameObject.name}' no tiene una avería asignada.", this);
            #endif
        }
    }

    private void Start()
    {
        // Acoplamiento visual y sonoro al estado inicial
        if (tipoDeAveria == TipoAveria.Fisura)
        {
            if (fxFisura != null) fxFisura.SetActive(true);
            ReproducirSonidoFuga(sfxFisura);
        }
        else if (tipoDeAveria == TipoAveria.FugaFuerte)
        {
            if (fxRotura != null) fxRotura.SetActive(true);
            ReproducirSonidoFuga(sfxRotura);
        }
    }

    // Método encapsulado para manejar la lógica repetitiva del audio
    private void ReproducirSonidoFuga(AudioClip clipFuga)
    {
        if (fuenteAudio != null && clipFuga != null)
        {
            fuenteAudio.clip = clipFuga;
            fuenteAudio.loop = true; // Side effect intencional: El agua debe sonar continuamente
            fuenteAudio.Play();
        }
    }

    public void Interact(PlayerInteractor interactor)
    {
        if (isRepaired)
        {
            return;
        }

        if (interactor.ObjetoEnMano == null)
        {
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
            }
        }
        else
        {
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

        // Side effect visual: Se detiene la emisión de partículas
        if (fxFisura != null) fxFisura.SetActive(false);
        if (fxRotura != null) fxRotura.SetActive(false);

        // Side effect sonoro: Se detiene el loop del agua y reproducimos el sonido de impacto/reparación
        if (fuenteAudio != null)
        {
            fuenteAudio.Stop();

            if (sfxReparacionExitosa != null)
            {
                fuenteAudio.PlayOneShot(sfxReparacionExitosa);
            }
        }
        // Invocación del evento global
        OnCualquierTuberiaReparada?.Invoke();
    }
}