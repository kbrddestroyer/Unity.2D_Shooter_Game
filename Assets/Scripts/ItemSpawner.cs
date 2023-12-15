using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject spawnablePrefab;
    [SerializeField, Range(0f, 10f)] protected float spawnTime;

    protected GameObject child = null;
    protected float passedTime = 0f;

    protected void Update()
    {
        if (child == null)
        {
            passedTime += Time.deltaTime;
            if (passedTime >= spawnTime)
            {
                child = Instantiate(spawnablePrefab, transform);
                passedTime = 0f;
            }
        }
    }
}
