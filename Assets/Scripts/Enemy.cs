using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int Health;
    public int moneyReward = 10;
    public Transform[] waypoints;

    private int currentWaypointIndex = 0;

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || Health <= 0)
        {
            return;
        }

        Health -= damage;
        if (Health <= 0)
        {
            Money.AddMoney(moneyReward);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        Transform target = waypoints[currentWaypointIndex];
        Vector3 dir = (target.position - transform.position).normalized;

        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                Destroy(gameObject);
            }
        }
    }
}
