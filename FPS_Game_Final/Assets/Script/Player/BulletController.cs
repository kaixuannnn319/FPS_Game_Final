using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float lifeTime = 5f;

    [Header("Damage")]
    public int damage;

    [Header("Effect")]
    [SerializeField] private GameObject impactEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        if (impactEffect != null)
        {
            Instantiate(
                impactEffect,
                transform.position,
                Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
