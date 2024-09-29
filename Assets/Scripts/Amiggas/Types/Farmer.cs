public class Farmer : Villager
{
    protected override void Start()
    {
        InitVillager();
        SetState(VillagerState.InProffesionBuilding);
        PlaySFX();
    }
}
