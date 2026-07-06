using UnityEngine;

public class DispensadorDeHerramientas : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject prefabHerramienta;

    public void Interact(PlayerInteractor interactor)
    {
        // Guard Clause: Si el jugador ya tiene algo en las manos, no se le da nada.
        if (interactor.objetoEnMano != null)
        {
            Debug.Log("Ya tienes las manos ocupadas.");
            return;
        }

        // 1. Instanciación: Creación de copia exacta del prefab en la posición actual del dispensador
        GameObject nuevaHerramienta = Instantiate(prefabHerramienta, transform.position, Quaternion.identity);

        // 2. Encadenamiento de Eventos: Forzamos a la nueva herramienta a ejecutar su propia lógica de recogerse
        // Como HerramientaBase implementa IInteractable, se llama a su contrato.
        if (nuevaHerramienta.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactable.Interact(interactor);
        }
    }
}