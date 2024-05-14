using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets the Player's Respawn Point to the Shrine.
public class Shrine : MonoBehaviour
{
    private bool saved = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!saved)
        {
            var player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.SetRespawnPoint(player.transform.position);
                saved = true;
            }
        }
        
    }
}
