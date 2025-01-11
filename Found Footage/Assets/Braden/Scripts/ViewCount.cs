using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ViewCount : MonoBehaviour
{
    [Header("Stats")]
    public float timeForViewerDecay = 20;
    public float decayTickLength = 2;
    public int minViewersLostPerTick = 5;
    public int maxViewersLostPerTick = 14;

    [Header("Data")]
    public bool isSystemEnabled = false;
    public bool noDecay = false;
    public int viewers = 0;

    [Header("Initial")]
    public int minInitialViewers = 0;
    public int maxInitialViewers = 0;

    [Header("References")]
    public TMP_Text viewCount;
    public TMP_Text viewDecayLabel;

    [Header("_Internal")]
    public float timeSinceViewGain = 0;
    public float timeSinceDecayLost = 0;
    public float greenTime = 0;
    public bool isDecaying = false;

    void Start()
    {
        AddViewers(Random.Range(minInitialViewers, maxInitialViewers), false);
        SetEnabled(isSystemEnabled);
    }

    void Update()
    {
        if (!isSystemEnabled || noDecay) return;

        if (isSystemEnabled)
        {
            if (!noDecay)
            {
                timeSinceViewGain += Time.deltaTime;

                if (timeSinceViewGain >= timeForViewerDecay)
                {
                    if (!isDecaying)
                        AddViewers(-1);
                    else
                    {
                        timeSinceDecayLost += Time.deltaTime;

                        if (timeSinceDecayLost >= decayTickLength)
                        {
                            int viewersLost = (int)(Random.Range(minViewersLostPerTick, maxViewersLostPerTick) * (timeSinceDecayLost / decayTickLength));
                            AddViewers(-viewersLost);

                            viewDecayLabel.enabled = true;
                            timeSinceDecayLost = 0;
                        }
                    }
                }
            }

            if (greenTime > 0)
            {
                greenTime = Mathf.Clamp(greenTime - Time.deltaTime, 0, greenTime);
                SetColor(Color.green);

                if (greenTime <= 0)
                    SetColor(Color.white);
            }
        }
    }

    // Private Functions

    void SetColor(Color targetColor)
    {
        viewCount.color = targetColor;
    }

    // Public Functions

    public void AddViewers(int viewerAmount, bool shouldColor = true)
    {
        // can be positive or negative amount
        viewers = Mathf.Clamp(viewers + viewerAmount, 0, 9999);
        viewCount.text = $"VI:{viewers}";

        if (viewerAmount >= 0)
        {
            timeSinceViewGain = 0;
            timeSinceDecayLost = 0; // just to reset it

            viewDecayLabel.enabled = false;

            if (viewerAmount > 0 && shouldColor)
                greenTime = 1f;
            else
                SetColor(Color.white);
        }
        else
            SetColor(Color.red);

        isDecaying = viewerAmount < 0;
    }

    public void SetEnabled(bool isEnabled)
    {
        isSystemEnabled = isEnabled;
        viewCount.enabled = isEnabled;
    }
}
