using UnityEngine;

public class MockWaterLeak : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteractor jugador)
    {
        Debug.LogWarning("¡Éxito! El sistema detectó la interacción. Jugador: " + jugador.gameObject.name);
    }

    // Cumplimiento estricto del contrato
    public void Enfocar()
    {
        // Al ser un Mock, usamos Debug.Log para comprobar que tu SphereCast
        // funciona correctamente sin necesidad de la UI de Axel.
        Debug.Log("Mock: El radar del jugador me ha detectado (Enfocado).");
    }

    public void Desenfocar()
    {
        Debug.Log("Mock: El radar del jugador me perdió de vista (Desenfocado).");
    }
}