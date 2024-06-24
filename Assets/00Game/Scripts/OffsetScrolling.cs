using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScrolling : MonoBehaviour
{
    public float scrollSpeed;
    [SerializeField] MeshRenderer meshrend;
    void Start()
    {
        
    }

    void Update()
    {
        float y = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2(0, y);
        meshrend.material.mainTextureOffset = offset;
    }
}
