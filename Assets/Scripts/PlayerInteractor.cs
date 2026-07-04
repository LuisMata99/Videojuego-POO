using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] // Permite modificar las variables privadas desde el inspector sin necesidad de cambiar el código
    private float interactionRadius; // Radio de la interacción

    [SerializeField]
    private Transform interactionPoint;  // Punto de origen de la interacción

    [SerializeField]
    private LayerMask interactableLayer; // Filtra qué objetos se pueden detectar dentro de la zona de interacción

    private PlayerMovement playerMovement; //Almacena la referencia del script de movimiento

    private void Awake()
    {
        // Se obtiene y se guarda la referencia en memoria una sola vez al instanciar el Prefab
        playerMovement = GetComponent<PlayerMovement>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Método que permite que la interacción se lleve a cabo solamentecuando
        {                               // la tecla 'E' esté presionada
            TryInteract();
        }

    }

    private void TryInteract()
    {
        Collider[] colliders =  Physics.OverlapSphere(interactionPoint.position, interactionRadius, interactableLayer);
        /* Se genera en la interacción un volumen de detección que permite optimizar el rendimiento al no calcular colisiones
         * sin sentido*/

        foreach (Collider col in colliders)
        {
            col.TryGetComponent<IInteractable>(out IInteractable interactable);
            interactable.Interact(playerMovement);
            break;
        }
    }
}
