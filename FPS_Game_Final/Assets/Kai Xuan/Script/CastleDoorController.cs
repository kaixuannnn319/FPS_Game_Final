using UnityEngine;

public class CastleDoorController : MonoBehaviour
{
    [SerializeField] private EnemyBase[] requiredEnemies;
    [SerializeField] private Animator animator;

    private bool opened = false;

    private void Update()
    {
        if (opened)
            return;

        foreach (EnemyBase enemy in requiredEnemies)
        {
            // Enemy still exists
            if (enemy != null)
                return;
        }

        OpenDoor();
    }

    private void OpenDoor()
    {
        opened = true;

        animator.SetTrigger("Open");

        Debug.Log("Castle door opened!");
    }
}