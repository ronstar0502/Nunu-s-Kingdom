using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour
{
    [SerializeField] protected VillagerData villagerData;
    [SerializeField] protected GameObject villagerTool; // unemployed doesnt have a tool
    private GameObject buildingTarget;
    private Vector2 targetPosition;
    private bool isUnemployed = true;
    private bool isGoingToProffesionBuilding;

    private void Awake()
    {
        villagerData.InitHealth();
    }

    private void Update()
    {
        //place holder will change later for better performance
        if(isUnemployed && isGoingToProffesionBuilding)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, villagerData.speed * Time.deltaTime);
            CheckVillagerArrival();
        }
    }

    private void CheckVillagerArrival() //checks if the unemployed villager arrive to the proffesion building destination
    {
        if (transform.position == (Vector3)targetPosition)
        {
            ProffesionBuilding proffesionBuilding = buildingTarget.GetComponent<ProffesionBuilding>();
            proffesionBuilding.VillagerProffesionChange_OnArrival(gameObject);
            isUnemployed = false;
            isGoingToProffesionBuilding = false;
        }
    }

    public VillagerData GetVillagerData() { return villagerData;}

    public void GoToProffesionBuilding(GameObject proffesionBuilding,Vector2 recruitPosition) // sets unemployed target proffesion building
    {
        targetPosition = recruitPosition;
        targetPosition.y = 0;
        buildingTarget = proffesionBuilding;
        isGoingToProffesionBuilding = true;
    }
}
