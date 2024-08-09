using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component is responsible for invoking the StepEvent at an increment according to the user
/// </summary>
public class TimeStepper : MonoBehaviour
{
    [Tooltip("Reference to the event used to notify the simulation to take a step")]
    public VoidEvent StepEvent;
    [Tooltip("Reference to the IntVariable controlling the time between steps (ideally)")]
    public IntValue StepIncrement;
    [Tooltip("Reference to the IntVariable used for tracking the actual increment we're hitting")]
    public IntValue ActualIncrement;
    [Tooltip("Reference to the BoolVariable controlling whether the sim is running")]
    public BoolValue IsPlaying;

    // Time.deltaTime is in seconds, avoid division every update
    float incrementInSeconds = 0.5f;
    float t = 0f;

    // Update is called once per frame
    void Update()
    {
        if (!IsPlaying.Value)
            return;

        if (t >= incrementInSeconds)
        {
            ActualIncrement.Value = (int)(t * 1000);
            StepEvent.Raise(null);
            t = 0f;
        }

        t += Time.deltaTime;
    }

    private void Awake()
    {
        ActualIncrement.Value = ActualIncrement.DefaultValue; // reset with notify
        StepIncrement.ResetValue();
        IsPlaying.ResetValue();
        incrementInSeconds = StepIncrement.Value / 1000f;
    }

    private void OnEnable()
    {
        StepIncrement.OnValueChanged += OnStepIncrementChanged;
    }

    private void OnDisable()
    {
        StepIncrement.OnValueChanged -= OnStepIncrementChanged;
    }

    private void OnStepIncrementChanged(dynamic value)
    {
        if (value is int) // it will be
        {
            incrementInSeconds = value / 1000f;
        }
    }
}
