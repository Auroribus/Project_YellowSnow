using UnityEngine;

public class CameraController : MonoBehaviour {

    private Transform player;

    public bool player_is_set = false;
	
	// Update is called once per frame
	void Update () {
		if(player_is_set)
        {
            player_is_set = false;
            player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
        if(player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y, -100f);
        }
	}
}
