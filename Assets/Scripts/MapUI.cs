using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField] GameObject mapContent;
    [SerializeField] GameObject roomUIprefab;
    [SerializeField] GameObject horizontalCorridorUIPrefab;
    [SerializeField] GameObject verticalCorridorUIPrefab;


    [SerializeField] float distanceBetweenRooms;

    public void CreateDungeonMapUI(Dungeon dungeon)
    {
        ClearMap();
        foreach (Room room in dungeon.GetRooms())
        {
            CreateRoomUI(room);
        }
        foreach (Corridor corridor in dungeon.GetCorridors())
        {
            CreateCorridorUI(corridor);
        }
    }

    private void ClearMap()
    {
        foreach (Transform child in mapContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateCorridorUI(Corridor corridor)
    {
        GameObject corridorInstance;
        Vector2 centralStepPosition;
        if (corridor.Orientation == CorridorOrientation.Horizontal)
        {
            float xPos = (corridor.Room1.Coords.x * distanceBetweenRooms + corridor.Room2.Coords.x * distanceBetweenRooms) / 2;
            centralStepPosition = new Vector2(xPos, corridor.Room1.Coords.y * distanceBetweenRooms);
            corridorInstance = Instantiate(horizontalCorridorUIPrefab, mapContent.transform);
        }
        else
        {
            float yPos = (corridor.Room1.Coords.y * distanceBetweenRooms + corridor.Room2.Coords.y * distanceBetweenRooms) / 2;
            centralStepPosition = new Vector2(corridor.Room1.Coords.x * distanceBetweenRooms, yPos);
            corridorInstance = Instantiate(verticalCorridorUIPrefab, mapContent.transform);
        }
        corridorInstance.transform.localPosition = centralStepPosition;
    }

    private void CreateRoomUI(Room room)
    {
        GameObject roomInstance = Instantiate(roomUIprefab, mapContent.transform);
        var pos = new Vector3(distanceBetweenRooms * room.Coords.x, distanceBetweenRooms * room.Coords.y, 0);
        roomInstance.transform.localPosition = pos;
    }



    private void Update()
    {
        float wheelScroll = Input.GetAxis("Mouse ScrollWheel");
        if (wheelScroll != 0)
        {
            mapContent.transform.localScale += new Vector3(wheelScroll, wheelScroll, 0);
        }
    }
}
