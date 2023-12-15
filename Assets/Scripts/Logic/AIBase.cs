using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class AIBase : MonoBehaviour, AI
{
    #region VARIABLES
    
    #region EDITOR_VARIABLES
    [Header("Base AI Settings")]
    [SerializeField, Range(0f, 10f)] protected float baseSpeed;
    [SerializeField, Range(0f, 10f)] protected float runSpeed;
    #endregion
    
    #region PROTECTED_VARIABLES
    protected new SpriteRenderer renderer;
    protected Animator animator;
    #endregion

    #endregion

    #region LIFECYCLE
    protected virtual void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    #endregion

    #region AI_LOGIC
    public virtual void WalkTo(Transform destination)
    {
        Vector3 direction = (destination.position - transform.position);
        transform.position += direction * runSpeed * Time.deltaTime;
        renderer.flipX = (direction.x < 0);

        animator.SetFloat("speed", runSpeed);
    }
    #endregion
}
