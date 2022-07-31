using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    [SerializeField] DungeonGenerator generator;
    
    private Dictionary<Vector2Int, Room> coordsToRoom = new Dictionary<Vector2Int, Room>();
    private Dictionary<Room, List<Room>> roomConnections = new Dictionary<Room, List<Room>>();
    private Dictionary<string, List<Room>> connections = new Dictionary<string, List<Room>>();
    private List<Room> rooms = new List<Room>();
    private List<Corridor> corridors = new List<Corridor>();

    private void Start()
    {
        
    }
}
