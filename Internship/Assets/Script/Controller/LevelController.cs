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

    // Track which levels have been used this cycle
    private HashSet<int> usedLevels = new HashSet<int>();

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
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

        // reset progress
        ParametersScript.scoreValue = 0;
        ParametersScript.healValue = 1000;
        PlayerPrefs.SetInt("level", 0);

        currentLevel = levelStartIndex;
        levelCount = 0;
        usedLevels.Clear();
        GenerateLevelQueue();
        nextLevel();
    }

    public void nextLevel()
    {
        Debug.Log("Next Level");

        // increment counters
        PlayerPrefs.SetInt("level", currentLevel);
        currentLevel++;
        levelCount++;

        // If it's time for the boss, reset for next cycle
        if (levelCount >= levelsBeforeBoss)
        {
            levelCount = 0;
            usedLevels.Clear();      // allow all levels again in next cycle
            GenerateLevelQueue();
            StartCoroutine(loadScene(bossLevelIndex));
            return;
        }

        // If queue empties, regenerate from the remaining levels
        if (levelQueue.Count == 0)
            GenerateLevelQueue();

        int chosenLevel = levelQueue.Dequeue();
        usedLevels.Add(chosenLevel);   // mark as used
        StartCoroutine(loadScene(chosenLevel));
    }

    private void GenerateLevelQueue()
    {
        // Build list of levels that have not yet been used
        List<int> levels = new List<int>();
        for (int i = levelStartIndex; i <= levelEndIndex; i++)
        {
            if (!usedLevels.Contains(i))
                levels.Add(i);
        }

        // Fisher–Yates shuffle
        for (int i = 0; i < levels.Count; i++)
        {
            int r = Random.Range(i, levels.Count);
            int tmp = levels[i];
            levels[i] = levels[r];
            levels[r] = tmp;
        }

        levelQueue = new Queue<int>(levels);
    }

    public void returnBase()
    {
        StartCoroutine(loadScene(0));
    }

    IEnumerator loadScene(int level)
    {
        Debug.Log("Load level " + level);
        progress.value = 0;
        loadingScreen.SetActive(true);

        AsyncOperation op = SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
        while (!op.isDone)
        {
            progress.value = op.progress * 100;
            yield return null;
        }

        loadingScreen.SetActive(false);
    }

    void Update() { }
}
