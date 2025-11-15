using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    // 核心遊玩邏輯 - 方塊移動、合併、生成 版面控制
    public GameManager gameManager;
    public Tile tilePrefab;

    public List<Tile> tiles;

    public TileState[] tileStates;

    private TileGrid grid;

    public bool waiting;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles= new List<Tile>(16);
    }
  public void ClearBoard()
    {
        foreach (var cell in grid.cells)
        {
            cell.tile = null;
        }
        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }
        tiles.Clear();
    }
    public void CreatTile()
    {
       Tile tile= Instantiate(tilePrefab,grid.transform);
        tile.SetState(tileStates[0],2);
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveTiles(Vector2Int.up,0,1,1,1);
        }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTiles(Vector2Int.left, 1, 1, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTiles(Vector2Int.down, 0, 1, grid.height-2, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTiles(Vector2Int.right,grid.width -2, -1, 0, 1);
        }
    }
    public void MoveTiles(Vector2Int direction ,int startX,int incrementX,int startY,int incrementY )
    {
       
        bool changed = false;
        for (int i = startX; i>=0&&i<grid.width;i+=incrementX)
        {
            for(int j = startY; j>=0&&j<grid.height;j+=incrementY)
            {
                TileCell cell = grid.GetCell(i,j);
                if (cell.occupied)
                {
                   changed|= MoveTile(cell.tile,direction);
                }
            }
        }
        if (changed)
        {      
            StartCoroutine(WaitForChange());          
        }
    }
    public bool MoveTile(Tile tile,Vector2Int directon)
    {
        TileCell NewCell =null;
        TileCell Adjacent = grid.GetAdjacentCell(tile.cell,directon);
        while (Adjacent != null)
        {
            if (Adjacent.occupied) {
                if (CanMarge(tile,Adjacent.tile))
                {
                    Merge(tile,Adjacent.tile);
                    return true;
                }

                break;
                        }
            NewCell = Adjacent;
            Adjacent = grid.GetAdjacentCell(NewCell,directon);
        }
        if (NewCell != null)
        {
            tile.MoveTO(NewCell);
            return true;
        }
        return false;
    }
    public bool CanMarge(Tile a,Tile b)
    {
        return a.number == b.number && !b.locked;
    }

    public void Merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);
        int index = Mathf.Clamp(IndexOf(b.state)+1,0,tileStates.Length-1);
        int number = b.number * 2;
        b.SetState(tileStates[index],number);
        gameManager.IncreaseScore(number);
        
    }
    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i])
            {
                return i;
            }
        }

        return -1;
    }
    public IEnumerator WaitForChange()
    {
        
        waiting = true;

        yield return new WaitForSeconds (0.1f);

        waiting = false;



        foreach (var item in tiles)
        {
            item.locked = false;
        }


        if (tiles.Count!=grid.size)
        {
            CreatTile();
        }
        if (CheckForGameOver())
        {
            gameManager.GameOver();
        }
    }
    public bool CheckForGameOver()
    {
        if (tiles.Count!=grid.size)
        {
            return false;
        }
        foreach (var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell,Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);
            if (up!=null&&CanMarge(tile,up.tile))
            {
                return false;
            }
            if (left != null && CanMarge(tile, left.tile))
            {
                return false;
            }
            if (right != null && CanMarge(tile, right.tile))
            {
                return false;
            }
            if (down != null && CanMarge(tile, down.tile))
            {
                return false;
                
            }           
        }
        return true;
    }
}
