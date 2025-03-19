using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard;

public class LoginManager : MonoBehaviour
{

    [Header("Inputs")]
    [SerializeField] private TMP_InputField _mobileInput;
    [SerializeField] private TMP_InputField _passwordInput;

    [Header("XR Keyboard")]
    [SerializeField] private XRKeyboardDisplay _displayKeyboard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchInputs(TMP_InputField _currentInput)
    {
        _displayKeyboard.gameObject.SetActive(true);
        _displayKeyboard.inputField = _currentInput;
    }

    public void DisableKeyboard()
    {
        _displayKeyboard.gameObject.SetActive(false);

    }
}
