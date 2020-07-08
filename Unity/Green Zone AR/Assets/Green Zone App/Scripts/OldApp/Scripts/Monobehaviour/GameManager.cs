using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameLogic;
using UnityEngine.SceneManagement;

public enum GameState
{
    PauseLevel,
    ActiveLevel,
    ResetLevel,
    NextLevel,
    QuitLevel
}

public class GameManager : MonoBehaviour {

    [Tooltip("Player Agents In the Scene")]
    [SerializeField] public List<Agents> playerUnits;
    [SerializeField] public List<GameStateData> playerData;
    [SerializeField] public GameStateData resetPlayerData;
    [SerializeField] public GameStateData levelData;
    [SerializeField] public ScreenManager screenManager;

    [SerializeField] private KeyCode _EscapeKey = KeyCode.Escape;

    public GameState state = GameState.ActiveLevel;

    bool isPaused = true;

    private void Start()
    {
        SplashSequence();
    }

    void Update()
    {
        CheckGameState();
    }

    private void CheckGameState()
    {
        foreach (GameStateData playerData in playerData)
        {
            // update game time
            if (Time.frameCount % 10 == 0 && !isPaused)
                playerData.timeInLevel -= 0.1f;

            if (playerData.timeInLevel <= 0)
            {
                RestartSequence();
            }
            if (playerData.checkpointInLevel >= levelData.checkpointInLevel)
            {
                //WinSequence();
            }
        }

        if (Input.GetKeyDown(_EscapeKey))
        {
            isPaused = !isPaused;
            MenuSequence();
            //QuitSequence();
        }
    }

    public void SplashSequence()
    {
        state = GameState.PauseLevel;
        isPaused = true;
        ResetGameData();
        StartCoroutine(screenManager.CallSplash());
    }

    public void MenuSequence()
    {
        if (isPaused)
        {
            state = GameState.PauseLevel;
            StartCoroutine(screenManager.CallMenu());
        }
        else
        {
            state = GameState.ActiveLevel;
            StartCoroutine(screenManager.CloseMenu());
        }
    }

    public void StartSequence()
    {
        state = GameState.ActiveLevel;
        ResetLevelData();
        StartCoroutine(screenManager.CloseMenu());
        isPaused = false;
    }

    public void WinSequence()
    {
        state = GameState.NextLevel;
        isPaused = true;
        ResetLevelData();
        StartCoroutine(screenManager.CallWin());
        StartCoroutine(UpdateLevel(state));
        ResetLevelData();
    }

    public void RestartSequence()
    {
        state = GameState.ResetLevel;
        isPaused = true;
        ResetLevelData();
        StartCoroutine(screenManager.CallLoose());
        StartCoroutine(UpdateLevel(state));
        ResetLevelData();
    }

    public void QuitSequence()
    {
        state = GameState.QuitLevel;
        isPaused = true;
        ResetGameData();
        StartCoroutine(screenManager.CallSplash());
        StartCoroutine(UpdateLevel(state));
    }

    public void UpdateScore(Agents currentPlayer, int score)
    {
        for (int index = 0; index < playerUnits.Count; index++)
        {
            if (playerUnits[index] == currentPlayer)
            {
                playerData[index].scoreValue += score;
            }
        }
    }
    public void UpdatePickups(Agents currentPlayer)
    {
        for (int index = 0; index < playerUnits.Count; index++)
        {
            if (playerUnits[index] == currentPlayer)
            {
                playerData[index].pickUpsCollected++;
            }
        }
    }
    public void UpdateCheckpoints(Agents currentPlayer)
    {
        for (int index = 0; index < playerUnits.Count; index++)
        {
            if (playerUnits[index] == currentPlayer)
            {
                playerData[index].checkpointInLevel++;
            }
        }
    }

    /// <summary>
    /// Reset Game Specific Data
    /// </summary>
    public void ResetGameData()
    {
        foreach (GameStateData playerData in playerData)
        {
            playerData.level = resetPlayerData.level;
            playerData.scoreValue = resetPlayerData.scoreValue;
        }
        ResetLevelData();
    }

    /// <summary>
    /// Reset Level Specific Data
    /// </summary>
    public void ResetLevelData()
    {
        foreach (GameStateData playerData in playerData)
        {
            playerData.level++;
            playerData.health = resetPlayerData.health;
            playerData.checkpointInLevel = resetPlayerData.checkpointInLevel;
            playerData.pickUpsCollected = resetPlayerData.pickUpsCollected;
            playerData.timeInLevel = levelData.timeInLevel;
        }
    }

    /// <summary>
    /// Test for end case
    /// </summary>
    /// <param name="setState"></param>
    public IEnumerator UpdateLevel(GameState setState)
    {
        yield return new WaitForSeconds(2.0f);
        if (setState == GameState.ResetLevel)
        {
            yield return new WaitForSeconds(5.0f);
            // Reload the level that is currently loaded.
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (setState == GameState.NextLevel)
        {
            // TODO Load the next level
            // Start loading the given scene and wait for it to finish.
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //StartCoroutine(LoadSceneAndSetActive(sceneName));
            //SceneManager.LoadScene(menuSceenName);
        }
        else if (setState == GameState.QuitLevel)
        {
            // Quit Game
            Quit();
        }
    }

    /// <summary>
    /// TODO Load Scene in background with loading bar
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        // Allow the given scene to load over several frames and add it to the already loaded scenes (just the Persistent scene at this point).
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Find the scene that was most recently loaded (the one at the last index of the loaded scenes).
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        // Set the newly loaded scene as the active scene (this marks it as the one to be unloaded next).
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    /// <summary>
    /// Quit application
    /// </summary>
    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    /// <summary>
    /// Game Stae Helper functions
    /// </summary>
    /// <param name="gameStateName"></param>
    /// <returns></returns>
    public static GameState GetGameStateFromString(string gameStateName)
    {
        switch (gameStateName)
        {
            case "ActiveLevel":
                return GameState.ActiveLevel;
            case "ResetLevel":
                return GameState.ResetLevel;
            case "NextLevel":
                return GameState.NextLevel;
            case "QuitLevel":
                return GameState.QuitLevel;
            default:
                return GameState.ActiveLevel;
        }
    }
    public string GetGameStateName()
    {
        switch (state)
        {
            case GameState.ActiveLevel:
                return "ActiveLevel";
            case GameState.ResetLevel:
                return "ResetLevel";
            case GameState.NextLevel:
                return "NextLevel";
            default:
                return "ActiveLevel";
        }
    }


}