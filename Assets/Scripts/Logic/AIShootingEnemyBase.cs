using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShootingEnemyBase : AIEnemyBase
{
    [Header("Shooting Enemy Base Settings")]
    [SerializeField, Range(0f, 10f)] protected float shootingRate;
    [SerializeField, Range(0f, 5f)] protected float shotPointOffset;
    [Header("Required objects")]
    [SerializeField] protected Transform shotPoint;
    [SerializeField] protected GameObject bulletPrefab;
    [Header("Editor")]
    [SerializeField] protected Color gizmoColorMax;
    [SerializeField] protected Color gizmoColorMin;
    [SerializeField] protected Color gizmoColorOffset;

    protected float shootingTimePassed = 0f;
    protected Player localPlayer;

    protected override void Awake()
    {
        base.Awake();
        localPlayer = FindObjectOfType<Player>();
    }

    protected override void Chase(Transform target)
    {
        base.Chase(target);
        animator.SetBool("shooting", false);
    }

    protected void Shoot(Transform target)
    {
        if (!animator.GetBool("shooting")) animator.SetBool("shooting", true);

        shootingTimePassed += Time.deltaTime;
        if (shootingTimePassed >= shootingRate)
        {
            shootingTimePassed = 0f;
            Instantiate(bulletPrefab, shotPoint.transform.position + new Vector3(shotPointOffset * ((renderer.flipX) ? (-1f) : (1f)), 0, 0), Quaternion.Euler(new Vector3(0, 0, (renderer.flipX) ? 180 : 0)));
        }
    }

    protected override void Seek()
    {
        base.Seek();
        animator.SetBool("shooting", false);
        animator.SetFloat("speed", 0);
    }

    protected virtual void Update()
    {
        float distance = Vector2.Distance(transform.position, localPlayer.transform.position);
        if (distance < maxDistanceTrigger)
        {
            if (distance > minDistanceTrigger)
            {
                Chase(localPlayer.transform);
            }
            else
            {
                Shoot(localPlayer.transform);
            }
        }
        else
        {
            Seek();
        }
    }

    #region EDITOR
#if UNITY_EDITOR
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColorMax;
        Gizmos.DrawWireSphere(transform.position, maxDistanceTrigger);
        Gizmos.color = gizmoColorMin;
        Gizmos.DrawWireSphere(transform.position, minDistanceTrigger);
        Gizmos.color = gizmoColorOffset;
        Gizmos.DrawWireSphere(transform.position, shotPointOffset);
    }
#endif
    #endregion
}
