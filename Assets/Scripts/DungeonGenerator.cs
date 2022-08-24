using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] DungeonSettings settings;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dungeon dungeon = CreateDungeon(settings);
            FindObjectOfType<MapUI>().CreateDungeonMapUI(dungeon);
        }
    }


    public Dungeon CreateDungeon(DungeonSettings settings) 
    {
        DungeonConstuctorParameters parameters = new DungeonConstuctorParameters();
        parameters.rooms = CreateRooms();
        parameters.coordsToRoom = CreateCoordsToRoomDictionary(parameters.rooms);
        parameters.roomToConnectedRooms = ConnectRooms(parameters.rooms);
        parameters.roomConnections = CreateRoomConnectionsDict(parameters.roomToConnectedRooms);
        parameters.corridors = CreateCorridors(parameters.roomToConnectedRooms);
        
        return new Dungeon(parameters);
    }
    public List<Room> CreateRooms()
    {
        List<Room> rooms = new List<Room>();
        Vector2Int currentCoords = Vector2Int.zero;
        List<Vector2Int> usedCoords = new List<Vector2Int>();
        int roomsToCreate = Random.Range(settings.minNumberOfRooms, settings.maxNumberOfRooms);
        while (roomsToCreate > 0)
        {
            if (rooms.Count > 0) currentCoords = GetNextCoords(currentCoords);
            if (usedCoords.Contains(currentCoords)) continue;

            Room room = new Room(currentCoords, rooms.Count + 1);
            rooms.Add(room);
            usedCoords.Add(currentCoords);
            roomsToCreate--;
        }
        return rooms;
    }

    private Vector2Int GetNextCoords(Vector2Int currentCoords)
    {
        // TODO can't go back to create room
        float randomNum = Random.Range(0, 1f);
        if (randomNum < 0.25f) return new Vector2Int(currentCoords.x - 1, currentCoords.y);
        if (randomNum < 0.5f) return new Vector2Int(currentCoords.x + 1, currentCoords.y);
        if (randomNum < 0.75f) return new Vector2Int(currentCoords.x, currentCoords.y - 1);
        return new Vector2Int(currentCoords.x, currentCoords.y + 1);
    }

    private Dictionary<Vector2Int, Room> CreateCoordsToRoomDictionary(List<Room> rooms)
    {
        Dictionary<Vector2Int, Room> coordsToRoom = new Dictionary<Vector2Int, Room>();
        foreach (var room in rooms)
        {
            coordsToRoom.Add(room.Coords, room);
        }
        return coordsToRoom;
    }

    public Dictionary<Room, List<Room>> ConnectRooms(List<Room> rooms)
    {

        Dictionary<Room, List<Room>> roomToConnectedRooms = new Dictionary<Room, List<Room>>();
        for (int i = 0; i < rooms.Count; i++)
        {
            List<Room> connectedRooms = new List<Room>();
            for (int j = 0; j < rooms.Count; j++)
            {
                if (i == j) continue;
                
                int xDistanceBetweenRooms = Mathf.Abs(rooms[i].Coords.x - rooms[j].Coords.x);
                int yDistanceBetweenRooms = Mathf.Abs(rooms[i].Coords.y - rooms[j].Coords.y);              
                if (xDistanceBetweenRooms + yDistanceBetweenRooms == 1) 
                {
                    connectedRooms.Add(rooms[j]);                   
                }
            }
            roomToConnectedRooms[rooms[i]] = connectedRooms;
        }
        return roomToConnectedRooms;    
    }

    private Dictionary<string, List<Room>> CreateRoomConnectionsDict(Dictionary<Room, List<Room>> roomToConnectedRoom)
    {
        Dictionary<string, List<Room>> roomConnections = new Dictionary<string, List<Room>>();
        foreach (var room in roomToConnectedRoom.Keys)
        {
            foreach (var connectedRoom in roomToConnectedRoom[room])
            {
                string connectionKey = "";
                if (room.RoomNumber < connectedRoom.RoomNumber)
                    connectionKey = "" + room.RoomNumber + connectedRoom.RoomNumber;
                else
                    connectionKey = "" + connectedRoom.RoomNumber + room.RoomNumber;
                if (!roomConnections.ContainsKey(connectionKey))
                {
                    var roomsToAdd = new List<Room>();
                    roomsToAdd.Add(room);
                    roomsToAdd.Add(connectedRoom);
                    roomConnections.Add(connectionKey, roomsToAdd);
                }
            }
        }
        return roomConnections;
    }

    public List<Corridor> CreateCorridors(Dictionary<Room, List<Room>> roomToConnectedRooms)
    {
        List<Corridor> corridors = new List<Corridor>();
        List<Room> connectedRooms = new List<Room>();
        foreach (var room in roomToConnectedRooms.Keys)
        {
            foreach (var connectedRoom in roomToConnectedRooms[room])
            {
                if (connectedRooms.Contains(connectedRoom)) continue;

                var newCorridor = new Corridor(room, connectedRoom);
                corridors.Add(newCorridor);
            }
            connectedRooms.Add(room);
        }
        return corridors;
    }

}

public class DungeonConstuctorParameters
{
    public Dictionary<Vector2Int, Room> coordsToRoom = new Dictionary<Vector2Int, Room>();
    public Dictionary<Room, List<Room>> roomToConnectedRooms = new Dictionary<Room, List<Room>>();
    public Dictionary<string, List<Room>> roomConnections = new Dictionary<string, List<Room>>();
    public List<Room> rooms = new List<Room>();
    public List<Corridor> corridors = new List<Corridor>();
}

