using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoMenuButton : CustomButton
{ 
    void Start()
    {
        clickEvent.AddListener(() => { Singleton.Scene.LoadScene("MainMenu"); });
    }
}