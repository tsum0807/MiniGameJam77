using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

	// static reference to self
	public static UIManager UI;

	[Header("UI Objects")]
	[SerializeField] private GameObject healthBar;
	[SerializeField] private GameObject courageBar;
	[SerializeField] private GameObject batteryBar;
	[SerializeField] private GameObject inventoryBar;
	[SerializeField] private GameObject noteObj;
	[SerializeField] private GameObject noteText;
	[SerializeField] private GameObject dialogueBox;
	[SerializeField] private GameObject dialogueText;
    [SerializeField] private GameObject safeZone;

    [Header("Objects")]
	[SerializeField] private GameObject player;

	public bool isInDialogue = false;
	public bool win = false;

	private string[] _dialogues;
	private int curDialogue = 0;
	private bool isIntro = true;

	void Awake()
	{
		if (UI != null)
		{
			Destroy(UI);
		}
		else
		{
			UI = this;
		}
	}

	void Start()
	{
		healthBar = GameObject.Find("Canvas/HealthBar");
		courageBar = GameObject.Find("Canvas/CourageBar");
		batteryBar = GameObject.Find("Canvas/BatteryBar");
		inventoryBar = GameObject.Find("Canvas/InventoryBar");
		//dialogueBox = GameObject.Find("Canvas/DialogueBox");
	}

	void Update()
	{
		if (isInDialogue && Input.GetMouseButtonDown(0))
		{
			UpdateDialogue();
		}else if(win && Input.GetMouseButtonDown(0))
        {
			Win();
        }

	}

	private void UpdateDialogue()
	{
		if (curDialogue < _dialogues.Length)
		{
			// Check for note indicator, if is play next one as note
			if (_dialogues[curDialogue] == "Note")
			{
				// next dialogue should be shown as a note
				curDialogue++;
				dialogueBox.SetActive(false);
				noteObj.SetActive(true);
				noteText.GetComponent<TMPro.TextMeshProUGUI>().text = _dialogues[curDialogue];
			}
			else if (_dialogues[curDialogue] == "!")
			{
				// Show the mosnter cutscene, how good is this code :)
				player.GetComponent<PlayerController>().PlayMonsterCutscene();
				noteObj.SetActive(false);
				dialogueBox.SetActive(true);
				dialogueText.GetComponent<TMPro.TextMeshProUGUI>().text = _dialogues[curDialogue];
			}
            else if (_dialogues[curDialogue] == "You can drive the monster off with the hi-beam of your flashlight. Press F, but remember that the hi-beam uses up your battery!")
            {
                if(curDialogue + 1 > _dialogues.Length - 1)
                {
                    safeZone.SetActive(false);
                }
            }
            else
			{
				noteObj.SetActive(false);
				dialogueBox.SetActive(true);
				dialogueText.GetComponent<TMPro.TextMeshProUGUI>().text = _dialogues[curDialogue];
			}
		}
		else
		{
			if (isIntro)
			{
				player.GetComponent<PlayerController>().MoveToTable();
				dialogueBox.SetActive(false);
				isIntro = false;
			}
			else
			{
				isIntro = false;
				isInDialogue = false;
				dialogueBox.SetActive(false);
				noteObj.SetActive(false);
			}
            if (win)
            {
				Win();
            }
		}
		// Next dialogue
		curDialogue++;
	}

	public void UpdateHealthBar(float newAmt)
	{
		healthBar.GetComponent<Slider>().value = newAmt;
	}
	public void UpdateCourageBar(float newAmt)
	{
		courageBar.GetComponent<Slider>().value = newAmt;
	}
	public void UpdateBatteryBar(float newAmt)
	{
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
		if (toRemove != null)
		{
			Destroy(toRemove);
		}
	}
	public void PlayDialogue(string[] dialogue)
	{
		for (int i = 0; i < dialogue.Length; i++)
		{
			// turn new lines into actual newlines
			dialogue[i] = dialogue[i].Replace("\\n", "\n");
		}
		isInDialogue = true;
		curDialogue = 0;
		_dialogues = dialogue;
		UpdateDialogue();
	}
	public void PlayDialogue(string dialogue)
	{
		string[] dialogues = new string[1];
		dialogues[0] = dialogue;
		PlayDialogue(dialogues);
	}

	public void Win()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void Lose()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
