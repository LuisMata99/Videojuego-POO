using UnityEngine;
using UnityEngine.UI; // Necesario para interactuar con el Slider del Canvas

public class FloodController : MonoBehaviour
{
    // Definimos los estados posibles de la tubería
    public enum PipeState { Intact, Fissured, Broken }

    [Header("UI Canvas - Barra de Inundación")]
    public Slider floodBar; // Tu Slider normalizado de 0 a 1
    public float floodIncreaseRate = 0.05f; // Velocidad de llenado por segundo

    [Header("Umbrales Matemáticos")]
    [Range(0f, 1f)] public float thresholdFissure = 0.5f; // 50%
    [Range(0f, 1f)] public float thresholdBroken = 0.9f;  // 90%

    [Header("Referencias del Prefab Interactable_Pipe")]
    public MeshRenderer pipeRenderer;
    public Material matIntact; // El material original
    public Material matWet;    // Mat_Pipe_Wet
    public Material matCritical; // Mat_Pipe_Critical

    public GameObject fxFissureWater;
    public GameObject fxBrokenGeyser;

    private float currentFloodValue = 0f;
    private PipeState currentState = PipeState.Intact;

    void Start()
    {
        // Inicializamos la barra y el estado
        currentFloodValue = 0f;
        floodBar.value = currentFloodValue;
        SetPipeState(PipeState.Intact);
    }

    void Update()
    {
        // 1. Aumentar el nivel de inundación gradualmente usando cálculo de tiempo delta
        currentFloodValue += floodIncreaseRate * Time.deltaTime;

        // Clampeamos el valor para que matemáticamente no pase de 1
        currentFloodValue = Mathf.Clamp01(currentFloodValue);
        floodBar.value = currentFloodValue;

        // 2. Evaluar los umbrales para cambiar de estado
        CheckFloodThresholds();
    }

    void CheckFloodThresholds()
    {
        // Lógica de transición de estados
        if (currentFloodValue >= thresholdBroken && currentState != PipeState.Broken)
        {
            SetPipeState(PipeState.Broken);
        }
        else if (currentFloodValue >= thresholdFissure && currentFloodValue < thresholdBroken && currentState != PipeState.Fissured)
        {
            SetPipeState(PipeState.Fissured);
        }
    }

    // Función principal para aplicar los cambios visuales
    public void SetPipeState(PipeState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case PipeState.Intact:
                pipeRenderer.material = matIntact;
                fxFissureWater.SetActive(false);
                fxBrokenGeyser.SetActive(false);
                break;

            case PipeState.Fissured:
                pipeRenderer.material = matWet;
                fxFissureWater.SetActive(true);
                fxBrokenGeyser.SetActive(false);
                break;

            case PipeState.Broken:
                pipeRenderer.material = matCritical;
                fxFissureWater.SetActive(false);
                fxBrokenGeyser.SetActive(true);
                break;
        }
    }
}