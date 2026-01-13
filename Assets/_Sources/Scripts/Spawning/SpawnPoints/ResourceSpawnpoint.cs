public class ResourceSpawnpoint : Spawnpoint<Resource>
{
    public override void Occupy(Resource spawnable)
    {
        base.Occupy(spawnable);

        if (IsFree)
        {
            Spawnable.PickedUp += OnResourcePickedUp;
        }
    }

    public override void MakeFree()
    {
        if (IsFree)
        {
            Spawnable.PickedUp -= OnResourcePickedUp;
        }

        base.MakeFree();
    }

    private void OnResourcePickedUp()
    {
        MakeFree();
    }
}