using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridor 
{
    private Room[] connectedRooms = new Room[2];
    private List<CorridorStep> corridorSteps = new List<CorridorStep>();

    public void AddConnectedRooms(Room room1, Room room2)
    {
        connectedRooms[0] = room1;
        connectedRooms[1] = room2;
    }

    public void AddCorridorSteps(CorridorStep step)
    {
        corridorSteps.Add(step);
    }
}

public class CorridorStep
{
    public GameObject instance;
}
