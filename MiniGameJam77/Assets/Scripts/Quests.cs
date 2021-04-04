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
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
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
                questText.text = "Find the power room.";
                break;
            case 2:
                questText.text = "Find a fuse.";
                break;
            case 3:
                questText.text = "Install the fuse in the power room.";
                break;
            case 4:
                // Fused installed so turn on the lights
                playerController.LightsOn();
                break;
        }
    }

    public void NextQuest()
    {
        curQuest++;
        UpdateQuestText();
    }
}