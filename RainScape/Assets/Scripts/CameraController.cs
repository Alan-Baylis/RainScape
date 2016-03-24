using UnityEngine;

public class CameraController : MonoBehaviour
{

    void Start()
    {
        // Why does people say Camera's bottom left is 0,0 when Camera's center is 0,0?
        var viewportHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        var viewportHalfHeight = Camera.main.orthographicSize;
        
        Vector3 newPos = new Vector3(transform.position.x + viewportHalfWidth,
                                     transform.position.y + viewportHalfHeight,
                                     transform.position.z);
        transform.position = newPos;
    }
}
