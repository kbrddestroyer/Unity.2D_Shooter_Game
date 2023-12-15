using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Base following controller")]
    [SerializeField] protected Transform localPlayer;
    [SerializeField, Range(0f, 10f)] protected float smoothness;
    [SerializeField, Range(0f, 10f)] protected float staticRadius;
    [Header("Gizmo settings")]
    [SerializeField] protected Color gizmoColor;

    [SerializeField] protected bool isMoving = false;
    protected new Camera camera;

    public Transform Target {
        get => localPlayer;
        set { localPlayer = value; }
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, staticRadius);
    }

    protected void Awake()
    {
        camera = GetComponent<Camera>();
    }

    protected void Update()
    {
        Vector3 destination = localPlayer.transform.position;
        destination.z = transform.position.z;

        float distance = Vector3.Distance(transform.position, destination);
        if (distance > staticRadius || isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, destination, smoothness * Time.deltaTime);
            isMoving = (distance >= 0.01f);
        }

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        camera.orthographicSize = Mathf.Clamp(Mathf.Lerp(camera.orthographicSize, camera.orthographicSize - mouseScroll * 100, Time.deltaTime * smoothness), 2, 5);
    }
}
