using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameWidgets
{
    FadeScreen,
    SplashScreen,
    MenuScreen,
    GameModeScreen,
    OptionsScreen,
    GameScreen,
    WinScreen,
    LooseScreen
}

public class Widgets : MonoBehaviour
{
    [Tooltip("Player Agents In the Scene")]
    [SerializeField] public GameWidgets gameWidgetName;
}
