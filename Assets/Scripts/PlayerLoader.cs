using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    public GameObject insPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerController.player == null)
        {
            Instantiate(insPlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
