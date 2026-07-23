using UnityEngine;

// Any damageable object (player, breakables, etc.) implements this.
// If your player script already has a damage method, just make it implement
// IDamageable instead of adding a new one.
public interface IDamageable
{
    void TakeDamage(float amount);
}

// Put on the archer's arrow/projectile prefab.
public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}