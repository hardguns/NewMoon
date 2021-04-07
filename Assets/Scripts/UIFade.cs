using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    public static UIFade instance;

    public bool isFadingToBlack;
    public bool isFadingFromBlack;

    public float fadeSpeed = 3f;
    public Image fadeScreen;

    // Start is called before the first frame update
    void Start()
    {
        //if (instance == null)
        //{
            instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadingToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 1f)
            {
                isFadingToBlack = false;
            }
        }

        if (isFadingFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                isFadingFromBlack = false;
            }
        }
    }

    public void FadeToBlack()
    {
        isFadingToBlack = true;
        isFadingFromBlack = false;
    }

    public void FadeFromBlack()
    {
        isFadingToBlack = false;
        isFadingFromBlack = true;
    }
}
