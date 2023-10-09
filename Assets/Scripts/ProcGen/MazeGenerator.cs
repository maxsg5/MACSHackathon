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
    private GameObject safeZonePrefab;  // Assign your safe zone tile in the inspector
    
    [SerializeField]
    private List<RoomConfig> roomConfigs;

    [SerializeField]
    private int roomSize = 10;  // Size of each room in tiles
    
    [SerializeField]
    private Tilemap tilemap;
    
    [SerializeField]
    private float delay = 0.1f;
    
    [SerializeField]
    private int mazeWidth, mazeHeight;
    
    private MazeCell[,] mazeGrid;

    [SerializeField] 
    private int seed;
    
    private Room[,] rooms;
    
    private int roomCounter = 0; //used for CreateSafeZone()

    void Start()
    {
        Random.InitState(seed);

        InitializeMaze();
        
        rooms[0, 0].TearDownWall(Direction.West);
        rooms[0, 0].TearDownWall(Direction.South);
        StartCoroutine(GenerateMaze(rooms[0, 0]));
        CreateSafeZones();
    }
    
    private void InitializeMaze()
    {
        // Initialize the rooms array
        rooms = new Room[mazeWidth, mazeHeight];
        
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                Vector3Int roomOrigin = new Vector3Int(x * roomSize, y * roomSize, 0);
                RoomConfig roomConfig = roomConfigs[Random.Range(0, roomConfigs.Count)];
                
                // Create a new Room object
                Room newRoom = new Room(roomOrigin, roomSize, tilemap, roomConfig);
            
                // Generate the room
                newRoom.GenerateRoom();
            
                // Store the room in the rooms array
                rooms[x, y] = newRoom;
            }
        }
    }

    private IEnumerator GenerateMaze(Room currentRoom)
    {
        
        currentRoom.MarkAsVisited();
        
        //change the room's test object to the visited version
        currentRoom.ChangeFloorTile(currentRoom.RoomConfig.floorTile);
        
        List<Room> unvisitedNeighbors = GetUnvisitedNeighbors(currentRoom);

        while (unvisitedNeighbors.Count > 0)
        {
            Room nextRoom = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
            ClearWallBetween(currentRoom, nextRoom);
            yield return new WaitForSeconds(delay);
            yield return StartCoroutine(GenerateMaze(nextRoom));
            unvisitedNeighbors = GetUnvisitedNeighbors(currentRoom);
        }
    }
    
    private List<Room> GetUnvisitedNeighbors(Room currentRoom)
    {
        List<Room> neighbors = new List<Room>();
        Vector3Int currentOrigin = currentRoom.Origin;
    
        int currentX = currentOrigin.x / roomSize;
        int currentY = currentOrigin.y / roomSize;

        // Check if the indices are within bounds before attempting to access the array
        if (currentX + 1 < mazeWidth)  // East neighbor
        {
            Room neighbor = rooms[currentX + 1, currentY];
            if (!neighbor.Visited)
                neighbors.Add(neighbor);
        }

        if (currentX - 1 >= 0)  // West neighbor
        {
            Room neighbor = rooms[currentX - 1, currentY];
            if (!neighbor.Visited)
                neighbors.Add(neighbor);
        }

        if (currentY + 1 < mazeHeight)  // North neighbor
        {
            Room neighbor = rooms[currentX, currentY + 1];
            if (!neighbor.Visited)
                neighbors.Add(neighbor);
        }

        if (currentY - 1 >= 0)  // South neighbor
        {
            Room neighbor = rooms[currentX, currentY - 1];
            if (!neighbor.Visited)
                neighbors.Add(neighbor);
        }

        return neighbors;
    }
    
    private Direction OppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.North: return Direction.South;
            case Direction.South: return Direction.North;
            case Direction.East: return Direction.West;
            case Direction.West: return Direction.East;
            default: throw new ArgumentException("Invalid direction: " + direction);
        }
    }
    
    private void ClearWallBetween(Room room1, Room room2)
    {
        Direction direction = room1.GetDirectionOf(room2);
        room1.TearDownWall(direction);
        room2.TearDownWall(OppositeDirection(direction));
    }
    
    private void CreateSafeZones()
    {
        
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                roomCounter++;
                if (roomCounter % 3 == 0)
                {
                    Room room = rooms[x, y];
                    GameObject safeZone = room.PrepareSafeZone(safeZonePrefab);
                }
            }
        }
    }
}