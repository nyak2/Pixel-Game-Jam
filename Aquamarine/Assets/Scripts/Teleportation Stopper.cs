using UnityEngine;

public class TeleportationStopper : MonoBehaviour
{
    public bool canTeleportTo = false;


    private void OnBecameInvisible()
    {
        this.canTeleportTo = false;
    }

    private void OnBecameVisible()
    {
        this.canTeleportTo = true;
    }

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
