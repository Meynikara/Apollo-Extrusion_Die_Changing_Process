using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ControllManger : MonoBehaviour
{
    public AudioSource _speaker; // play all task & popup audio
    [SerializeField] private GenPopupManager _popup; // popup prefab
    public List<ProgressData> _progressDatas = new List<ProgressData>(); // user task list
    private int _currentTask; // current task id
    [SerializeField] private string _currentTaskName; // for our ref find which task is running
    public static ControllManger instance;

    [SerializeField] private bool _onStart;

    void Start()
    {
      //  _screenFader = _playerController.GetComponent<ScreenFader>();

        if (_onStart)
        {
            Invoke("StartApp", 3f);
        }
        instance = this;
    }

    void Update()
    {
        // this is for testing purpose "if press N it will call next task".
        if (Input.GetKeyDown(KeyCode.N)) 
        {
            NextTask();
        }
    }

    /*
        Context menu for testing purpose, once testing is completed 
        we need to call the STARTAPP function on any mathod.
        
        "STARTAPP is must call after that all task will cal this function call first task in the progress list".
     */
    
    [ContextMenu("StartApp")]
    public void StartApp()
    {
        NextTask();
    }


    // this function call nexttask " we can use in events or other function to call nexttask "
    public void NextTask()
    {
        if (_currentTask != 0)
            _progressDatas[_currentTask - 1]._resetTasks.Invoke();
        
        _progressDatas[_currentTask]._tasks.Invoke();
        PlayAudio();
        ShowPopup();

        _currentTaskName = _progressDatas[_currentTask]._taskName; // showing current task name in scene
        _currentTask++;

    }

    // this also call nexttask but user need to give current step label.
    // "if the label was same it will run. it prevent call many steps".
    public void NextTask(string _label)
    {
        if (_progressDatas[_currentTask]._label == _label)
        {
            NextTask();
        }
        else { return; }
    }


    public void NextTask(float _time)
    {
        Invoke("NextTask", _time);
    }


    // this is moustly use testing cause. user given taskid, that task will call.
    public void NextTask(int _taskid)
    {
        _progressDatas[_currentTask - 1]._resetTasks.Invoke();
        _progressDatas[_taskid]._tasks.Invoke();
        PlayAudio();
        ShowPopup();

        _currentTaskName = _progressDatas[_currentTask]._taskName; // showing current task name in scene
        _currentTask = _taskid + 1;

    }

    // this function use to play current task audio
    void PlayAudio()
    {
        if (_progressDatas[_currentTask]._voiceOver)
        {
            _speaker.clip = _progressDatas[_currentTask]._voiceOver;
            _speaker.Play();
        } else { return; }
    }

    // this will show current task popup.
    // "if current taks need popup, we should enable "enbPopup" in the task".
    void ShowPopup()
    {
        if (_progressDatas[_currentTask]._enbPopup)
        {
            var _currentprogress = _progressDatas[_currentTask]._popup;

            _popup.gameObject.SetActive(true);
            _popup.ShowPopup(
                _currentprogress._tittle,
                _currentprogress._content, 
                _currentprogress._primaryButtons,
                _currentprogress._secondaryButtons
                );
        }
        else
        {
            _popup?.gameObject.SetActive(false);
            return; 
        }
    }

}
