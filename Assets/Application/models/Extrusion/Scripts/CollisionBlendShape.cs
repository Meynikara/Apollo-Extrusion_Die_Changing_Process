using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBlendShape : MonoBehaviour
{
    public string targetTag = "Target"; 
    public SkinnedMeshRenderer skinnedMeshRenderer; 
    public Animator animator; 
    public float blendShapeSpeed = 30f;
    public float fastBlendShapeSpeed = 100f;
    public float animatorDuration = 1f; 

    private int rollLeftToRightIndex = -1;
    private int rollRightToLeftIndex = -1;
    private bool isColliding = false;
    private bool leftCompleted = false;
    private bool rightStarted = false;
    private bool rightCompleted = false;
    public Outline _outline;
    public MonoBehaviour grabbableScript;
    public ScenarioManager scenarioManager;
    private void Start()
    {
       
        Mesh mesh = skinnedMeshRenderer.sharedMesh;
        for (int i = 0; i < mesh.blendShapeCount; i++)
        {
            string blendShapeName = mesh.GetBlendShapeName(i);
            if (blendShapeName == "roll left to right")
            {
                rollLeftToRightIndex = i;
            }
            else if (blendShapeName == "roll right to left")
            {
                rollRightToLeftIndex = i;
            }
        }

        if (rollLeftToRightIndex == -1 || rollRightToLeftIndex == -1)
        {
            Debug.LogError("Blend shapes 'roll left to right' or 'roll right to left' not found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if(_outline.enabled == true)
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

    private void Update()
    {
        
        if (isColliding && !leftCompleted && rollLeftToRightIndex != -1)
        {
            float currentLeft = skinnedMeshRenderer.GetBlendShapeWeight(rollLeftToRightIndex);
            float newLeft = Mathf.MoveTowards(currentLeft, 100f, blendShapeSpeed * Time.deltaTime);
            skinnedMeshRenderer.SetBlendShapeWeight(rollLeftToRightIndex, newLeft);

            if (newLeft >= 100f)
            {
                leftCompleted = true; 
                rightStarted = true;  
            }
        }

       
        if (rightStarted && !rightCompleted && rollRightToLeftIndex != -1)
        {
            float currentRight = skinnedMeshRenderer.GetBlendShapeWeight(rollRightToLeftIndex);
            float newRight = Mathf.MoveTowards(currentRight, 100f, fastBlendShapeSpeed * Time.deltaTime);
            skinnedMeshRenderer.SetBlendShapeWeight(rollRightToLeftIndex, newRight);

            if (newRight >= 100f)
            {
                rightCompleted = true; 
                animator.enabled = true; 
                StartCoroutine(DisableAnimatorAfterDelay(animatorDuration)); 
            }
        }
    }

    private IEnumerator DisableAnimatorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.enabled = false;
        scenarioManager.NextTask();
        if (grabbableScript != null)
        {
            //grabbableScript.enabled = true;
        }
    }
}
