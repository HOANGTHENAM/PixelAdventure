using UnityEngine;
using static PlayerController;

public class FlyVerticalPlatformController : MonoBehaviour
{
    public float speedPlatform;
    public float startPos;
    public float endPos;
    private bool moveUp = true;
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
        if (moveUp)
        {
            transform.Translate(Vector3.up * speedPlatform * Time.deltaTime);
            if (transform.position.y >= endPos)
            {
                moveUp = false;
                Timer = stopTime;
            }
        }
        else
        {
            transform.Translate(Vector3.down * speedPlatform * Time.deltaTime);
            if (transform.position.y <= startPos)
            {
                moveUp = true;
                Timer = stopTime;
            }
        }
    }

}


