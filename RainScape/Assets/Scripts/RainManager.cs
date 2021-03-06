﻿using UnityEngine;
using System.Collections;

public class RainManager : MonoBehaviour
{
    public GameObject rain;
    public float rainInterval;
    public int numRainAtOnce;
    
    
    void Start()
    {
        StartCoroutine(PutRain());
    }

    IEnumerator PutRain()
    {            
        var maxY = Camera.main.orthographicSize * 2.0f;
        var minY = 0;
        var maxX = Camera.main.orthographicSize * Screen.width / Screen.height * 2.0f;
        var minX = 0;
        
        while (true)
        {
            yield return new WaitForSeconds(rainInterval);
            
            for (int i = 0; i < numRainAtOnce; i++)
            {
                var posY = Random.Range(minY, maxY);
                var posX = Random.Range(minX, maxX);
                
                Instantiate(rain, new Vector3(posX, posY, 0), Quaternion.identity);
            }
        }
    }
}
