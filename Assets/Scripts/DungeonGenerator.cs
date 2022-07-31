using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] DungeonSettings dungeonSettings;
    [SerializeField] GameObject mapContent;

    private Dictionary<Vector2Int, Room> coordsToRoom = new Dictionary<Vector2Int, Room>();
    private Dictionary<Room, List<Room>> roomConnections = new Dictionary<Room, List<Room>>();
    private Dictionary<string, List<Room>> connections = new Dictionary<string, List<Room>>();
    private List<Room> rooms = new List<Room>();
    private List<Corridor> corridors = new List<Corridor>();


    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearDungeon();
            CreateDungeon(dungeonSettings);
        }
    }

    private void ClearDungeon()
    {
        foreach (Transform child in mapContent.transform)
        {
            Destroy(child.gameObject);
        }
        coordsToRoom.Clear();
        rooms.Clear();
        roomConnections.Clear();
        corridors.Clear();
        connections.Clear();
    }

   
    public void CreateDungeon(DungeonSettings settings)
    {
        Vector2Int currentCoords = Vector2Int.zero;
        int roomsToCreate = Random.Range(settings.minNumberOfRooms, settings.maxNumberOfRooms);
        while (roomsToCreate > 0)
        {
            if (rooms.Count > 0) currentCoords = GetNextCoords(currentCoords);
            if (coordsToRoom.ContainsKey(currentCoords)) continue;

            Room room = CreateRoom(settings, currentCoords);
            coordsToRoom.Add(currentCoords, room);
            rooms.Add(room);
            roomsToCreate--;
        }

        ConnectRooms(rooms);
        foreach (var key in connections.Keys)
        {
            CreateCorridor(connections[key][0], connections[key][1]);
        }        
    }

    private void ConnectRooms(List<Room> rooms)
    {
        
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
                    
                    //connection key forms from two room numbers converted to a string. Smaller room number always goes first.
                    string connectionKey = "";
                    if (rooms[i].RoomNumber < rooms[j].RoomNumber)  
                        connectionKey = "" + rooms[i].RoomNumber + rooms[j].RoomNumber;
                    else
                        connectionKey = "" + rooms[j].RoomNumber + rooms[i].RoomNumber;                    
                    if (!connections.ContainsKey(connectionKey))
                    {
                        var roomsToAdd = new List<Room>();
                        roomsToAdd.Add(rooms[i]);
                        roomsToAdd.Add(rooms[j]);
                        connections.Add(connectionKey, roomsToAdd);
                    }
                }
            }
            roomConnections[rooms[i]] = connectedRooms;
        }
    }

    private void CreateCorridor(Room room1, Room room2)
    {

        if (room1.Coords.x != room2.Coords.x && room1.Coords.y != room2.Coords.y) return;

        Corridor newCorridor = new Corridor();
        newCorridor.AddConnectedRooms(room1, room2);
        CreateCorridorUI(room1, room2);
        corridors.Add(newCorridor);
    }

    private void CreateCorridorUI(Room room1, Room room2)
    {
        GameObject corridor = null;
        Vector2 centralStepPosition;
        if (room1.Coords.y == room2.Coords.y)
        {
            int xPos = (room1.Coords.x * 200 + room2.Coords.x * 200) / 2;
            centralStepPosition = new Vector2(xPos, room1.Coords.y * 200);
            corridor = Instantiate(dungeonSettings.horizontalCorridor, mapContent.transform);
        }
        else
        {
            int yPos = (room1.Coords.y * 200 + room2.Coords.y * 200) / 2;
            centralStepPosition = new Vector2(room1.Coords.x * 200, yPos);
            corridor = Instantiate(dungeonSettings.verticalCorridor, mapContent.transform);
        }
        corridor.transform.localPosition = centralStepPosition;
    }

    private Room CreateRoom(DungeonSettings settings, Vector2Int coords)
    {
        Vector3 pos = Vector3.zero;
        if (rooms.Count > 0)
        {
            pos = new Vector3(pos.x + settings.distanceBetweenRooms * coords.x, pos.y + settings.distanceBetweenRooms * coords.y, pos.z);
            print(pos);
        }
        GameObject roomInstance = Instantiate(dungeonSettings.roomPrefab, mapContent.transform);
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

}

