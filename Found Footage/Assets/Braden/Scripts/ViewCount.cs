using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewCount : MonoBehaviour
{
    [Header("Stats")]
    public float initialViewerDecay = 40;
    public float viewerDecay = 20;
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
    public Image viewIcon;

    [Header("_Internal")]
    public float timeSinceViewGain = 0;
    public float timeSinceDecayLost = 0;
    public float greenTime = 0;
    public bool isDecaying = false;
    public bool isFirstIncrease = true;

    void Start()
    {
        AddViewers(Random.Range(minInitialViewers, maxInitialViewers), false);
        isFirstIncrease = true;

        SetEnabled(isSystemEnabled);
    }

    void Update()
    {
        if (isSystemEnabled)
        {
            if (!noDecay)
            {
                timeSinceViewGain += Time.deltaTime;

                float timeForViewerDecay = viewerDecay;

                if (isFirstIncrease)
                    timeForViewerDecay = initialViewerDecay;

                if (timeSinceViewGain >= timeForViewerDecay && viewers > 0)
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

                            timeSinceDecayLost = 0;
                            viewDecayLabel.enabled = viewers > 0;
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

            if (Input.GetKeyDown(KeyCode.P))
                AddViewers(50);
        }
    }

    // Private Functions

    void SetColor(Color targetColor)
    {
        viewCount.color = targetColor;
        viewIcon.color = targetColor;
    }

    // Public Functions

    public void AddViewers(int viewerAmount, bool shouldColor = true)
    {
        // can be positive or negative amount
        viewers = Mathf.Clamp(viewers + viewerAmount, 0, 9999);
        viewCount.text = $"{viewers}";

        if (viewerAmount >= 0)
        {
            timeSinceViewGain = 0;
            timeSinceDecayLost = 0; // just to reset it

            viewDecayLabel.enabled = false;
            isFirstIncrease = false;

            if (viewerAmount > 0 && shouldColor)
                greenTime = 0.8f;
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
        viewIcon.enabled = isEnabled;
    }
}
