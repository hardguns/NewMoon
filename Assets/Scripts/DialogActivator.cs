using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    //Literally lines a NPC or an object will show
    public string[] lines;

    //Validates if player is closed to the dialog activator
    private bool canActivate;

    //Specifies if the object is a person or not
    public bool isPerson = true;

    //----------------------------------------- Quest -------------------------------------
    //If true the gameobject activates a quest
    public bool shouldACtivateQuest;

    //Manages the quest variable to mark it as begun
    public string questToMark;

    //Marks the quest as complete
    public bool markComplete;
    //----------------------------------------- Quest -------------------------------------

    //Validates if dialog should start at enter
    public bool shouldBeginOnEnter;

    //Dialog is open
    public bool fireDialog;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (canActivate && Input.GetButtonDown("Fire1") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            StartDialog();
        }

        if (canActivate && shouldBeginOnEnter)
        {
            StartCoroutine(StartDialogOnEnterCO());
        }

    }

    public void StartDialog()
    {
        DialogManager.instance.ShowDialog(lines, isPerson);
        DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
        fireDialog = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            canActivate = false;
        }
    }

    public IEnumerator StartDialogOnEnterCO()
    {
        shouldBeginOnEnter = false;
        yield return new WaitForSeconds(1);

        DialogManager.instance.ShowDialog(lines, isPerson);
        DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
    }
}
