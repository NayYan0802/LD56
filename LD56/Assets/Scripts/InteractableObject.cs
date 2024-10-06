using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour ,IInteractable
{
    public enum InteractionType
    {
        Push, Hide, Turn, Trigger
    }
    [FoldoutGroup("Interaction")] public int interactPriority = 0;
    [FoldoutGroup("Interaction")] public InteractionType type;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D m_collider;

    [FoldoutGroup("Interaction")] public LayerMask groundLayer;
    [FoldoutGroup("Interaction")] public float fallTime = 1;
    private bool isHiding;

    public int frightMeterInRange;
    public int frightMeterOutRange;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        m_collider = this.GetComponent<Collider2D>();
        isHiding = false;
    }

    public int GetInteractPriority()
    {
        return interactPriority;
    }

    public bool Interact(InteractArgs args)
    {
        switch (type)
        {
            case InteractionType.Push:
                spriteRenderer.sortingOrder = -1;
                rb.gravityScale = 1;
                m_collider.excludeLayers = ~0; // Everything

                StartCoroutine(Fall());
                break;
            case InteractionType.Hide:
                if (!isHiding)
                {
                    spriteRenderer.sortingOrder = 6;
                    PlayerStateMachine.Instance.ChangeToHideState();
                    isHiding = true;
                }
                else
                {
                    spriteRenderer.sortingOrder = 0;
                    PlayerStateMachine.Instance.SwitchToPreviousState();
                    isHiding = false;
                }
                break;

        }
        
        return false;
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallTime);
        // Trigger fall events
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

    public void OnTargetedEnter()
    {

    }

    public void OnTargetedExit()
    {

    }
}
