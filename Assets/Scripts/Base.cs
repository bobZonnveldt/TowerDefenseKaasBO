using UnityEngine;

public class Base : MonoBehaviour
{
    public int Health = 1000;

    void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            Health -= enemy.Health;
            Destroy(enemy.gameObject);
        }  
    }
}
