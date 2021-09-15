using System;
using UnityEngine;

public class GUIText : GUIEntity
{
    
    #region Variables
    
    
    private const float topRelOffset = 0.1f;
    protected string outputText;
    private string initText;

    private GUIStyle outputStyle;
  

    #endregion

    #region Constructor

    public GUIText(Vector2 newRelativeSize, string newInitText) : base(newRelativeSize)
    {
        initText = newInitText;
        outputText = newInitText;
    }

    #endregion

    #region Events

    public void AssignEvents(ref Action onEnableEvent, ref Action<float> onUpdateEvent, ref Action<Rect> 
        onScreenSizeUpdateEvent, ref Action onRestart, ref Action<string> onScoreChanged)
    {
        base.AssignEvents(ref onEnableEvent, ref onUpdateEvent, ref onScreenSizeUpdateEvent, ref onRestart);
        onScoreChanged += OnScoreChangedHandler;
    }

    protected override void OnEnableHandler()
    {
        RecalculateSize();
        RecalculateInitPosition();
    }

    protected override void OnScreenSizeUpdateHandler(Rect newScreenRect)
    {
        base.OnScreenSizeUpdateHandler(newScreenRect);
        UpdateOutputStyle();
    }

    protected override void OnUpdateHandler(float deltaTime)
    {
        
    }
    
    
    private void OnScoreChangedHandler(string newScoreString)
    {
        outputText = newScoreString;
    }

    protected override void OnRestartHandler()
    {
        outputText = initText;
    }
    
    #endregion

    #region Draw
            
    public override void DrawItself()
    {
        drawRect.position = InitPosition;
        drawRect.size = size;
        
        var cashedBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.blue;
        
        var cashedColor = GUI.color;
        GUI.color = Color.white;        
        
        GUI.TextArea(new Rect(drawRect), outputText, outputStyle);
        
        GUI.color = cashedColor;            
        GUI.backgroundColor = cashedBackgroundColor;
    }

    #endregion

    #region Calculations

    protected override void RecalculateSize()
    {
        size.x = relativeSize.x * screenRect.width;
        size.y = relativeSize.y * screenRect.height;
    }

    protected override void RecalculateInitPosition()
    {
        InitPosition = new Vector2(screenRect.center.x, screenRect.y + screenRect.height * topRelOffset);     
    }

    protected void UpdateOutputStyle()
    {
        outputStyle = new GUIStyle(GUI.skin.label);
        outputStyle.alignment = TextAnchor.MiddleCenter;
        outputStyle.fontSize = (int)(size.y * 0.5f);
    }

    #endregion
    
}
