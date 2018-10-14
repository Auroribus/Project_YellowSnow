using UnityEngine;

public enum RoomTypes
{
    start,
    general
}

public class RoomCreator : MonoBehaviour {

    public GameObject Wall_piece;   
    public GameObject Floor_piece;  
    public GameObject Door_piece;   
    public GameObject Player_prefab;

    private GameObject[,] room_grid;
    public int rows = 11, columns = 15;
    public int amount_of_enemies = 0;

    public int dead_end_chance = 5;

    public RoomTypes room_type;

    [System.NonSerialized] public Transform WallPieces, DoorPieces, FloorPieces;

    public bool generate_onStart = false;

    public Vector2 middle_point;

    private void Awake()
    {
        WallPieces = transform.Find("Wall Pieces");
        DoorPieces = transform.Find("Door Pieces");
        FloorPieces = transform.Find("Floor Pieces");
    }

    // Use this for initialization
    void Start () {

        if (generate_onStart)
            GenerateRoom(DoorDirection.bottom);
	}

    public void GenerateRoom(DoorDirection door_direction)
    {
        switch(room_type)
        {
            case RoomTypes.start:
                room_grid = new GameObject[rows, columns];
                RoomType_Start();
                break;
            case RoomTypes.general:
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
                //edges
                if (row == 0 || row == rows - 1 || col == 0 || col == columns - 1)
                {
                    //Create door in bottom row
                    if (row == 0 && col == Mathf.Floor(columns / 2))
                    {
                        room_grid[row, col] = Instantiate(Door_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.Euler(new Vector3(0, 0, 180)), DoorPieces);
                        room_grid[row, col].GetComponent<DoorController>().door_direction = DoorDirection.bottom;
                        room_grid[row, col].GetComponent<DoorController>().room_parent = gameObject;
                    }
                    //set wall
                    else
                    {
                        room_grid[row, col] = Instantiate(Wall_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.identity, WallPieces);
                    }
                }
                else
                {
                    room_grid[row, col] = Instantiate(Floor_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.identity, FloorPieces);

                    //Create player in middle of room
                    if (row == Mathf.Floor(rows / 2) && col == Mathf.Floor(columns / 2))
                    {
                        middle_point = new Vector2(transform.position.x + col, transform.position.y + row);

                        Instantiate(Player_prefab, middle_point, Quaternion.identity);

                        //Set camera position, middle point and player reference
                        Camera.main.transform.position = new Vector3(middle_point.x, middle_point.y, -100f);
                        Camera.main.GetComponent<CameraController>().player = GameObject.FindWithTag("Player").transform;
                        Camera.main.GetComponent<CameraController>().room_middle_point = middle_point;

                        //Set camera on player position
                        //Camera.main.GetComponent<CameraController>().player_is_set = true;
                    }
                }
            }
        }
    }

    private void RoomType_General(DoorDirection door_direction)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
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
                            room_grid[row, col] = Instantiate(Door_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.Euler(new Vector3(0, 0, 0)), DoorPieces);
                            
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
                            room_grid[row, col] = Instantiate(Wall_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.identity, WallPieces);
                        }
                        else if (room_grid[row, col] == null)
                        {
                            room_grid[row, col] = Instantiate(Door_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.Euler(new Vector3(0, 0, 270)), DoorPieces);
                            
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
                            room_grid[row, col] = Instantiate(Door_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.Euler(new Vector3(0, 0, 180)), DoorPieces);
                            
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
                            room_grid[row, col] = Instantiate(Wall_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.identity, WallPieces);
                        }
                        else if (room_grid[row, col] == null)
                        {
                            room_grid[row, col] = Instantiate(Door_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.Euler(new Vector3(0, 0, 90)), DoorPieces);
                           
                            room_grid[row, col].GetComponent<DoorController>().door_direction = DoorDirection.left;
                            room_grid[row, col].GetComponent<DoorController>().room_parent = gameObject;

                            if (door_direction == DoorDirection.right)
                                room_grid[row, col].GetComponent<DoorController>().DoorState(true);
                        }
                    }
                    //set wall
                    else
                    {
                        room_grid[row, col] = Instantiate(Wall_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.identity, WallPieces);
                    }
                }
                else
                {
                    room_grid[row, col] = Instantiate(Floor_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.identity, FloorPieces);
                }
            }
        }
    }
    #endregion
}
