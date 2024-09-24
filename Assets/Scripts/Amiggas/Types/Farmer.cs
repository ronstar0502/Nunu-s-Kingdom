using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Farmer : Villager
{

    protected override void Start()
    {
        InitAmmiga();
        SetState(AmiggaState.InProffesionBuilding);
        base.PlaySFX();
    }
}
