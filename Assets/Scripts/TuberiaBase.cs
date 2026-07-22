using UnityEngine;

// Se definen los tipos de averías que puede tener una tubería
public enum TipoAveria
{
    FugaFuerte, // Requiere Llave Inglesa
    Fisura      // Requiere Cinta Adhesiva
}

[RequireComponent(typeof(MeshRenderer))] // Obliga a Unity a añadir un MeshRender en caso de no haber uno para no crear una referencia nula
public class TuberiaBase : MonoBehaviour, IInteractable
{
    public TipoAveria tipoDeAveria;

    private bool isRepaired = false;
    private MeshRenderer meshRender; // Referencia al componente MeshRender (Encargado de hacer visible el objeto en escena)
    
    void Start()
    {
        meshRender = GetComponent<MeshRenderer>(); // Llamada a la referencia
    }

    public void Interact(PlayerInteractor interactor) // Implementación del método para interactuar (Polimorfismo)
    {
        if (isRepaired)
        {
            Debug.Log("La tubería no necesita reparación");
            return;
        }

        if (interactor.objetoEnMano == null)
        {
            Debug.Log("Necesitas una herramienta para reparar esto.");
            return;
        }

        if (interactor.objetoEnMano.TryGetComponent<HerramientaBase>(out HerramientaBase herramienta))
        {
            if (herramienta.PuedeReparar(tipoDeAveria))
            {
                RepararTuberia();
            }
            else
            {
                Debug.Log("Herramienta incorrecta para este tipo de avería.");
            }
        }
        else
        {
            Debug.Log("El objeto que sostienes no es una herramienta válida para reparaciones.");
        }

    }
    private void RepararTuberia()
    {
        isRepaired = true;
        meshRender.material.color = Color.blue;
        Debug.Log("¡Tubería reparada exitosamente!");
    }
}
