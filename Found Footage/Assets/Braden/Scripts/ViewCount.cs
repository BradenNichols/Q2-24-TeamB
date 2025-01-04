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
    public int viewers = 0;
    [HideInInspector]
    public float timeSinceViewGain = 0;
    float timeSinceDecayLost = 0;

    [Header("References")]
    public TMP_Text viewCount;

    void Start()
    {
        AddViewers(0);
        SetEnabled(isSystemEnabled);
    }

    void Update()
    {
        if (!isSystemEnabled) return;

        timeSinceViewGain += Time.deltaTime;

        if (timeSinceViewGain >= timeForViewerDecay)
        {
            timeSinceDecayLost += Time.deltaTime;

            if (timeSinceDecayLost >= decayTickLength)
            {
                int viewersLost = (int)(Random.Range(minViewersLostPerTick, maxViewersLostPerTick) * (timeSinceDecayLost / decayTickLength));
                AddViewers(-viewersLost);

                timeSinceDecayLost = 0;
            }
        }
    }

    // Public Functions

    public void AddViewers(int viewerAmount)
    {
        // can be positive or negative amount
        viewers = Mathf.Clamp(viewers + viewerAmount, 0, 9999);
        viewCount.text = $"Viewers: {viewers}";

        if (viewerAmount >= 0)
        {
            timeSinceViewGain = 0;
            timeSinceDecayLost = 0; // just to reset it
            viewCount.color = Color.white;
        } else
            viewCount.color = Color.red;
    }

    public void SetEnabled(bool isEnabled)
    {
        isSystemEnabled = isEnabled;
        viewCount.enabled = isEnabled;
    }
}
