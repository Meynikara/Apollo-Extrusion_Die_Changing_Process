using UnityEngine;
using Proyecto26;

[System.Serializable]
public class QuestionData
{
    public string type;
    public string question;
    public int score;
}

[System.Serializable]
public class ScoreSubmitData
{
    public string user_id;
    public string team_id;
    public QuestionData questions;
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private string teamId;
    private readonly string apiUrl = "https://dev.meynikara.com/energy-transfer-server/save-score";

    private int _score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTeamId(string id)
    {
        teamId = id;
        Debug.Log($"Team ID set: {teamId}");
    }

    public void SubmitQuestion(string description, bool isMainStep, int score)
    {
         _score+=score;
        if (string.IsNullOrEmpty(teamId))
        {
            Debug.LogError(" Team ID not set! Cannot submit score.");
            return;
        }

        QuestionData question = new QuestionData
        {
            type = isMainStep ? "main" : "sub",
            question = description,
            score = score
        };

        ScoreSubmitData submitData = new ScoreSubmitData
        {
            user_id = SystemInfo.deviceUniqueIdentifier, // Using device ID as user ID
            team_id = teamId,
            questions = question // ⬅️ Send a single object, not a list
        };

        string jsonData = JsonUtility.ToJson(submitData, true);
        Debug.Log($"Sending Data: {jsonData}");

        RestClient.Post(apiUrl, jsonData)
        .Then(response =>
        {
            Debug.Log($" Successfully submitted: {description}");
        })
        .Catch(error =>
        {
            Debug.LogError($" Submission failed: {error.Message}");
        });
       
    }
    public int GetScore(){
        return _score;
    }
}
