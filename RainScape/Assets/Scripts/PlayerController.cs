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
        
        var viewportWidth = Camera.main.orthographicSize * Screen.width / Screen.height * 2.0f;
        var viewportHeight = Camera.main.orthographicSize * 2.0f;

        var renderer = GetComponent<SpriteRenderer>();
        var playerHalfWidth = renderer.bounds.size.x * 0.5f;
        var playerHalfHeight = renderer.bounds.size.y * 0.5f;

        if (curPos.x < 0 + playerHalfWidth)
            curPos.x = 0 + playerHalfWidth;

        if (curPos.x > viewportWidth - playerHalfWidth)
            curPos.x = viewportWidth - playerHalfWidth;

        if (curPos.y < 0 + playerHalfHeight)
            curPos.y = 0 + playerHalfHeight;

        if (curPos.y > viewportHeight - playerHalfHeight)
            curPos.y = viewportHeight - playerHalfHeight;
            
        transform.position = curPos;
    }
    
    float getLogRatio(float curVal, float maxVal)
    {
        // When curval is 0, I want log result to be 0. Hence + 1
        return Mathf.Log(curVal + 1, maxVal + 1);
    }
}
