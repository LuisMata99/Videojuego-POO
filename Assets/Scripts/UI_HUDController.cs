using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HUDController : MonoBehaviour
{
    [SerializeField] private Image barraAgua;
    [SerializeField] private TextMeshProUGUI textoTemporizador;

    private void OnEnable()
    {
        FloodManager.OnWaterLevelChanged += ActualizarBarraAgua;
        FloodManager.OnTimeChanged += ActualizarTemporizador;
    }

    private void OnDisable()
    {
        FloodManager.OnWaterLevelChanged -= ActualizarBarraAgua;
        FloodManager.OnTimeChanged -= ActualizarTemporizador;
    }

    private void ActualizarBarraAgua(float porcentajeNormalizado)
    {
        if (barraAgua != null)
        {
            barraAgua.fillAmount = porcentajeNormalizado;
        }
    }

    private void ActualizarTemporizador(float tiempoRestante)
    {
        if (textoTemporizador != null)
        {
            int minutos = Mathf.FloorToInt(tiempoRestante / 60);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60);
            textoTemporizador.text = $"{minutos:00}:{segundos:00}";
        }
    }
}