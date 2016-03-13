using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public int health;
    
    
    private float scaleChangeRate;
    private float speedChangeRate;
    
    public void getHit()
    {
        health--;
        
        var curScale = transform.localScale;
        var newScale = new Vector3(curScale.x - scaleChangeRate, curScale.y - scaleChangeRate, curScale.z - scaleChangeRate);
        transform.localScale = newScale;
        
        speed -= speedChangeRate;
        
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        scaleChangeRate = transform.localScale.x / health;
        speedChangeRate = speed / health;
    }

    void Update()
    {
        var forceDir = new Vector2(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed);
        gameObject.GetComponent<Rigidbody2D>().AddForce(forceDir);
        
    }
    
    void FixedUpdate()
    {
        var curPos = transform.position;
        
        var viewportHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        var viewportHalfHeight = Camera.main.orthographicSize;

        var renderer = GetComponent<SpriteRenderer>();
        var playerHalfWidth = renderer.bounds.size.x * 0.5f;
        var playerHalfHeight = renderer.bounds.size.y * 0.5f;

        if (curPos.x < -viewportHalfWidth + playerHalfWidth)
            curPos.x = -viewportHalfWidth + playerHalfWidth;

        if (curPos.x > viewportHalfWidth - playerHalfWidth)
            curPos.x = viewportHalfWidth - playerHalfWidth;

        if (curPos.y < -viewportHalfHeight + playerHalfHeight)
            curPos.y = -viewportHalfHeight + playerHalfHeight;

        if (curPos.y > viewportHalfHeight - playerHalfHeight)
            curPos.y = viewportHalfHeight - playerHalfHeight;
            
        transform.position = curPos;
    }
}
