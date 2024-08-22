using UnityEngine;

public class BlackSmith : ProffesionBuilding
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RecruitVillagerProffesion();
        }
    }
}
