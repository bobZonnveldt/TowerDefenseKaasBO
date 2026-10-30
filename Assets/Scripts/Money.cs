using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    public int startingMoney = 500;
    public static int currentMoney;
    public TMP_Text moneyText;

    void Awake()
    {
        currentMoney = startingMoney;
    }

    public bool CanAffordTower(int cost)
    {
        return currentMoney >= cost;
    }

    public void SpendMoney(int cost)
    {
        currentMoney -= cost;
    }

    public static void AddMoney(int amount)
    {
        if (amount > 0)
        {
            currentMoney += amount;
        }
    }

    void Update()
    {
        moneyText.text = "Money: " + currentMoney;
    }
}
