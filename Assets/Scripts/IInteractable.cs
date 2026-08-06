using UnityEngine;

public interface IInteractable
{
    void Interact(PlayerInteractor interactor);

    void Enfocar();
    void Desenfocar();
}
