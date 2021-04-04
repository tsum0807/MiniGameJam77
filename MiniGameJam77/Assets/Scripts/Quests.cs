using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quests : MonoBehaviour
{

    public static Quests QUEST;

    [SerializeField] TMPro.TextMeshProUGUI questText;

    private int curQuest = 0;
    private PlayerController playerController;
    private MonsterController monsterController;
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject player;

    void Awake()
    {
        if (QUEST != null)
        {
            Destroy(QUEST);
        }
        else
        {
            QUEST = this;
        }
    }

    void Start()
    {
        if(questText == null)
        {
            // questText obj wasnt set
            // so try find it
            questText = GameObject.Find("Canvas/QuestContainer/QuestText").GetComponent<TMPro.TextMeshProUGUI>();
        }
        playerController = player.GetComponent<PlayerController>();
        monsterController = monster.GetComponent<MonsterController>();
    }

    void Update()
    {
    }

    private void UpdateQuestText()
    {
        switch (curQuest)
        {
            case 0:
                questText.text = "Investigate the area.";
                break;
            case 1:
                questText.text = "Open the red door.";
                break;
            case 2:
                questText.text = "Find the power room.";
                // Spawn Monster
                monster.SetActive(true);
                break;
            case 3:
                questText.text = "Find a fuse.";
                monsterController.Jump();
                break;
            case 4:
                questText.text = "Install the fuse in the power room.";
                monsterController.Jump();
                break;
            case 5:
                // Fused installed so turn on the lights
                playerController.LightsOn();
                monster.SetActive(false);
                // Give player "PowerOn"
                player.GetComponent<Inventory>().AddToInventory("PowerOn");
                break;
            case 6:
                // Win
                UIManager.UI.Win();
                break;
        }
    }

    public void NextQuest()
    {
        curQuest++;
        UpdateQuestText();
    }
}