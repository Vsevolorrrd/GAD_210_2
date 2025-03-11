using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private GameObject winPannel;
    [SerializeField] private PlayersManager playersManager;
    
    [SerializeField] private TMP_Text winnerText;

    [SerializeField] private float checkInterval = 1f;
    private float timer = 0f;
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            CheckVictory();
            timer = 0f;
        }
    }
    
    private void CheckVictory()
    {
        NPC[] allUnits = FindObjectsOfType<NPC>();

        int playerOneCount = 0;
        int playerTwoCount = 0;
        
        foreach (NPC unit in allUnits)
        {
            if (unit.faction == playersManager.playerOneFaction)
                playerOneCount++;
            else if (unit.faction == playersManager.playerTwoFaction)
                playerTwoCount++;
        }
        
        if (playerOneCount == 0 && playerTwoCount > 0)
        {
            DeclareWinner("Player 2");
        }
        else if (playerTwoCount == 0 && playerOneCount > 0)
        {
            DeclareWinner("Player 1");
        }
    }
    
    private void DeclareWinner(string winner)
    {
        if (winnerText != null)
        {
            winPannel.SetActive(true);
            winnerText.text = "Winner: " + winner;
        }
        Debug.Log("Winner: " + winner);
        
        // Time.timeScale = 0f;
    }
}
