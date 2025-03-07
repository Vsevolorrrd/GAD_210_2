using UnityEngine;

public class CleanUp : MonoBehaviour
{
    public void CleanUpArena()
    {
        NPC[] allNPCs = FindObjectsOfType<NPC>();
        foreach (NPC npc in allNPCs)
        {
            Destroy(npc.gameObject);
        }
    }
}
