using UnityEngine;

public class MesaMostrador : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform puntoDeMesa; // Arrastra aquí el objeto hijo "PuntoDeMesa"

    // Es privada porque la controla el código, el inspector no debe tocarla.
    private GameObject herramientaAlmacenada;

    public void Interact(PlayerInteractor interactor)
    {

        bool jugadorOcupado = interactor.objetoEnMano != null;
        bool mesaOcupada = herramientaAlmacenada != null;

        // Si el jugador no tiene nada en las manos y la mesa no tiene nada guardado
        if (!jugadorOcupado && !mesaOcupada)
        {
            Debug.Log("Ninguno de los dos tiene nada para interactuar.");
            return;
        }

        // Si el jugador tiene las manos ocupadas y la mesa está llena
        if (jugadorOcupado && mesaOcupada)
        {
            Debug.Log("La mesa está llena. No puedes guardar la herramienta.");
            return;
        }

        // 3. Si el jugador tiene las manos ocupadas y la mesa no está llena
        if (jugadorOcupado && !mesaOcupada)
        {
            herramientaAlmacenada = interactor.objetoEnMano;
            interactor.RemoverObjeto();

            herramientaAlmacenada.transform.SetParent(puntoDeMesa);
            herramientaAlmacenada.transform.localPosition = Vector3.zero;
            herramientaAlmacenada.transform.localRotation = Quaternion.identity;

            return;
        }

        // Si el jugador tiene las manos vacías y la mesa está llena
        if (herramientaAlmacenada.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactable.Interact(interactor);
        }
        herramientaAlmacenada = null;
    }
}