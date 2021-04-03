using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObj : MonoBehaviour{

    [SerializeField] private Sprite[] sprites;

    enum STATE{
        Normal,
        Interacted
    }

    private STATE state;

    void Start(){
        state = STATE.Normal;
    }

    void Update(){
        
    }

    public void Interact(){
        state = STATE.Interacted;
        // Change sprite if exists
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sprites.Length > 1){
            sr.sprite = sprites[1];
        }else{
            // Disable object if no "disabled" sprite
            gameObject.SetActive(false);
            return;
        }

        // Disable any hitbox if exists
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if(collider != null){
            collider.enabled = false;
        }
    }

    public bool IsInteractable(){
        return state == STATE.Normal ? true : false;
    }
}
