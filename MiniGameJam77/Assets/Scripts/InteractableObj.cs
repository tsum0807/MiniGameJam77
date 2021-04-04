using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObj : MonoBehaviour{

    [Header("Sprites Settings")]
    [SerializeField] private Sprite[] sprites;

    [Header("Requirement Settings")]
    [SerializeField] private bool isItem;
    [SerializeField] private bool isNote;
    [SerializeField] private bool isQuestTrigger;
    [SerializeField] private bool hasDialogue;
    [SerializeField] private string[] dialogue;
    [SerializeField] private string itemRequired;
    [SerializeField] private bool itemGetsUsed;

    private GameObject player;
    private bool noteActive;

    enum STATE{
        Normal,
        Interacted
    }

    private STATE state;

    void Start(){
        player = GameObject.Find("Player");
        state = STATE.Normal;
        noteActive = false;
    }

    void Update()
    {
        // Close note if clicked on screen
        if (noteActive && Input.GetMouseButtonDown(0))
        {
            transform.Find("NoteCanvas").gameObject.SetActive(false);
            noteActive = false;
        }
    }

    public bool Interact(){
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Inventory playerInventory = player.GetComponent<Inventory>();

        // Give item if is an item
        if (isItem)
        {
            playerInventory.AddToInventory(gameObject.name);
            UIManager.UI.InsertInventoryBar(gameObject.name, sr.sprite);
        }

        // Show note if note
        if (isNote && !noteActive)
        {
            // Show note
            transform.Find("NoteCanvas").gameObject.SetActive(true);
            noteActive = true;
            // because notes should just stay there and not disappear
            return false;
        }


        if (itemRequired != "")
        {
            if (!playerInventory.HaveItem(itemRequired))
            {
                // Dont have required item
                // Show indicator 
                if (itemRequired == "PowerOn")
                {
                    UIManager.UI.PlayDialogue("The door to the next section isn’t powered with batteries. The power core will need to be repaired before it will open.");
                }
                else
                {
                    UIManager.UI.PlayDialogue("I need a " + itemRequired + ". There must be one around somewhere.");
                }
                return false;
            }else if (itemGetsUsed)
            {
                playerInventory.UseItem(itemRequired);
            }
        }

        // Dialogue shows with the given dialogue text
        if (hasDialogue)
        {
            UIManager.UI.PlayDialogue(dialogue);
        }
        else
        {
            state = STATE.Interacted;
        }

        // Change sprite if exists
        if (sprites.Length > 1){
            sr.sprite = sprites[1];
        }else{
            // Disable object if no "disabled" sprite
            gameObject.SetActive(false);
        }

        // Disable any hitbox if exists
        // Mainly for doors
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if(collider != null){
            collider.enabled = false;
        }

        // Advance quest line if item is a trigger
        //QuestTrigger trigger = GetComponent<QuestTrigger>();
        if(isQuestTrigger)
        {
            Quests.QUEST.NextQuest();
            // make sure cant trigger off same thing
            isQuestTrigger = false;
        }

        return true;
    }

    public bool IsInteractable(){
        return state == STATE.Normal ? true : false;
    }
}
