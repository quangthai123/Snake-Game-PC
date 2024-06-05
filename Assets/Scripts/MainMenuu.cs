using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuu : MonoBehaviour
{
    [SerializeField] private GameObject highScoreUI;
    [SerializeField] private GameObject creditUI;
    [SerializeField] private List<TextMeshProUGUI> scores;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OpenHighScoreUI()
    {
        highScoreUI.SetActive(true);
        for (int i = 0; i<5; i++)
        {
            scores[i].text = PlayerPrefs.GetInt("score"+i).ToString();
        }
    }
    public void CloseHighScoreUI()
    {
        highScoreUI.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void OpenCreditUI()
    {
        creditUI.SetActive(true);
        GameAudio.instance.PlayBGM(0);
    }
    public void CloseCreditUI()
    {
        creditUI.SetActive(false);
        GameAudio.instance.StopBGM(0);
    }
}
