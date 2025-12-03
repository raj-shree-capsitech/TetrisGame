//using UnityEngine;

//public class TetrominoMove : MonoBehaviour
//{
//    private float previoustime;
//    private float falltime=0.3f;
//    public static int width = 10;
//    public static int height = 20;
//    public static int score = 0;
//    public Vector3 rotation;  //rotation of the tetromino
//    public static Transform[,] grid=new Transform[width,height];

//    void Update()
//    {
//        if(Input.GetKeyDown(KeyCode.LeftArrow))
//        {
//            transform.position += new Vector3(-1, 0, 0);
//            if (!ValidMove())
//            {
//                transform.position -= new Vector3(-1, 0, 0);
//            }
//        }
//        else if (Input.GetKeyDown(KeyCode.RightArrow))
//        {
//            transform.position += new Vector3(1, 0, 0);
//            if (!ValidMove())
//            {
//                transform.position -= new Vector3(1, 0, 0);
//            }
//        }
//        else if(Input.GetKeyDown(KeyCode.UpArrow))  //rotation
//        {
//            transform.RotateAround(transform.TransformPoint(rotation), new Vector3(0, 0, 1), 90);
//            if (!ValidMove())
//            {
//                transform.RotateAround(transform.TransformPoint(rotation), new Vector3(0, 0, 1), -90);
//            }
//        }

//        if (Time.time - previoustime > (Input.GetKey(KeyCode.DownArrow) ? falltime / 10 : falltime))  //tetromino fall FASTER BY PRESSING DOWNARROW KEY
//        {
//            transform.position += new Vector3(0, -1, 0);
//            if (!ValidMove())
//            {
//                transform.position -= new Vector3(0, -1, 0);
//                AddToGrid();
//                CheckLines();
//                this.enabled = false;    //jb move kr rhe ho to spawn disable kr rhe h
//                FindObjectOfType<SpawnTetro>().SpawnTetromino();  
//            }
//            previoustime = Time.time;
//        }
//    }

//    void CheckLines()
//    {
//        for(int i=height-1;i>=0;i--)
//        {
//            if(HasLine(i))
//            {
//                DeleteLine(i);
//                RowDown(i);
//                Score(1);
//            }
//        }
//    }

//    bool HasLine(int i)
//    {
//        for(int j=0;j<width;j++)
//        {
//            if (grid[j, i] == null)
//            {
//                return false;
//            }
//        }
//        return true;
//    }

//    void DeleteLine(int i)
//    {
//        for (int j = 0; j < width; j++)
//        {
//            Destroy(grid[j,i].gameObject);
//            grid[j, i] = null;
//        }
//    }

//    void RowDown(int i)
//    {
//        for (int y = i; y < height; y++)
//        {
//            for (int x = 0; x < width; x++)
//            {
//                if (grid[x, y] != null)
//                {
//                    grid[x, y - 1] = grid[x, y];
//                    grid[x, y] = null;
//                    grid[x, y - 1].transform.position -= new Vector3(0, 1, 0);
//                }
//            }
//        }
//    }

//    void AddToGrid()
//    {
//        foreach(Transform t in transform)
//        {
//            int roundX = Mathf.RoundToInt(t.transform.position.x);
//            int roundY = Mathf.RoundToInt(t.transform.position.y);

//            grid[roundX, roundY] = t;
//        }
//    }

//    public bool ValidMove()   //FOR MOVE WITHIN THE BACKGROUND LFFT/RIGHT/BOTTOM
//    {
//        foreach(Transform t in transform)
//        {
//            int roundX=Mathf.RoundToInt(t.transform.position.x);
//            int roundY = Mathf.RoundToInt(t.transform.position.y);

//            if(roundX<0 || roundX>=width || roundY<0 || roundY>=height)
//            {
//                return false;
//            }

//            if(grid[roundX, roundY]!=null)
//            {
//                return false;
//            }
//        }
//        return true;
//    }

//    public void Score(int value)
//    {
//        score += value;
//        UIManager.instance.UpdateScore(score);
//    }
//}

using UnityEngine;

public class TetrominoMove : MonoBehaviour
{
    private float previoustime;
    private float falltime = 0.8f;

    public static int width = 10;
    public static int height = 20;

    public static int score = 0;
    public Vector3 rotation;

    public static Transform[,] grid = new Transform[width, height];

    private Vector2 startTouchPos;
    private bool swipedDown = false;

    void Update()
    {
        HandleTouchControls(); 

        // Auto fall logic stays the same
        if (Time.time - previoustime > (swipedDown ? falltime / 10 : falltime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckLines();
                this.enabled = false;
                FindObjectOfType<SpawnTetro>().SpawnTetromino();
            }
            previoustime = Time.time;
            swipedDown = false;
        }
    }

    void HandleTouchControls()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        // TAP DETECTION
        if (touch.phase == TouchPhase.Ended)
        {
            float x = touch.position.x;
            float y = touch.position.y;

            // SCREEN WIDTH
            float w = Screen.width;
            float h = Screen.height;

            // LEFT TAP → MOVE LEFT
            if (x < w * 0.33f)
                MoveLeft();

            // RIGHT TAP → MOVE RIGHT
            else if (x > w * 0.66f)
                MoveRight();

            // MIDDLE TAP → ROTATE
            else
                RotatePiece();
        }

        // SWIPE DOWN DETECTION
        if (touch.phase == TouchPhase.Began)
        {
            startTouchPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            Vector2 delta = touch.position - startTouchPos;

            if (delta.y < -80f)   // swipe downward
            {
                swipedDown = true;
            }
        }
    }

    void MoveLeft()
    {
        transform.position += new Vector3(-1, 0, 0);
        if (!ValidMove())
            transform.position -= new Vector3(-1, 0, 0);
    }

    void MoveRight()
    {
        transform.position += new Vector3(1, 0, 0);
        if (!ValidMove())
            transform.position -= new Vector3(1, 0, 0);
    }

    void RotatePiece()
    {
        transform.RotateAround(transform.TransformPoint(rotation), new Vector3(0, 0, 1), 90);
        if (!ValidMove())
            transform.RotateAround(transform.TransformPoint(rotation), new Vector3(0, 0, 1), -90);
    }

    void CheckLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
                Score(1);
            }
        }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void AddToGrid()
    {
        foreach (Transform t in transform)
        {
            int roundX = Mathf.RoundToInt(t.position.x);
            int roundY = Mathf.RoundToInt(t.position.y);

            grid[roundX, roundY] = t;
        }
    }

    public bool ValidMove()
    {
        foreach (Transform t in transform)
        {
            int roundX = Mathf.RoundToInt(t.position.x);
            int roundY = Mathf.RoundToInt(t.position.y);

            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
                return false;

            if (grid[roundX, roundY] != null)
                return false;
        }
        return true;
    }

    public void Score(int value)
    {
        score += value;
        UIManager.instance.UpdateScore(score);
    }
}


