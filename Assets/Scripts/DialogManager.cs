using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    //Creates the instance of dialog manager to use it in other scripts
    public static DialogManager instance;

    //Text to asign dialog
    public Text DialogText;

    //Text to asign name
    public Text nameText;

    //Gameobjects to show or not the dialog and name box
    public GameObject dialogBox;
    public GameObject nameBox;

    //Dialog the object or NPC has
    public string[] dialogLines;

    public int currentLine;

    private bool justStarted;

    //------------------------ Quest -----------------------
    //Name of quest tha is gonna be tracked
    private string questToMark;

    //Marks the quest as complete after finish a dialog
    private bool markQuestComplete;

    //Marks if a quest is active or not
    private bool shouldMarkQuest;

    //------------------------ Quest -----------------------

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //DialogText.text = dialogLines[currentLine];
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                if (!justStarted)
                {
                    if (checkIfTextFinished)
                    {
                        currentLine++;
                    }

                    if (currentLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                        //Set variable in game manager for moving or not the player
                        GameManager.instance.dialogActive = false;

                        //Mark quest
                        if (shouldMarkQuest)
                        {
                            shouldMarkQuest = false;
                            if (markQuestComplete)
                            {
                                QuestManager.instance.MarkQuestComplete(questToMark);
                            }
                            else
                            {
                                QuestManager.instance.MarkQuestIncomplete(questToMark);
                            }
                        }
                    }
                    else
                    {
                        CheckIfName();

                        if (checkIfTextFinished)
                        {
                            StartCoroutine(AnimateText(dialogLines[currentLine]));
                        }
                        //DialogText.text = dialogLines[currentLine];
                    }
                }
                else
                {
                    justStarted = false;
                }
            }
        }
    }

    private bool checkIfTextFinished = true;

    public IEnumerator AnimateText(string strComplete)
    {
        DialogText.text = "";

        int i = 0;

        while (i < strComplete.Length)
        {
            checkIfTextFinished = false;
            DialogText.text += strComplete[i++];
            yield return new WaitForSeconds(0.02F);
        }
        checkIfTextFinished = true;
    }

    public void ShowDialog(string[] newLines, bool isPerson)
    {
        dialogLines = newLines;

        currentLine = 0;

        CheckIfName();
        //DialogText.text = dialogLines[currentLine];
        if (checkIfTextFinished)
        {
            StartCoroutine(AnimateText(dialogLines[currentLine]));
        }

        dialogBox.SetActive(true);
        justStarted = true;

        if (isPerson)
        {
            nameBox.SetActive(true);
        }
        else
        {
            nameBox.SetActive(false);
        }

        //Set variable in game manager for moving or not the player
        GameManager.instance.dialogActive = true;
    }

    public void CheckIfName()
    {
        if (dialogLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }

    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;

        shouldMarkQuest = true;
    }
}
