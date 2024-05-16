using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float maxLeftSide;
    [SerializeField] private float maxRightSide;
    [SerializeField] private float maxTopSide;
    [SerializeField] private float maxBottomSide;

    void LateUpdate()
    {
        transform.position = player.transform.position;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, maxLeftSide, maxRightSide), Mathf.Clamp(transform.position.y, maxBottomSide, maxTopSide), -10);
    }
}
