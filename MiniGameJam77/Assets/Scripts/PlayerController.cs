using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private FieldOfView fov;
    [SerializeField] private float speed;

    private Rigidbody2D rigidBody;

    void Start(){
        rigidBody = GetComponent<Rigidbody2D>();
        fov = GetComponentInChildren<FieldOfView>();
    }

    void Update(){
        HandleInput();
    }

    private void HandleInput(){
        // Movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        // Pass horizontal and vertical directions
        Move(h, v);

        // Aim
        // Get mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        FacePosition(mousePos);

        // Switch flashlight mode
        if(Input.GetButtonDown("Switch Mode")){
            fov.SwitchMode();
        }
    }

    private void Move(float h, float v){
        // Apply movement to velocity
        Vector2 velocity = new Vector2();
        velocity.x = speed * h * Time.deltaTime;
        velocity.y = speed * v * Time.deltaTime;

        rigidBody.velocity = velocity;
    }

    private void FacePosition(Vector3 mousePos){
        Vector3 lookPos = mousePos - transform.position;
        float theta = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg ;
        // Quaternion angle = Quaternion.Euler(new Vector3(0, 0, theta));
        if(theta < 0){
            theta += 360;
        }

        fov.AimAtAngle(theta);
    }
}
