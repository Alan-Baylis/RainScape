using UnityEngine;
using System.Collections;

public class Rain : MonoBehaviour
{
    public float rainShowDuration;
    
    private float spawnTime;
    void Start()
    {
        spawnTime = Time.time;
    }

    void FixedUpdate()
    {
        if (Time.time - spawnTime >= rainShowDuration)
        {
            /* It is kinda frustrating that I can't use raycast casting from rain position with high z to a player with low z
                while detecting NPC in between z. I was expecting the raycast to detect the NPC and stop there
                since NPC has higher sorting layer and also higher z. However, it sometimes hits NPC and sometimes the player
                even when the player is below the NPC.
               This is a workaround to get it working using layermasks instead.
            */
            int layerMask = 1 << LayerMask.NameToLayer("Player");
            var hit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, layerMask);
            
            if (hit.collider)
            {
                layerMask = 1 << LayerMask.NameToLayer("NPC");
                var NPCHit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, layerMask);
                if (!NPCHit.collider)
                {
                    Destroy(hit.collider.gameObject);
                }
            }
            
            Destroy(gameObject);
        }
    }
}
