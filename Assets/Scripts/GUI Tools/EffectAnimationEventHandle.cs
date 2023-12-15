using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimationEventHandle : MonoBehaviour
{
    protected LevelLoader levelLoader;

    protected void Awake()
    {
        levelLoader = GameObject.FindObjectOfType<LevelLoader>();
    }

    public void ExitTrigger() { levelLoader.CanExit = true; }
}
