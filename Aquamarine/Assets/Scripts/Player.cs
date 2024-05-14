using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.TMP_Compatibility;

public class Player : MonoBehaviour
{
    public static Player instance;
    private float horizontal;
    private float speed = 3f;
    public float jumpingPower = 10f;
    private bool isFacingRight = true;
    private static bool _isProtected = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private bool _active = true;
    private Collider2D _collider;

    private Vector2 respawnPosition;
    [SerializeField] private GameObject fakePlayer;
    private Vector2 fakePlayerPos = new Vector2(-10f, -10f);
    public static GameObject mostNearByAnchor;

    private void Start()
    {
        instance = this;
        _collider = GetComponent<Collider2D>();
        SetRespawnPoint(transform.position);
        fakePlayer.transform.position = fakePlayerPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_active) return;

        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (PlayerAttribute.instance.IsCurrAbilityAquaHop() && PlayerAttribute.instance.IsOnWaterSource())
        {
            mostNearByAnchor = Player.instance.findNearbyAnchor();
            if (mostNearByAnchor != null)
            {
                Vector2 anchorPosition = new Vector2(mostNearByAnchor.transform.position.x, mostNearByAnchor.transform.position.y);
                fakePlayer.transform.position = anchorPosition;
            }
        }
        else
        {
            instance.ResetFakePlayer();
        }
    }

    public bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void ResetFakePlayer()
    {
        fakePlayer.transform.position = fakePlayerPos;
    }

    private void MiniJump()
    {
        rb.velocity = new Vector2(0, jumpingPower / 2);
    }

    public void Die()
    {
        _active = false;
        _collider.enabled = false;
        MiniJump();
        StartCoroutine(Respawn());
    }

    public void SetRespawnPoint(Vector2 position)
    {
        respawnPosition = position;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        transform.position = respawnPosition;  
        _active = true;
        _collider.enabled = true;
        MiniJump();
    }

    public void MakeProtected()
    {
        // TODO: Instantiate a shield object here
        _isProtected = true;
    }

    public void MakeUnProtected()
    {
        // TODO: Remove the shield object here
        if (_isProtected)
        {
            _isProtected = false;
            jumpingPower *= 2f;
            speed *= 2f;
        }
      
    }

    public bool isProtected()
    {
        return _isProtected;
    }

    public async Task SlowMovementFor(int duration)
    {
        SlowPlayerMovement();
        await Task.Delay(duration);
    }

    private void SlowPlayerMovement()
    {
        jumpingPower /= 2f;
        speed /= 2f;
    }

    public void AquaTeleportation()
    {
        if (mostNearByAnchor != null)
        {
            instance.transform.position = mostNearByAnchor.transform.position;
            instance.ResetFakePlayer();
        }
        
    }

    public void ResetFakePlayerPos()
    {
        fakePlayer.transform.position = fakePlayerPos;
    }

    private GameObject findNearbyAnchor()
    {
        GameObject[] anchors = GameObject.FindGameObjectsWithTag("Teleport Anchor");
        if (anchors.Length > 0)
        {
            float currMinDist = float.MaxValue;
            GameObject mostNearbyAnchor = anchors[0];
            foreach (var anchor in anchors)
            {
                float anchorDist = Vector2.Distance(this.transform.position, anchor.transform.position);
                if (anchor.GetComponent<TeleportationStopper>().canTeleportTo && anchorDist < currMinDist)
                {
                    mostNearbyAnchor = anchor; 
                    currMinDist = anchorDist;
                }
            }
            return mostNearbyAnchor;
        }
        return null;
    }
    
}
