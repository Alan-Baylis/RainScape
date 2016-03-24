using UnityEngine;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    public int rowCount;
    public int colCount;
    
    private IList<IList<Vector2>> cells;
    private float cellWidth;
    private float cellHeight;
    
    void Start()
    {
        var viewportWidth = Camera.main.orthographicSize * Screen.width / Screen.height * 2.0f;
        var viewportHeight = Camera.main.orthographicSize * 2.0f;

        cellWidth = viewportWidth / colCount;
        cellHeight = viewportHeight / rowCount;
        
        cells = new List<IList<Vector2>>(colCount);
        for (int i = 0; i < colCount; i++)
        {
            cells.Add(new List<Vector2>(rowCount));
            
            var posX = cellWidth / 2.0f + cellWidth * i;
            for (int j = 0; j < rowCount; j++)
            {
                var posY = cellHeight / 2.0f + cellHeight * j;
                cells[i].Add(new Vector2(posX, posY));
                
                print(posX + ", " + posY + "= " + GetTilePos(new Vector2(posX, posY)));
            }
        }
    }
    
    // Pre: actualPos must be valid
    public Vector2 GetTilePos(Vector2 actualPos)
    {
        float x = actualPos.x;
        float y = actualPos.y;
        
        int tileX = (int) Mathf.Round((x - cellWidth / 2.0f) / cellWidth);
        int tileY = (int) Mathf.Round((y - cellHeight / 2.0f) / cellHeight);
        return new Vector2(tileX, tileY);
    }
    
    // Pre: tilePos must be valid
    public Vector2 GetActualPos(Vector2 tilePos)
    {
        float actualX = cellWidth / 2.0f + cellWidth * tilePos.x;
        float actualY = cellHeight / 2.0f + cellHeight * tilePos.y;
        
        return new Vector2(actualX, actualY);
    }
    
    public bool IsTileValid(Vector2 tilePos)
    {
        return tilePos.x >= 0 && tilePos.x < colCount && tilePos.y >= 0 && tilePos.y < rowCount;
    }
}
