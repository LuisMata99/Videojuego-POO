using UnityEngine;
using System;

public class FloodManager : MonoBehaviour
{
    // EVENTOS DE ESTADO (UI y otros sistemas escucharán esto)
    public static event Action<float> OnWaterLevelChanged;
    public static event Action<float> OnTimeChanged;
    public static event Action OnVictoria;
    public static event Action OnDerrota;

    [Header("Mecánicas de Inundación")]
    [SerializeField] private float nivelMaximoAgua = 100f;
    [SerializeField] private float velocidadInundacion = 5f;
    private float nivelActualAgua = 0f;

    [Header("Temporizador")]
    [SerializeField] private float tiempoMaximoNivel = 180f;
    private float tiempoRestante;

    [Header("Condición de Victoria")]
    [SerializeField] private int tuberiasTotalesNivel;
    private int tuberiasReparadas = 0;

    // Bandera de control de estado para evitar que los eventos de fin de juego se disparen en cada frame del Update
    private bool juegoTerminado = false;

    void Start()
    {
        tiempoRestante = tiempoMaximoNivel;
        // Nos aseguramos de que el tiempo corra normalmente al reiniciar la escena
        Time.timeScale = 1f;

        TuberiaBase[] tuberiasEnEscena = FindObjectsByType<TuberiaBase>(FindObjectsInactive.Exclude);
        tuberiasTotalesNivel = tuberiasEnEscena.Length;

        if (tuberiasTotalesNivel == 0)
        {
            #if UNITY_EDITOR
            Debug.LogWarning("FloodManager: Memoria vacía. No se detectaron objetos TuberiaBase en la jerarquía.");
            #endif
        }
    }

    // PATRÓN OBSERVER: Suscripción y Desuscripción
    private void OnEnable()
    {
        TuberiaBase.OnCualquierTuberiaReparada += RegistrarTuberiaReparada;
    }

    private void OnDisable()
    {
        TuberiaBase.OnCualquierTuberiaReparada -= RegistrarTuberiaReparada;
    }

    void Update()
    {
        // Early Return: Si el juego ya terminó, abortamos el cálculo matemático para ahorrar CPU
        if (juegoTerminado) return;

        ManejarInundacion();
        ManejarTiempo();
        ValidarCondicionesDeDerrota();
    }

    private void ManejarInundacion()
    {
        nivelActualAgua += velocidadInundacion * Time.deltaTime;
        nivelActualAgua = Mathf.Clamp(nivelActualAgua, 0, nivelMaximoAgua);
        OnWaterLevelChanged?.Invoke(nivelActualAgua / nivelMaximoAgua);
    }

    private void ManejarTiempo()
    {
        tiempoRestante -= Time.deltaTime;
        tiempoRestante = Mathf.Max(0, tiempoRestante);
        OnTimeChanged?.Invoke(tiempoRestante);
    }

    // EVALUACIÓN DE DERROTA
    private void ValidarCondicionesDeDerrota()
    {
        if (nivelActualAgua >= nivelMaximoAgua || tiempoRestante <= 0)
        {
            EjecutarFinDeJuego(victoria: false);
        }
    }

    // EVALUACIÓN DE VICTORIA (Invocado por el evento estático de TuberiaBase)
    private void RegistrarTuberiaReparada()
    {
        if (juegoTerminado) return;

        tuberiasReparadas++;

        // Reducimos un porcentaje del agua como recompensa al reparar (opcional)
        ReducirAgua(15f);

        if (tuberiasReparadas >= tuberiasTotalesNivel)
        {
            EjecutarFinDeJuego(victoria: true);
        }
    }

    public void ReducirAgua(float cantidad)
    {
        nivelActualAgua -= cantidad;
        nivelActualAgua = Mathf.Clamp(nivelActualAgua, 0, nivelMaximoAgua);
        OnWaterLevelChanged?.Invoke(nivelActualAgua / nivelMaximoAgua);
    }

    // GESTOR CENTRAL DE ESTADO
    private void EjecutarFinDeJuego(bool victoria)
    {
        juegoTerminado = true;

        // Efecto secundario (Side effect): Congelamos el motor de físicas y deltaTime globalmente
        Time.timeScale = 0f;

        if (victoria)
        {
            OnVictoria?.Invoke();
        }
        else
        {
            OnDerrota?.Invoke();
        }
    }
}