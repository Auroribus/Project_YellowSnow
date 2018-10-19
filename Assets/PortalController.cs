using UnityEngine;

public class PortalController : MonoBehaviour {

    private TextMesh interact_text;
    private bool player_in_range = false;

    GameObject player;

    private void Start()
    {
        interact_text = GetComponentInChildren<TextMesh>();
        interact_text.text = "";
    }

    private void Update()
    {
        if(player_in_range)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                MovePlayerToPoint();
            }
        }
        else if(!player_in_range)
        {

        }
    }

    private void MovePlayerToPoint()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");

        player.transform.position = LevelController.instance.crossroad_room.middle_point;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            interact_text.text = "'f' to use";
            player_in_range = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            interact_text.text = "";
            player_in_range = false;
        }
    }
}
