using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatchery : Building
{
    [SerializeField] private GameObject villagerPrefab;
    [SerializeField] private Transform spawnPoint;
    private HQ HQ;

    private void Start()
    {
        HQ =  FindObjectOfType<HQ>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RecruitVillager();
        }
    }
    public void RecruitVillager()
    {
        if (HQ.CanRecruitVillager())
        {
            GameObject newVillager = Instantiate(villagerPrefab, spawnPoint.position,Quaternion.identity);
            HQ.AddVillager(newVillager);
        }
    }

    
}
