using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D r_body;

    public float input_speed;
    private float horizontal_input, vertical_input;

	// Use this for initialization
	void Start () {
        r_body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        horizontal_input = Input.GetAxis("Horizontal");
        vertical_input = Input.GetAxis("Vertical");

        r_body.velocity = new Vector2(horizontal_input, vertical_input) * input_speed;
	}
}
