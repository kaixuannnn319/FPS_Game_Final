using UnityEngine;
using System.Collections.Generic;

public class PlayerInventoryStub : MonoBehaviour
{
    public static PlayerInventoryStub Instance;
    private List<string> collectedKeys = new List<string>();

    private void Awake()
    {
        Instance = this;
    }

    public void CollectKey(string keyID)
    {
        collectedKeys.Add(keyID);
        Debug.Log("Collected key: " + keyID);
    }

    public bool HasKey(string keyID)
    {
        return collectedKeys.Contains(keyID);
    }
}