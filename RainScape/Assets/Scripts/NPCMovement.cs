using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform destPoint;
    public float speed;
    public float umbrellaAlpha;
    public Map map;
    public NPCManager npcManager;
    public float slowDownDuration;
    public float slowDownSpeedRatio;
    
    private bool wasOnScreen;
    private Vector2 curTilePos;
    private Vector2 destTilePos;
    private Vector2 finalDestTilePos;
    
    private Vector2 destActualPos;
    
    private Vector2[] directions;
    private float slowDownTimer;
    private bool isSlowed;
    
    // Use this for initialization
    void Start()
    {
        transform.position = spawnPoint.position;
        curTilePos = map.GetTilePos(transform.position);
        destTilePos = curTilePos;
        finalDestTilePos = map.GetTilePos(destPoint.position);
        
        wasOnScreen = false;
        isSlowed = false;
        slowDownTimer = 0;
        
        GetComponent<SpriteRenderer>().color = Color.white;
        
        directions = new Vector2[4];
        directions[0] = new Vector2(-1, 0);
        directions[1] = new Vector2(0, 1);
        directions[2] = new Vector2(1, 0);
        directions[3] = new Vector2(0, -1);
        
        // diagonals
        /*
        directions[4] = new Vector2(-1, 1);
        directions[5] = new Vector2(1, 1);
        directions[6] = new Vector2(1, -1);
        directions[7] = new Vector2(-1, -1);
        */
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 curPos = transform.position;
        var curTilePos = map.GetTilePos(curPos);
        GameObject npcOnDest = npcManager.GetOtherNPCOnTile(gameObject.GetInstanceID(), destTilePos);
        GameObject npcOnCur = null;
        if (curTilePos != destTilePos)
        {
            npcOnCur = npcManager.GetOtherNPCOnTile(gameObject.GetInstanceID(), curTilePos);
        }
        
        //if (curTilePos == destTilePos || npcOnDest || npcOnCur)
        var widthDiffRatio = Mathf.Abs(1 - destActualPos.x / curPos.x);
        var heightDiffRatio = Mathf.Abs(1 - destActualPos.y / curPos.y);
        
        if (widthDiffRatio < 0.05f && heightDiffRatio < 0.05f)
        {
            if (npcOnDest)
            {
                if (npcOnDest.GetInstanceID() < gameObject.GetInstanceID())
                {
                    npcOnDest.GetComponent<NPCMovement>().WuhWuhSlowDown();
                }
                else
                {
                    WuhWuhSlowDown();
                }
            }
            
            if (npcOnCur)
            {
                if (npcOnCur.GetInstanceID() < gameObject.GetInstanceID())
                {
                    npcOnCur.GetComponent<NPCMovement>().WuhWuhSlowDown();
                }
                else
                {
                    WuhWuhSlowDown();
                }
            }
            
            if (!isSlowed)
            {
                Vector2 tempDestTilePos = map.GetTilePos(curPos);
                float minDist = 10000;
                foreach (var direction in directions)
                {
                    if (npcOnCur && direction.y == 0) continue;
                    
                    var potentialDestTile = destTilePos + direction;
                    var npcOnPotDest = npcManager.GetOtherNPCOnTile(gameObject.GetInstanceID(), potentialDestTile);
                    if (!map.IsTileWithinVerticalBound(potentialDestTile) || npcOnPotDest)
                    {
                        continue;
                    }
                    
                    var distToFinalDest = (finalDestTilePos - potentialDestTile).magnitude;
                    
                    if (distToFinalDest < minDist)
                    {
                        minDist = distToFinalDest;
                        tempDestTilePos = potentialDestTile;
                    }
                }
                
                destTilePos = tempDestTilePos;
                //print("finalDestPos: " + finalDestTilePos + " destTilePos: " + destTilePos + " minDist: " + minDist);
            }
        }
        
        destActualPos = map.GetActualPos(destTilePos);
        var dir = (destActualPos - curPos).normalized;
        
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
            npcManager.DeactiveNPC(gameObject.GetInstanceID()); // oops typo
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
    
    public void WuhWuhSlowDown()
    {
        isSlowed = true;
        GetComponent<SpriteRenderer>().color = Color.blue;
        
        if (slowDownTimer <= 0.0f)
        {
            StartCoroutine(SlowDown());
        }
        else
        {
            slowDownTimer += slowDownDuration;
        }
    }
    
    IEnumerator SlowDown()
    {
        speed *= slowDownSpeedRatio;
        
        slowDownTimer = slowDownDuration;

        while (slowDownTimer > 0.0f)
        {
            yield return new WaitForSeconds(slowDownTimer);
            slowDownTimer -= slowDownTimer;
        }
        
        speed /= slowDownSpeedRatio;
        isSlowed = false;
        GetComponent<SpriteRenderer>().color = Color.white;        
    }
}
