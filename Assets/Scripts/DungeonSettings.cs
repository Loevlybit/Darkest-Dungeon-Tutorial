using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dungeon", menuName = "Create new dungeon")]
public class DungeonSettings : ScriptableObject
{
    [SerializeField] public int maxNumberOfRooms;
    [SerializeField] public int minNumberOfRooms;
    [SerializeField] public DungeonOrientation orientation;
    [SerializeField] public int distanceToBoss;
    [SerializeField] public GameObject roomPrefab;
    [SerializeField] public GameObject corridorStepPrefab;
    [SerializeField] public float distanceBetweenRooms = 4f;

}

public enum DungeonOrientation
{
    UpLeft,
    UpRight,
    DownLeft,
    DownRight,
    LeftUp,
    LeftDown,
    RightUp,
    RightDown
}
