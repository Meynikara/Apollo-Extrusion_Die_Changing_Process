using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ScenarioManager : MonoBehaviour
{
    public AudioSource _speaker; // play all task & popup audio
    [SerializeField] private GenPopupManager _popup; // popup prefab
    public VideoPlayer _tutoPlayer; // tutorial video player

    [SerializeField] private int _currentTask; // current task id
    private int _currentScenario;
    [SerializeField] private string _scenarioName; // for our ref find which Scenario is running
    [SerializeField] private string _currentTaskName; // for our ref find which task is running

    [SerializeField] private bool _onStart;
    public List<ScenarioData> _scenarios = new List<ScenarioData>();

    [Header("Subtitle")]
    public GameObject _subtitle;
    public TMP_Text _subtitle_text;

    [Header("Choose Mode")]
    public static bool UnGuided;
    public static bool Guided;
    // Start is called before the first frame update
    public static ScenarioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (_onStart)
        {
           // ChooseUnGuided();
            Invoke("StartApp", 3f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Invoke("NextTask",0.2f);
        }
    }


    public void ChooseGuided()
    {
        UnGuided = false;
        Guided = true;
        Debug.Log("Choosing Guided Mode");
    }

    [ContextMenu("ChooseUnGuided")]
    public void ChooseUnGuided()
    {
        Guided = false;
        UnGuided = true;
        Debug.Log("Choosing UnGuided Mode");
        GetScenario("Unguided");

    }


    [ContextMenu("StartApp")]
    public void StartApp()
    {
        NextTask();
    }

    [ContextMenu("TestScenario")]
    public void TestScenario()
    {
        GetScenario("Crane Scenario");
    }


    public void GetScenario(string scenarioName)
    {
         _scenarioName = scenarioName;

        for(int i=0; i< _scenarios.Count; i++)
        {
            if (_scenarioName == _scenarios[i]._scenarioName)
            {
                _currentScenario = i;
                _currentTask = 0;
                StartApp();
            }
        }
    }


    // this function call nexttask " we can use in events or other function to call nexttask "
    public void NextTask()
    {
        if (_popup != null)
        {
            _popup.gameObject.SetActive(false);
        }

        if (_currentTask != 0)
            _scenarios[_currentScenario]._procedures[_currentTask - 1]._resetTasks.Invoke();


        if (_scenarios[_currentScenario]._procedures[_currentTask]._skipUnGuided && UnGuided) // Skiping Current Step For UnGuided Mode 
        {
            Invoke("NextTask", .2f);
            IncreaseTaskId();
          //  return;
        }


        if (_scenarios[_currentScenario]._procedures[_currentTask]._skipGuided && Guided) // Skiping Current Step For Guided Mode 
        {
            Invoke("NextTask", .2f);
            IncreaseTaskId();
           // return ;
        }


        NextStep();
        IncreaseTaskId();
       // return;
    }

    void NextStep()
    {
        
        _scenarios[_currentScenario]._procedures[_currentTask]._tasks.Invoke();
        PlayAudio();
        ShowPopup();
        ShowSubtitle();
    }

    void IncreaseTaskId()
    {
        _currentTaskName = _scenarios[_currentScenario]._procedures[_currentTask]._taskName; // showing current task name in scene
        _currentTask++;
    }


    // this also call nexttask but user need to give current step label.
    // "if the label was same it will run. it prevent call many steps".
    public void NextTask(string _label)
    {
        if (_scenarios[_currentScenario]._procedures[_currentTask-1]._label == _label)
        {
            NextTask();
        }
        else { return; }
    }


    public void NextTask(float _time)
    {
        Debug.Log("invoked the event");
        Invoke("NextTask", _time);
    }


    // this is moustly use testing cause. user given taskid, that task will call.
    public void NextTask(int _taskid)
    {
        _popup.gameObject.SetActive(false);

        _scenarios[_currentScenario]._procedures[_currentTask -1 ]._resetTasks.Invoke();
        _scenarios[_currentScenario]._procedures[_taskid]._tasks.Invoke();
        PlayAudio();
        ShowPopup();
        ShowSubtitle();

        _currentTaskName = _scenarios[_currentScenario]._procedures[_currentTask]._taskName; // showing current task name in scene
        _currentTask = _taskid + 1;

    }

    // this function use to play current task audio
    void PlayAudio()
    {
        if (_scenarios[_currentScenario]._procedures[_currentTask]._voiceOver != null)
        {
            _speaker.clip = _scenarios[_currentScenario]._procedures[_currentTask]._voiceOver;
            _speaker.Play();
        }
        else
        {
            _speaker.Stop();
            return;
        }
    }

    // this will show current task popup.
    // "if current taks need popup, we should enable "enbPopup" in the task".
    void ShowPopup()
    {
        
        var _curTask = _scenarios[_currentScenario]._procedures[_currentTask];

        if (_curTask._enbPopup)
        {
            var _currentprogress = _curTask._popup;

            _popup.gameObject.SetActive(true);
            _popup.ShowPopup(
                _currentprogress._tittle,
                _currentprogress._content, 
                _currentprogress._primaryButtons,
                _currentprogress._secondaryButtons
            );
            _tutoPlayer.clip = _currentprogress._videoClip;
            _tutoPlayer.Play();
        }
        else
        {
            _popup?.gameObject.SetActive(false);
            return; 
        }
    }


    void ShowSubtitle()
    {
        if (_subtitle!=null)
        {
            _subtitle.SetActive(false);
        }

        var _curTask = _scenarios[_currentScenario]._procedures[_currentTask];

        if (_curTask._subtitle)
        {
            _subtitle.SetActive(true);
            _subtitle_text.text = _curTask._subtitleText;
        }
        else
        {
            if (_subtitle != null)
            {
                _subtitle.SetActive(false);
            }
            return;
        }
    }


    public void LoadScene(string _sceneName)
    {
        SceneManager.LoadScene( _sceneName );
    }
}
