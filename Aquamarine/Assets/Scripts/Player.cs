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
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpingPower = 9f;
    [SerializeField] private float jumpingMultiplier = 1.25f;
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

    [SerializeField] private Animator playeranim;
    private bool isJumping;
    private bool canJump = true;
    [SerializeField] private float jumpradius;
    private float tempJumpPower;

    private void Start()
    {
        instance = this;
        _collider = GetComponent<Collider2D>();
        SetRespawnPoint(transform.position);
        fakePlayer.transform.position = fakePlayerPos;
        tempJumpPower = jumpingPower;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_active || playeranim.GetCurrentAnimatorStateInfo(0).IsName("ability"))
        {
            isJumping = false;
            rb.velocity = new Vector3(0,rb.velocity.y,0);
            return;
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if(playeranim.GetCurrentAnimatorStateInfo(0).IsName("idle") && rb.velocity.x != 0 && !isJumping)
        {
            playeranim.Play("start run", 0, 0);
        }
        else if((playeranim.GetCurrentAnimatorStateInfo(0).IsName("run") || playeranim.GetCurrentAnimatorStateInfo(0).IsName("start run") )&& rb.velocity.x == 0 && !isJumping)
        {
            playeranim.Play("idle", 0, 0);
        }

        if (Input.GetButtonDown("Jump") && !isJumping && (isGrounded() || 
            PlayerAttribute.instance.IsOnWaterSource() || 
            PlayerAttribute.instance.IsOnWaterPlatform()))
        {
            isJumping = true;
            playeranim.Play("start jump", 0, 0);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (PlayerAttribute.instance.IsCurrAbilityAquaHop() && PlayerAttribute.instance.IsOnWaterSource())
        {
            mostNearByAnchor = instance.findNearbyAnchor();
            if (mostNearByAnchor != null)
            {
                Vector2 anchorPosition = new Vector2(mostNearByAnchor.transform.position.x, mostNearByAnchor.transform.position.y + 0.36f);
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
        return Physics2D.OverlapCircle(groundCheck.position, jumpradius, groundLayer);
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
        playeranim.Play("idle", 0, 0);
        isJumping = false;
        yield return new WaitForSeconds(0.5f);
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

    public void CheckForFalling()
    {
        if(rb.velocity.y <= 0 && (isGrounded() || PlayerAttribute.instance.IsOnWaterPlatform() || PlayerAttribute.instance.IsOnWaterSource()))
        {
            playeranim.Play("jump end");

        }
    }

    public void SetNotJumping()
    {
        isJumping = false;
        canJump = true;
    }

    public void CheckJumpPower()
    {
        if (PlayerAttribute.instance.IsOnWaterSource() || PlayerAttribute.instance.IsOnWaterPlatform())
            jumpingPower *= jumpingMultiplier;
    }

    public void ApplyJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        jumpingPower = tempJumpPower;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, jumpradius);
    }
}
