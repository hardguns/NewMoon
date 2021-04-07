using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    public string transitionName;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerController.player != null)
        {
            if (transitionName == PlayerController.player.areaTransitionName)
            {
                PlayerController.player.transform.position = transform.position;
            }

            UIFade.instance.FadeFromBlack();
            GameManager.instance.fadingBetweenAreas = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
