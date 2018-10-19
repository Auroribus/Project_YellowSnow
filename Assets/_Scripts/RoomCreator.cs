using UnityEngine;

public class RoomCreator : MonoBehaviour {

    public GameObject Wall_piece;   
    public GameObject Floor_piece;  
    public GameObject Door_piece;
    public GameObject crossroad_portal;
    public GameObject Player_prefab;

    private GameObject[,] room_grid;
    public int rows = 11, columns = 15;

    public int dead_end_chance = 5;

    public float tile_size = 1;

    public RoomType room_type;

    [System.NonSerialized] public Transform WallPieces, DoorPieces, FloorPieces;

    public bool generate_onStart = false;
    [System.NonSerialized] public bool room_cleared = false;

    [System.NonSerialized] public Vector2 middle_point;
    private Vector2 instance_point;

    [System.NonSerialized] public SpawnController spawn_control;
    [System.NonSerialized] public Transform crossroad_room;

    private void Awake()
    {
        WallPieces = transform.Find("Wall Pieces");
        DoorPieces = transform.Find("Door Pieces");
        FloorPieces = transform.Find("Floor Pieces");

        spawn_control = GetComponent<SpawnController>();
    }

    // Use this for initialization
    void Start () {

        if (generate_onStart)
            GenerateRoom(DoorDirection.bottom);

        instance_point = new Vector2(0f, 0f);
	}

    public void GenerateRoom(DoorDirection door_direction)
    {
        switch(room_type)
        {
            case RoomType.start:
                room_grid = new GameObject[rows, columns];
                RoomType_Start();
                break;
            case RoomType.general:
                room_grid = new GameObject[rows, columns];
                RoomType_General(door_direction);
                break;
        }
    }

    #region Room Types


    private void RoomType_Start()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                //set instance point values
                instance_point.x = transform.position.x + (col * tile_size);
                instance_point.y = transform.position.y + (row * tile_size);

