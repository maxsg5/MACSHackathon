using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{

    // [SerializeField]
    // private AstarPath astarPath;
    
    [SerializeField]
    private List<RoomConfig> roomConfigs;

    [SerializeField]
    private int roomSize = 10;  // Size of each room in tiles
    
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private TileBase wallTile;

    [SerializeField] 
    private TileBase floorTile;
    
    [SerializeField]
    private float delay = 0.1f;
   
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

        InitializeMaze();
        //StartCoroutine(GenerateMaze(new Vector3Int(0, 0, 0)));
        //GenerateMaze(new Vector3Int(0, 0, 0));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetMaze();
        }
    }
    
    private void InitializeMaze()
    {
        for (int x = 0; x < mazeWidth * roomSize; x += roomSize)
        {
            for (int y = 0; y < mazeHeight * roomSize; y += roomSize)
            {
                RoomConfig roomConfig = roomConfigs[Random.Range(0, roomConfigs.Count)];
                CreateRoom(new Vector3Int(x, y, 0), roomConfig);
            }
        }
    }
    
    private void CreateRoom(Vector3Int origin, RoomConfig config)
    {
        for (int x = 0; x < roomSize; x++)
        {
            for (int y = 0; y < roomSize; y++)
            {
                Vector3Int position = origin + new Vector3Int(x, y, 0);
                if (x == 0) tilemap.SetTile(position, config.westWallTile);
                else if (x == roomSize - 1) tilemap.SetTile(position, config.eastWallTile);
                else if (y == 0) tilemap.SetTile(position, config.southWallTile);
                else if (y == roomSize - 1) tilemap.SetTile(position, config.northWallTile);
                else tilemap.SetTile(position, config.floorTile);
            }
        }
    }

    private IEnumerator GenerateMaze(Vector3Int currentPos)
    {
        ClearTile(currentPos);
        List<Vector3Int> unvisitedNeighbors = GetUnvisitedNeighbors(currentPos);

        while (unvisitedNeighbors.Count > 0)
        {
            Vector3Int nextPos = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
            ClearTile(nextPos);
            ClearWallBetween(currentPos, nextPos);
            yield return new WaitForSeconds(delay);
            yield return StartCoroutine(GenerateMaze(nextPos));
            unvisitedNeighbors = GetUnvisitedNeighbors(currentPos);
        }
    }

    
    private void ResetMaze()
    {
        tilemap.ClearAllTiles();
        InitializeMaze();
        GenerateMaze(new Vector3Int(0, 0, 0));
    }

    private void ClearTile(Vector3Int position)
    {
        tilemap.SetTile(position, floorTile);
    }
    
    private List<Vector3Int> GetUnvisitedNeighbors(Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

        foreach (Vector3Int dir in directions)
        {
            Vector3Int neighborPos = position + dir;
            if (tilemap.HasTile(neighborPos) && tilemap.GetTile(neighborPos) == wallTile)
            {
                neighbors.Add(neighborPos);
            }
        }

        return neighbors;
    }
    
    private void ClearWallBetween(Vector3Int pos1, Vector3Int pos2)
    {
        Vector3Int wallPos = (pos1 + pos2) / 2;
        ClearTile(wallPos);
    }
}
