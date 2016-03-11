using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform destPoint;
    public float speed;
    public float umbrellaAlpha;
    
    private bool wasOnScreen;
    
    // Use this for initialization
    void Start()
    {
        transform.position = spawnPoint.position;
        wasOnScreen = false;
    }

    // Update is called once per frame
    void Update()
    {
        var curPos = transform.position;
        var dir = (destPoint.position - curPos).normalized;
        var move = dir * speed * Time.deltaTime;
        
        transform.position = new Vector3(curPos.x + move.x, curPos.y + move.y, 0);
        curPos = transform.position;
        
        var renderer = GetComponent<SpriteRenderer>();
        var NPCHalfWidth = renderer.bounds.size.x * 0.5f;
        var viewportHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        
        if (curPos.x < viewportHalfWidth && curPos.x > -viewportHalfWidth)
            wasOnScreen = true;
            
        if (wasOnScreen && (curPos.x > viewportHalfWidth + NPCHalfWidth || 
                            curPos.x < -viewportHalfWidth - NPCHalfWidth))
        {
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = umbrellaAlpha;
            GetComponent<SpriteRenderer>().color = color;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 1.0f;
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}
