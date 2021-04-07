using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMaker : MonoBehaviour
{
    //Quest that will be looking for to mark it
    public string questToMark;

    //Marks if a quest is completed or not
    public bool markComplete;

    //Marks if a quest starts on enter or not
    public bool markOnEnter;
    
    //After marking quest deactivate zone or not
    public bool deactivatedOnMarking;

    //Is it possible to mark the quest?
    private bool canMark;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMark && Input.GetButtonDown("Fire1"))
        {
            canMark = false;
            MarkQuest();
        }
    }

    public void MarkQuest()
    {
        if (markComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }
        else
        {
            QuestManager.instance.MarkQuestIncomplete(questToMark);
        }

        gameObject.SetActive(!deactivatedOnMarking);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (markOnEnter)
            {
                MarkQuest();
            }
            else
            {
                canMark = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canMark = false;
        }
    }
}
