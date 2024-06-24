using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField]AnimController animCtrl;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private float horizontal;
    private bool isFacingRight = true;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 7f;
    //[SerializeField] float doubleJumpForce = 7f;
    private bool isDoubleJumped;

    private bool isWallSliding;
    private float wallSlidingSpeed = 3f;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingForce = new Vector2(1f, 10f);

    public bool isDead = false;

    public int score;

    public enum PLAYERSTATE
    {
        IDLE = 0,
        JUMP = 1,
        DOUBLEJUMP = 2,
        WALLJUMP = 3,
        FALL = 4,
        RUN = 5,
        HIT = 6,
        DEATH = 7
    }
    public PLAYERSTATE state = PLAYERSTATE.IDLE;

    private void Start()
    {
        animCtrl = GetComponentInChildren<AnimController>();
    }
    void Update()
    {
        Movement();
        

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }

        UpdateState();
        animCtrl.ChangeAnim(state);
    }


    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }
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

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingForce.x, wallJumpingForce.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (IsGrounded() && !(Input.GetKey(KeyCode.Space)))
        {
            isDoubleJumped = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded() || isDoubleJumped)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isDoubleJumped = !isDoubleJumped;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    void Hit()
    {
        state = PLAYERSTATE.HIT;
        animCtrl.ChangeAnim(state);
    }
    void UpdateState()
    {
        if (IsGrounded())
        {
            if (horizontal != 0)
            {
                state = PLAYERSTATE.RUN;
                
            }
            else
            {
                state = PLAYERSTATE.IDLE;
            }
        }
        else
        {
            if (rb.velocity.y > 0.1f)
            {
                state = PLAYERSTATE.JUMP;
            }
            if (rb.velocity.y > 0.1f && !isDoubleJumped)
            {
                state = PLAYERSTATE.DOUBLEJUMP;
            }

            if (rb.velocity.y < -0.1f)
            {
                state = PLAYERSTATE.FALL;
            }

            if (isWallSliding)
                state = PLAYERSTATE.WALLJUMP;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Flag"))
        {
            this.transform.position = new Vector2 (-7, -4);
            MissionCompeleted();
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Hit();
            FindObjectOfType<GameManager>().GameOver();
            enabled = false;
            AudioManager.instance.PlaySfx("Hit");
            StartCoroutine(DeathRoutine());
        }

        if (collision.gameObject.CompareTag("Spike"))
        {
            Hit();
            FindObjectOfType<GameManager>().GameOver();
            enabled = false;
            AudioManager.instance.PlaySfx("Hit");
            StartCoroutine(DeathRoutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHead"))
        {
            DestroyEnemy(collision.gameObject);
            AudioManager.instance.PlaySfx("EnemyDeath");
        }

        if (collision.gameObject.CompareTag("Fruit"))
        {
            CollectFruit(collision.gameObject);
        }

    }
    IEnumerator DeathRoutine()
    {
        isDead = true;
        yield return new WaitForSeconds(1f);
        animCtrl.ChangeAnim(PLAYERSTATE.DEATH);
    }
    private void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy.transform.parent.gameObject); //xoa parent vi enemyhead la con cua enemy
        AddScore(10);
    }
    private void CollectFruit(GameObject fruit)
    {
        Destroy(fruit);
        AddScore(5);
        AudioManager.instance.PlaySfx("CollectFruit");
    }
    void AddScore(int points)
    {
        score += points;
    }
 
    public void MissionCompeleted()
    {
        AudioManager.instance.PlaySfx("MissionCompleted");
        FindObjectOfType<GameManager>().NextLevel();
    }
}
