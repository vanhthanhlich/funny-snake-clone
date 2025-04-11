using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{

    private Tilemap tm;
    private Canvas cv;
    private Button btn;
    public Snake snake;
    public Apple apple;

    private void Awake()
    {
        tm = GetComponentInChildren<Tilemap>();
        cv = GetComponentInChildren<Canvas>();
        cv.enabled = false;

        btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(ResetGame);

        snake = GetComponentInChildren<Snake>();
        apple = GetComponentInChildren<Apple>();


        snake.Initialize(this);
        apple.Initialize(this);
    }

    private void Start()
    {
        apple.Spawn(snake.SnakeBody);
    }

    public void GameOver()
    {
        cv.enabled = true;
    }

    public void Set(List<Vector3Int> Positions, Tile tl)
    {
        foreach (Vector3Int Position in Positions) tm.SetTile(Position, tl);
    }

    public void Clear(List<Vector3Int> Positions)
    {
        foreach (Vector3Int Position in Positions) tm.SetTile(Position, null);
    }

    private void ResetGame()
    {
        for(int i = Const.xMin; i < Const.xMax; i++)
        {
            for(int j = Const.yMin; j < Const.yMax; j++)
                tm.SetTile(new Vector3Int(i, j), null);
        }

        snake = GetComponentInChildren<Snake>();
        snake.Initialize(this);
        apple.Spawn(snake.SnakeBody);
        cv.enabled = false;
    }

}
