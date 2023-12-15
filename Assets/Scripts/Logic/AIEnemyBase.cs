using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIEnemyBase : AIDamagableBase, IDamagable
{
    [Header("Enemy Settings")]
    [SerializeField, Range(0f, 10f)] protected float maxDistanceTrigger;
    [SerializeField, Range(0f, 10f)] protected float minDistanceTrigger;

    protected virtual void Chase(Transform target)
    {
        WalkTo(target);
    }
    protected virtual void Seek()
    {
        // TODO: Make seek logic

        // <...>
    }
}
