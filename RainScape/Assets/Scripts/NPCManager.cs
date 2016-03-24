using UnityEngine;
using System.Collections;

public class NPCManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Transform[] destPoints;
    public Map map;
    
    // TODO: improve perf by reusing npcs instead of keep creating/destroying new ones
    public GameObject NPC;
    public float spawnInterval;

    void Start()
    {
        StartCoroutine(SpawnNPC());
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
            
            var NPCObject = Instantiate(NPC) as GameObject;
            var NPCScript = NPCObject.GetComponent<NPCMovement>();
            NPCScript.spawnPoint = spawnPoint;
            NPCScript.destPoint = destPoint;
            NPCScript.map = map;
        }
    }
}
