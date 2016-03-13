using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float defaultSpeed;
    public int defaultHealth;
    
    
    private Vector3 defaultScale;
    private int health;
    private float speed;
    public void getHit()
    {
        health--;
        
        var logRatio = getLogRatio(health, defaultHealth);
        transform.localScale = defaultScale * logRatio;
        speed = defaultSpeed * logRatio;
        
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        health = defaultHealth;
        speed = defaultSpeed;
        defaultScale = transform.localScale;
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
    
    float getLogRatio(float curVal, float maxVal)
    {
        // When curval is 0, I want log result to be 0. Hence + 1
        return Mathf.Log(curVal + 1, maxVal + 1);
    }
}
