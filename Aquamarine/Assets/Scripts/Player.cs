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

    [SerializeField] public bool _active = true;
    private Collider2D _collider;

    private Vector2 respawnPosition;
    [SerializeField] private GameObject fakePlayer;
    private Vector2 fakePlayerPos = new Vector2(-10f, -10f);
    public static GameObject mostNearByAnchor;

    [SerializeField] private Animator playeranim;
    private bool isJumping;
    [SerializeField] private Vector3 size;
    private float tempJumpPower;
    public float slowFactor = 1.75f;

    [SerializeField] private GameObject shieldObject;
    private GameObject tempObject;
    private bool isRunning;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource loseSfx;
    [SerializeField] private AudioSource runSfx;
    [SerializeField] private AudioSource jumpSfx;
    [SerializeField] private AudioSource failSfx;
    [SerializeField] private AudioClip normalRunClip;
    [SerializeField] private AudioClip puddleRunClip;

    private void Start()
    {
        instance = this;
        _collider = GetComponent<Collider2D>();
        SetRespawnPoint(transform.position);
        
        if(fakePlayer != null)
        {
            fakePlayer.transform.position = fakePlayerPos;
        }
        tempJumpPower = jumpingPower;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_active || playeranim.GetCurrentAnimatorStateInfo(0).IsName("ability"))
        {
            isJumping = false;
            jumpSfx.Stop();
            runSfx.Stop();
            rb.velocity = new Vector3(0,rb.velocity.y,0);
            return;
        }


        if(isRunning && !isJumping && playeranim.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            if(isGrounded())
            {
                runSfx.clip = normalRunClip;
            }
            else if(PlayerAttribute.instance.IsOnWaterSource() ||
            PlayerAttribute.instance.IsOnWaterPlatform())
            {
                runSfx.clip = puddleRunClip;
            }

            if (!isGrounded() &&
            !PlayerAttribute.instance.IsOnWaterSource() &&
            !PlayerAttribute.instance.IsOnWaterPlatform())
            {
                runSfx.Stop();
            }
            else
            {
                if(!runSfx.isPlaying)
                {
                    runSfx.Play();
                }
            }
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (playeranim.GetCurrentAnimatorStateInfo(0).IsName("idle") && rb.velocity.x != 0 && !isJumping)
        {
            PlayRunAudio();
            isRunning = true;
            playeranim.Play("run", 0, 0);
        }
        else if((playeranim.GetCurrentAnimatorStateInfo(0).IsName("run") || playeranim.GetCurrentAnimatorStateInfo(0).IsName("start run") )&& rb.velocity.x == 0 && !isJumping)
        {
            runSfx.Stop();
            isRunning = false;
            playeranim.Play("idle", 0, 0);
        }

        if (Input.GetButtonDown("Jump") && !isJumping && (isGrounded() || 
            PlayerAttribute.instance.IsOnWaterSource() || 
            PlayerAttribute.instance.IsOnWaterPlatform()))
        {
            isJumping = true;
            runSfx.Stop();
            jumpSfx.Play();
            playeranim.Play("start jump", 0, 0);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if(fakePlayer == null)
        { 
            return;
        }
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

    public void SetPlayerInactive()
    {
        _active = false;
        instance.GetComponent<Animator>().Play("idle", 0, 0);
    }

    public bool isGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, size, 0, groundLayer);
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
        rb.velocity = new Vector2(0, jumpingPower / 3);
    }

    public void Die()
    {
        _active = false;
        _collider.enabled = false;
        LeanTween.rotateZ(gameObject,90.0f, 0.4f);
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
        playeranim.enabled = false;
        isJumping = false;
        loseSfx.Play();
        yield return new WaitForSeconds(0.8f);
        playeranim.enabled = true;
        transform.position = respawnPosition;  
        transform.rotation = Quaternion.identity;
        _active = true;
        _collider.enabled = true;
        MiniJump();
    }

    public void MakeProtected()
    {
        tempObject = Instantiate(shieldObject, transform);
        PlayerAttribute.instance.StartFlashtext(true);
        // TODO: Instantiate a shield object here
        _isProtected = true;
    }

    public void MakeUnProtected()
    {
        PlayerAttribute.instance.StartFlashtext(false);
        Destroy(tempObject);
        // TODO: Remove the shield object here
        if (_isProtected)
        {
            _isProtected = false;
            jumpingPower *= slowFactor;
            speed *= slowFactor;
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
        jumpingPower /= slowFactor;
        speed /= slowFactor;
    }

    public void AquaTeleportation()
    {
        if (mostNearByAnchor != null)
        {
            instance.transform.position = mostNearByAnchor.transform.position;
            instance.ResetFakePlayer();
        }
        else
        {
            PlayerAttribute.instance.abilitysfx.Stop();
            failSfx.Play();
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
            GameObject mostNearbyAnchor = null;
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
        if(rb.velocity.y <= 0.1f && (isGrounded() || PlayerAttribute.instance.IsOnWaterPlatform() || PlayerAttribute.instance.IsOnWaterSource()))
        {
            playeranim.Play("jump end");

        }
    }

    public void SetNotJumping()
    {
        isJumping = false;
    }

    public void CheckJumpPower()
    {
        tempJumpPower= jumpingPower;
        if (PlayerAttribute.instance.IsOnWaterSource() || PlayerAttribute.instance.IsOnWaterPlatform())
            jumpingPower *= jumpingMultiplier;
    }

    public void ApplyJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        jumpingPower = tempJumpPower;

    }

    public void PlayRunAudio()
    {
        runSfx.PlayDelayed(0.12f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, size);
    }
}
