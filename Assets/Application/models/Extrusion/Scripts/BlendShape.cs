using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShape : MonoBehaviour
{
    public string targetTag = "Target";
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public int rollBlendShapeIndex = 2; // Change if "roll" has a different index
    public float increaseSpeed = 50f; // Speed at which roll increases
    private bool isColliding = false;
    public Animator animator;
    public Outline _outline; // Reference to the outline component
    public ScenarioManager scenarioManager;
    void Start()
    {
        
        if (animator != null)
        {
            animator.enabled = false; 
        }

       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if (_outline != null && _outline.enabled)
            {
                _outline.enabled = false;
            }
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            isColliding = false;
        }
    }

    void Update()
    {
        if (isColliding && skinnedMeshRenderer != null)
        {
            float currentValue = skinnedMeshRenderer.GetBlendShapeWeight(rollBlendShapeIndex);
            if (currentValue < 100)
            {
                float newValue = Mathf.MoveTowards(currentValue, 100, increaseSpeed * Time.deltaTime);
                skinnedMeshRenderer.SetBlendShapeWeight(rollBlendShapeIndex, newValue);
            }
            else
            {
                if (animator != null)
                {
                    animator.enabled = true; 
                }
                scenarioManager.NextTask();
                isColliding = false; 
            }
        }
    }
}
