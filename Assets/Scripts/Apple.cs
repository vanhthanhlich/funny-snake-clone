using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Apple : MonoBehaviour
{
    public Vector3Int Position;
    [SerializeField] private Tile appleTile;
    private Grid grid;

    private List<Vector3Int> Pos { get { return new List<Vector3Int>() { Position };  } }

    public void Initialize(Grid grid)
    {
        this.grid = grid;
        Spawn(grid.snake.SnakeBody);
    }

    private void Set()
    {
        grid.Set(Pos, appleTile);
    }

    private void Clear()
    {
        grid.Clear(Pos);
    }

    private void LateUpdate()
    {
        Clear();
        Set();
    }

    public void Spawn(List<Vector3Int> pos)
    {
        List<Vector3Int> potential = new List<Vector3Int>();
        for(int i = Const.xMin; i < Const.xMax; i++)
        {
            for(int j = Const.yMin; j < Const.yMax; j++)
            {
                Vector3Int p = new Vector3Int(i, j);
                if(!pos.Contains(p)) potential.Add(p);
            }
        }

        int ind = Random.Range(0, potential.Count);

        Position = potential[ind];
    }

}
