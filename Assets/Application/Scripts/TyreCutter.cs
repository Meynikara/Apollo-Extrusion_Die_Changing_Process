using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyreCutter : MonoBehaviour
{

    [SerializeField] private ScoreManager _scoreManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cutter"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            _scoreManager.CheckTriggers();
        }
    }
}
