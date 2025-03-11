using UnityEngine;

public class FactionButtonBattle : MonoBehaviour
{
    [SerializeField] private GameObject factionButton1;
    [SerializeField] private GameObject factionButton2;
    public Faction faction;
    public Spawn Spawn;


    public void Change()
    {
        Spawn.ChangeFaction(faction);
        factionButton1.SetActive(false);
        factionButton2.SetActive(false);
    }
}