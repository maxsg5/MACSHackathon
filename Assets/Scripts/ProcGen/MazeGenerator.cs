using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{

    // [SerializeField]
    // private AstarPath astarPath;
    
    [SerializeField]
    private MazeCell mazeCellPrefab;
    
    [SerializeField]
    private int mazeWidth, mazeHeight;
    
    private MazeCell[,] mazeGrid;

    [SerializeField] 
    private int seed;

    void Start()
    {
        Random.InitState(seed);
        
        mazeGrid = new MazeCell[mazeWidth*10, mazeHeight*10];

        for (int x = 0; x < mazeWidth*10; x+=10)
        {
            for (int y = 0; y < mazeHeight*10; y+=10)
            {
                MazeCell newCell = Instantiate(mazeCellPrefab, new Vector3(x, y, 0), Quaternion.identity);
                mazeGrid[x, y] = newCell;
            }
        }
        
        GenerateMaze(null, mazeGrid[0, 0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetMaze();
        }
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);
        
            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
        
        var firstCell = mazeGrid[0, 0];
        //var lastCell = mazeGrid[mazeWidth*10 - 10, mazeHeight*10 - 10];
        firstCell.ClearLeftWall();
        //lastCell.ClearTopWall();
        
        // astarPath.Scan();
    }
    
    private void ResetMaze()
    {
        foreach (var cell in mazeGrid)
        {
            cell.Unvisit();
            cell.AddAllWalls();
        }
        GenerateMaze(null, mazeGrid[0, 0]);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();

    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int y = (int)currentCell.transform.position.y;

        if (x + 10 < mazeWidth * 10)
        {
            var cellToRight = mazeGrid[x + 10, y];
            if (!cellToRight.isVisited)
            {
                yield return cellToRight;
            }
        }
        
        if (x - 10 >= 0)
        {
            var cellToLeft = mazeGrid[x - 10, y];
            if (!cellToLeft.isVisited)
            {
                yield return cellToLeft;
            }
        }
        
        if (y + 10 < mazeHeight * 10)
        {
            var cellToTop = mazeGrid[x, y + 10];
            if (!cellToTop.isVisited)
            {
                yield return cellToTop;
            }
        }
        
        if (y - 10 >= 0)
        {
            var cellToBottom = mazeGrid[x, y - 10];
            if (!cellToBottom.isVisited)
            {
                yield return cellToBottom;
            }
        }
        
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }
        
        if(previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }
        
        if(previousCell.transform.position.y < currentCell.transform.position.y)
        {
            previousCell.ClearTopWall();
            currentCell.ClearBottomWall();
            return;
        }
        
        if(previousCell.transform.position.y > currentCell.transform.position.y)
        {
            previousCell.ClearBottomWall();
            currentCell.ClearTopWall();
            return;
        }
    }
}
