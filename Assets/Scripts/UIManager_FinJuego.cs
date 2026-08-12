using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_FinJuego : MonoBehaviour
{
    [Header("Pantallas de Fin de Juego")]
    [SerializeField] private GameObject menuVictoria;
    [SerializeField] private GameObject menuDerrota;

    private void OnEnable()
    {
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
    }

    private void MostrarPantallaDerrota()
    {
        if (menuDerrota != null) menuDerrota.SetActive(true);
    }

    // MÉTODOS PÚBLICOS PARA LOS BOTONES DE LA UI
    public void ReiniciarNivel()
    {
        // Restauramos la escala del tiempo antes de recargar la escena
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirJuego()
    {
        Application.Quit();
    }
}