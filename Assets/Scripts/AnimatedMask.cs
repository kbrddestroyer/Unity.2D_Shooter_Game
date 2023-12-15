using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMask : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer parentPlayer;
    protected SpriteMask mask;

    protected void Awake()
    {
        mask = GetComponent<SpriteMask>();
    }

    protected void Update()
    {
        mask.sprite = parentPlayer.sprite;
    }
}
