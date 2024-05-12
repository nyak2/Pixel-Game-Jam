using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Collider2D colliderObject;
    [SerializeField] private ContactFilter2D contactFilter2D;
    private List<Collider2D> colliders = new List<Collider2D>(1);

    private void Start()
    {
        colliderObject = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        colliderObject.OverlapCollider(contactFilter2D, colliders);
        foreach (var c in colliders)
        {
            OnInteract(c.gameObject);
        }
    }

    private void OnInteract(GameObject gameObject)
    {
        
    }

}
