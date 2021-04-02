using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed = 2f;

    private Rigidbody2D rigidBody;

    void Start(){
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update(){
        HandleInput();
    }

    private void HandleInput(){
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        // Pass horizontal and vertical directions
        Move(h, v);
    }

    private void Move(float h, float v){
        //
        Vector2 velocity = new Vector2();
        velocity.x = speed * h * Time.deltaTime;
        velocity.y = speed * v * Time.deltaTime;

        rigidBody.velocity = velocity;
        // gameObject.transform.position = new Vector2 (transform.position.x + (h * speed), transform.position.y + (v * speed));
    }
}
