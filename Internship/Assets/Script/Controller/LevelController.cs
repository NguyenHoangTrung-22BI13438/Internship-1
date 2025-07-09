using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
    private static LevelController _instance;
    public static LevelController Instance
    {
        get { return _instance; }
    }

    public GameObject loadingScreen;
    public Slider progress;

    [Header("Dungeon Randomization Settings")]
    public int[] randomLevelIndices;     // Set in Inspector (e.g. [2, 3, 4, 5])
    public int bossLevelIndex = 6;       // Set to the build index of Boss scene
    public int levelsBeforeBoss = 3;     // How many dungeon levels before Boss

    private int currentLevel;
    private int levelCount = 0;
    private List<int> usedLevels = new List<int>();

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // optional, if you want persistence
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
        usedLevels.Clear();

        nextLevel(); // Start with randomized logic right away
    }

    public void nextLevel()
    {
        Debug.Log("Next Level");
        PlayerPrefs.SetInt("level", currentLevel);
        currentLevel++;
        levelCount++;

        // Boss condition
        if (levelCount >= levelsBeforeBoss)
        {
            levelCount = 0;
            usedLevels.Clear(); // optional: reset used level tracking
            StartCoroutine(loadScene(bossLevelIndex));
            return;
        }

        // Get available levels not yet used
        List<int> available = new List<int>(randomLevelIndices);
        available.RemoveAll(i => usedLevels.Contains(i));

        // If all used, reset
        if (available.Count == 0)
        {
            usedLevels.Clear();
            available = new List<int>(randomLevelIndices);
        }

        int chosen = available[Random.Range(0, available.Count)];
        usedLevels.Add(chosen);

        StartCoroutine(loadScene(chosen));
    }

    public void returnBase()
    {
        StartCoroutine(loadScene(0)); // Assuming 0 is base camp or menu
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
