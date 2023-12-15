using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappingHook : MonoBehaviour
{
    [SerializeField] protected Transform weaponHolder;
    [SerializeField, Range(0f, 10f)] protected float maxDistance;
    [SerializeField, Range(0f, 10f)] protected float speed;

    protected Player parentPlayer;
    [SerializeField] protected bool isAttached = false;
    public bool Attached
    {
        get => isAttached;
        protected set { isAttached = value; rb.gravityScale = (isAttached) ? 0 : 1; rb.velocity = Vector3.zero; }
    }
    protected Vector3 attachPosition;
    protected Rigidbody2D rb;
    protected LineRenderer lr;

    protected void Awake()
    {
        parentPlayer = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
    }

    protected void InitGrappingHook()
    {
        Vector3 direction = (transform.localScale.x < 0) ? -weaponHolder.right : weaponHolder.right;
        RaycastHit2D hit = Physics2D.Raycast(weaponHolder.transform.position, direction, maxDistance);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                attachPosition = hit.point;
                Attached = true;

                lr.positionCount = 2;
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, attachPosition);
            }
        }
    }

    protected void Grab(Vector3 attachPosition)
    {
        if (isAttached)
        {
            transform.position = Vector3.Slerp(transform.position, attachPosition, Time.deltaTime * speed);

            lr.SetPosition(0, transform.position);
            if (Vector2.Distance(transform.position, attachPosition) <= 0.2f)
            {
                Attached = false;
                lr.positionCount = 0;
            }
        }
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            InitGrappingHook();
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Grab(attachPosition);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Attached = false;

            lr.positionCount = 0;
        }
    }
}
