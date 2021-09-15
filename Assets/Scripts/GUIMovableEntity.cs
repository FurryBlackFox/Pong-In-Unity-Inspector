using System;
using UnityEditor;
using UnityEngine;

public abstract class GUIMovableEntity : GUIEntity
{
    #region Variables

    protected float relativeSpeed;
    protected float speed;

    public float LeftBorder => Position.x;
    public float RightBorder => Position.x + size.x;
    public float TopBorder => Position.y;
    public float BottomBorder => Position.y + size.y;

    protected Vector2 offsetPosition = Vector2.zero;
    protected Vector2 Position => InitPosition + offsetPosition;

    #endregion

    #region Constructor

    protected GUIMovableEntity(Vector2 newRelativeSize, float newRelativeSpeed) : base(newRelativeSize)
    {
        relativeSpeed = newRelativeSpeed;
    }

    #endregion

    #region Events

    public void AssignEvents(ref Action onEnableEvent, ref Action<float> onUpdateEvent, ref Action<Rect> 
        onScreenSizeUpdateEvent, ref Action onRestart, ref Action<Event> onGUIEvent)
    {
        base.AssignEvents(ref onEnableEvent, ref onUpdateEvent, ref onScreenSizeUpdateEvent, ref onRestart);
        onGUIEvent += OnGUIEventHandler;
    }

    protected override void OnScreenSizeUpdateHandler(Rect newScreenRect)
    {
        base.OnScreenSizeUpdateHandler(newScreenRect);
        
        RecalculateInitPosition();
        RecalculateSize();
        RecalculateSpeed();
    }

    protected abstract void OnGUIEventHandler(Event currEvent);

    protected abstract void CheckForCollision();
    
    #endregion

    #region Movement

    protected void Move(Vector2 direction, float deltaTime)
    {
        var distance = speed * deltaTime;
        offsetPosition += direction * distance;
        CheckForCollision();
    }


    #endregion

    #region Clear

    public void ClearOffset()
    {
        offsetPosition = Vector2.zero;
    }

    #endregion
    
    #region Draw

    public override void DrawItself()
    {
        drawRect.position = Position;
        drawRect.size = size;
        EditorGUI.DrawRect( drawRect, drawColor);
    }

    #endregion

    #region Calculations

    protected void RecalculateSpeed()
    {
        speed = relativeSpeed * screenRect.width;
    }

    #endregion
    
    

    
  


  

  
 
    

  

}
