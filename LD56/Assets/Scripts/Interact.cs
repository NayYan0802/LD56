using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [HideInInspector] public IInteractable interactTarget;
    public float interactCheckRange = 0.5f;
    private Collider2D[] overlapResults = new Collider2D[8];
    private List<IInteractable> interactablesInRange = new();

    public Transform pickOffset;
    private void Update()
    {
        interactablesInRange.Clear();

        bool originalHitTest = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = true;
        var count = Physics2D.OverlapCircleNonAlloc(transform.position, interactCheckRange, overlapResults); // TODO: transform.position implies the player position
        Physics2D.queriesHitTriggers = originalHitTest;

        for (int i = 0; i < count; ++i)
        {
            /*var interactable = overlapResults[i].gameObject.GetComponent<LeverInteractable>();
            if (interactable)
                interactablesInRange.Add(interactable);*/

            IInteractable interactable = overlapResults[i].gameObject.GetComponent<IInteractable>();
            if (interactable != null && !interactablesInRange.Contains(interactable))
                interactablesInRange.Add(interactable);
        }

        if (interactablesInRange.Count > 0)
        {
            interactablesInRange.Sort((a, b) => -(a.GetInteractPriority().CompareTo(b.GetInteractPriority())));
            TargetNewInteractable(interactablesInRange[0]);
        }
        else
        {
            TargetNewInteractable(null);
        }
    }

    public void TargetNewInteractable(IInteractable target)
    {
        if (target == interactTarget)
            return;

        interactTarget?.OnTargetedExit();

        interactTarget = target;
        interactTarget?.OnTargetedEnter();
    }

    // Call this function when interact (press the interact button) 
    public void InteractWithObject()
    {
        if (interactTarget != null)
            interactTarget.Interact(new InteractArgs(this.gameObject));
    }

    public PickableObject InteractWithPickableObject(Player player)
    {
        if (interactTarget != null)
            return (interactTarget as InteractableObject).GetComponent<PickableObject>().PickUp(player);
        else
            return null;
    }

    public void PutDownObject()
    {
        if (interactTarget != null)
            (interactTarget as InteractableObject).GetComponent<PickableObject>().PutDown();
    }
}
