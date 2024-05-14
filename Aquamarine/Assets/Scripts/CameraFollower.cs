using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float maxLeftSide;
    [SerializeField] private float maxRightSide;
    [SerializeField] private float maxTopSide;
    [SerializeField] private float maxBottomSide;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, maxLeftSide, maxRightSide), Mathf.Clamp(transform.position.y, maxBottomSide, maxTopSide), -10);
    }
}
