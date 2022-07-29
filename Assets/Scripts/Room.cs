using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private Vector3 position;
    private GameObject roomInstance;
    private int roomNumber;

    private Vector2Int coords;

    public GameObject RoomInstance { get => roomInstance; }
    public Vector2Int Coords { get => coords; }
    public int RoomNumber { get => roomNumber; }

    public Vector3 GetRoomPosition()
    {
        return position;
    }

    public Room(Vector3 position, GameObject roomInstance, Vector2Int coords, int roomNumber)
    {
        this.position = position;
        this.roomInstance = roomInstance;
        this.coords = coords;
        this.roomNumber = roomNumber;
    }

    /*public IEnumerable<Room> GetConnectedRooms(IEnumerable<Room> rooms, float distanceBetweenRooms)
    {
        List<Room> connectedRooms = new List<Room>();
        foreach (var room in rooms)
        {
            if (room == this || (room.GetRoomPosition() - GetRoomPosition()).magnitude > distanceBetweenRooms) continue;
            connectedRooms.Add(room);
        }
        foreach (var room in connectedRooms)
        {
            Debug.Log($"{GetRoomPosition()} - {room.GetRoomPosition()}");
        }
        return connectedRooms;
    }*/

}
