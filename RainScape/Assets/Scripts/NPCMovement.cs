using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCMovement : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform destPoint;
    public float speed;
    public float umbrellaAlpha;
    public Map map;
    public NPCManager npcManager;
    
    private bool wasOnScreen;
    private Vector2 curTilePos;
    private Vector2 destTilePos;
    private Vector2 finalDestTilePos;
    
    private Vector2 destActualPos;
    
    private Vector2[] directions;
    
    // Use this for initialization
    void Start()
    {
        transform.position = spawnPoint.position;
        curTilePos = map.GetTilePos(transform.position);
        destTilePos = curTilePos;
        finalDestTilePos = map.GetTilePos(destPoint.position);
        
        wasOnScreen = false;
        
        GetComponent<SpriteRenderer>().color = Color.white;
        
        directions = new Vector2[8];
        // Kepp this order. hori first
        directions[0] = new Vector2(-1, 0);
        directions[1] = new Vector2(1, 0);
        // then vert
        directions[2] = new Vector2(0, 1);
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
        var curTilePos = map.GetTilePos(curPos);
        GameObject npcOnDest = npcManager.GetOtherNPCOnTile(gameObject.GetInstanceID(), destTilePos);
        GameObject npcOnCur = null;
        if (curTilePos != destTilePos)
        {
            npcOnCur = npcManager.GetOtherNPCOnTile(gameObject.GetInstanceID(), curTilePos);
        }
        
        var widthDiffRatio = Mathf.Abs(1 - destActualPos.x / curPos.x);
        var heightDiffRatio = Mathf.Abs(1 - destActualPos.y / curPos.y);
        
        // Those who aren't directly on the dest or cur tile aren't slowed even though they are neighbors, right now
        // Also below is very slow!!
        if ((widthDiffRatio < 0.04f && heightDiffRatio < 0.03f) || npcOnDest || npcOnCur)
        {
            Vector2 tempDestTilePos = map.GetTilePos(curPos);
            float minDist = 10000;
            
            var neighborNPCs = new List<GameObject>();
            foreach (var direction in directions)
            {
                var neighborTile = curTilePos + direction;
                var neighborNPC = npcManager.GetOtherNPCOnTile(gameObject.GetInstanceID(), neighborTile);
                if (neighborNPC)
                {
                    //print("Found Neightbor: " + neighborNPC + " at tile: " + map.GetTilePos(neighborNPC.transform.position));
                    neighborNPCs.Add(neighborNPC);
                }
            }
            
            foreach (var direction in directions)
            {
                var potentialDestTile = curTilePos + direction;
                if (!map.IsTileWithinVerticalBound(potentialDestTile))
                {
                    //print("Tile is out of bounds. Skipping direction: " + direction);
                    continue;
                }
                
                if (npcOnCur)
                {
                    var relPosOfOther = GetRelActualPosOfOther(npcOnCur);
                    if (direction.x == relPosOfOther.x || direction.y == relPosOfOther.y)
                    {
                        //print("Other npc on cur tile. Skipping direction: " + direction);
                        continue;
                    }
                }
                
                bool shouldContinue = false;
                foreach (var neighborNPC in neighborNPCs)
                {
                    if (potentialDestTile == map.GetTilePos(neighborNPC.transform.position))
                    {
                        //print("NPC is on potential dest tile.");
                        shouldContinue = true;
                        break;
                    }
                    
                    var relPosOfOther = GetRelTilePosOfOther(neighborNPC);
                    //print("RelPosOfOther is: " + relPosOfOther);
                    bool isOtherOnHori = false;
                    bool isOtherOnVert = false;
                    for (int i = 0; i <= 1; i++)
                    {
                        if (relPosOfOther == directions[i])
                        {
                            //print("Other NPC is on Hori: " + directions[i].x);
                            isOtherOnHori = true;
                        }
                    }
                    
                    for (int i = 2; !isOtherOnHori && i <= 3; i++)
                    {
                        if (relPosOfOther == directions[i])
                        {
                            //print("Other NPC is on Vert: " + directions[i].y);
                            isOtherOnVert = true;
                        }
                    }
                    
                    if (isOtherOnHori && direction.x == relPosOfOther.x)
                    {
                        //print("Other NPC is on same Hori as dir: " + direction.x);
                        shouldContinue = true;
                        break;
                    }
                    
                    if (isOtherOnVert && direction.y == relPosOfOther.y)
                    {
                        //print("Other NPC is on same Vert as dir: " + direction.y);
                        shouldContinue = true;
                        break;
                    }
                }
                if (shouldContinue)
                {
                    //print("Skipping direction: " + direction);
                    continue;
                }
                    
                var distToFinalDest = (finalDestTilePos - potentialDestTile).magnitude;
                
                if (distToFinalDest < minDist)
                {
                    minDist = distToFinalDest;
                    tempDestTilePos = potentialDestTile;
                    //print("Updated tempDestTilePos to: " + tempDestTilePos);
                }
            }
            
            //print("New destTilePos is: " + tempDestTilePos);
            if (npcOnDest && destTilePos == tempDestTilePos)
            {
                print("STUCK");
                return;
            }
            destTilePos = tempDestTilePos;
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
    
    Vector2 GetRelActualPosOfOther(GameObject otherNPC)
    {
        var oPos = otherNPC.transform.position;
        var myPos = transform.position;
        var relPos = Vector2.zero;
        
        relPos += new Vector2(oPos.x - myPos.x, 0).normalized;
        relPos += new Vector2(0, oPos.y - myPos.y).normalized;
        
        return relPos;
    }
    
        Vector2 GetRelTilePosOfOther(GameObject otherNPC)
    {
        var oTilePos = map.GetTilePos(otherNPC.transform.position);
        var myTilePos = map.GetTilePos(transform.position);
        var relPos = Vector2.zero;
        
        relPos += new Vector2(oTilePos.x - myTilePos.x, 0).normalized;
        relPos += new Vector2(0, oTilePos.y - myTilePos.y).normalized;
        
        return relPos;
    }
}
