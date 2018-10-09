using UnityEngine;

public enum RoomTypes
{
    start,
    general,
    hall
}

public class RoomCreator : MonoBehaviour {

    public GameObject Wall_piece;   
    public GameObject Floor_piece;  
    public GameObject Door_piece;   
    public GameObject Player_prefab;

    private GameObject[,] room_grid;
    public int rows = 11, columns = 15;
    public int amount_of_enemies = 0;

    public RoomTypes room_type;

    private Transform WallPieces, DoorPieces, FloorPieces;

    public bool generate_onStart = false;

    private void Awake()
    {
        WallPieces = transform.Find("Wall Pieces");
        DoorPieces = transform.Find("Door Pieces");
        FloorPieces = transform.Find("Floor Pieces");
    }

    // Use this for initialization
    void Start () {

        if (generate_onStart)
            GenerateRoom();
	}

    public void GenerateRoom()
    {
        switch(room_type)
        {
            case RoomTypes.start:
                room_grid = new GameObject[rows, columns];
                RoomType_Start();
                break;
            case RoomTypes.general:
                room_grid = new GameObject[rows, columns];
                RoomType_General();
                break;
            case RoomTypes.hall:
                room_grid = new GameObject[rows, columns];
                RoomType_Hall();
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
                        Instantiate(Player_prefab, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.identity);
                        Camera.main.GetComponent<CameraController>().player_is_set = true;
                    }
                }
            }
        }
    }

    private void RoomType_General()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                //edges
                if (row == 0 || row == rows - 1 || col == 0 || col == columns - 1)
                {
                    //doors
                    //top
                    //top door always open
                    if (row == rows - 1 && col == Mathf.Floor(columns / 2))
                    {
                        if(room_grid[row,col] == null)
                        {
                            room_grid[row, col] = Instantiate(Door_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.Euler(new Vector3(0, 0, 0)), DoorPieces);
                            room_grid[row, col].GetComponent<DoorController>().DoorState(true);
                            room_grid[row, col].GetComponent<DoorController>().door_direction = DoorDirection.top;
                        }                        
                    }
                    //right
                    else if (row == Mathf.Floor(rows / 2) && col == columns - 1)
                    {
                        if (room_grid[row, col] == null)
                        {
                            room_grid[row, col] = Instantiate(Door_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.Euler(new Vector3(0, 0, 270)), DoorPieces);
                            room_grid[row, col].GetComponent<DoorController>().door_direction = DoorDirection.right;
                        }
                    }
                    //bottom
                    else if (row == 0 && col == Mathf.Floor(columns / 2))
                    {
                        if (room_grid[row, col] == null)
                        {
                            room_grid[row, col] = Instantiate(Door_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.Euler(new Vector3(0, 0, 180)), DoorPieces);
                            room_grid[row, col].GetComponent<DoorController>().door_direction = DoorDirection.bottom;
                        }
                    }
                    //left
                    else if (row == Mathf.Floor(rows / 2) && col == 0)
                    {
                        if (room_grid[row, col] == null)
                        {
                            room_grid[row, col] = Instantiate(Door_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.Euler(new Vector3(0, 0, 90)), DoorPieces);
                            room_grid[row, col].GetComponent<DoorController>().door_direction = DoorDirection.left;
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

    private void RoomType_Hall()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                //if (row == 0 || row == rows - 1 || col == 0 || col == columns - 1)
                if((row == 0 && col == 0) || (row == 0 && col == columns - 1) || (row == rows - 1 && col == 0) || (row == rows - 1 && col == columns - 1))
                {
                    //if (room_grid[row, col] == null)
                        room_grid[row, col] = Instantiate(Wall_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.identity, WallPieces);
                }
                else if(col == 0 || col == columns - 1)
                {
                    //if (room_grid[row, col] == null)
                        room_grid[row, col] = Instantiate(Wall_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.identity, WallPieces);
                }
                else
                {
                    //if (room_grid[row, col] == null)
                        room_grid[row, col] = Instantiate(Floor_piece, new Vector2(transform.position.x + col, transform.position.y + row), Quaternion.identity, FloorPieces);
                }
            }
        }
    }

    #endregion
}
