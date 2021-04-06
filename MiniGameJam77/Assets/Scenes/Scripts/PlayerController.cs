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

    [Header ("Fear stuff")]
    [SerializeField] private float courageRefillPerSec;
    [SerializeField] private float courageThreshold;

    private float curHealth;
    private float curCourage;
    private float curBattery;
    private Vector2 fearRunningDir;

    private Rigidbody2D rigidBody;
    private GameObject darkness;
    private Inventory inventory;

    public bool isMoving = false;
    public bool isFeared = false;
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
        darkness.SetActive(true);
        GameObject canvas = GameObject.Find("Canvas");
        canvas.SetActive(true);

        curHealth = maxHealth;
        curCourage = maxCourage;
        curBattery = maxBattery;
        UIManager.UI.UpdateHealthBar(curHealth);
        UIManager.UI.UpdateCourageBar(curCourage);
        UIManager.UI.UpdateBatteryBar(curBattery);

        PlayIntroCutscene();
        //AudioManager.AM.PlayAmbience();
    }

    void Update(){
        HandleInput();
    }

    private void HandleInput(){
        // Cant do anything while in dialogue
        if (UIManager.UI.isInDialogue)
        {
            return;
        }
        // Movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Pass horizontal and vertical directions
        if (!isFeared)
        {
            Move(h, v);
            isMoving = (h == 0 && v == 0) ? false : true;
        }
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

        if (isFeared)
        {
            //Move in fear
            Move(fearRunningDir.x, fearRunningDir.y);

            // Courage fill back up while feared
            ChangeCourage(courageRefillPerSec * Time.deltaTime);
            // Stop being feared 
            if(curCourage >= courageThreshold)
            {
                isFeared = false;
            }
        }

        // Debug
        if(Input.GetButtonDown("Debug Reset")){
            // left alt
            if(darkness)
                darkness.SetActive(!darkness.activeInHierarchy);
        }

    }

    public void ChangeHealth(float amt){
        curHealth += amt;
        AudioManager.AM.PlayPlayerHurtSound();
        if (curHealth > maxHealth){
            curHealth = maxHealth;
        }else if(curHealth <= 0){
            curHealth = 0;
            // Die
            AudioManager.AM.PlayScreamSound();
            UIManager.UI.Lose();
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

        // run out of courage
        if(curCourage <= 0)
        {
            AudioManager.AM.PlayRunSound();
            AudioManager.AM.PlayScreamSound();
            isFeared = true;
            FearRun();
        }
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
        //print("moving " + h + " " + v);
        // Apply movement to velocity
        Vector2 velocity = new Vector2();
        velocity.x = speed * h * Time.deltaTime;
        velocity.y = speed * v * Time.deltaTime;

        rigidBody.velocity = velocity;
    }

    private void FearRun()
    {
        float randH = Random.Range(-1f, 1f);
        float randV = Random.Range(-1f, 1f);

        fearRunningDir = new Vector2(randH, randV);

        Move(randH, randV);
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

    public void LightsOn()
    {
        darkness.SetActive(false);
    }

    private void PlayIntroCutscene()
    {
        string[] dialogues = new string[3];
        dialogues[0] = "<coughing> (Urgh, my head is killing me. Where am I?)";
        dialogues[1] = "(The cryo chamber. Right. What’s going on? Why is it so dark?)";
        dialogues[2] = "(Why isn’t anyone else awake? Urgh. The computer must have woken me up too early. Typical Mokse Corp technology. The Captain’s going to answer for this.)";
        UIManager.UI.PlayDialogue(dialogues);
    }

    public void MoveToTable()
    {
        Move(0.5f, -0.1f);
        StartCoroutine(StopAfter(3f));
    }

    IEnumerator StopAfter(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Move(0f, 0f);
    }

    public void PlayMonsterCutscene()
    {
        // Face forward (up)
        FacePosition(transform.position + new Vector3(0, 1, 0));
        StartCoroutine(DelayAction(0.6f));
    }

    IEnumerator DelayAction(float delayTime)
    {
        yield return new WaitForSeconds(1f);
        Move(0f, -1f);
        yield return new WaitForSeconds(delayTime);
        Move(0f, 0f);
    }
}
