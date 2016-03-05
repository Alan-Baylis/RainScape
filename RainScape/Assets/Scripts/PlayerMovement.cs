using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    
	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        var moveHori = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        var moveVert = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
        
        var curPos = transform.position;
        var dest = new Vector3(curPos.x + moveHori, curPos.y + moveVert, 0);
        Move(dest);
    }
    
    private void Move(Vector3 dest)
    {
        var viewportHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        var viewportHalfHeight = Camera.main.orthographicSize;
        
        var renderer = GetComponent<SpriteRenderer>();
        var playerHalfWidth = renderer.bounds.size.x * 0.5f;
        var playerHalfHeight = renderer.bounds.size.y * 0.5f;

        if (dest.x < -viewportHalfWidth + playerHalfWidth)
            dest.x = -viewportHalfWidth + playerHalfWidth;
            
        if (dest.x > viewportHalfWidth - playerHalfWidth)
            dest.x = viewportHalfWidth - playerHalfWidth;
            
        if (dest.y < -viewportHalfHeight + playerHalfHeight)
            dest.y = -viewportHalfHeight + playerHalfHeight;
        
        if (dest.y > viewportHalfHeight - playerHalfHeight)
            dest.y = viewportHalfHeight - playerHalfHeight;
            
        transform.position = dest;
    }
}
