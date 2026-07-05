using UnityEngine;

[RequireComponent(typeof(MeshRenderer))] // Obliga a Unity a añadir un MeshRender en caso de no haber uno para no crear una referencia nula
public class TuberiaBase : MonoBehaviour, IInteractable
{
    private bool isRepaired = false;
    private MeshRenderer meshRender; // Referencia al componente MeshRender (Encargado de hacer visible el objeto en escena)
    
    void Start()
    {
        meshRender = GetComponent<MeshRenderer>(); // Llamada a la referencia
    }

    public void Interact(PlayerMovement jugador) // Implementación del método para interactuar (Polimorfismo)
    {
        if (!isRepaired)
        {
            isRepaired = true;
            meshRender.material.color = Color.blue; // Después de reparar la tubería, ésta cambia de color
        }

        else
        {
            Debug.Log("La tubería ya está reparada");
        }
    }
}
