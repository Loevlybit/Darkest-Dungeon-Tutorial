using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon
{
    private DungeonConstuctorParameters structure;

    public Dungeon(DungeonConstuctorParameters structure)
    {
        this.structure = structure;
    }

    public IEnumerable<Room> GetRooms()
    {
        return structure.rooms;
    }

    public IEnumerable<Corridor> GetCorridors()
    {
        return structure.corridors;
    }
}
