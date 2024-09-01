using UnityEngine;

public class Archery : ProffesionBuilding
{
    //TBD
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            RecruitVillagerProffesion();
        }
    }
}
