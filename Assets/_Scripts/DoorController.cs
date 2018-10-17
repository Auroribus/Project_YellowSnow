using UnityEngine;

public enum DoorDirection
{
    top, right, bottom, left
}

public class DoorController : MonoBehaviour {

    public bool is_open = false;
    public bool was_open = false;
    public bool trigger_new_room = false;

    public DoorDirection door_direction;

    private Animator an;

    public GameObject room_parent;

    private void Awake()
    {
        an = GetComponent<Animator>();
    }
    
    public void SetDoorRotation(int rotation)
    {
        transform.eulerAngles = new Vector3(0f, rotation, 0f);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(collision.tag == "Player" && !is_open && room_parent.GetComponent<RoomCreator>().room_type == RoomType.start)
        {
            is_open = true;
            DoorState(true);
            LevelController.instance.CreateRoom(transform, door_direction);     
        }
        else if(collision.tag == "Player" && !is_open && was_open && room_parent.GetComponent<RoomCreator>().room_cleared)
        {
            is_open = true;
            DoorState(true);
        }
        else if(collision.tag == "Player" && !is_open && !was_open && room_parent.GetComponent<RoomCreator>().room_cleared)
        {
            is_open = true;
            DoorState(true);
            LevelController.instance.CreateRoom(transform, door_direction);
        }
        else if(collision.tag == "Player" && is_open &&  was_open)
        {
            Camera.main.GetComponent<CameraController>().set_room_point_once = false;
            Camera.main.GetComponent<CameraController>().room_middle_point = room_parent.GetComponent<RoomCreator>().middle_point;
            Camera.main.GetComponent<CameraController>().active_room = room_parent.GetComponent<RoomCreator>();
        }
    }

    public void DoorState(bool open)
    {
        if (open)
        {
            an.SetBool("door_open", open);
            was_open = true;
            is_open = true;
        }
        else if(!open)
        {
            is_open = false;
            an.SetBool("door_open", open);
        }
    }
}
