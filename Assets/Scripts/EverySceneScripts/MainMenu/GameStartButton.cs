using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartButton : CustomButton
{
    [SerializeField] private DIFFICULT difficult = DIFFICULT.DIFFICULT_NOTHING;

    private void Start()
    {
        clickEvent.AddListener(() => { Singleton.Scene.GameStart(difficult); });
    }

}
