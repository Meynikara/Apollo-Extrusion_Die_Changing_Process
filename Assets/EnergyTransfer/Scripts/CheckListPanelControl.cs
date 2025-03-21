using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CheckListPanelControl : MonoBehaviour
{
    [System.Serializable]
    private class ChecklistStep
    {
        public string description;
        public bool isCompleted;
        public int originalIndex;

        public ChecklistStep(string desc, int index)
        {
            description = desc;
            isCompleted = false;
            originalIndex = index;
        }
    }

    [Header("UI References")]
    [SerializeField] private GameObject checklistItemPrefab;
    [SerializeField] private Transform checklistContainer;
    [SerializeField] private TMP_Text errorMessageText;
    [SerializeField] private TMP_Text checklistInstructionText;

    [Header("Configuration")]
    [SerializeField] private bool randomizeOnStart = true;
    [SerializeField] private float errorMessageDuration = 3f;

    private List<GameObject> instantiatedItems = new List<GameObject>();
    private List<ChecklistStep> checklistSteps;
    private int currentRequiredStep;
    private float errorMessageTimer;
    private const string DEFAULT_INSTRUCTION = "Review the steps below and select the correct step.";

    private void Awake()
    {
        InitializeChecklistData();
    }

    private void Start()
    {
        if (ValidateReferences())
        {
            InitializeUI();
        }
    }

    private void Update()
    {
        HandleErrorMessageTimer();
    }

    private void InitializeChecklistData()
    {
        checklistSteps = new List<ChecklistStep>
        {
            new ChecklistStep("Pre-work checklist.", 0),
            new ChecklistStep("Notify Gas Control and Operations.", 1),
            new ChecklistStep("Press and hold the Transfer Test button.", 2),
            new ChecklistStep("Select and wear the appropriate PPE.", 3),
            new ChecklistStep("Perform voltage checks using the multimeter.", 4),
            new ChecklistStep("Record data in Form S230D.", 5),
            new ChecklistStep("Ensure generator switches back to shore power and enters cooldown mode.", 6)
        };

        if (randomizeOnStart)
        {
            RandomizeSteps();
        }
    }

    private bool ValidateReferences()
    {
        if (checklistItemPrefab == null || checklistContainer == null)
        {
            Debug.LogError("Checklist item prefab or container is not assigned!");
            return false;
        }
        return true;
    }

    private void InitializeUI()
    {
        if (checklistInstructionText != null)
        {
            checklistInstructionText.text = DEFAULT_INSTRUCTION;
        }
        InitializeChecklistItems();
    }

    private void InitializeChecklistItems()
    {
        ClearExistingItems();
        CreateChecklistItems();
        ClearErrorMessage();
    }

    private void ClearExistingItems()
    {
        foreach (var item in instantiatedItems)
        {
            if (item != null)
                Destroy(item);
        }
        instantiatedItems.Clear();
    }

    private void CreateChecklistItems()
    {
        for (int i = 0; i < checklistSteps.Count; i++)
        {
            GameObject checklistItem = Instantiate(checklistItemPrefab, checklistContainer);
            instantiatedItems.Add(checklistItem);

            Button button = checklistItem.GetComponent<Button>();
            Toggle toggle = checklistItem.GetComponentInChildren<Toggle>();
            TMP_Text descriptionText = checklistItem.GetComponentInChildren<TMP_Text>();

            if (descriptionText != null)
            {
                descriptionText.text = checklistSteps[i].description;
            }

            int originalIndex = checklistSteps[i].originalIndex;

            if (checklistSteps[i].isCompleted)
            {
                button.interactable = false;
                if (toggle != null) toggle.isOn = true;
            }
            else
            {
                button.interactable = true;
                if (toggle != null) toggle.isOn = false;
            }

            button.onClick.AddListener(() => HandleStepSelection(originalIndex));
        }
    }

    private void HandleStepSelection(int originalIndex)
    {
        if (originalIndex == currentRequiredStep)
        {
            HandleCorrectStep(originalIndex);
        }
        else
        {
            HandleIncorrectStep(originalIndex);
        }
    }

    private void HandleCorrectStep(int originalIndex)
    {
        var step = checklistSteps.Find(s => s.originalIndex == originalIndex);
        if (step != null)
        {
            step.isCompleted = true;
            AdvanceToNextTask();
        }
    }

    private void HandleIncorrectStep(int originalIndex)
    {
        ShowErrorMessage();
    }

    private void AdvanceToNextTask()
    {
        if (ScenarioManager.Instance != null)
        {
            ScenarioManager.Instance.NextTask();
        }
        else
        {
            Debug.LogWarning("ScenarioManager instance is null!");
        }
    }

    private void ShowErrorMessage()
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = "Incorrect step! Try again.";
            errorMessageTimer = errorMessageDuration;
        }
    }

    private void HandleErrorMessageTimer()
    {
        if (errorMessageTimer > 0)
        {
            errorMessageTimer -= Time.deltaTime;
            if (errorMessageTimer <= 0)
            {
                ClearErrorMessage();
            }
        }
    }

    private void ClearErrorMessage()
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = string.Empty;
        }
    }

    private void RandomizeSteps()
    {
        checklistSteps = checklistSteps.OrderBy(x => Random.value).ToList();

        // Reassign indices to maintain proper order
        for (int i = 0; i < checklistSteps.Count; i++)
        {
            checklistSteps[i].originalIndex = i;
        }
    }
}
