using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObj : MonoBehaviour{

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private bool isItem;

    private GameObject player;

    enum STATE{
        Normal,
        Interacted
    }

    private STATE state;

    void Start(){
        player = GameObject.Find("Player");
        state = STATE.Normal;
    }

    void Update(){
        
    }

    public void Interact(){
        state = STATE.Interacted;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // Give item if is an item
        if (isItem)
        {
            player.GetComponent<Inventory>().AddToInventory(gameObject.name);
            UIManager.UI.InsertInventoryBar(gameObject.name, sr.sprite);
        }

        // Change sprite if exists
        if(sprites.Length > 1){
            sr.sprite = sprites[1];
        }else{
            // Disable object if no "disabled" sprite
            gameObject.SetActive(false);
            return;
        }

        // Disable any hitbox if exists
        // Mainly for doors
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if(collider != null){
            collider.enabled = false;
        }
    }

    public bool IsInteractable(){
        return state == STATE.Normal ? true : false;
    }
}
