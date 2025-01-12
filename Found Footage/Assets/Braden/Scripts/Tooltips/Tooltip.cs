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

    [HideInInspector]
    public int playedTimes = 0;

    [Header("References")]
    public TMP_Text textLabel;

    // Start is called before the first frame update
    void Start()
    {
        if (!textLabel)
            textLabel = GameObject.Find("Tooltip").GetComponent<TMP_Text>();

        if (playOnStart)
            Play();
    }

    public void Play()
    {
        StartCoroutine(PlayTooltip());
    }

    IEnumerator PlayTooltip()
    {
        playedTimes++;

        yield return new WaitForSeconds(initialWait);
        textLabel.enabled = true;
        textLabel.text = "";

        foreach (TooltipEntry entry in text)
        {
            for (int i = 0; i < entry.text.Length; i++)
            {
                textLabel.text += entry.text[i];

                if (textLabel.text == "") // if something cancels us
                    yield break;

                yield return new WaitForSeconds(entry.typeTime);
            }

            yield return new WaitForSeconds(entry.waitTime);

            if (textLabel.text == "") // if something cancels us
                yield break;

            textLabel.text += "\n";
        }

        textLabel.enabled = false;
    }
}
