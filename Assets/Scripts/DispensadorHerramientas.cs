using UnityEngine;

public class DispensadorDeHerramientas : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject prefabHerramienta;

    private FeedbackVisualInteractuable feedbackVisual;

    private void Awake()
    {
        feedbackVisual = GetComponent<FeedbackVisualInteractuable>();
    }

    public void Interact(PlayerInteractor interactor)
    {
        // Guard Clause: Aborta la ejecución para evitar un UnassignedReferenceException en la instanciación si el diseñador de niveles omitió la dependencia
        if (prefabHerramienta == null)
        {
            Debug.LogWarning($"FALTA DE DISEÑO DE NIVEL: La mesa '{gameObject.name}' no tiene una herramienta asignada para dispensar. Revisa el Inspector.", this);
            return;
        }

        // Guard Clause: Bloquea la creación de memoria basura evitando instanciar objetos si el jugador no puede recibirlos
        if (interactor.ObjetoEnMano != null)
        {
            Debug.Log("Ya tienes las manos ocupadas.");
            return;
        }

        GameObject nuevaHerramienta = Instantiate(prefabHerramienta, transform.position, Quaternion.identity);

        // Se aprovecha el contrato de la interfaz IInteractable para delegar la lógica de "recoger" al propio objeto instanciado, manteniendo el acoplamiento bajo
        if (nuevaHerramienta.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactable.Interact(interactor);
        }
    }
    public void Enfocar()
    {
        if (feedbackVisual != null) feedbackVisual.Encender();
    }
    public void Desenfocar()
    {
        if (feedbackVisual != null) feedbackVisual.Apagar();
    }
}