using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private GameObject impactEffect;

    [Header("Audio")]
    [SerializeField] private AudioClip impactSound;
    [SerializeField][Range(0f, 1f)] private float impactVolume = 1f;

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
        {
            enemy.TakeDamage(damage);

            if (impactSound != null)
            {
                AudioSource.PlayClipAtPoint(
                    impactSound,
                    collision.contacts[0].point,
                    impactVolume);
            }
        }
            

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