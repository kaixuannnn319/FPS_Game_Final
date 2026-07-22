using UnityEngine;

public class KeyPickup : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Interact(GameObject player)
    {

        Destroy(gameObject);
    }

}
