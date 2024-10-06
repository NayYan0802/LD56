using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [FoldoutGroup("Turn")] public Animator animator;
    [FoldoutGroup("Turn")] public bool isOn;

    [FoldoutGroup("Trigger")] public UnityEvent _event;


    private bool isHiding;

    public int frightMeterInRange;
    public int frightMeterOutRange;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        m_collider = this.GetComponent<Collider2D>();
        isHiding = false;
        if((animator = this.GetComponent<Animator>()) && type == InteractionType.Turn)
            animator.SetBool("IsOn", isOn);
    }

    public int GetInteractPriority()
    {
        return interactPriority;
    }

    public virtual bool Interact(InteractArgs args)
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
            case InteractionType.Turn:
                isOn = !isOn;
                animator.SetBool("IsOn", isOn);
                break;
            case InteractionType.Trigger:
                _event.Invoke();
                break;
        }
        
        return false;
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallTime);
        // Trigger fall events
        Scared(transform.position.x) ;
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

    protected void Scared(float posX)
    {
        int zone;
        if (posX < GameManagement.Instance.border1)
        {
            zone = 0;
        }
        else if (posX > GameManagement.Instance.border2)
        {
            zone = 2;
        }
        else
        {
            zone = 1;
        }

        Customer[] customers = FindObjectsByType<Customer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach(var customer in customers)
        {
            if (customer.currentZone == zone)
            {
                customer.Scared(frightMeterInRange);
            }
            else
            {
                customer.Scared(frightMeterOutRange);
            }
        }
    }

    public void OnTargetedEnter()
    {

    }

    public void OnTargetedExit()
    {

    }

    public void ReverseAnimatorBoolean()
    {
        isOn = !isOn;
        animator.SetBool("IsOn", isOn);
    }
}
