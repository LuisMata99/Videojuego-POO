using UnityEngine;
using System; // Requerido para usar Action

public class FloodManager : MonoBehaviour
{
    // EVENTO: Avisa al resto del juego que el agua subió
    public static event Action<float> OnWaterLevelChanged;
    
    // EVENTO: Desacopla el sistema lógico del tiempo de los sistemas de renderizado gráfico (UI)
    public static event Action<float> OnTimeChanged;

    [SerializeField] private float nivelMaximoAgua = 100f;
    [SerializeField] private float velocidadInundacion = 5f; // Cuánta agua sube por segundo
    private float nivelActualAgua = 0f;

    [SerializeField] private float tiempoMaximoNivel = 180f; // Tiempo total de la partida en segundos
    private float tiempoRestante;


    void Start()
    {
        // Inicialización del estado base para evitar arrancar en 0 o mantener valores residuales en memoria si la escena recarga
        tiempoRestante = tiempoMaximoNivel;
    }

    void Update()
    {
        if (nivelActualAgua < nivelMaximoAgua)
        {
            // Aumenta el nivel de agua basado en el tiempo, no en los frames
            nivelActualAgua += velocidadInundacion * Time.deltaTime;

            // Clampeamos para que no pase del máximo
            nivelActualAgua = Mathf.Clamp(nivelActualAgua, 0, nivelMaximoAgua);

            // Cálculo del porcentaje (de 0.0 a 1.0) para que la UI lo entienda fácilmente
            float porcentajeAgua = nivelActualAgua / nivelMaximoAgua;

            // DISPARO DEL EVENTO: El operador '?.' evita crasheos si nadie está escuchando
            OnWaterLevelChanged?.Invoke(porcentajeAgua);
        }

        if (tiempoRestante > 0)
        {
            // Time.deltaTime garantiza una sustracción en segundos reales, ignorando las fluctuaciones de FPS del hardware
            tiempoRestante -= Time.deltaTime;

            // Evitar desbordamientos negativos y renderizados erróneos en la interfaz (ej. -00:01)
            tiempoRestante = Mathf.Max(0, tiempoRestante);

            OnTimeChanged?.Invoke(tiempoRestante);
        }
    }

    // Método que se llama más adelante cuando el jugador repare una tubería
    public void ReducirAgua(float cantidad)
    {
        nivelActualAgua -= cantidad;
        nivelActualAgua = Mathf.Clamp(nivelActualAgua, 0, nivelMaximoAgua);
        OnWaterLevelChanged?.Invoke(nivelActualAgua / nivelMaximoAgua);
    }
}