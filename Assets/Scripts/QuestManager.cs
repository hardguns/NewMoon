using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    //Instance to use it in other scripts
    public static QuestManager instance;

    //Names of quests
    public string[] questMakerNames;

    //Checks if a quest is finish or not
    public bool[] questMakersComplete;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        questMakersComplete = new bool[questMakerNames.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(CheckIfComplete("Quest test"));
            MarkQuestComplete("Quest test");
            MarkQuestIncomplete("Fight the demon");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveQuestData();
            Debug.Log("Quests saved!");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadQuestData();
            Debug.Log("Quests load!");
        }
    }

    public int GetQuestNumber(string questToFind)
    {
        for (int i = 0; i < questMakerNames.Length; i++)
        {
            if (questMakerNames[i] == questToFind)
            {
                return i;
            }
        }

        Debug.LogError("Quest " + questToFind + " does not exist");
        return 0;
    }

    public bool CheckIfComplete(string questToCheck)
    {
        if (GetQuestNumber(questToCheck) != 0)
        {
            return questMakersComplete[GetQuestNumber(questToCheck)];
        }

        return false;
    }

    public void MarkQuestComplete(string questToMark)
    {
        questMakersComplete[GetQuestNumber(questToMark)] = true;

        UpdateLocalQuestObjects();
    }

    public void MarkQuestIncomplete(string questToMark)
    {
        questMakersComplete[GetQuestNumber(questToMark)] = false;

        UpdateLocalQuestObjects();
    }

    public void UpdateLocalQuestObjects()
    {
        QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();

        for (int i = 0; i < questObjects.Length; i++)
        {
            questObjects[i].CheckCompletion();
        }
    }

    public void SaveQuestData()
    {
        for (int i = 0; i < questMakerNames.Length; i++)
        {
            if (questMakersComplete[i])
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMakerNames[i], 1);
            }
            else
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMakerNames[i], 0);
            }
        }
    }

    public void LoadQuestData()
    {
        for (int i = 0; i < questMakerNames.Length; i++)
        {
            int valueToSet = 0;
            if (PlayerPrefs.HasKey("QuestMarker_" + questMakerNames[i]))
            {
                valueToSet = PlayerPrefs.GetInt("QuestMarker_" + questMakerNames[i]);
            }

            if (valueToSet == 0)
            {
                questMakersComplete[i] = false;
            }
            else
            {
                questMakersComplete[i] = true;
            }
        }
    }
}
