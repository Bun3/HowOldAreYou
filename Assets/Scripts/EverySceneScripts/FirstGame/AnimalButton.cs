using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimalButton : CustomButton
{
    [Space(20)] [SerializeField] private ANIMALS animal = ANIMALS.NOTHIING;

    private void Start()
    {
        clickEvent.AddListener(() => { FirstGame.Instance.CompareMaxAnimal(animal); });
    }

}
