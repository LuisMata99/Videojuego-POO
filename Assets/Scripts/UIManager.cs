using UnityEngine;
using UnityEngine.UI; // Requerido para manejar componentes de UI nativos
using TMPro;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image uiBarFloodLevel; // Imagen de la barra de agua
    [SerializeField] private TextMeshProUGUI uiTextTimer; // Variable para el temporizador

    // SUSCRIPCIÓN AL EVENTO: Ocurre cuando el objeto se activa
    private void OnEnable()
    {
        FloodManager.OnWaterLevelChanged += ActualizarBarraAgua;
        FloodManager.OnTimeChanged += ActualizarTextoTiempo;

    }

    // DESUSCRIPCIÓN: Crítico para evitar fugas de memoria o NullReferenceExceptions si cambian de escena
    private void OnDisable()
    {
        FloodManager.OnWaterLevelChanged -= ActualizarBarraAgua;
        FloodManager.OnTimeChanged -= ActualizarTextoTiempo;
    }

    // El método que reacciona cuando el cerebro grita que el agua cambió
    private void ActualizarBarraAgua(float porcentaje)
    {
        if (uiBarFloodLevel != null)
        {
            uiBarFloodLevel.fillAmount = porcentaje; // fillAmount requiere un valor entre 0 y 1
        }
    }

    private void ActualizarTextoTiempo(float tiempoRestante)
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);

        uiTextTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}