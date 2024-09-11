using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Villager
{
    protected override void Start()
    {
        InitAmmiga();
        SetState(AmiggaState.InProffesionBuilding);
    }
}
