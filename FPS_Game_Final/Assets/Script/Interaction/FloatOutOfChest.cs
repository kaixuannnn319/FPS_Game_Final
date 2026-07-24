using UnityEngine;
using System.Collections;

public class FloatOutOfChest : MonoBehaviour
{
    [SerializeField] private float floatHeight = 0.5f;
    [SerializeField] private float floatDuration = 1f;

    private Vector3 startPosition;
    private Vector3 endPosition;

    void Awake()
    {
        startPosition = transform.localPosition;
        endPosition = startPosition + Vector3.up * floatHeight;
    }

    public void PlayFloatAnimation()
    {
        StartCoroutine(Float());
    }

    private IEnumerator Float()
    {
        float elapsed = 0f;

        while (elapsed < floatDuration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsed / floatDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = endPosition;
    }
}