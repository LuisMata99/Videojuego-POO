using UnityEngine;

public class UIManager_FinJuego : MonoBehaviour
{
    [Header("Pantallas de Fin de Juego")]
    [SerializeField] private GameObject menuVictoria;
    [SerializeField] private GameObject menuDerrota;

    private void OnEnable()
    {
        // Suscripción a los eventos del motor de Luis
        FloodManager.OnVictoria += MostrarPantallaVictoria;
        FloodManager.OnDerrota += MostrarPantallaDerrota;
    }

    private void OnDisable()
    {
        FloodManager.OnVictoria -= MostrarPantallaVictoria;
        FloodManager.OnDerrota -= MostrarPantallaDerrota;
    }

    private void MostrarPantallaVictoria()
    {
        if (menuVictoria != null) menuVictoria.SetActive(true);
        Debug.Log("UI: Pantalla de Victoria encendida.");
    }

    private void MostrarPantallaDerrota()
    {
        if (menuDerrota != null) menuDerrota.SetActive(true);
        Debug.Log("UI: Pantalla de Derrota encendida.");
    }
}