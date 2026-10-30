using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Shoot))]
public class Upgrade : MonoBehaviour
{
    public int upgradeCost = 50;
    public int damageIncrease = 10;
    public float rangeIncrease;
    public float shootIntervalReduction;

    private Button upgradeButton;
    private bool upgraded;

    private void Awake()
    {
        BoxCollider2D towerCollider = GetComponent<BoxCollider2D>();
        if (towerCollider == null)
        {
            towerCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null && sprite.sprite != null)
        {
            towerCollider.size = sprite.sprite.bounds.size;
        }
    }

    public void SetUpgradeButton(Button button)
    {
        upgradeButton = button;
    }

    private void OnMouseDown()
    {
        if (upgraded)
        {
            if (upgradeButton != null)
            {
                upgradeButton.gameObject.SetActive(false);
            }
            return;
        }

        if (upgradeButton == null)
        {
            Debug.LogError("Assign an upgrade button to TowerPlacer.", this);
            return;
        }

        TMP_Text buttonText = upgradeButton.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.text = "Upgrade - " + upgradeCost;
        }

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(UpgradeTower);
        upgradeButton.gameObject.SetActive(true);
    }

    private void UpgradeTower()
    {
        Money money = FindFirstObjectByType<Money>();
        if (upgraded || money == null || !money.CanAffordTower(upgradeCost))
        {
            return;
        }

        money.SpendMoney(upgradeCost);

        Shoot shoot = GetComponent<Shoot>();
        shoot.damage += damageIncrease;
        shoot.shootInterval = Mathf.Max(0.1f, shoot.shootInterval - shootIntervalReduction);

        EnemyDetection detection = GetComponent<EnemyDetection>();
        if (detection != null)
        {
            detection.range += rangeIncrease;
        }

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = Color.Lerp(sprite.color, Color.yellow, 10.5f);
        }

        upgraded = true;
        upgradeButton.gameObject.SetActive(false);
    }
}
