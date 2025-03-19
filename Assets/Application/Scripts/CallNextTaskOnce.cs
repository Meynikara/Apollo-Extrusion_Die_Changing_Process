using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallNextTaskOnce : MonoBehaviour
{
    private bool hasBeenCalled = false;
    public ScenarioManager scenarioManager;
    void OnEnable()
    {
        if (!hasBeenCalled)
        {
            
            if (scenarioManager != null)
            {
                scenarioManager.NextTask();
                hasBeenCalled = true;
            }
            else
            {
                Debug.LogError("ScenarioManager not found in the scene!");
            }
        }
    }
}
