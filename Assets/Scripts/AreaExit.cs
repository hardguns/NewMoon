using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    public string areaToLoad;

    public string areaTransitionName;

    public AreaEntrance areaEntrance;

    public float waitToLoad = 1f;

    private bool shouldLoadAfterFade;

    // Start is called before the first frame update
    void Start()
    {
        areaEntrance.transitionName = areaTransitionName;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;

            if (waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //SceneManager.LoadScene(areaToLoad);
            shouldLoadAfterFade = true;

            //Set variable in game manager for moving or not the player
            GameManager.instance.fadingBetweenAreas = true;

            UIFade.instance.FadeToBlack();

            PlayerController.player.areaTransitionName = areaTransitionName;
        }
    }
}
