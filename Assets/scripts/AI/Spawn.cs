using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class Spawn : MonoBehaviour
{

    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private GameObject meleePrefab;
    [SerializeField] private GameObject rangePrefab;
    [SerializeField] private GameObject giantPrefab;

    [SerializeField] public Faction selectedFaction = Faction.Faction1;
    [SerializeField] private UnitType selectedUnit = UnitType.Melee;
    [SerializeField] private LayerMask environmentLayer;

    [SerializeField] private Material faction1Material;
    [SerializeField] private Material faction2Material;
    [SerializeField] private Material faction3Material;

    [SerializeField] private GameObject[] factionButtonsOverlay;
    [SerializeField] private GameObject[] unitButtonsOverlay;

    PlayersManager battle;
    public bool factionChosen = false;

    public float pressTime = 0;
    public event Action<int> OnUnitSpawned;
    private string sceneName = "Sandbox";

    void Update()
    {
        if(sceneName == SceneManager.GetActiveScene().name)
        {
            Spawning();
        }
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
        factionChosen = true;
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

    public void Spawning()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // Prevent clicking through UI

        if (Input.GetMouseButton(0)) // Left-click to spawn
        {
            pressTime += Time.unscaledDeltaTime;
            if (pressTime > 0.1f)
            {
                SpawnUnit();
                pressTime = 0f;
            }
        }
    }

    public int GetUnitCost(UnitType unit)
    {
        switch (unit)
        {
            case UnitType.Melee:
                return 10;
            case UnitType.Ranged:
                return 15;
            case UnitType.Giant:
                return 30;
            default:
                return 0;
        }
    }
    public int GetCurrentUnitCost()
    {
        return GetUnitCost(selectedUnit);
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
        int cost = GetUnitCost(selectedUnit);
        OnUnitSpawned?.Invoke(cost);
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

    public void ResetFaction()
    {
        factionChosen = false;
        ChangeFaction(Faction.Faction1);
    }
}
public enum UnitType
{
    Melee,
    Ranged,
    Giant
}