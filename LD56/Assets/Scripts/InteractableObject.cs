using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour ,IInteractable
{
    public int interactPriority = 0;

    public int frightMeterInRange;
    public int frightMeterOutRange;

    public int GetInteractPriority()
    {
        return interactPriority;
    }

    public bool Interact(InteractArgs args)
    {
        throw new System.NotImplementedException();
    }

    public void OnTargetedEnter()
    {

    }

    public void OnTargetedExit()
    {

    }
}
