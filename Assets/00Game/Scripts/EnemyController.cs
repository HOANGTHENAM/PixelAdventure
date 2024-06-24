using UnityEngine;
using static PlayerController;

public class EnemyController : MonoBehaviour
{
    public float speedEnemy;
    public float startPos;
    public float endPos;
    private bool moveRight = true;
    public float stopTime;
    private float Timer = 0.0f; // bo dem thoi gian
    [SerializeField] Animator animator;

    private void Start()
    {
        animator.SetTrigger("RUN");
    }
    void Update()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
            
            animator.SetTrigger("IDLE");
            return;
        }

        // Di chuyển enemy
        if (moveRight)
        {
            transform.Translate(Vector3.right * speedEnemy * Time.deltaTime);
            animator.SetTrigger("RUN");
            // Nếu đến tọa độ x tối đa, đổi hướng di chuyển
            if (transform.position.x >= endPos)
            {
                moveRight = false;
                Timer = stopTime;
                this.transform.localScale = Vector3.one;         
            }
        }
        else
        {
            transform.Translate(Vector3.left * speedEnemy * Time.deltaTime);
            animator.SetTrigger("RUN");
            // Nếu đến tọa độ x tối thiểu, đổi hướng di chuyển
            if (transform.position.x <= startPos)
            {
                moveRight = true;
                Timer = stopTime;                
                this.transform.localScale = new Vector3(-1, 1, 1);             
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("HIT");
        }
    }

}


