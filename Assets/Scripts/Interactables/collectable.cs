using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class collectable : MonoBehaviour
{
    public abstract void Collect(Player player);
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Collect(collision.GetComponent<Player>());
        }
    }
}
