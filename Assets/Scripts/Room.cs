using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private GameObject roomInstance;
    private int roomNumber;
    private Vector2Int coords;

    public GameObject RoomInstance { get => roomInstance; }
    public Vector2Int Coords { get => coords; }
    public int RoomNumber { get => roomNumber; }


    public Room(GameObject roomInstance, Vector2Int coords, int roomNumber)
    {
        this.roomInstance = roomInstance;
        this.coords = coords;
        this.roomNumber = roomNumber;
    }
}
