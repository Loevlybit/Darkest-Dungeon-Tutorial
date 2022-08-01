using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    [SerializeField] DungeonGenerator generator;

    private DungeonConstuctorParameters parameters;


    public Dungeon(DungeonConstuctorParameters parameters)
    {
        this.parameters = parameters;
    }

    
}
