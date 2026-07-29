using UnityEngine;

public class RespawnablePickup : MonoBehaviour
{
    private void Start()
    {
        PickupManager.Instance.RegisterPickup(this);
    }

    public void Collect()
    {
        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
    }
}