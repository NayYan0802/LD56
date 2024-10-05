using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour ,IInteractable
{
    public int interactPriority = 0;

    public int frightMeterInRange;
    public int frightMeterOutRange;

    [FoldoutGroup("InteractionType")]public bool push, pick, hide, turn, trigger;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    public int GetInteractPriority()
    {
        return interactPriority;
    }

    public bool Interact(InteractArgs args)
    {
        if (push)
        {
            Push();
        }

        if (pick)
        {

        }
        return false;
    }

    private void Push()
    {
        spriteRenderer.sortingOrder = -1;
        rb.gravityScale = 1;
    }

    public void OnTargetedEnter()
    {

    }

    public void OnTargetedExit()
    {

    }
}
