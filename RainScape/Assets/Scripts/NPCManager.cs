using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Transform[] destPoints;
    public Map map;
    public float npcRatioToCell;
    
    // TODO: improve perf by reusing npcs instead of keep creating/destroying new ones
    public GameObject npcPrefab;
    public float spawnInterval;
    private IDictionary<int, GameObject> npcs;
    private Vector3 npcSpriteScale;

    void Start()
    {
        npcs = new Dictionary<int, GameObject>();
        
        var npcObject = Instantiate(npcPrefab) as GameObject;
        var spriteSize = npcObject.GetComponent<SpriteRenderer>().bounds.size;
        
        /* change back to this later
        float spriteToCellSizeRatio;
        if (map.GetCellWidth() <= map.GetCellHeight())
        {
            spriteToCellSizeRatio = spriteSize.x / (map.GetCellWidth() * npcRatioToCell);
        }
        else
        {
            spriteToCellSizeRatio = spriteSize.y / (map.GetCellHeight() * npcRatioToCell);
            
        }
        
        var ratioToApply = 1.0f / spriteToCellSizeRatio;
        var curScale = transform.localScale;
        npcSpriteScale = new Vector3(curScale.x * ratioToApply, curScale.y * ratioToApply, curScale.z * ratioToApply);
        */
        
        // Debugging
        var widthRatio = spriteSize.x / map.GetCellWidth();
        var heightRatio = spriteSize.y / map.GetCellHeight();
        var widthRatioToApply = 1.0f / widthRatio;
        var heightRatioToApply = 1.0f / heightRatio;
        var curScale = transform.localScale;
        npcSpriteScale = new Vector3(curScale.x * widthRatioToApply, curScale.y * heightRatioToApply, curScale.z);
        
        Destroy(npcObject);
        
        StartCoroutine(SpawnNPC());
        
        // TEST
        /*
        {
            var spawnPoint = spawnPoints[0];
            spawnPoint.position = new Vector2(2.0f, 1.4f);
            var destPoint = destPoints[0];

            var npcObjecttest = Instantiate(npcPrefab) as GameObject;
            var npcScript = npcObjecttest.GetComponent<NPCMovement>();
            npcScript.spawnPoint = spawnPoint;
            npcScript.destPoint = destPoint;
            npcScript.map = map;
            npcScript.npcManager = this;
            
            npcObjecttest.transform.localScale = npcSpriteScale;
            
            npcs.Add(npcObjecttest.GetInstanceID(), npcObjecttest);
        }
        
        {
            var spawnPoint = destPoints[0];
            spawnPoint.position = new Vector2(2.0f, 1.4f);
            var destPoint = spawnPoints[0];
            
            var npcObjecttest = Instantiate(npcPrefab) as GameObject;
            var npcScript = npcObjecttest.GetComponent<NPCMovement>();
            npcScript.spawnPoint = spawnPoint;
            npcScript.destPoint = destPoint;
            npcScript.map = map;
            npcScript.npcManager = this;
            
            npcObjecttest.transform.localScale = npcSpriteScale;
            
            npcs.Add(npcObjecttest.GetInstanceID(), npcObjecttest);
        }
        */
    }
    
    IEnumerator SpawnNPC()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var destPoint = destPoints[Random.Range(0, destPoints.Length)];

            if (Random.Range(0, 2) == 1)
            {
                var temp = spawnPoint;
                spawnPoint = destPoint;
                destPoint = temp;
            }
            
            var npcObject = Instantiate(npcPrefab) as GameObject;
            var npcScript = npcObject.GetComponent<NPCMovement>();
            npcScript.spawnPoint = spawnPoint;
            npcScript.destPoint = destPoint;
            npcScript.map = map;
            npcScript.npcManager = this;
            
            npcObject.transform.localScale = npcSpriteScale;
            
            npcs.Add(npcObject.GetInstanceID(), npcObject);
            
        }
    }
    
    public void DeactiveNPC(int instanceId)
    {
        if (npcs.ContainsKey(instanceId))
        {
            Destroy(npcs[instanceId]);
            npcs.Remove(instanceId);
        }
    }
    
    // There aren't going to be many NPCs. Perf should be okay
    public GameObject GetOtherNPCOnTile(int npcId, Vector2 tilePos)
    {
        foreach (var npc in npcs.Values)
        {
            if (npc.GetInstanceID() == npcId) continue;
            
            if (map.GetTilePos(npc.transform.position) == tilePos)
            {
                return npc;
            }
        }
        
        return null;
    }
}
