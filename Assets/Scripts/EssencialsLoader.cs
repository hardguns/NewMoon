using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssencialsLoader : MonoBehaviour
{
    public GameObject player;
    public GameObject UIScreen;
    public GameObject gameManage;
    public GameObject audioManage;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerController.player == null)
        {
            PlayerController.player = Instantiate(player).GetComponent<PlayerController>();
        }

        if (UIFade.instance == null)
        {
            UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
        }

        if (GameManager.instance == null)
        {
            GameManager.instance = Instantiate(gameManage).GetComponent<GameManager>();
        }

        if (AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(audioManage).GetComponent<AudioManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
