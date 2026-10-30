using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;
    private float speed;
    private int damage;

    public void SetTarget(Enemy newTarget, float projectileSpeed, int projectileDamage)
    {
        target = newTarget;
        speed = projectileSpeed;
        damage = projectileDamage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position,
            speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
