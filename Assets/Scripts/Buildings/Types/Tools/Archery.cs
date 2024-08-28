using UnityEngine;

public class Archery : ProffesionBuilding
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            RecruitVillagerProffesion();
        }
    }
}
