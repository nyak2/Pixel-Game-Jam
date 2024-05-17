using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Sets the Player's Respawn Point to the Shrine.
public class Shrine : MonoBehaviour
{
    private bool saved = false;
    [SerializeField] private GameObject savetext;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!saved)
        {
            var player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.SetRespawnPoint(player.transform.position);
                saved = true;
                StartCoroutine(ShowSaveText());
            }
        }
        
    }

    private IEnumerator ShowSaveText()
    {
        Vector3 tempos = savetext.transform.position;
        LeanTween.moveLocalX(savetext, 0 , 0.5f).setEaseOutBack();
        yield return new WaitForSeconds(1.5f);
        LeanTween.moveLocalX(savetext, -1367 , 0.5f).setEaseInBack();
        yield return new WaitForSeconds(1.0f);
        savetext.transform.position = tempos;
    }
}
