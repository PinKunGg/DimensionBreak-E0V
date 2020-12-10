using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPoint : MonoBehaviour
{
    [SerializeField] bool GroundSpawn;
    [SerializeField] bool FlySpawn;

    public bool GetGroundSpawn
    {
        get
        {
            return GroundSpawn;
        }
    }
    public bool GetFlySpawn
    {
        get
        {
            return FlySpawn;
        }
    }
}
