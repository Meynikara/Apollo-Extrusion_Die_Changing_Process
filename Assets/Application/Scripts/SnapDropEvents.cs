using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnapDropEvents : MonoBehaviour
{
    public ScenarioManager _ScenarioManager;
    public UnityEvent _events;
    public string _tag;

    private void Start()
    {
      
    }
    public void OnTriggerEnter(Collider go)
    {
        if(go.gameObject.tag == _tag)
        {         
            _events.Invoke();

        }


      /*  if (change_oculushand_anim)
        {
            _handanim.SetBool(animname, false); 
        }*/

    }
}
