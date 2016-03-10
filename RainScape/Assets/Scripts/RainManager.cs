using UnityEngine;
using System.Collections;

public class RainManager : MonoBehaviour
{
    public GameObject player;
    public GameObject rain;
    public int rainRowCount;
    public int rainColCount;
    public float rainInterval;
    
    private float rowInterval;
    private float colInterval;
    
    void Start()
    {
        // alot of redundant usage. refactor
        var viewportHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        var viewportHalfHeight = Camera.main.orthographicSize;
        
        colInterval = viewportHalfWidth * 2.0f / (rainColCount + 1);
        rowInterval = viewportHalfHeight * 2.0f / (rainRowCount + 1);
        
        StartCoroutine(PutRain());
        
    }

    IEnumerator PutRain()
    {
        while (true)
        {
            yield return new WaitForSeconds(rainInterval);
            
            var maxY = rowInterval * rainRowCount / 2.0f;
            var minY = -maxY;
            var maxX = colInterval * rainColCount / 2.0f;
            var minX = -maxX;
            
            var posY = Random.Range(minY, maxY);
            var posX = Random.Range(minX, maxX);
            
            var rainObject = Instantiate(rain, new Vector3(posX, posY, 0), Quaternion.identity) as GameObject;
        }
    }
}
