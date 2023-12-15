using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListSpawnables : MonoBehaviour
{
    [SerializeField] protected WorldObjectController[] controllers;
    public WorldObjectController get(int id) { return controllers[id]; }
}
