using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private GameObject impactEffect;

    public int damage;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Use velocity instead of moving the Transform manually
        rb.linearVelocity = transform.forward * moveSpeed;

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit " + collision.collider.name);

        EnemyBase enemy = collision.collider.GetComponentInParent<EnemyBase>();

        if (enemy != null)
            enemy.TakeDamage(damage);

        if (impactEffect != null)
        {
            ContactPoint cp = collision.contacts[0];

            Instantiate(
                impactEffect,
                cp.point,
                Quaternion.LookRotation(cp.normal));
        }

        Destroy(gameObject);
    }
}