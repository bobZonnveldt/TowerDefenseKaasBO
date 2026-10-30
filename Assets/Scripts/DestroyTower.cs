using UnityEngine;
using TMPro;

public class DestroyTower : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float towerLossInterval = 30f;
    [SerializeField] private TMP_Text countdownText;

    private Camera gameplayCamera;
    private float timeRemaining;
    private bool enemyOnScreen;
    private bool countdownStarted;

    void Awake()
    {
        gameplayCamera = Camera.main;
        if (gameplayCamera == null)
        {
            Debug.LogError("DestroyTower requires a camera tagged MainCamera.", this);
        }

        if (countdownText == null)
        {
            Debug.LogError("Assign a TextMeshPro text component to display the tower countdown.", this);
        }

        timeRemaining = GetTowerLossInterval();
    }

    void Update()
    {
        enemyOnScreen = HasEnemyOnScreen();
        if (enemyOnScreen)
        {
            countdownStarted = true;
            timeRemaining = Mathf.Max(0f, timeRemaining - Time.deltaTime);
            if (timeRemaining <= 0f && DestroyRandomTower())
            {
                timeRemaining = GetTowerLossInterval();
            }
        }

        UpdateCountdownText();
    }

    private bool HasEnemyOnScreen()
    {
        if (gameplayCamera == null)
        {
            return false;
        }

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            Vector3 viewportPosition = gameplayCamera.WorldToViewportPoint(enemy.transform.position);
            if (viewportPosition.z > 0f &&
                viewportPosition.x >= 0f && viewportPosition.x <= 1f &&
                viewportPosition.y >= 0f && viewportPosition.y <= 1f)
            {
                return true;
            }
        }

        return false;
    }

    private bool DestroyRandomTower()
    {
        Shoot[] towers = FindObjectsByType<Shoot>(FindObjectsSortMode.None);
        if (towers.Length == 0)
        {
            return false;
        }

        int towerIndex = Random.Range(0, towers.Length);
        Destroy(towers[towerIndex].gameObject);
        return true;
    }

    private float GetTowerLossInterval()
    {
        return Mathf.Max(0.1f, towerLossInterval);
    }

    private void UpdateCountdownText()
    {
        if (countdownText != null && countdownStarted)
        {
            countdownText.text = $"Next tower eaten in: {Mathf.CeilToInt(timeRemaining)}s";
        }
    }
}
