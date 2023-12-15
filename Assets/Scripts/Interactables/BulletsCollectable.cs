using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsCollectable : collectable
{
    [SerializeField, Range(0f, 50f)] protected int bulletCount;

    protected Collider2D col;
    protected Rigidbody2D rb;
    protected SpriteRenderer sprite;
    protected AudioSource audioSource;

    protected void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void Collect(Player player)
    {
        player.AmmoInInventory += bulletCount;

        col.enabled = false;
        rb.simulated = false;
        sprite.enabled = false;
        audioSource.Play();
        Destroy(this.gameObject, audioSource.clip.length);
    }
}
