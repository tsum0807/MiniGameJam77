using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour{

	[SerializeField] private List<string> inventory;
	
	void Start(){
		inventory = new List<string>();
	}

	void Update(){
		
	}

	public void AddToInventory(string item)
    {
		inventory.Add(item);
    }

	public bool HaveItem(string item)
    {
		return inventory.Contains(item);
    }

	public bool UseItem(string item)
	{
		if (HaveItem(item)) { 
			inventory.Remove(item);
			return true;
		}
		return false;
	}

	public void GetItem(string itemName, Sprite itemSprite)
	{

	}
}
