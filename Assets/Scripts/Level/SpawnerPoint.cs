using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPoint : MonoBehaviour
{
    public Transform spawnTransform;
    public Vector3 spawnPosition;
    [SerializeField] public string spawnType;
    public bool occupied;
}
