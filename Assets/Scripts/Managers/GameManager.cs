using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIFFICULT
{
    DIFFICULT_NOTHING = 0, DIFFICULT_EASY = 1, DIFFICULT_NORMAL = 2, DIFFICULT_HARD = 3, 
};

public class GameManager : MonoBehaviour
{
    private DIFFICULT nowDifficult = DIFFICULT.DIFFICULT_EASY;
    private int age = 100;

    public DIFFICULT NowDifficult { get => nowDifficult; set => nowDifficult = value; }
    public int Age { get => age; set => age = value; }


    private void Awake()
    {


    }

}