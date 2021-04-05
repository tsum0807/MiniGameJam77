using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour{

    [SerializeField] private GameObject interactText;

    private InteractableObj parentObj;

    private bool playerInside;

    void Start(){
        playerInside = false;
        parentObj = transform.parent.GetComponent<InteractableObj>();
    }

    void Update(){
        if(playerInside && Input.GetButtonDown("Interact")){
            if (parentObj.Interact())
            {
                // Interacted so disable text
                interactText.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            playerInside = true;
            // Only show interact text if interactable
            if(parentObj.IsInteractable()){
                interactText.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Player"){
            playerInside = false;
            interactText.SetActive(false);
        }
    }
}
