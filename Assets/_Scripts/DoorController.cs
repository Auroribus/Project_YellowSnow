using UnityEngine;

public enum DoorDirection
{
    top, right, bottom, left
}

public class DoorController : MonoBehaviour {

    public bool is_open = false;
    public bool trigger_new_room = false;

    public DoorDirection door_direction;

    private Animator an;

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
        if(!is_open && collision.tag == "Player")
        { 
            is_open = true;
            DoorState(true);
            LevelController.instance.CreateRoom(transform, RoomTypes.hall, door_direction);
        }
    }

    public void DoorState(bool open)
    {
        if (open)
        {
            an.SetTrigger("open_door");
            is_open = true;
        }
        else
        {
            is_open = false;
            an.SetTrigger("close_door");
        }
    }
}
