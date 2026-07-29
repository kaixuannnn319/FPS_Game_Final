using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager Instance;

    private List<RespawnablePickup> pickups = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterPickup(RespawnablePickup pickup)
    {
        if (!pickups.Contains(pickup))
            pickups.Add(pickup);
    }

    public void ResetPickups()
    {
        foreach (RespawnablePickup pickup in pickups)
        {
            pickup.Respawn();
        }

        Debug.Log("All consumable pickups respawned.");
    }
}