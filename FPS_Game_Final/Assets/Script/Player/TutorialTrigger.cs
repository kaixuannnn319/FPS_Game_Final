using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private TMP_Text tutorialText;

    [TextArea]
    [SerializeField] private string message;

    [SerializeField] private float displayTime = 5f;

    private bool triggered = false;

    private void Start()
    {
        tutorialText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(ShowTutorial());
        }
    }

    IEnumerator ShowTutorial()
    {
        tutorialText.gameObject.SetActive(true);

        tutorialText.text = message;

        yield return new WaitForSeconds(displayTime);

        tutorialText.gameObject.SetActive(false);
    }
}