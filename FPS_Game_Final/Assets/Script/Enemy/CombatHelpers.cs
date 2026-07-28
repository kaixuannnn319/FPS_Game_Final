using UnityEngine;

// Put on the archer's arrow/projectile prefab.
public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 5f;

    [Tooltip("Layers this projectile should stop on even without dealing damage (walls, ground, etc). If left as \"Everything\", it stops on anything that isn't specifically ignored below.")]
    public LayerMask stopOnLayers = ~0; // default: everything

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage((int)damage);
            Destroy(gameObject);
            return;
        }

        // Didn't hit the player — but if it hit something solid (wall, floor, prop),
        // stop here too instead of flying straight through it.
        if ((stopOnLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}