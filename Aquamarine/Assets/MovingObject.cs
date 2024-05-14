using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 offSet;
    public float speed = 0.1f;
    private Transform dragging = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerAttribute.instance.IsCurrAbilityPuddleBuddy())
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 
                    float.PositiveInfinity, LayerMask.GetMask("Movable"));

                if (hit)
                {
                    dragging = hit.transform;
                    offSet = dragging.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            } 
            else if (Input.GetMouseButtonUp(0))
            {
                dragging = null;
            }

            if (dragging != null)
            {
                dragging.transform.Translate((Camera.main.ScreenToWorldPoint(Input.mousePosition).x + offSet.x) * Time.deltaTime, 0f,
                    (Camera.main.ScreenToWorldPoint(Input.mousePosition).y + offSet.y) * Time.deltaTime);
            }
        }

        
    }

}
