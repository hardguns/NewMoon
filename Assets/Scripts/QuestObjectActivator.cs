using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectActivator : MonoBehaviour
{
    //Object to show when a quest is active or not
    public GameObject objectToActivate;

    //Name of the current quest
    public string questToCheck;

    //Variable to check if the object is active or not when the quest is complete
    public bool activeIfComplete;

    //variable to check when update and not in the start
    private bool initialCheck;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialCheck)
        {
            initialCheck = true;

            CheckCompletion();
        }
    }

    public void CheckCompletion()
    {
        if (QuestManager.instance.CheckIfComplete(questToCheck))
        {
            objectToActivate.SetActive(activeIfComplete);
        }
    }
}
