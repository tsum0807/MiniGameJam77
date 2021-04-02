using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private GameObject darkness;
    void Start(){
        darkness.SetActive(true);
    }

    void Update(){
        
    }
}
