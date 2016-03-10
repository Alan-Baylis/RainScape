using UnityEngine;
using System.Collections;

public class Rain : MonoBehaviour
{
    public float rainShowDuration;
    
    private float spawnTime;
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        var hit = Physics2D.Raycast(transform.position, -Vector2.up);
        if (hit.collider)
        {
            var hitGameObject = hit.collider.gameObject;
            if (hitGameObject.CompareTag("Player"))
            {
                Destroy(hitGameObject);
            }
        }
            
        if (Time.time - spawnTime >= rainShowDuration)
        {
            Destroy(gameObject);
        }
    }
}
