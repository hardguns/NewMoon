using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDeactivator : MonoBehaviour
{
    public GameObject dialogToDeactivate;
    private bool isDialogDeactivated;
    public string questComplete;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DeactivateDialog();
    }

    public void DeactivateDialog()
    {
        if (!isDialogDeactivated)
        {
            for (int i = 0; i < QuestManager.instance.questMakerNames.Length; i++)
            {
                if (QuestManager.instance.questMakerNames[i] == questComplete)
                {
                    if (QuestManager.instance.questMakersComplete[i])
                    {
                        dialogToDeactivate.SetActive(false);
                        isDialogDeactivated = true;
                        break;
                    }
                }
            }
        }
    }

}
