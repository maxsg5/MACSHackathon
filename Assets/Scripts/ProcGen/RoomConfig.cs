using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "RoomConfig", menuName = "Maze/RoomConfig", order = 0)]
public class RoomConfig : ScriptableObject
{
    public TileBase floorTile;
    public TileBase northWallTile;
    public TileBase southWallTile;
    public TileBase eastWallTile;
    public TileBase westWallTile;
}