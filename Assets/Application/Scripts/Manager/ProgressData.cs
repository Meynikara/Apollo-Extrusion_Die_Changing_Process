using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[Serializable]
public class ProgressData 
{
    [Header("Task Info")]
    public string _taskName;
    public string _label;
    public AudioClip _voiceOver;

    [Header("POPUP")]
    public bool _enbPopup;
    public GenericPopUp _popup;
    public bool _subtitle;
    public string _subtitleText;

    [Header("Guided Type")]
    public bool _skipGuided;
    public bool _skipUnGuided;

    [Header("Events")]
    public UnityEvent _tasks;
    public UnityEvent _resetTasks;
}

[Serializable]
public class ScenarioData
{
    public string _scenarioName;
    public List<ProgressData> _procedures;
}
