using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] DungeonSettings dungeonSettings;
    [SerializeField] GameObject mapContent;

    private Dictionary<Vector3, Room> positionToRoom = new Dictionary<Vector3, Room>();
    private Dictionary<Vector2Int, Room> coordsToRoom = new Dictionary<Vector2Int, Room>();
    private Dictionary<Room, List<Room>> roomConnections = new Dictionary<Room, List<Room>>();
    private Dictionary<string, List<Room>> connections = new Dictionary<string, List<Room>>();
    private List<ConnectionPair> connectionPairs = new List<ConnectionPair>();
    private List<Room> rooms = new List<Room>();


    private void Start()
    {
        //CreateDungeon(dungeonSettings);
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
        foreach (var room in rooms)
        {
            Destroy(room.RoomInstance);
            print("destroyed");
        }
        positionToRoom.Clear();
        coordsToRoom.Clear();
        rooms.Clear();
        roomConnections.Clear();
    }

   
    public void CreateDungeon(DungeonSettings settings)
    {
        Room previousRoom = null;
        Vector2Int currentCoords = Vector2Int.zero;
        int roomsToCreate = UnityEngine.Random.Range(settings.minNumberOfRooms, settings.maxNumberOfRooms);
        while (roomsToCreate > 0)
        {
            Vector3 nextPos = GetNextPos(previousRoom);
            currentCoords = GetNextCoords(currentCoords);
            if (rooms.Count == 0) currentCoords = Vector2Int.zero;
            if (coordsToRoom.ContainsKey(currentCoords)) continue;
            previousRoom = CreateRoom(settings, nextPos, currentCoords);
            positionToRoom.Add(nextPos, previousRoom);
            coordsToRoom.Add(currentCoords, previousRoom);
            rooms.Add(previousRoom);
            roomsToCreate--;
        }
        ConnectRooms(rooms);

        foreach (var key in connections.Keys)
        {
            CreateCorridors(connections[key][0].Coords, connections[key][1].Coords);
        }

        foreach (var room in positionToRoom.Values)
        {
            //room.GetConnectedRooms(positionToRoom.Values, dungeonSettings.distanceBetweenRooms);
            print($"Room coords = {room.Coords}");
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
                /*if (Mathf.Abs(rooms[i].RoomNumber - rooms[j].RoomNumber) == 1)
                {
                    connectionPairs.Add(new ConnectionPair() { roomNumber1 = rooms[i].RoomNumber, roomNumber2 = rooms[j].RoomNumber });
                    connectionPairs.Add(new ConnectionPair() { roomNumber1 = rooms[j].RoomNumber, roomNumber2 = rooms[i].RoomNumber });
                    connectedRooms.Add(rooms[j]);
                    continue;
                }*/
                
                int xDistanceBetweenRooms = Mathf.Abs(rooms[i].Coords.x - rooms[j].Coords.x);
                int yDistanceBetweenRooms = Mathf.Abs(rooms[i].Coords.y - rooms[j].Coords.y);              
                if (xDistanceBetweenRooms + yDistanceBetweenRooms == 1) 
                {
                    connectedRooms.Add(rooms[j]);
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

    public void CreateDungeon2(DungeonSettings settings)
    {
        Room room = null;
        Vector2Int currentCoords = Vector2Int.zero;
        int roomsToCreate = Random.Range(settings.minNumberOfRooms, settings.maxNumberOfRooms);
        while (roomsToCreate > 0)
        {
            Vector3 nextPos = GetNextPos(room);
            currentCoords = GetNextCoords(currentCoords);
            room = CreateRoom(settings, nextPos, currentCoords);
            positionToRoom.Add(nextPos, room);
            rooms.Add(room);
            room = rooms[Random.Range(0, rooms.Count)];
            roomsToCreate--;
        }
    }

    private void CreateCorridors(Vector2Int roomOneCoords, Vector2Int roomTwoCoords)
    {
        
        if (roomOneCoords.x != roomTwoCoords.x && roomOneCoords.y != roomTwoCoords.y) return;

        if (roomOneCoords.y == roomTwoCoords.y)
        {
            //int pos = (roomOneCoords.x * (int)dungeonSettings.distanceBetweenRooms + roomTwoCoords.x * (int)dungeonSettings.distanceBetweenRooms) / 2;
            int pos = (roomOneCoords.x * 200 + roomTwoCoords.x * 200) / 2;

            print($"ROom1Coords = {roomOneCoords} and room2Coords = {roomTwoCoords} and pos = {pos}");
            //Instantiate(dungeonSettings.corridorStepPrefab, new Vector2(pos, roomOneCoords.y * dungeonSettings.distanceBetweenRooms), Quaternion.identity);
            GameObject corridorCentralStep = Instantiate(dungeonSettings.corridorStepPrefab, mapContent.transform);           
            corridorCentralStep.transform.localPosition = new Vector2(pos, roomOneCoords.y * 200);

        }
        if (roomOneCoords.x == roomTwoCoords.x)
        {
            //int pos = (roomOneCoords.y * (int)dungeonSettings.distanceBetweenRooms + roomTwoCoords.y * (int)dungeonSettings.distanceBetweenRooms) / 2;
            int pos = (roomOneCoords.y * 200 + roomTwoCoords.y * 200) / 2;
            print($"ROom1Coords = {roomOneCoords} and room2Coords = {roomTwoCoords} and pos = {pos}");
            //Instantiate(dungeonSettings.corridorStepPrefab, new Vector2(roomOneCoords.x * dungeonSettings.distanceBetweenRooms, pos), Quaternion.identity);
            GameObject corridorCentralStep = Instantiate(dungeonSettings.corridorStepPrefab, mapContent.transform);
            corridorCentralStep.transform.localPosition = new Vector2(roomOneCoords.x * 200, pos);
        }
        
    }

    private Vector3 GetNextPos(Room room)
    {
        if (room == null) return Vector3.zero;
        var nextPos = room.GetRoomPosition() + GetNextTurn();
        //print(nextPos);
        if (positionToRoom.ContainsKey(nextPos)) return GetNextPos(room);
        //print(nextPos);
        return nextPos;
    }

    private Room CreateRoom(DungeonSettings settings, Vector3 position, Vector2Int coords)
    {
        Vector3 pos = Vector3.zero;
        if (rooms.Count > 0)
        {
            pos = new Vector3(pos.x + settings.distanceBetweenRooms * coords.x, pos.y + settings.distanceBetweenRooms * coords.y, pos.z);
            print(pos);
        }
        //GameObject roomInstance = Instantiate(settings.roomPrefab, pos, Quaternion.identity);
        GameObject roomInstance = Instantiate(dungeonSettings.roomPrefab, mapContent.transform);
        pos = new Vector3(200f * coords.x, 200f * coords.y, 0);
        roomInstance.transform.localPosition = pos;
        Room newRoom = new Room(position, roomInstance, coords, rooms.Count + 1);       
        return newRoom;
    }

    private Vector3 GetNextTurn()
    {
        float offset = dungeonSettings.distanceBetweenRooms;
        float randomNum = Random.Range(0, 1f);
        if (randomNum < 0.7f) return new Vector3(-offset, 0, 0);
        if (randomNum < 0.8f) return new Vector3(offset, 0, 0);
        if (randomNum < 0.9f) return new Vector3(0, -offset, 0);
        if (randomNum < 1f) return new Vector3(0, offset, 0);   

        return Vector3.up;
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

    public struct ConnectionPair
    {
        public int roomNumber1;
        public int roomNumber2;
    }
    // connection logic. У комнат есть номера и все номера соединяются последовательно. Если комната стоит на одном иксе или игреке, но номера отличаются больше чем на 1, то комната соединяется с вероятностью.
}

