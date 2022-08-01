using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] DungeonSettings settings;
    [SerializeField] GameObject mapContent;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearDungeon();
            CreateDungeon();
        }
    }

    private void ClearDungeon()
    {
        foreach (Transform child in mapContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public Dungeon CreateDungeon() 
    {
        DungeonConstuctorParameters parameters = new DungeonConstuctorParameters();
        parameters.rooms = CreateRooms();
        parameters.coordsToRoom = CreateCoordsToRoomDictionary(parameters.rooms);
        parameters.roomToConnectedRooms = ConnectRooms(parameters.rooms);
        parameters.roomConnections = CreateRoomConnectionsDict(parameters.roomToConnectedRooms);
        parameters.corridors = CreateCorridors(parameters.roomConnections);
        
        Dungeon newDungeon = new Dungeon(parameters);
        return newDungeon;
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

            Room room = CreateRoom(rooms, settings, currentCoords);
            usedCoords.Add(currentCoords);
            rooms.Add(room);
            roomsToCreate--;
        }
        return rooms;
    }
    private Room CreateRoom(List<Room> rooms, DungeonSettings settings, Vector2Int coords)
    {
        Vector3 pos = Vector3.zero;
        if (rooms.Count > 0)
        {
            pos = new Vector3(pos.x + settings.distanceBetweenRooms * coords.x, pos.y + settings.distanceBetweenRooms * coords.y, pos.z);
            print(pos);
        }
        GameObject roomInstance = Instantiate(settings.roomPrefab, mapContent.transform);
        pos = new Vector3(200f * coords.x, 200f * coords.y, 0);
        roomInstance.transform.localPosition = pos;
        Room newRoom = new Room(roomInstance, coords, rooms.Count + 1);       
        return newRoom;
    }


    private Vector2Int GetNextCoords(Vector2Int currentCoords)
    {
        // TODO can't go back to create room
        float randomNum = Random.Range(0, 1f);
        if (randomNum < 0.25f) return new Vector2Int(currentCoords.x - 1, currentCoords.y);
        if (randomNum < 0.5f) return new Vector2Int(currentCoords.x + 1, currentCoords.y);
        if (randomNum < 0.75f) return new Vector2Int(currentCoords.x, currentCoords.y - 1);
        if (randomNum < 1f) return new Vector2Int(currentCoords.x, currentCoords.y + 1);

        return Vector2Int.zero;
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

    public List<Corridor> CreateCorridors(Dictionary<string, List<Room>> roomConnections)
    {
        List<Corridor> corridors = new List<Corridor>();
        foreach (var key in roomConnections.Keys)
        {
            var newCorridor = CreateCorridor(roomConnections[key][0], roomConnections[key][1]);
            corridors.Add(newCorridor);
        }
        return corridors;
    }

    private Corridor CreateCorridor(Room room1, Room room2)
    {
        if (room1.Coords.x != room2.Coords.x && room1.Coords.y != room2.Coords.y) return null;

        Corridor newCorridor = new Corridor();
        newCorridor.AddConnectedRooms(room1, room2);
        CreateCorridorUI(room1, room2);
        return newCorridor;
    }

    private void CreateCorridorUI(Room room1, Room room2)
    {
        GameObject corridor = null;
        Vector2 centralStepPosition;
        if (room1.Coords.y == room2.Coords.y)
        {
            int xPos = (room1.Coords.x * 200 + room2.Coords.x * 200) / 2;
            centralStepPosition = new Vector2(xPos, room1.Coords.y * 200);
            corridor = Instantiate(settings.horizontalCorridor, mapContent.transform);
        }
        else
        {
            int yPos = (room1.Coords.y * 200 + room2.Coords.y * 200) / 2;
            centralStepPosition = new Vector2(room1.Coords.x * 200, yPos);
            corridor = Instantiate(settings.verticalCorridor, mapContent.transform);
        }
        corridor.transform.localPosition = centralStepPosition;
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

