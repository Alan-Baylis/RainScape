using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform destPoint;
    public float speed;
    public float umbrellaAlpha;
    public Map map;
    
    private bool wasOnScreen;
    private Vector2 curTilePos;
    private Vector2 destTilePos;
    private Vector2 finalDestTilePos;
    
    private Vector2[] directions;
    
    // Use this for initialization
    void Start()
    {
        transform.position = spawnPoint.position;
        curTilePos = map.GetTilePos(transform.position);
        destTilePos = curTilePos;
        finalDestTilePos = map.GetTilePos(destPoint.position);
        
        wasOnScreen = false;
        
        directions = new Vector2[8];
        directions[0] = new Vector2(-1, 0);
        directions[1] = new Vector2(0, 1);
        directions[2] = new Vector2(1, 0);
        directions[3] = new Vector2(0, -1);
        
        // diagonals
        directions[4] = new Vector2(-1, 1);
        directions[5] = new Vector2(1, 1);
        directions[6] = new Vector2(1, -1);
        directions[7] = new Vector2(-1, -1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 curPos = transform.position;
        if (map.GetTilePos(curPos) == destTilePos)
        {
            Vector2 tempDestTilePos = new Vector2(); // guaranteed to be something else
            float minDist = 10000;
            foreach (var direction in directions)
            {
                var potentialDest = destTilePos + direction;
                if (!map.IsTileWithinVerticalBound(potentialDest)) continue;
                
                var distToFinalDest = (finalDestTilePos - potentialDest).magnitude;
                
                if (distToFinalDest < minDist)
                {
                    minDist = distToFinalDest;
                    tempDestTilePos = potentialDest;
                }
            }
            
            destTilePos = tempDestTilePos;
            print("finalDestPos: " + finalDestTilePos + " destTilePos: " + destTilePos + " minDist: " + minDist);
        }
        
        var destPos = map.GetActualPos(destTilePos);
        var dir = (destPos - curPos).normalized;
        
        var move = dir * speed * Time.deltaTime;
        
        transform.position = new Vector3(curPos.x + move.x, curPos.y + move.y, 0);
        curPos = transform.position;
        
        var renderer = GetComponent<SpriteRenderer>();
        var NPCHalfWidth = renderer.bounds.size.x * 0.5f;
        var viewportWidth = Camera.main.orthographicSize * Screen.width / Screen.height * 2.0f;
        
        if (curPos.x < viewportWidth && curPos.x > 0)
            wasOnScreen = true;
            
        if (wasOnScreen && (curPos.x > viewportWidth + NPCHalfWidth || 
                            curPos.x < 0 - NPCHalfWidth))
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
