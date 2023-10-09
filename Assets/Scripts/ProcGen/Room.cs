using System;
using UnityEngine;
using UnityEngine.Tilemaps;
public enum Direction
{
    North,
    South,
    East,
    West
}

[Serializable]
public class Room
{
    public Vector3Int Origin { get; private set; }
    public int RoomSize { get; private set; }
    public Tilemap Tilemap { get; private set; }
    public RoomConfig RoomConfig { get; private set; }
    public bool Visited { get; private set; } = false;

    public Room(Vector3Int origin, int roomSize, Tilemap tilemap, RoomConfig roomConfig)
    {
        Origin = origin;
        RoomSize = roomSize;
        Tilemap = tilemap;
        RoomConfig = roomConfig;
    }

    public void GenerateRoom()
    {
        for (int x = 0; x < RoomSize; x++)
        {
            for (int y = 0; y < RoomSize; y++)
            {
                Vector3Int position = Origin + new Vector3Int(x, y, 0);
                TileBase tileToSet = null;

                if (x == 0) tileToSet = RoomConfig.westWallTile;
                else if (x == RoomSize - 1) tileToSet = RoomConfig.eastWallTile;
                else if (y == 0) tileToSet = RoomConfig.southWallTile;
                else if (y == RoomSize - 1) tileToSet = RoomConfig.northWallTile;
                else tileToSet = RoomConfig.floorTile;

                Tilemap.SetTile(position, tileToSet);
            }
        }
    }

    public GameObject PrepareSafeZone(GameObject safeZonePrefab)
    {
        Vector3 safeZonePosition = GetCenterCoords() + new Vector3(0.5f, 0.5f, 0);
        GameObject safeZone = GameObject.Instantiate(safeZonePrefab, safeZonePosition, Quaternion.identity);
        return safeZone;
    }
    
    public Vector3Int GetCenterCoords()
    {
        return new Vector3Int(Origin.x + RoomSize / 2, Origin.y + RoomSize / 2, 0);
    }

    public void TearDownWall(Direction direction)
    {
        Vector3Int wallStart, wallEnd;
        GetWallCoordinates(direction, out wallStart, out wallEnd);
        ReplaceWallWithFloor(wallStart, wallEnd);
    }
    
    private void GetWallCoordinates(Direction direction, out Vector3Int wallStart, out Vector3Int wallEnd)
    {
        switch (direction)
        {
            case Direction.North:
                wallStart = new Vector3Int(Origin.x + 1, Origin.y + RoomSize - 1, 0); // +1 to skip corner tile
                wallEnd = new Vector3Int(Origin.x + RoomSize - 2, Origin.y + RoomSize - 1, 0); // -2 to skip corner tile
                break;

            case Direction.South:
                wallStart = new Vector3Int(Origin.x + 1, Origin.y, 0); // +1 to skip corner tile
                wallEnd = new Vector3Int(Origin.x + RoomSize - 2, Origin.y, 0); // -2 to skip corner tile
                break;

            case Direction.East:
                wallStart = new Vector3Int(Origin.x + RoomSize - 1, Origin.y + 1, 0); // +1 to skip corner tile
                wallEnd = new Vector3Int(Origin.x + RoomSize - 1, Origin.y + RoomSize - 2, 0); // -2 to skip corner tile
                break;

            case Direction.West:
                wallStart = new Vector3Int(Origin.x, Origin.y + 1, 0); // +1 to skip corner tile
                wallEnd = new Vector3Int(Origin.x, Origin.y + RoomSize - 2, 0); // -2 to skip corner tile
                break;

            default:
                throw new ArgumentException("Invalid direction: " + direction);
        }
    }

    private void ReplaceWallWithFloor(Vector3Int wallStart, Vector3Int wallEnd)
    {
        // Determine if the wall is horizontal or vertical
        bool isHorizontal = wallStart.y == wallEnd.y;

        if (isHorizontal)
        {
            // For horizontal walls, skip the first and last tiles
            for (int x = wallStart.x; x <= wallEnd.x; x++)
            {
                Vector3Int position = new Vector3Int(x, wallStart.y, 0);
                Tilemap.SetTile(position, RoomConfig.floorTile);
            }
        }
        else
        {
            // For vertical walls, skip the first and last tiles
            for (int y = wallStart.y; y <= wallEnd.y; y++)
            {
                Vector3Int position = new Vector3Int(wallStart.x, y, 0);
                Tilemap.SetTile(position, RoomConfig.floorTile);
            }
        }
    }
    
    public Direction GetDirectionOf(Room other)
    {
        Vector3Int direction = other.Origin - this.Origin;
        if (direction.x > 0) return Direction.East;
        if (direction.x < 0) return Direction.West;
        if (direction.y > 0) return Direction.North;
        if (direction.y < 0) return Direction.South;
        throw new ArgumentException("The provided room is not adjacent.");
    }

    
    public void MarkAsVisited()
    {
        Visited = true;
    }
    
    public void ChangeFloorTile(TileBase newTile)
    {
        for (int x = 1; x < RoomSize - 1; x++)
        {
            for (int y = 1; y < RoomSize - 1; y++)
            {
                Vector3Int position = Origin + new Vector3Int(x, y, 0);
                Tilemap.SetTile(position, newTile);
            }
        }
    }
}