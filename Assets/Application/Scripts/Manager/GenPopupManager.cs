using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
public class GenPopupManager : MonoBehaviour
{
    [Header("REFERENCE")]
    public TMP_Text _tittle; // for popup tittle
    public TMP_Text _content; // for popup content
    public VideoClip _videoClip;
    public Button _primaryButton; // next task button
    public Button _secondaryButton; // close buttton

    private ControllManger _controllingManger; // controll manger ref

    void Start()
    {
        _controllingManger = ControllManger.instance;
    }

    // this will show current taks popup
    public void ShowPopup(string tittle,string content, bool _primary, bool _secondary)
    {
        _tittle.text = tittle;
        _content.text = content;


       /* if (_primary)
        {
            _primaryButton.gameObject.SetActive(true);
            _primaryButton.onClick.RemoveAllListeners();
            _primaryButton.onClick.AddListener(() => _controllingManger.NextTask());
        }
        else { _primaryButton.gameObject.SetActive(false); }

        if (_secondary)
        {
            _secondaryButton.gameObject.SetActive(true);
            _secondaryButton.onClick.RemoveAllListeners();
            _secondaryButton.onClick.AddListener(() => DisablePopup());
        }
        else { _secondaryButton.gameObject.SetActive(false); }*/
    }


    // this will use for disable popup
    void DisablePopup()
    {
       gameObject.SetActive(false);
    }
}
