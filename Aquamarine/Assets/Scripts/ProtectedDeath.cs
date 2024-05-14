using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectedDeath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null && !player.isProtected())
        {
            player.Die();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null && !player.isProtected())
        {
            player.Die();
        }
    }
}
