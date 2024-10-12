public class Farmer : Villager
{
    protected override void Start()
    {
        SetState(VillagerState.InProffesionBuilding);
        PlaySFX();
    }
}
