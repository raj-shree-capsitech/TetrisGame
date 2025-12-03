using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
#endif

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject ingamepanel;
    [SerializeField] private Button pausebutton;
    public static UIManager instance;
    [SerializeField] private TextMeshProUGUI scoretext;
    private bool ispaused = false;
    [SerializeField] private TextMeshProUGUI pausetext;
    [SerializeField] private TextMeshProUGUI resumetext;
    [SerializeField] private Button mainpanel;
    private SpawnTetro spawn;
    public static bool restart = false;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if(restart)
        {
            StartGame();
            ResetVal();
        }
        else
        {
            Debug.Log("Start");
            startPanel.SetActive(true);
        }  
    }
    public void StartGame()
    {
        startPanel.SetActive(false);
        ingamepanel.SetActive(true);
        SpawnTetro.hasgamestart = true;    //tell the game start
        FindObjectOfType<SpawnTetro>().StartSpawn();
    }

    public void UpdateScore(int val)
    {
        scoretext.text = "Score: " + val;
    }

    public void GameOver()
    {
        endPanel.SetActive(true);
        ingamepanel.SetActive(false);
    }

    public void RestartGame()
    {
        restart = true;
        ResetVal();
        SceneManager.LoadScene("GameScene");
    }

    public void ResetVal()
    {
        TetrominoMove.score = 0;
        scoretext.text = "Score: 0";
        TetrominoMove.grid=new Transform[TetrominoMove.width, TetrominoMove.height];
    }

    public void PauseResume()
    {
        if(ispaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        ispaused = true;
        Time.timeScale = 0f;
        resumetext.gameObject.SetActive(true);
        pausetext.gameObject.SetActive(false);
    }

    public void Resume()
    {
        ispaused = false;
        Time.timeScale = 1f;
        resumetext.gameObject.SetActive(false);
        pausetext.gameObject.SetActive(true);
    }

    public void MainPanel()
    {
        SceneManager.LoadScene("GameScene");
        ingamepanel.SetActive(false);
    }
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // quit Unity player
#endif
    }
}
