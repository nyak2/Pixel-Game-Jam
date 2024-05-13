using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationStopper : MonoBehaviour
{
    public bool canTeleportTo = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            this.canTeleportTo = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            this.canTeleportTo = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            this.canTeleportTo = true;
        }
    }
}
