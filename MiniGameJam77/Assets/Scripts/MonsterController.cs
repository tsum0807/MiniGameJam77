using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Sprite landingIndicator;
    [SerializeField] private Sprite monsterSprite;

    [SerializeField] private int maxHealth;
    [SerializeField] private int speed;
    [SerializeField] private float jumpTime;
    [SerializeField] private float landingTime;


    private int curHealth;
    private float curJumpTime;
    private float curLandingTime;
    private STATE state;

    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D collider;

    enum STATE{
        Walking,
        Jumping,
        Landing
    }

    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    void Start(){
        // Find player obj
        player = GameObject.Find("Player");
        curHealth = maxHealth;
        curJumpTime = jumpTime;
        curLandingTime = landingTime;
        state = STATE.Walking;
    }

    void Update(){
        HandleStates();
    }

    private void HandleStates(){
        if(state == STATE.Walking){
            MoveTowardsObj(player);
        }else if(state == STATE.Jumping){
            curJumpTime -= Time.deltaTime;
            CheckJumpTimer();
        }else if(state == STATE.Landing){
            curLandingTime -= Time.deltaTime;
            PlayLandingAnim();
            CheckLandTimer();
        }
    }

    private void CheckJumpTimer(){
        if(curJumpTime <= 0f){
            curJumpTime = jumpTime;
            // Finished jumping
            // Start land animation and stuff
            state = STATE.Landing;
            
            // Show monster landing location indicator
            transform.position = player.transform.position;
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = landingIndicator;
            transform.localScale = Vector3.zero;
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
            collider.enabled = true;
            // Set scale to normal
            transform.localScale = new Vector3(1f, 1f, 1f);
            print("landed");
        }
    }

    private void PlayLandingAnim()
    {
        // expand circle
        float curAmt = (landingTime - curLandingTime) / landingTime;
        Vector3 newScale = new Vector3(curAmt, curAmt, 1);
        transform.localScale = newScale;
    }


    private void MoveTowardsObj(GameObject target){
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }

    public void TakeDamage(){
        curHealth--;
        PlayHurtAnim();
        if(curHealth <= 0){
            // Die
            
        }

        // "Jump" disappear from scene and reappear later
        spriteRenderer.enabled = false;
        collider.enabled = false;
        // play some jump anim
        state = STATE.Jumping;
        // gameObject.SetActive(false);
    }

    private void PlayHurtAnim(){
        // GetComponent<SpriteRenderer>().color = Color.red;
    }
}
