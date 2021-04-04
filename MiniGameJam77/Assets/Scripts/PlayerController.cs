using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private FieldOfView fov;
    [SerializeField] private float speed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxCourage;
    [SerializeField] private float maxBattery;

    private float curHealth;
    private float curCourage;
    private float curBattery;

    private Rigidbody2D rigidBody;
    private GameObject darkness;
    private Inventory inventory;

    public bool isMoving = false;
    public int facingDir = 0;
    // 0 - down
    // 1 - left
    // 2 - up
    // 3 - right

    void Start(){
        rigidBody = GetComponent<Rigidbody2D>();
        inventory = GetComponent<Inventory>();
        fov = GetComponentInChildren<FieldOfView>();
        darkness = transform.Find("Darkness").gameObject;

        curHealth = maxHealth;
        curCourage = maxCourage;
        curBattery = maxBattery;
        UIManager.UI.UpdateHealthBar(curHealth);
        UIManager.UI.UpdateCourageBar(curCourage);
        UIManager.UI.UpdateBatteryBar(curBattery);
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
        isMoving = (h == 0 && v == 0) ? false : true;
        //CalculateDir(h, v);

        // Aim
        // Get mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        FacePosition(mousePos);
        CalculateDir(mousePos);

        // Switch flashlight mode
        if(Input.GetButtonDown("Switch Mode")){
            fov.SwitchMode();
        }

        // Interact, done in player detector

        // Debug
        if(Input.GetButtonDown("Debug Reset")){
            // left alt
            if(darkness)
                darkness.SetActive(!darkness.activeInHierarchy);
        }

    }

    public void ChangeHealth(float amt){
        curHealth += amt;
        if(curHealth > maxHealth){
            curHealth = maxHealth;
        }else if(curHealth <= 0){
            curHealth = 0;
            // Die
        }
        UIManager.UI.UpdateHealthBar(curHealth);
    }

    public void ChangeCourage(float amt){
        curCourage += amt;
        if(curCourage > maxCourage){
            curCourage = maxCourage;
        }else if(curCourage <= 0){
            curCourage = 0;
        }
        UIManager.UI.UpdateCourageBar(curCourage);
    }

    public void ChangeBattery(float amt){
        curBattery += amt;
        if(curBattery > maxBattery){
            curBattery = maxBattery;
        }else if(curBattery <= 0){
            curBattery = 0;
        }
        UIManager.UI.UpdateBatteryBar(curBattery);
    }
    public float GetCurBattery(){
        return curBattery;
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

    //private void calculatedir(float h, float v)
    //{
    //    if (h == 0 && v == 0)
    //    {
    //        ismoving = false;
    //        return;
    //    }
    //    ismoving = true;
    //    if (h > 0)
    //    {
    //        right
    //       facingdir = 3;
    //    }
    //    else if (h < 0)
    //    {
    //        left
    //       facingdir = 1;
    //    }

    //    if (v > 0)
    //    {
    //        up
    //       facingdir = 2;
    //    }
    //    else if (v < 0)
    //    {
    //        down
    //       facingdir = 0;
    //    }
    //}

    private void CalculateDir(Vector3 mousePos)
    {
        Vector3 lookPos = mousePos - transform.position;

        if(lookPos.x >= lookPos.y)
        {
            // bot right diag
            if(lookPos.x >= -lookPos.y)
            {
                // right triangle
                facingDir = 3;
            }
            else
            {
                // bottom triangle
                facingDir = 0;
            }
        }
        else
        {
            // top right diag
            if (lookPos.x >= -lookPos.y)
            {
                // top triangle
                facingDir = 2;
            }
            else
            {
                // left triangle
                facingDir = 1;
            }
        }
    }

}
