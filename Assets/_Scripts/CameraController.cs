using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform player;
    public Vector2 room_middle_point;
    private Vector2 old_middle_point;

    public float treshold_x;
    public float treshold_y;
    public float scroll_speed;
    private float scroll_speed_multiplier;

    public bool set_room_point_once = false;

    public RoomCreator active_room;
    
	// Update is called once per frame
	void Update () {

        if(player != null)
        {
            if(room_middle_point == old_middle_point)
            {
                if(set_room_point_once)
                    set_room_point_once = false;

                //MoveCameraWithPlayer();
            }
            else 
            {
                if (room_middle_point.x < transform.position.x || room_middle_point.x > transform.position.x)
                {
                    scroll_speed_multiplier = 20;
                }
                else if (room_middle_point.y < transform.position.y || room_middle_point.y > transform.position.y)
                {
                    scroll_speed_multiplier = 10;
                }

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(room_middle_point.x, room_middle_point.y, -100f), scroll_speed * Time.deltaTime * scroll_speed_multiplier);
                
                if (!set_room_point_once)
                {
                    set_room_point_once = true;
                    Invoke("SetMiddlePoint_Old", 2f);

                    player.GetComponent<PlayerController>().input_speed = 0;
                }
            }
        }
	}

    private void SetMiddlePoint_Old()
    {
        old_middle_point = room_middle_point;
        CancelInvoke("SetMiddlePoint_Old");

        LevelController.instance.crossroad_room = active_room;

        player.GetComponent<PlayerController>().input_speed = 10;

        if (active_room.room_type != RoomType.start && !active_room.spawn_control.has_spawned )
        {
            foreach(Transform t in active_room.DoorPieces)
            {
                t.GetComponent<DoorController>().DoorState(false);
            }
            active_room.spawn_control.SpawnEnemies();
        }
    }

    private void MoveCameraWithPlayer()
    {
        //move camera left if player on left side of room
        if (player.position.x < room_middle_point.x - treshold_x)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(room_middle_point.x - (treshold_x / 2), transform.position.y, -100f), scroll_speed * Time.deltaTime);
        }
        //move camera right if player on right side of room
        else if (player.position.x > room_middle_point.x + treshold_x)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(room_middle_point.x + (treshold_x / 2), transform.position.y, -100f), scroll_speed * Time.deltaTime);
        }
        //move camera back to middle
        else if (player.position.x > room_middle_point.x - treshold_x && player.position.x < room_middle_point.x + treshold_x)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(room_middle_point.x, transform.position.y, -100f), scroll_speed * Time.deltaTime);
        }

        //move camera down if player on bottom side of room
        if (player.position.y < room_middle_point.y - treshold_y)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, room_middle_point.y - (treshold_y / 5), -100f), scroll_speed * Time.deltaTime);
        }
        //move camera up if player on top side of room
        else if (player.position.y > room_middle_point.y + treshold_y)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, room_middle_point.y + (treshold_y / 5), -100f), scroll_speed * Time.deltaTime);
        }
        //move camera back to middle
        else if (player.position.y > room_middle_point.y - treshold_y && player.position.y < room_middle_point.y + treshold_y)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, room_middle_point.y, -100f), scroll_speed * Time.deltaTime);
        }
    }
    
}
