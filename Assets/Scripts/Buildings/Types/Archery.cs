using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archery : Building
{
    [SerializeField] GameObject bowPrefab;
    public int bowAmount;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            CraftBow();
        }
    }
    public void CraftBow()
    {
        bowAmount++;
        print($"katans in Archery: {bowAmount}");
    }
}
