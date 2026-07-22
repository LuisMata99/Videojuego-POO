using UnityEngine;

public class MockWaterLeak : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteractor jugador)
    {
        Debug.LogWarning("¡Éxito! El sistema detectó la interacción. Jugador: " + jugador.gameObject.name);
    }
}