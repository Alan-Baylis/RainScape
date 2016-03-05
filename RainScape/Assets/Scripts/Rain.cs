using UnityEngine;
using System.Collections;

public class Rain : MonoBehaviour
{
    public Transform player;
    public float rainInterval;
    
    private float prevRainTime;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        prevRainTime = Time.time;
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - prevRainTime >= rainInterval)
        {
            spriteRenderer.enabled = true;
            
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
