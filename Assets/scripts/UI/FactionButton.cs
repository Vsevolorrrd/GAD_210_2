using UnityEngine;

public class FactionButton : MonoBehaviour
{
    public Faction faction;
    public Spawn Spawn;

    public void Change()
    {
        Spawn.ChangeFaction(faction);
    }
}
