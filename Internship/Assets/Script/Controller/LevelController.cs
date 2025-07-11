using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
    private static LevelController _instance;
    public static LevelController Instance => _instance;

    public GameObject loadingScreen;
    public Slider progress;

    [Header("Dungeon Randomization Settings")]
    public int levelStartIndex = 1; // First playable dungeon level
    public int levelEndIndex = 5;   // Last non-boss level
    public int bossLevelIndex = 6;  // Index of the boss level
    public int levelsBeforeBoss = 3;

    private int currentLevel;
    private int levelCount = 0;

    private Queue<int> levelQueue = new Queue<int>();

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Optional if persistence is needed
            GenerateLevelQueue();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void startGame()
    {
        Debug.Log("Start Game");

        int levelSaved = PlayerPrefs.GetInt("level", 0);
        PlayerPrefs.SetInt("score", ParametersScript.scoreValue);
        PlayerPrefs.SetInt("heal", ParametersScript.healValue);

        currentLevel = levelSaved + 1;
        levelCount = 0;
        GenerateLevelQueue(); // Reset level sequence
        nextLevel();
    }

    public void nextLevel()
    {
        Debug.Log("Next Level");
        PlayerPrefs.SetInt("level", currentLevel);
        currentLevel++;
        levelCount++;

        if (levelCount >= levelsBeforeBoss)
        {
            levelCount = 0;
            GenerateLevelQueue(); // Reset queue for next dungeon cycle
            StartCoroutine(loadScene(bossLevelIndex));
            return;
        }

        if (levelQueue.Count == 0)
        {
            GenerateLevelQueue();
        }

        int chosenLevel = levelQueue.Dequeue();
        StartCoroutine(loadScene(chosenLevel));
    }

    private void GenerateLevelQueue()
    {
        List<int> levels = new List<int>();
        for (int i = levelStartIndex; i <= levelEndIndex; i++)
        {
            levels.Add(i);
        }

        // Fisher-Yates shuffle
        for (int i = 0; i < levels.Count; i++)
        {
            int randomIndex = Random.Range(i, levels.Count);
            int temp = levels[i];
            levels[i] = levels[randomIndex];
            levels[randomIndex] = temp;
        }

        levelQueue = new Queue<int>(levels);
    }

    public void returnBase()
    {
        StartCoroutine(loadScene(0)); // Assuming 0 is the main menu
    }

    IEnumerator loadScene(int level)
    {
        Debug.Log("Load level " + level);
        progress.value = 0;
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);

        while (!operation.isDone)
        {
            progress.value = operation.progress * 100;
            Debug.Log($"Load {progress.value}%");
            yield return null;
        }

        loadingScreen.SetActive(false);
        yield return null;
    }

    void Update() { }
}
