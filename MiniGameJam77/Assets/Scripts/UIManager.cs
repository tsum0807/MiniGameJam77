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
	[SerializeField] private GameObject dialogueBox;
	[SerializeField] private GameObject dialogueText;

	private bool isInDialogue = false;
	private string[] _dialogues;
	private int curDialogue = 0;


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
		//dialogueBox = GameObject.Find("Canvas/DialogueBox");
	}

	void Update(){
        if (isInDialogue && Input.GetMouseButtonDown(0))
        {
			// Next dialogue
			curDialogue++;
			if (curDialogue < _dialogues.Length)
			{
				dialogueText.GetComponent<TMPro.TextMeshProUGUI>().text = _dialogues[curDialogue];
			}
			else
			{
				isInDialogue = false;
				dialogueBox.SetActive(false);
			}
        }
	}

	public void UpdateHealthBar(float newAmt){
		healthBar.GetComponent<Slider>().value = newAmt;
	}
	public void UpdateCourageBar(float newAmt){
		courageBar.GetComponent<Slider>().value = newAmt;
	}
	public void UpdateBatteryBar(float newAmt){ 
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
	public void PlayDialogue(string[] dialogue)
	{
		for(int i=0; i<dialogue.Length; i++)
		{
			// turn new lines into actual newlines
			dialogue[i] = dialogue[i].Replace("\\n", "\n");
		}
		dialogueBox.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = dialogue[0];
        dialogueBox.SetActive(true);
		isInDialogue = true;
		curDialogue = 0;
		_dialogues = dialogue;
	}
	public void PlayDialogue(string dialogue)
	{
		string[] dialogues = new string[1];
		dialogues[0] = dialogue;
		PlayDialogue(dialogues);
	}
}
