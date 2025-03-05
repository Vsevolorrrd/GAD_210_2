using UnityEngine;
using UnityEngine.EventSystems;

public class Spawn : MonoBehaviour
{

    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private GameObject meleePrefab;
    [SerializeField] private GameObject rangePrefab;
    [SerializeField] private GameObject giantPrefab;

    [SerializeField] private Faction selectedFaction = Faction.Faction1;
    [SerializeField] private UnitType selectedUnit = UnitType.Melee;
    [SerializeField] private LayerMask environmentLayer;

    [SerializeField] private Material faction1Material;
    [SerializeField] private Material faction2Material;
    [SerializeField] private Material faction3Material;

    [SerializeField] private GameObject[] factionButtonsOverlay;
    [SerializeField] private GameObject[] unitButtonsOverlay;

    public float pressTime = 0;


    void Update()
    {
        Spawning();
        SetFaction();
        SetUnit();
    }

    public void SetFaction()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) ChangeFaction(Faction.Faction1);
        if(Input.GetKeyDown(KeyCode.Alpha2)) ChangeFaction(Faction.Faction2);
        if(Input.GetKeyDown(KeyCode.Alpha3)) ChangeFaction(Faction.Faction3);
    }
    private void SetUnit()
    {
        if (Input.GetKeyDown(KeyCode.Q)) ChangeUnit(UnitType.Melee);
        if (Input.GetKeyDown(KeyCode.W)) ChangeUnit(UnitType.Ranged);
        if (Input.GetKeyDown(KeyCode.E)) ChangeUnit(UnitType.Giant);
    }
    public void ChangeFaction(Faction faction)
    {
        // Disable all faction overlays
        foreach (var overlay in factionButtonsOverlay)
        overlay.SetActive(false);

        selectedFaction = faction;

        int factionIndex = (int)faction;
        if (factionIndex >= 0 && factionIndex < factionButtonsOverlay.Length)
        factionButtonsOverlay[factionIndex].SetActive(true);
    }

    public void ChangeUnit(UnitType unit)
    {
        // Disable all unit overlays
        foreach (var overlay in unitButtonsOverlay) overlay.SetActive(false);

        selectedUnit = unit;

        int unitIndex = (int)unit;
        if (unitIndex >= 0 && unitIndex < unitButtonsOverlay.Length)
        unitButtonsOverlay[unitIndex].SetActive(true);
    }

    private void Spawning()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // Prevent clicking through UI

        if (Input.GetMouseButton(0)) // Left-click to spawn
        {
            pressTime += Time.deltaTime;
            if (pressTime > 0.1f)
            {
                SpawnUnit();
                pressTime = 0f;
            }
        }
    }
    private void SpawnUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, environmentLayer))
        {
            Vector3 spawnPosition = hit.point;
            Instantiate(spawnEffect, spawnPosition, Quaternion.identity);

            GameObject unitPrefab = GetUnitPrefab();
            if (unitPrefab == null) return;

            GameObject unit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
            NPC npcComponent = unit.GetComponent<NPC>();

            if (npcComponent != null)
            npcComponent.faction = selectedFaction;

            MaterialSwitch(unit);
        }
    }
    
    private GameObject GetUnitPrefab()
    {
        switch (selectedUnit)
        {
            case UnitType.Melee: return meleePrefab;
            case UnitType.Ranged: return rangePrefab;
            case UnitType.Giant: return giantPrefab;
            default: return null;
        }
    }

    public void MaterialSwitch(GameObject npcInstance)
    {
        Renderer renderer = npcInstance.GetComponent<Renderer>();
                if(renderer != null)
                {
                    switch(selectedFaction)
                    {
                        case Faction.Faction1:
                            renderer.material = faction1Material;
                            break;
                        case Faction.Faction2:
                            renderer.material = faction2Material;
                            break;
                        case Faction.Faction3:
                            renderer.material = faction3Material;
                            break;
                    }
                }
    }
}
public enum UnitType
{
    Melee,
    Ranged,
    Giant
}