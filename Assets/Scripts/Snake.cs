using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class Snake : MonoBehaviour
{

    [SerializeField] private Tile snakeTile;

    private Grid grid;
    private List<Vector3> snakeSegments;
    Vector3 currentDirection;
    Vector3 headPosition;
    private int snakeLength = 5;
    private float moveSpeed = 12f;
    private bool dead;

    private Vector3Int Head
    {
        get
        {
            return Round(headPosition);
        }
    }

    Vector3Int Round(Vector3 v)
    {
        int x = Mathf.CeilToInt(v.x);
        int y = Mathf.CeilToInt(v.y);
        return new Vector3Int(x, y);
    }
    public List<Vector3Int> SnakeBody
    {
        get
        {
            List<Vector3Int> body = new List<Vector3Int>();
            foreach(Vector3 pos in snakeSegments) { 
                body.Add(Round(pos));
            }
            return body;
        }
    }

    public void Initialize(Grid grid)
    {
        this.grid = grid;
        snakeSegments = new List<Vector3>() { new Vector3(0, 0, 0), };
        headPosition = Vector3.zero;
        currentDirection = new Vector3Int(0, 0, 0);
        snakeLength = 5;
        dead = false;
    }


    private void Grow()
    {
        snakeLength++;
    }

    void Set()
    {
        grid.Set(SnakeBody, snakeTile);
    }

    void Clear()
    {
        grid.Clear(SnakeBody);
    }

    Vector3 GetInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) return Vector3Int.down;
        if (Input.GetKeyDown(KeyCode.UpArrow)) return Vector3Int.up;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) return Vector3Int.left;
        if (Input.GetKeyDown(KeyCode.RightArrow)) return Vector3Int.right;

        return currentDirection;
    }

    private void Update()
    {
        if(dead) return;

        Clear();

        Vector3 dir = GetInput();

        Move(dir);
        Eat();

        Set();

    }
    private void Eat()
    {
        if (grid.apple.Position == Head)
        {
            Grow();
            grid.apple.Spawn(SnakeBody);
        }
    }

    private void LogBody()
    {
        string str = "";
        foreach (var x in snakeSegments) str += x.ToString() + " ";
        Debug.Log(str);
    }

    private void Die()
    {
        dead = true;
        grid.GameOver();
    }

    private void Wrap(ref float x, int min, int max)
    {
        if (x < min) x = max;
        if (x > max) x = min; 
    }

    private void Move(Vector3 dir)
    {
        if (dir == currentDirection * -1) dir = currentDirection;

        headPosition += dir * moveSpeed * Time.deltaTime;

        Wrap(ref headPosition.x, Const.xMin - 1, Const.xMax - 1);
        Wrap(ref headPosition.y, Const.yMin - 1, Const.yMax - 1);

        var last = snakeSegments[snakeSegments.Count - 1];
        if (Round(last) != Round(headPosition)) {
            if (SnakeBody.Contains(Round(headPosition))) {
                Die();
            }
            snakeSegments.Add(headPosition);
        }

        if(snakeSegments.Count > snakeLength) { 
            snakeSegments.RemoveAt(0);
        }

        //LogBody();
        
        currentDirection = dir;
    }

}
