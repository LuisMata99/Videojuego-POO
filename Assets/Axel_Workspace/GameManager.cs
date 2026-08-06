using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Patrón Singleton para acceso global
    public static GameManager Instance { get; private set; }

    [Header("UI Menús de Estado")]
    [SerializeField] private GameObject uiEndGameMenu;

    private void Awake()
    {
        // Garantizar instancia única
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Método público invocado por FloodController al llegar al nivel máximo de agua.
    /// </summary>
    public void NivelInundado()
    {
        if (uiEndGameMenu != null)
        {
            uiEndGameMenu.SetActive(true);
        }
        else
        {
            Debug.LogError("GameManager: No se ha asignado el GameObject de UI_EndGameMenu en el Inspector.");
        }

        // Pausar el flujo del tiempo en el motor
        Time.timeScale = 0f;
        Debug.Log("Lógica ejecutada: Nivel Inundado -> Juego Pausado.");
    }
}