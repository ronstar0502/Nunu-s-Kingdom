using UnityEngine;

public class BlackSmith : ProffesionBuilding
{
    //TBD
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RecruitVillagerProffesion();
        }
    }
}
