using UnityEngine;

public class InteractResults
{
    public bool successfullyInteracted;
}

public class InteractArgs
{
    public GameObject interactor;

    public InteractArgs(GameObject interactor)
    {
        this.interactor = interactor;
    }
}

public interface IInteractable
{
    public int GetInteractPriority();

    bool Interact(InteractArgs args);
    void OnTargetedEnter();
    void OnTargetedExit();
}
