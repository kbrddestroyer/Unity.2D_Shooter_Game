using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredDialogue : DialoguePlayer
{
    [Header("Trigger Settings")]
    [SerializeField] protected bool once;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.PlayDialogue();
            if (once) this.GetComponent<Collider2D>().enabled = false;
        }
    }
}
