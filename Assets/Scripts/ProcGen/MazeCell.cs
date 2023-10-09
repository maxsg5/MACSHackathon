using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{   
    [SerializeField]
    private GameObject topWall, rightWall, bottomWall, leftWall, unvistedBlock;
    
    public bool isVisited { get; private set; }

    public void Visit()
    {
        isVisited = true;
        unvistedBlock.SetActive(false);
    }

    public void Unvisit()
    {
        isVisited = false;
        unvistedBlock.SetActive(true);
    }
    
    public void ClearLeftWall()
    {
        leftWall.SetActive(false);
    }
    
    public void ClearRightWall()
    {
        rightWall.SetActive(false);
    }
    
    public void ClearTopWall()
    {
        topWall.SetActive(false);
    }
    
    public void ClearBottomWall()
    {
        bottomWall.SetActive(false);
    }
    
    public void AddAllWalls()
    {
        topWall.SetActive(true);
        rightWall.SetActive(true);
        bottomWall.SetActive(true);
        leftWall.SetActive(true);
    }
    
}
