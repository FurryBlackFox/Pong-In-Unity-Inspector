using System;
using UnityEngine;

public abstract class GUIEntity
{

    #region Variables

    protected Vector2 relativeSize;
    protected Vector2 size = new Vector2();
    
    private Vector2 _initPosition;
    protected Vector2 InitPosition
    {
        get => _initPosition;
        set => _initPosition = value - size * 0.5f;
    }
    
    protected Color drawColor = Color.white;
    protected Rect drawRect = new Rect();

    protected Rect screenRect;

    protected GUIEntity(Vector2 newRelativeSize)
    {
        relativeSize = newRelativeSize;
    }

    #endregion

    #region Events

    protected void AssignEvents(ref Action onEnableEvent, ref Action<float> onUpdateEvent, ref Action<Rect> 
        onScreenSizeUpdateEvent, ref Action onRestart)
    {
        onEnableEvent += OnEnableHandler;
        onUpdateEvent += OnUpdateHandler;
        onScreenSizeUpdateEvent += OnScreenSizeUpdateHandler;
        onRestart += OnRestartHandler;
    }
    
    protected virtual void OnScreenSizeUpdateHandler(Rect newScreenRect)
    {
        screenRect = newScreenRect;
        RecalculateSize();
        RecalculateInitPosition();
    }
    
    protected abstract void OnEnableHandler();

    protected abstract void OnUpdateHandler(float deltaTime);

    protected abstract void OnRestartHandler();
    
    #endregion

    #region Draw

    public abstract void DrawItself();

    #endregion

    #region Calculations

    protected abstract void RecalculateSize();
    
    protected abstract void RecalculateInitPosition();

    #endregion

    
}
