using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI highestScore;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button exitButton;

    private string _user;
    private int _score;
    private FirebaseDatabase _database;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonPressed);

        plusButton.onClick.AddListener(OnPlusButtonPressed);
        minusButton.onClick.AddListener(OnMinusButtonPressed);

        saveButton.onClick.AddListener(OnSaveButtonPressed);
        exitButton.onClick.AddListener(OnExitButtonPressed);
    }

    private void Start()
    {
        _database = FirebaseDatabase.DefaultInstance;
    }

    private void OnStartButtonPressed()
    {
        _user = userInput.text.ToUpper();
        userName.text = _user;

        _database.GetReference("Users").GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot snapshot = task.Result;
            var highScore = snapshot.Child(_user).Value.ToString();
            highestScore.text = highScore ?? "00";
        });

        loginPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    private void OnPlusButtonPressed()
    {
        _score++;
        scoreText.text = $"SCORE {_score:00}";
    }

    private void OnMinusButtonPressed()
    {
        _score--;
        scoreText.text = $"SCORE {_score:00}";
    }

    private void OnSaveButtonPressed()
    {
        Debug.Log($"Saving score for {_user}...");
        _database.RootReference.Child("Users").Child(_user).SetValueAsync($"{_score:00}");
    }

    private void OnExitButtonPressed()
    {
        mainPanel.SetActive(false);
        loginPanel.SetActive(true);
        _score = 0;
        scoreText.text = $"SCORE {_score:00}";
    }
}
