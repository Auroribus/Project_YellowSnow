using UnityEngine;

public class ScoreDrop : MonoBehaviour {

    public float score_value;
    private Rigidbody2D rb;

    private float force_x;
    private float force_y;

    Vector2 converted_position;
    public bool position_set = false;

    private float move_speed;
    public float destroy_distance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        converted_position = Camera.main.ScreenToWorldPoint(Score.instance.transform.position);

        force_x = Random.Range(-5,5);
        force_y = Random.Range(1,3);

        rb.AddForce(new Vector2(force_x, force_y), ForceMode2D.Impulse);
        Invoke("FreezePosition", .5f);

        move_speed = Random.Range(2,4);
    }

    private void Update()
    {
        RotateItem();

        if (position_set)
        {
            transform.position = Vector2.Lerp(transform.position, converted_position, Time.deltaTime * move_speed);
            
            float distance_y = Mathf.Abs(transform.position.y - converted_position.y);
            
            if(distance_y <= destroy_distance)
            {
                Score.instance.new_value += score_value;
                Destroy(gameObject);
            }
        }
    }      

    private void RotateItem()
    {
        transform.Rotate(0, 180 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !position_set)
        {
            if(!position_set)
                position_set = true;
        }
    }

    private void FreezePosition()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
}
