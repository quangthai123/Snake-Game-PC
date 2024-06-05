using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Transform foodPrefab;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI yourScoreText;
    [SerializeField] private GameObject endgameUI;
    [SerializeField] private GameObject pausegameUI;
    [SerializeField] private GameObject countdownGameObject;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject selectDifficultyUI;
    [SerializeField] private GameObject highScoreUI;
    [SerializeField] private List<TextMeshProUGUI> scores;
    private float countdownTimer = 4;
    private Player player;
    private float second = 0;
    private int score = 0;
    private bool endGame = false;
    [HideInInspector] public bool pause = false;
    void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        scoreText.text = "Score: " + score;
        timeText.text = "";
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        SpawnFoodInARandomPos();
        Time.timeScale = 0f;
    }
    void Update()
    {
        if (player.startGame && !pause)
            ControlTimer();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pause && !selectDifficultyUI.activeInHierarchy)
                PauseGame();
            else if (pause && !countdownGameObject.activeInHierarchy)
                StartCountdownToContinueGame();
        }
        if (pause && countdownGameObject.activeInHierarchy)
        {
            countdownTimer -= Time.deltaTime;
            int countdown = (int)countdownTimer;
            countdownText.text = countdown + "";
        }
    }

    private void ControlTimer()
    {
        second += Time.deltaTime;
        int secondInt = Mathf.FloorToInt(second % 60);
        int minute = Mathf.FloorToInt(second / 60);
        timeText.text = string.Format("{0:00}:{1:00}", minute, secondInt);
        if (minute == 60)
            SetEndGame();
    }

    public void SpawnFoodInARandomPos()
    {
        Vector3 rdPos = new Vector3(Random.Range(-25, 26), Random.Range(-13, 10), 0f);
        for (int i = 0; i < player.snakeParts.Count; i++)
        {
            if (rdPos == player.snakeParts[i].position)
                rdPos = new Vector3(Random.Range(-25, 26), Random.Range(-13, 10), 0f);
        }
        Instantiate(foodPrefab, rdPos, Quaternion.identity);
    }
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }
    public void SetEndGame()
    {
        endGame = true;
        endgameUI.SetActive(true);
        yourScoreText.text = "Your Score: " + score;
        Time.timeScale = 0;
        if (score <= PlayerPrefs.GetInt("score4"))
            return;           
        for(int i=0; i<5; i++)
        {
            if(score > PlayerPrefs.GetInt("score"+i))
            {
                for(int j=4; j>=i+1; j--)
                    PlayerPrefs.SetInt("score" + j, PlayerPrefs.GetInt("score" + (j-1)));
                PlayerPrefs.SetInt("score"+i, score);
                break;
            }
        }
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void PauseGame()
    {
        if (!endGame)
        {
            Time.timeScale = 0;
            pausegameUI.SetActive(true);
            pause = true;
        }
    }
    public void StartCountdownToContinueGame()
    {
        pausegameUI.SetActive(false);
        Time.timeScale = 1f;

        if (player.startGame)
        {
            countdownGameObject.SetActive(true);
            Invoke("ContinueGame", 3f);
        }
        else
        {
            Time.timeScale = 1f;
            pause = false;
            StartCoroutine(player.MoveToTheNextPosition());
        }

    }
    private void ContinueGame()
    {
        StartCoroutine(player.MoveToTheNextPosition());
        countdownGameObject.SetActive(false);
        pause = false;
        countdownTimer = 4;
    }
    public void SelectDifficulty(int d)
    {
        if (d == 1)
            player.timePerStep = 0.3f;
        else if (d == 2)
            player.timePerStep = 0.2f;
        else if (d == 3)
            player.timePerStep = 0.1f;
        else if (d == 4)
            player.timePerStep = 0.05f;
        else if (d == 5)
        {
            player.timePerStep = 0.025f;
            player.scoreToAdd = 1;
        }

        selectDifficultyUI.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void CloseHighScoreUI()
    {
        highScoreUI.SetActive(false);
    }
    public void OpenHighScoreUI()
    {
        highScoreUI.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            scores[i].text = PlayerPrefs.GetInt("score" + i).ToString();
        }
    }
}
