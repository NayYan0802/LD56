using MoreMountains.Feedbacks;
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
    [FoldoutGroup("Turn")] public bool hasAnimation;
    [FoldoutGroup("Turn")] public Sprite Img_On;
    [FoldoutGroup("Turn")] public Sprite Img_Off;
    [FoldoutGroup("Turn")] public bool hasTurned;


    [FoldoutGroup("Trigger")] public UnityEvent _event;
    [FoldoutGroup("Trigger")] public bool hasTriggered;

    [FoldoutGroup("Feedbacks")] public MMF_Player pushFB, hideFB, turnFB, triggerFB, loopingFB;

    private bool isHiding;

    public int frightMeterInRange;
    public int frightMeterOutRange;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        m_collider = this.GetComponent<Collider2D>();
        isHiding = false;
        if ((animator=this.GetComponent<Animator>()) && type == InteractionType.Turn)
        {
            hasAnimation = true;
            animator.SetBool("IsOn", isOn);
        }
        else
        {
            hasAnimation = false;
        }
        hasTurned = false;
        hasTriggered = false;
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
                m_collider.excludeLayers = ~0 - LayerMask.GetMask("Item"); // Everything
                if (pushFB != null)
                    pushFB.PlayFeedbacks();
                break;
            case InteractionType.Hide:
                if (!isHiding)
                {
                    //spriteRenderer.sortingOrder = 6;
                    Color color = spriteRenderer.color;
                    color.a = 0.5f;
                    spriteRenderer.color = color;
                    PlayerStateMachine.Instance.ChangeToHideState();
                    isHiding = true;
                }
                else
                {
                    //spriteRenderer.sortingOrder = 0;
                    Color color = spriteRenderer.color;
                    color.a = 1f;
                    spriteRenderer.color = color;
                    PlayerStateMachine.Instance.SwitchToPreviousState();
                    isHiding = false;
                }
                if (hideFB != null)
                    hideFB.PlayFeedbacks();
                break;
            case InteractionType.Turn:
                if(hasTurned)
                {
                    return false;
                }
                isOn = !isOn;
                if (isOn && !hasTurned)
                {
                    Scared(transform.position.x);
                    hasTurned = true;
                }
                if (hasAnimation)
                {
                    animator.SetBool("IsOn", isOn);
                }
                else
                {
                    if (isOn)
                    {
                        this.spriteRenderer.sprite = Img_On;
                    }
                    else
                    {
                        this.spriteRenderer.sprite = Img_Off;
                    }
                }
                if (turnFB != null)
                    turnFB.PlayFeedbacks();
                break;
            case InteractionType.Trigger:
                if (!hasTriggered)
                {
                    _event.Invoke();
                    Scared(transform.position.x);
                    hasTriggered = true;
                    if (triggerFB != null)
                        triggerFB.PlayFeedbacks();
                }
                break;
        }
        if (loopingFB != null)
            loopingFB.PlayFeedbacks();     
        return false;
    }

    public void DirectInteract()
    {
        Interact(null);
    }

    public void Scared(float posX)
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
        Player player = FindFirstObjectByType<Player>();
        player.InteractBtn.SetActive(true);
        if (this.GetComponent<PickableObject>())
        {
            player.PickBtn.SetActive(true);
        }
    }

    public void OnTargetedExit()
    {
        Player player = FindFirstObjectByType<Player>();
        player.InteractBtn.SetActive(false);
        if (this.GetComponent<PickableObject>())
        {
            player.PickBtn.SetActive(false);
        }
    }
}
