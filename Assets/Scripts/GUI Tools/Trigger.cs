using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{
    [SerializeField] private UnityEvent action;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            action.Invoke();
    }
}
