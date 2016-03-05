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
        var moveSpeed = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        transform.Rotate(0, speed, 0);
	}
}
