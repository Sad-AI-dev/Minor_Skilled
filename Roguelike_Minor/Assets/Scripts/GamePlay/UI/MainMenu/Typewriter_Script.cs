using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.1f; // Delay between each character
    public float startDelay = 1.0f; // Delay before starting the typewriter effect
    public string fullText;
    private string currentText = "";
    private TMP_Text textComponent; // Use TMP_Text instead of Text

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>(); // Use GetComponent<TMP_Text>() for TextMeshPro
        if (textComponent != null)
        {
            StartCoroutine(StartTypewriter());
        }
        else
        {
            Debug.LogError("Text component not found!");
        }
    }

    IEnumerator StartTypewriter()
    {
        yield return new WaitForSeconds(startDelay); // Wait for the specified start delay

        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textComponent.text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }
}


