using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObjectController : MonoBehaviour
{
    [SerializeField] protected GameObject playerItemPrefab;
    [SerializeField, Range(0f, 10f)] protected float interactDistance;
    [SerializeField] protected GameObject outliner;
    [Header("Editor")]
    [SerializeField] protected Color gizmoColor;

    protected static Player localPlayer = null;

#if UNITY_EDITOR
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
#endif

    protected void Awake()
    {
        // For singleplayer
        if (localPlayer == null)
        {
            localPlayer = FindObjectOfType<Player>();
        }
    }

    public GameObject PlayerItemPrefab { get => playerItemPrefab; }

    protected abstract void Pickup();

    protected void Update()
    {
        if (Vector2.Distance(localPlayer.transform.position, transform.position) <= interactDistance)
        {
            outliner.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) Pickup();
        }
        else outliner.SetActive(false);
    }
}
