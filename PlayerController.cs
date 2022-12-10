using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float moveSpeed = 5f;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){
        float xInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(rb.velocity.x + xInput * moveSpeed * Time.deltaTime,rb.velocity.y);
    }
}
