using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour{

    [SerializeField] private GameObject interactText;

    void Start(){
        
    }

    void Update(){
        
    }

    void OnTriggerEnter2D(Collider2D other){
        print("entered");
        if(other.tag == "Player"){
            interactText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        print("left");
        if(other.tag == "Player"){
            interactText.SetActive(false);
        }
    }
}
