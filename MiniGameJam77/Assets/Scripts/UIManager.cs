using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour{

    // static reference to self
    public static UIManager UI;

    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject courageBar;
    [SerializeField] private GameObject batteryBar;

    void Awake(){
        if(UI != null){
            Destroy(UI);
        }else{
            UI = this;
        }
    }

    void Start(){
        healthBar = GameObject.Find("Canvas/HealthBar");
        courageBar = GameObject.Find("Canvas/CourageBar");
        batteryBar = GameObject.Find("Canvas/BatteryBar");
    }

    void Update(){
        
    }

    public void UpdateHealthBar(float newAmt){
        healthBar.GetComponent<Slider>().value = newAmt;
    }
    public void UpdateCourageBar(float newAmt){
        courageBar.GetComponent<Slider>().value = newAmt;
    }
    public void UpdateBatteryBar(float newAmt){
        print(newAmt);
        batteryBar.GetComponent<Slider>().value = newAmt;
    }
}
