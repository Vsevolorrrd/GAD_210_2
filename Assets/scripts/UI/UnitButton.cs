using UnityEngine;

public class UnitButton : MonoBehaviour
{
    public UnitType unit;
    public Spawn Spawn;

    public void Change()
    {
        Spawn.ChangeUnit(unit);
    }
}