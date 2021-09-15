using UnityEngine;

public enum PlayerSide
{
    Left,
    Right
}

public class GUIPlayer : GUIMovableEntity
{
    
    #region Variables

    public int score = 0;
    
    private const float PlayerRelOffset = 0.01f;

    private readonly PlayerSide playerSide;
    private KeyCode upKeyCode;
    private KeyCode downKeyCode;
    private int input;

    #endregion

    #region Constructor

    public GUIPlayer(PlayerSide newPlayerSide , KeyCode newUpKeyCode, KeyCode 
        newDownKeyCode) : base(new Vector2(0.02f, 0.2f), 0.2f)
    {
        playerSide = newPlayerSide;
        upKeyCode = newUpKeyCode;
        downKeyCode = newDownKeyCode;
    }

    #endregion

    #region Events

    protected override void OnEnableHandler()
    {
    }

    protected override void OnUpdateHandler(float deltaTime)
    {
        Move(Vector2.up * input, deltaTime);
    }

    protected override void OnGUIEventHandler(Event currEvent)
    {
        GetInput(currEvent);
    }

    protected override void OnRestartHandler()
    {
        ClearOffset();
        score = 0;
    }

    #endregion

    #region Draw

    public void DrawInfo(GUIStyle textStyle, int bigFontSize, int smallFontSize)
    {
        using (new GUILayout.VerticalScope())
        {
            textStyle.fontSize = bigFontSize;
            GUILayout.Label($"{playerSide} player", textStyle);

            textStyle.fontSize = smallFontSize;
            GUILayout.Label($"Up - {upKeyCode} \nDown - {downKeyCode}", textStyle);
        }
    }

    #endregion
    
    #region Calculations

    protected override void RecalculateInitPosition()
    {
        var borderOffset = screenRect.width * PlayerRelOffset;
        var halfSizeOffset = size.x * 0.5f;
        var xPos = playerSide == PlayerSide.Left
            ? screenRect.x + borderOffset + halfSizeOffset
            : screenRect.width - borderOffset - halfSizeOffset;
        InitPosition = new Vector2(xPos, screenRect.center.y);
    }

    protected override void RecalculateSize()
    {
        size.x = relativeSize.x * screenRect.width;
        size.y = relativeSize.y * screenRect.height;
    }

    #endregion

    #region Collision

    protected override void CheckForCollision()
    {
        var halfPlayerSize = size * 0.5f;
        var halfScreenSize = screenRect.size * 0.5f;

        var targetPos = halfScreenSize.y - halfPlayerSize.y - screenRect.x - PlayerRelOffset;
        
        if (Mathf.Abs(offsetPosition.y) >= targetPos)
        {
            var sign = Mathf.Sign(offsetPosition.y);
            offsetPosition.y = sign * targetPos;
        }
    }

    #endregion
    
    #region Input

    private void GetInput(Event currentEvent)
    {
        if (currentEvent.type == EventType.KeyDown)
        {
            if (currentEvent.keyCode == upKeyCode)
                input = -1;
            else if (Event.current.keyCode == downKeyCode)
                input = 1;
        }
        else if (currentEvent.type == EventType.KeyUp)
            if (Event.current.keyCode == upKeyCode || Event.current.keyCode == downKeyCode)
                input = 0;
    }

    #endregion
    
}
