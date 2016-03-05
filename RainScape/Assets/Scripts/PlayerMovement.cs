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
        transform.position = new Vector3(curPos.x + moveHori, curPos.y + moveVert, 0);
    }
}
