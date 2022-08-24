using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private int roomNumber;
    private Vector2Int coords;

    public Vector2Int Coords { get => coords; }
    public int RoomNumber { get => roomNumber; }


    public Room(Vector2Int coords, int roomNumber)
    {
        this.coords = coords;
        this.roomNumber = roomNumber;
    }
}
