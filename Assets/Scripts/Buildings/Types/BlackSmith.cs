using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSmith : Building
{
    [SerializeField]private GameObject katanaPrefab;
    public int katanaAmount;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CraftKatana();
        }
    }
    public void CraftKatana()
    {
        katanaAmount++;
        print($"katans in BlackSmith: {katanaAmount}");
    }
}
