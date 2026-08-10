using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UI_BarraInundacion : MonoBehaviour
{
    private Slider barraInundacion;

    private void Awake()
    {
        barraInundacion = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        // Escucha el cambio de agua emitido por el FloodManager
        FloodManager.OnWaterLevelChanged += ActualizarBarra;
    }

    private void OnDisable()
    {
        FloodManager.OnWaterLevelChanged -= ActualizarBarra;
    }

    private void ActualizarBarra(float porcentajeNivel)
    {
        if (barraInundacion != null)
        {
            // Actualiza visualmente el relleno
            barraInundacion.value = porcentajeNivel;
        }
    }
}