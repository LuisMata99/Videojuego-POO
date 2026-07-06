using UnityEngine;

// La clase implementa IInteractable
public class MockWaterLeak : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteractor jugador)
    {
        // Usamos Debug.LogWarning para que el texto resalte en amarillo en la consola
        Debug.LogWarning("¡Éxito! El sistema detectó la interacción. Jugador: " + jugador.gameObject.name);
    }
}