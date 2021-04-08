using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject monster;
    [SerializeField] private Sprite landingIndicator;
    [SerializeField] private Sprite monsterSprite;

    [SerializeField] private int maxHealth;
    [SerializeField] private float speed;
    [SerializeField] private float jumpTime;
    [SerializeField] private float landingTime;


    private int curHealth;
    private float curJumpTime;
    private float curLandingTime;
    private float curSpeed;
    private STATE state;

    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D _collider;
    private Animator _animator;

    public int facingDir;
    public bool isMoving;

    enum STATE{
        Walking,
        Jumping,
        Landing
    }

    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
    }

    void Start(){
        // Find player obj
        player = GameObject.Find("Player");
        curHealth = maxHealth;
        curSpeed = speed;
        curJumpTime = jumpTime;
        curLandingTime = landingTime;
        state = STATE.Walking;
    }

    void Update(){
        HandleStates();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        if(collider.tag == "Player")
        {
            // Hit player so player takes damage
            collider.GetComponent<PlayerController>().ChangeHealth(-1f);
            // Play attack sound
            AudioManager.AM.PlayAttackSound();
            // Jump
            //Jump();
        }        
        
        if(collider.tag == "Untagged" || collider.tag == "Object")
        {
            // ignore collision with these if jumping
            if(state == STATE.Jumping)
            {
                Physics2D.IgnoreCollision(collision.collider, _collider);
            }
        }
        
    }

    private void HandleStates()
    {
        if(state == STATE.Walking)
        {
            MoveTowardsObj(player);
            AudioManager.AM.PlayEnemyWalkSound();
        }
        else if(state == STATE.Jumping)
        {
            curJumpTime -= Time.deltaTime;
            // follow player position
            //transform.position = player.transform.position - new Vector3(0f, 0.5f, 0f);
            MoveTowardsObj(player);
            ExpandCircle();
            CheckJumpTimer();
        }
        else if(state == STATE.Landing)
        {
            curLandingTime -= Time.deltaTime;
            ShrinkCircle();
            CheckLandTimer();
        }
    }

    private void CheckJumpTimer(){
        if(curJumpTime <= 0f){
            curJumpTime = jumpTime;
            // Finished jumping
            // Start land animation and stuff
            state = STATE.Landing;
            
            //transform.localScale = Vector3.zero;
        }
    }

    private void CheckLandTimer(){
        if(curLandingTime <= 0f){
            curLandingTime = landingTime;
            // Finished landing
            // Land animation and stuff
            state = STATE.Walking;
            // Land
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = monsterSprite;
            _collider.enabled = true;
            // Set scale to normal
            transform.localScale = new Vector3(1f, 1f, 1f);
            // Set speed to normal
            curSpeed = speed;
            // Set layer to normal
            gameObject.layer = LayerMask.NameToLayer("Monster");
            // Give back fear
            gameObject.GetComponentInChildren<FearCircle>().canFear = true;
            _animator.enabled = true;
        }
    }

    private void ExpandCircle()
    {
        // expand circle
        float curAmt = (jumpTime - curJumpTime) / jumpTime;
        Vector3 newScale = new Vector3(curAmt, curAmt, 1);
        transform.localScale = newScale;
    }

    private void ShrinkCircle()
    {
        // shrink circle
        float curAmt = curLandingTime / landingTime;
        Vector3 newScale = new Vector3(curAmt, curAmt, 1);
        transform.localScale = newScale;
    }


    private void MoveTowardsObj(GameObject target){
        float step = curSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }

    public void TakeDamage(){
        curHealth--;
        PlayHurtAnim();
        AudioManager.AM.PlayEnemyHurtSound();
        if (curHealth <= 0){
            // Die
            //gameObject.SetActive(false);
            Destroy(monster);
        }
        Invoke("Jump", 2);
    }

    public void Jump()
    {
        // "Jump" disappear from scene and reappear later
        //_collider.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("MonsterJumping");
        // play some jump anim
        state = STATE.Jumping;
        curJumpTime = jumpTime;
        curLandingTime= landingTime;
        // cant fear while jumping
        gameObject.GetComponentInChildren<FearCircle>().canFear = false;

        // Show monster landing location indicator
        _animator.enabled = false;
        spriteRenderer.sprite = landingIndicator;

        // Reduce speed while in mid jump
        curSpeed = speed * 0.5f;
    }

    private void PlayHurtAnim(){
        // GetComponent<SpriteRenderer>().color = Color.red;
    }
}
