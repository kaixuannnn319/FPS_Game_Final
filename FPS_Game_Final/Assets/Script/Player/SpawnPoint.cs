using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        GameManager.Instance.SetDefaultSpawnPoint(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
