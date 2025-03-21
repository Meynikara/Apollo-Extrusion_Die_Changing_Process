using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagerLocal : MonoBehaviour
{
    [Header("Tyre Triggers")]
    [SerializeField] private List<BoxCollider> _leftTriggers;
    [SerializeField] private List<BoxCollider> _rightTriggers;

    public bool _leftTyreBool;
    public bool _rightTyreBool;

    [Header("REF")]
    [SerializeField] private ScenarioManager _scenarioManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
    public void CheckTriggers()
    {
        Debug.Log("testing");
        CheckLeftTriggers(); // Checking Left Tyre collider all cutted 
        CheckRightTriggers(); // Checking Right Tyre collider all cutted

    }

    void CheckLeftTriggers()
    {
        bool allDisabled = true;

        foreach (BoxCollider col in _leftTriggers)
        {
            if (col != null && col.enabled)
            {
                allDisabled = false;
                break;
            }
        }

        if (allDisabled)
        {
            Debug.Log("All colliders are disabled");
            _leftTyreBool = true;
            PlayLeftTyreAnimation(); // Calling Next Task when all colliders are disable
        }
        else
        {
            Debug.Log("Some colliders are still enabled");
        }
    }


    void CheckRightTriggers()
    {
        bool allDisabled = true;

        foreach (BoxCollider col in _rightTriggers)
        {
            if (col != null && col.enabled)
            {
                allDisabled = false;
                break;
            }
        }

        if (allDisabled)
        {
            Debug.Log("All colliders are disabled");
            _rightTyreBool = true;
            PlayRightTyreAnimation(); // Calling Next Task when all colliders are disable
        }
        else
        {
            Debug.Log("Some colliders are still enabled");
        }
    }




    //  Calling Left Tyre Animations
    void PlayLeftTyreAnimation()
    {
        if (_leftTyreBool)
        {
            _scenarioManager.NextTask("Remaining material");
            return ;
        }
    }


    //  Calling Left Tyre Animations
    void PlayRightTyreAnimation()
    {
        if (_rightTyreBool)
        {
            _scenarioManager.NextTask("Remaining material");
            return ;
        }
    }



    // Reseting All Tyer Colliders 

    public void ResetAllTyerColliders()
    {
        RestLeftCollider(); // Reseting Left Colliders
        RestRightCollider(); // Reseting Left Colliders
    }

    void RestLeftCollider()
    {
        foreach(BoxCollider _col in _leftTriggers)
        {
            if(_col != null)
            {
                _col.enabled = true;
            }
        }
        _leftTyreBool = false;
        return;
    }

    void RestRightCollider()
    {
        foreach(BoxCollider _col in _rightTriggers)
        {
            if(_col != null)
            {
                _col.enabled = true;
            }
        }
        _rightTyreBool = false;
        return;
    }
}
