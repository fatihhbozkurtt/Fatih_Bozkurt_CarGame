using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Debug")]
    public bool isLevelActive = false;

    public event System.Action LevelStartedEvent;
    public event System.Action LevelSuccessEvent; // fired only on success
    public event System.Action LevelFailedEvent; // fired only on fail

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame() // LevelStarted
    {
        isLevelActive = true;
        LevelStartedEvent?.Invoke();
    }

    public void EndGame(bool success)
    {
        isLevelActive = false;

        if (success) LevelSuccessEvent?.Invoke();
        else LevelFailedEvent?.Invoke();
    }

    public void NextStage()
    {
        int sceneIndex = 1;

        if (SceneManager.GetActiveScene().buildIndex == 1)  sceneIndex = -1;


        LoadScene(SceneManager.GetActiveScene().buildIndex + sceneIndex); 
    }

    public void RestartStage()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadScene(int targetScene)
    {
        SceneManager.LoadScene(targetScene);
    }
}
