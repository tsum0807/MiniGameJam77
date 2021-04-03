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
	[SerializeField] private GameObject inventoryBar;

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
		inventoryBar = GameObject.Find("Canvas/InventoryBar");
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
	public void InsertInventoryBar(string itemName, Sprite itemSprite)
	{
		GameObject newItem = new GameObject();
		newItem.name = itemName;
		newItem.AddComponent<Image>();
		newItem.GetComponent<Image>().sprite = itemSprite;
		Instantiate(newItem, inventoryBar.transform);
	}
	public void RemoveFromInventoryBar(string itemName)
    {
		GameObject toRemove = inventoryBar.transform.Find(itemName).gameObject;
		if(toRemove != null)
        {
			Destroy(toRemove);
        }
    }
}
