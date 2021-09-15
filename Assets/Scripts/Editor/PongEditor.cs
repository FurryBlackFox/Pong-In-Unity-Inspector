using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pong))]
public class PongEditor : Editor
{
    #region Variables

    public static event Action OnEnableEvent;
    public static event Action<float> OnUpdateEvent;
    public static event Action<Event> OnGUIEvent; 
    public static event Action<Rect> OnUpdateScreenRect;
    public static event Action<string> OnScoreChanged;
    public static event Action OnRestart;

    private const float ScreenOffset = 5f;
    private const float AspectRatio = 9f / 16f;
    private const int InfoBigFontSize = 20;
    private const int InfoSmallFontSize = 15;
    private const KeyCode RestartKey = KeyCode.R;

    private GUIBall ball = new GUIBall();
    private GUIPlayer player1 = new GUIPlayer(PlayerSide.Left , KeyCode.W, KeyCode.S);
    private GUIPlayer player2 = new GUIPlayer(PlayerSide.Right , KeyCode.UpArrow, KeyCode.DownArrow);
    private GUIText scoreText = new GUIText(new Vector2(0.5f, 0.35f), "0:0");

    private Rect backscreenRect;
    private Rect screenRect;
    private float cashedWidth;

    private GUIStyle infoStyle;
    
    private float deltaTime;
    private double cashedTime;

    #endregion
    
    #region Events

    private void OnEnable()
    {
        EditorApplication.update += OnUpdate;

        AssignEventsInElements();

        ball.OnHitOnPlayersGates += OnHitOnPlayersGatesHandler;
        ball.player1 = player1;
        ball.player2 = player2;
        
        OnEnableEvent?.Invoke();
        
        ClearValues();

    }

    private void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
    }


    public override void OnInspectorGUI()
    {
        UpdateScreenRect();

        CheckInput(Event.current);
        OnGUIEvent?.Invoke(Event.current);

        OnUpdateEvent?.Invoke(deltaTime);

        DrawScreen();
        DrawElements();
        DrawInfo();
    }
    
    private void OnUpdate()
    {
        deltaTime = CalculateDeltaTime();
        Repaint();
    }

    private void OnHitOnPlayersGatesHandler(PlayerSide playerSide)
    {
        var winner = playerSide == PlayerSide.Left ? player2 : player1;
        winner.score++;
        OnScoreChanged?.Invoke($"{player1.score}:{player2.score}");
        
        ball.RecalculateDirection();
        ball.ClearOffset();
    }

    #endregion

    #region DeltaTime
    
    private float CalculateDeltaTime()
    {
        var currentTime= EditorApplication.timeSinceStartup;
        var dTime = (float) (currentTime - cashedTime);
        cashedTime = currentTime;
        return dTime;
    }

    #endregion

    #region Draw

    private void DrawScreen()
    {
        GUI.DrawTexture(backscreenRect, EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill, false,
            0, Color.white, Vector4.one * ScreenOffset, 0f );
        EditorGUI.DrawRect(screenRect, Color.black);
    }

    private void DrawElements()
    {
        scoreText.DrawItself();
        ball.DrawItself();
        player1.DrawItself();
        player2.DrawItself();
    }

    private void DrawInfo()
    {
        infoStyle.fontSize = InfoSmallFontSize;
        GUILayout.Label($"Restart - {RestartKey}", infoStyle);
        using (new GUILayout.HorizontalScope())
        {
            player1.DrawInfo(infoStyle, InfoBigFontSize, InfoSmallFontSize);
            player2.DrawInfo(infoStyle, InfoBigFontSize, InfoSmallFontSize);
        }
    }

    #endregion

    #region Calculations

    private void UpdateScreenRect()
    {
        var width = EditorGUIUtility.currentViewWidth;
        var height = width * AspectRatio;
        
        GUILayoutUtility.GetRect(width, height);
        
        screenRect = new Rect(ScreenOffset, ScreenOffset, width - 2 * ScreenOffset, height - 2 * ScreenOffset);
        backscreenRect = new Rect(0, 0, width, height);

        if (Math.Abs(cashedWidth - width) > 0.01f)
        {
            OnUpdateScreenRect?.Invoke(screenRect);
            cashedWidth = width;
            
            infoStyle = new GUIStyle(GUI.skin.label);
            infoStyle.alignment = TextAnchor.MiddleLeft; 
        }
    }

    private void ClearValues()
    {
        cashedTime = EditorApplication.timeSinceStartup;
        cashedWidth = 0f;
    }

    #endregion

    #region AssignElementsEvents

    private void AssignEventsInElements()
    {
        player1.AssignEvents(ref OnEnableEvent, ref OnUpdateEvent, ref OnUpdateScreenRect, ref OnRestart, ref OnGUIEvent);
        player2.AssignEvents(ref OnEnableEvent, ref OnUpdateEvent, ref OnUpdateScreenRect, ref OnRestart, ref OnGUIEvent);
        ball.AssignEvents(ref OnEnableEvent, ref OnUpdateEvent, ref OnUpdateScreenRect, ref OnRestart, ref OnGUIEvent);
        scoreText.AssignEvents(ref OnEnableEvent, ref OnUpdateEvent, ref OnUpdateScreenRect, ref OnRestart, ref OnScoreChanged);
    }  

    #endregion

    #region Input

    private void CheckInput(Event currentEvent)
    {
        if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == RestartKey)
            OnRestart?.Invoke();
    }

    #endregion

}
