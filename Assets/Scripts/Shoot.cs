using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Projectile projectilePrefab;
    public float projectileSpeed = 8f;
    public int damage = 25;
    public float shootInterval = 1f;
    public int towerCost = 100;

    private EnemyDetection enemyDetection;
    private float nextShotTime;

    void Start()
    {
        enemyDetection = GetComponent<EnemyDetection>();
    }

    void Update()
    {
        if (enemyDetection == null || enemyDetection.target == null || projectilePrefab == null)
        {
            return;
        }

        if (Time.time >= nextShotTime)
        {
            Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.SetTarget(enemyDetection.target, projectileSpeed, damage);
            nextShotTime = Time.time + shootInterval;
        }
    }
}
