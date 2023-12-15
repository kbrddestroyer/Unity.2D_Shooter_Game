using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDamagableBase : AIBase, IDamagable
{
    #region VARIABLES
    [Header("AI Damagable")]
    [SerializeField, Range(0f, 100f)] protected float fMaxHP;
    protected float fHP;
    #endregion

    #region GETTER_SETTER
    public float HP {
        get => fHP;
        set
        {
            fHP = value;
            
            if (fHP <= 0)
            {
                Die();
            }
        }
    }
    #endregion

    #region LOGIC
    protected void Die() { Destroy(this.gameObject); }

    protected override void Awake()
    {
        base.Awake();
        fHP = fMaxHP;
    }
    #endregion
}
