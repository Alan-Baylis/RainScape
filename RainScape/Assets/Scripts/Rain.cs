using UnityEngine;
using System.Collections;

public class Rain : MonoBehaviour
{
    public Transform player;
    public float rainInterval;
    
    private float prevRainTime;
    void Start()
    {
        prevRainTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - prevRainTime >= rainInterval)
        {
            var hit = Physics2D.Raycast(Camera.main.transform.position, -Vector2.up);
            if (hit.collider)
            {
                var hitGameObject = hit.collider.gameObject;
                if (hitGameObject.CompareTag("Player"))
                {
                    Destroy(hitGameObject);
                }
            }
            
            prevRainTime = Time.time;
        }
    }
}
