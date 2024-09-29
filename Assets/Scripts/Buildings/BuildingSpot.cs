using UnityEngine;
public class BuildingSpot : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject buildingObj;
    [SerializeField] private GameObject buildingGhost;
    [SerializeField] private GameObject buildingSpotPopUp;
    [SerializeField] private GameObject pileBuildingSpritePreview;
    [SerializeField] private AudioClip buildSound;
    [SerializeField] private SoundEffectManger soundEffectManger;
    private Player player;
    private HQ HQ;
    private BuildingData buildingData;
    [SerializeField]private SpriteRenderer _sr;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        HQ = FindObjectOfType<HQ>();
        buildingData = buildingObj.GetComponent<Building>().GetBuildingData();
        soundEffectManger= FindAnyObjectByType<SoundEffectManger>();
        pileBuildingSpritePreview.GetComponent<SpriteRenderer>().sprite = buildingData.sprite;
        buildingGhost.SetActive(false);
        buildingSpotPopUp.SetActive(false);
    }

    public void ShowBuildingGhost()
    {
        buildingGhost.SetActive(true);
        buildingSpotPopUp.SetActive(true);
        buildingSpotPopUp.GetComponent<BuildingPopUp>().EnableBuildingSpotPopUp(buildingData.cost,buildingData.level);
        _sr.enabled = false;
    }

    public void HideBuildingGhost()
    {
        buildingGhost.SetActive(false);
        buildingSpotPopUp.SetActive(false);
        _sr.enabled = true;
    }
    //method to build a building at the current building spot
    public void BuildAtSpot()
    {
        if (buildingObj == null) //check first if the buildingObj prefab is not null
        {
            print("No Building Prefab Avilable");
            return;
        }
        if(player.GetPlayerData().seedAmount >= buildingData.cost)
        {
            GameObject building = Instantiate(buildingObj, buildingGhost.transform.position, Quaternion.identity); //spawns the building at the desired position
            building.GetComponent<Building>().SetBuildingSpot(gameObject);
            player.GetPlayerData().SubstarctSeedsAmount(buildingData.cost); //substracts seeds amount based on building starting cost
            HQ.villageInfoUI.SetSeedsText();

            //add a delay with animation of the building being built?

            gameObject.SetActive(false);
        }
    }
    public void Interact() //interaction with the building spot
    {
        if (player.GetPlayerData().seedAmount >= buildingData.cost)
        {
            BuildAtSpot();
            if (buildSound != null && soundEffectManger != null)
            {
                soundEffectManger.PlaySFX(buildSound);
            }
        }
        else
        {
            print("not enough seeds!");
        }
    }
}
