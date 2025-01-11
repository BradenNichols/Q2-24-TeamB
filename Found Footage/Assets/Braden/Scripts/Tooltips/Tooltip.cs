using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]

public struct TooltipEntry
{
    public string text;
    public float typeTime;
    public float waitTime;
}

public class Tooltip : MonoBehaviour
{
    [Header("Tooltip Data")]
    public List<TooltipEntry> text = new();
    public float initialWait;
    public bool playOnStart = true;

    [Header("References")]
    public TMP_Text textLabel;

    // Start is called before the first frame update
    void Start()
    {
        if (playOnStart)
            Type();
    }

    public void Type()
    {
        StartCoroutine(PlayTooltip());
    }

    IEnumerator PlayTooltip()
    {
        yield return new WaitForSeconds(initialWait);
        textLabel.enabled = true;
        textLabel.text = "";

        foreach (TooltipEntry entry in text)
        {
            for (int i = 0; i < entry.text.Length; i++)
            {
                textLabel.text += entry.text[i];
                yield return new WaitForSeconds(entry.typeTime);
            }

            yield return new WaitForSeconds(entry.waitTime);
            textLabel.text += "\n";
        }

        textLabel.enabled = false;
    }
}
