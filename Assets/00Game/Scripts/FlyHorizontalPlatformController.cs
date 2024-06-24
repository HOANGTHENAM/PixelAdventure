using UnityEngine;
using static PlayerController;

public class FlyHorizontalPlatformController : MonoBehaviour
{
    public Vector3 lastPlatformPosition;
    public float speedPlatform;
    public float startPos;
    public float endPos;
    private bool moveRight = true;
    public float stopTime;
    private float Timer = 0.0f; // bo dem thoi gian

    void Update()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
            return;
        }

        // Di chuyển 
        if (moveRight)
        {
            transform.Translate(Vector3.right * speedPlatform * Time.deltaTime);
            // Nếu đến tọa độ x tối đa, đổi hướng di chuyển
            if (transform.position.x >= endPos)
            {
                moveRight = false;
                Timer = stopTime;
            }
        }
        else
        {
            transform.Translate(Vector3.left * speedPlatform * Time.deltaTime);
            // Nếu đến tọa độ x tối thiểu, đổi hướng di chuyển
            if (transform.position.x <= startPos)
            {
                moveRight = true;
                Timer = stopTime;
            }
        }
    }
    //lay vi tri cuoi cung cua nhan vat khi cham vao platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            lastPlatformPosition = transform.position;
        }
    }

    //khi o tren platform nhan vat di chuyen theo platform
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Vector3 platformDelta = transform.position - lastPlatformPosition;
            collision.transform.position += platformDelta;
            lastPlatformPosition = transform.position;
        }
    }


}


