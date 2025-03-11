using TMPro;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    [SerializeField] private GameObject resumeGame;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private TMP_Text money1;
    [SerializeField] private TMP_Text money2;
    public int playerOneMoney = 250;
    public int playerTwoMoney = 200;

    private int currentPlayer = 1;


    public int meleeCost = 10;
    public int rangedCost = 15;
    public int giantCost = 30;

    public bool competitive;
    public bool waitingForContinue = false;


    private Spawn spawn;

    void Awake()
    {
        competitive = true;

        spawn = FindObjectOfType<Spawn>();
        if (spawn != null)
        {
            spawn.OnUnitSpawned += OnUnitSpawned;
        }


    }

    void OnDestroy()
    {
        if (spawn != null)
        {
            spawn.OnUnitSpawned -= OnUnitSpawned;
        }
    }
    void Start()
    {
        money1.text = "Player one money = " + playerOneMoney;
        money2.text = "Player two money = " + playerTwoMoney;
    }

    void Update()
    {
        if(!waitingForContinue)
        {
            if(spawn.factionChosen == true)
            {
                if ((currentPlayer == 1 && playerOneMoney >= spawn.GetCurrentUnitCost()) || (currentPlayer == 2 && playerTwoMoney >= spawn.GetCurrentUnitCost()))
                {
                    spawn.Spawning();
                }
            }
        }
    }

    private void OnUnitSpawned(int cost)
    {
        if (currentPlayer == 1)
        {
            playerOneMoney -= cost;
            money1.text = "Player one money = " + playerOneMoney;
            Debug.Log("Player 1 spent " + cost + ". Balance: " + playerOneMoney);
            if (playerOneMoney < spawn.GetUnitCost(UnitType.Melee))
            {
                waitingForContinue = true;
                continueButton.SetActive(true);
            }
        }
        else if (currentPlayer == 2)
        {
            playerTwoMoney -= cost;
            money2.text = "Player two money = " + playerTwoMoney;
            Debug.Log("Player 2 spent " + cost + ". Balance: " + playerTwoMoney);
            if (playerTwoMoney <= spawn.GetUnitCost(UnitType.Melee))
            {
                resumeGame.SetActive(true);
            }
        }
    }

    public void ContinuePlacement()
    {
        if (waitingForContinue)
        {
            currentPlayer = 2;
            waitingForContinue = false;
            continueButton.SetActive(false);

            spawn.ResetFaction();
        }
    }
}
