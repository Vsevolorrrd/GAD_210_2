using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private GameObject meleePrefab;
    [SerializeField] private GameObject rangePrefab;
    [SerializeField] private Faction selectedFaction = Faction.Faction1;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private Material faction1Material;
    [SerializeField] private Material faction2Material;
    [SerializeField] private Material faction3Material;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetFaction();
        MeleeSpawn();
        RangeSpawn();
    }

    public void SetFaction()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedFaction = Faction.Faction1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedFaction = Faction.Faction2;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedFaction = Faction.Faction3;
        }
    }

    public void MeleeSpawn()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100f, environmentLayer))
            {
                Vector3 spawnPosition = hit.point;
                Instantiate(spawnEffect, spawnPosition, Quaternion.identity);
                GameObject melee = Instantiate(meleePrefab, spawnPosition, Quaternion.identity);

                NPC npcComponent = melee.GetComponent<NPC>();
                if(npcComponent != null)
                {
                    npcComponent.faction = selectedFaction;
                }

                MaterialSwitch(melee);
            }

        }
    }
    
    public void RangeSpawn()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100f, environmentLayer))
            {
                Vector3 spawnPosition = hit.point;
                Instantiate(spawnEffect, spawnPosition, Quaternion.identity);
                GameObject range = Instantiate(rangePrefab, spawnPosition, Quaternion.identity);

                NPC npcComponent = range.GetComponent<NPC>();
                if(npcComponent != null)
                {
                    npcComponent.faction = selectedFaction;
                }

                MaterialSwitch(range);
            }
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
