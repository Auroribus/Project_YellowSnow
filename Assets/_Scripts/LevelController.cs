using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public static LevelController instance;

    public List<GameObject> Rooms = new List<GameObject>();
    public GameObject RoomParent;

    private Vector2 spawn_position;
    private int rows = 19, columns = 35;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private int count;

    public void CreateRoom(Transform door_transform, DoorDirection door_direction)
    {
        switch (door_direction)
        {
            case DoorDirection.bottom:  //bottom
                spawn_position = new Vector2(door_transform.position.x - (Mathf.Floor(columns / 2)), door_transform.position.y - rows);
                break;
            case DoorDirection.left:    //left
                spawn_position = new Vector2(door_transform.position.x - columns, door_transform.position.y - (Mathf.Floor(rows / 2)));
                break;
            case DoorDirection.right:   //right
                spawn_position = new Vector2(door_transform.position.x + 1, door_transform.position.y - (Mathf.Floor(rows / 2)));
                break;
        }

        Rooms.Add(Instantiate(RoomParent, spawn_position, Quaternion.identity));

        GameController.instance.Rooms.text = "Rooms: " + Rooms.Count;

        //only change if using varying room sizes
        Rooms[Rooms.Count - 1].GetComponent<RoomCreator>().rows = rows;
        Rooms[Rooms.Count - 1].GetComponent<RoomCreator>().columns = columns;

        Rooms[Rooms.Count - 1].GetComponent<RoomCreator>().room_type = RoomType.general;
        Rooms[Rooms.Count - 1].GetComponent<RoomCreator>().GenerateRoom(door_direction);
    }
}
