using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public static LevelController instance;

    public List<GameObject> Rooms = new List<GameObject>();
    public GameObject RoomParent;

    private Vector2 spawn_position;
    private int rows, columns;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private int count;

    public void CreateRoom(Transform door_transform, RoomTypes room_type, DoorDirection door_direction)
    {
        if(room_type == RoomTypes.hall)
        {
            switch(door_direction)
            {
                case DoorDirection.bottom:
                    CreateHall_and_Room(door_transform, room_type, Quaternion.Euler(new Vector3(0, 0, 0)));
                    break;
                case DoorDirection.left:
                    CreateHall_and_Room(door_transform, room_type, Quaternion.Euler(new Vector3(0, 0, 90)));
                    break;
            }
        }

    }

    private void CreateHall_and_Room(Transform door_transform, RoomTypes room_type, Quaternion rotation)
    {
        //add hall, testing
        rows = Random.Range(5, 9);
        columns = 5;

        spawn_position = new Vector2(door_transform.position.x - (Mathf.Floor(columns / 2)), door_transform.position.y - rows);

        Rooms.Add(Instantiate(RoomParent, spawn_position, Quaternion.identity));
        Rooms[Rooms.Count - 1].GetComponent<RoomCreator>().rows = rows;
        Rooms[Rooms.Count - 1].GetComponent<RoomCreator>().columns = columns;

        Rooms[Rooms.Count - 1].GetComponent<RoomCreator>().room_type = room_type;
        Rooms[Rooms.Count - 1].GetComponent<RoomCreator>().GenerateRoom();
        
        Rooms[Rooms.Count - 1].GetComponent<RoomCreator>().transform.rotation = rotation;

        int old_rows = rows;

        //add another room connected to hall
        rows = Random.Range(10, 16);
        columns = Random.Range(15, 20);

        spawn_position = new Vector2(door_transform.position.x - (Mathf.Floor(columns / 2)), door_transform.position.y - (rows + old_rows));

        Rooms.Add(Instantiate(RoomParent, spawn_position, Quaternion.identity));
        Debug.Log("am rooms: " + Rooms.Count);
        Rooms[Rooms.Count - 1].GetComponent<RoomCreator>().rows = rows;
        Rooms[Rooms.Count - 1].GetComponent<RoomCreator>().columns = columns;

        Rooms[Rooms.Count - 1].GetComponent<RoomCreator>().room_type = RoomTypes.general;
        Rooms[Rooms.Count - 1].GetComponent<RoomCreator>().GenerateRoom();
    }
}