                //edges
                if (row == 0 || row == rows - 1 || col == 0 || col == columns - 1) //extra layer of walls || row == 1 || row == rows - 2 || col == 1 || col == columns - 2)
                {
                    //Create door in bottom row
                    if (row == 0 && col == Mathf.Floor(columns / 2))
                    {
                        room_grid[row, col] = Instantiate(Door_piece, instance_point, Quaternion.Euler(new Vector3(0, 0, 180)), DoorPieces);
                        room_grid[row, col].GetComponent<DoorController>().door_direction = DoorDirection.bottom;
                        room_grid[row, col].GetComponent<DoorController>().room_parent = gameObject;
                    }
                    //for door position, if using extra layer of walls
                    //else if(row == 0 && col == Mathf.Floor(columns/2))
                    //{
                    //    room_grid[row, col] = Instantiate(Floor_piece, instance_point, Quaternion.identity, FloorPieces);
                    //}
                    //set wall
                    else
                    {
                        room_grid[row, col] = Instantiate(Wall_piece, instance_point, Quaternion.identity, WallPieces);
                    }
                }
                else
                {
                    room_grid[row, col] = Instantiate(Floor_piece, instance_point, Quaternion.identity, FloorPieces);

                    //Create player in middle of room
                    if (row == Mathf.Floor(rows / 2) && col == Mathf.Floor(columns / 2))
                    {
                        middle_point = new Vector2((transform.position.x + col) * tile_size, (transform.position.y + row) * tile_size);
                       
                        GameObject player = GameObject.FindWithTag("Player");

                        if (player == null)
                            Instantiate(Player_prefab, middle_point, Quaternion.identity);
                        else
                            player.transform.position = middle_point;

                        //Set camera position, middle point and player reference
                        Camera.main.transform.position = new Vector3(middle_point.x, middle_point.y, -100f);
                        Camera.main.GetComponent<CameraController>().player = GameObject.FindWithTag("Player").transform;
                        Camera.main.GetComponent<CameraController>().room_middle_point = middle_point;
                        Camera.main.GetComponent<CameraController>().active_room = this;
                    }
                }
            }
        }
    }

    private void RoomType_General(DoorDirection door_direction)
    {
        //Spawn enemies for room
        int amount_enemies = 4; // Random.Range(2, 5);
        spawn_control.amount_of_enemies = amount_enemies;
       
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                //set instance point values
                instance_point.x = transform.position.x + (col * tile_size);
                instance_point.y = transform.position.y + (row * tile_size);

                //set spawn points for enemies in room
                if (spawn_control.spawn_points.Count < amount_enemies)
                {
                    if ((row == 3 && col == 3) || (row == 3 && col == columns - 3) || (row == rows - 3 && col == 3) || (row == rows - 3 && col == columns - 3))
                        spawn_control.spawn_points.Add(instance_point);
                }
                
                //Set middle point
                if (row == Mathf.Floor(rows / 2) && col == Mathf.Floor(columns / 2))
                {
                    middle_point = new Vector2(transform.position.x + col, transform.position.y + row);
                }

                //edges
                if (row == 0 || row == rows - 1 || col == 0 || col == columns - 1)
                {
                    //doors
                    //top
                    //top door always open
                    if (row == rows - 1 && col == Mathf.Floor(columns / 2) && door_direction == DoorDirection.bottom)
                    {
                        if(room_grid[row,col] == null)
                        {
                            room_grid[row, col] = Instantiate(Door_piece, instance_point, Quaternion.Euler(new Vector3(0, 0, 0)), DoorPieces);
                            
                            room_grid[row, col].GetComponent<DoorController>().door_direction = DoorDirection.top;
                            room_grid[row, col].GetComponent<DoorController>().room_parent = gameObject;

                            room_grid[row, col].GetComponent<DoorController>().DoorState(true);
                        }                        
                    }
                    //right
                    else if (row == Mathf.Floor(rows / 2) && col == columns - 1)
                    {
                        //random for dead end, but only if spawn door is not left
                        if (door_direction != DoorDirection.left && Random.Range(0, dead_end_chance) == 0)
                        {
                            room_grid[row, col] = Instantiate(Wall_piece, instance_point, Quaternion.identity, WallPieces);
                        }
                        else if (room_grid[row, col] == null)
                        {
                            room_grid[row, col] = Instantiate(Door_piece, instance_point, Quaternion.Euler(new Vector3(0, 0, 270)), DoorPieces);
                            
                            room_grid[row, col].GetComponent<DoorController>().door_direction = DoorDirection.right;
                            room_grid[row, col].GetComponent<DoorController>().room_parent = gameObject;

                            if (door_direction == DoorDirection.left)
                                room_grid[row, col].GetComponent<DoorController>().DoorState(true);
                        }
                    }
                    //bottom
                    else if (row == 0 && col == Mathf.Floor(columns / 2) && door_direction != DoorDirection.left && door_direction != DoorDirection.right)
                    {
                        if (room_grid[row, col] == null)
                        {
                            room_grid[row, col] = Instantiate(Door_piece, instance_point, Quaternion.Euler(new Vector3(0, 0, 180)), DoorPieces);
                            
                            room_grid[row, col].GetComponent<DoorController>().door_direction = DoorDirection.bottom;
                            room_grid[row, col].GetComponent<DoorController>().room_parent = gameObject;
                        }
                    }
                    //left
                    else if (row == Mathf.Floor(rows / 2) && col == 0)
                    {
                        //random for dead end, but only if spawn door is not right
                        if(door_direction != DoorDirection.right && Random.Range(0, dead_end_chance) == 0)
                        {
                            room_grid[row, col] = Instantiate(Wall_piece, instance_point, Quaternion.identity, WallPieces);
                        }
                        else if (room_grid[row, col] == null)
                        {
                            room_grid[row, col] = Instantiate(Door_piece, instance_point, Quaternion.Euler(new Vector3(0, 0, 90)), DoorPieces);
                           
                            room_grid[row, col].GetComponent<DoorController>().door_direction = DoorDirection.left;
                            room_grid[row, col].GetComponent<DoorController>().room_parent = gameObject;

                            if (door_direction == DoorDirection.right)
                                room_grid[row, col].GetComponent<DoorController>().DoorState(true);
                        }
                    }
                    //set wall
                    else
                    {
                        room_grid[row, col] = Instantiate(Wall_piece, instance_point, Quaternion.identity, WallPieces);
                    }
                }
                else
                {
                    room_grid[row, col] = Instantiate(Floor_piece, instance_point, Quaternion.identity, FloorPieces);
                }
            }
        }
        
    }
    #endregion
}
