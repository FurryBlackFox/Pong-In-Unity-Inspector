using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GUIBall : GUIMovableEntity
{

    #region Variables

    public event Action<PlayerSide> OnHitOnPlayersGates;
    protected Vector2 currentDirection;

    private const float MaxStartRandomAngle = 35f;
    private const float MaxRicochetRandomAngle = 10f;

    private float contactOffset = 2.5f;

    public GUIPlayer player1, player2;

    #endregion

    #region Constructor

    public GUIBall() : base(new Vector2(0.03f, 0.03f), 0.2f)
    {
    }

    #endregion

    #region Events

    protected override void OnEnableHandler()
    {
        RecalculateDirection();
    }

    protected override void OnUpdateHandler(float deltaTime)
    {
        Move(currentDirection, deltaTime);
    }

    protected override void OnGUIEventHandler(Event currEvent)
    {
    }

    protected override void OnRestartHandler()
    {
        ClearOffset();
        RecalculateDirection();
    }

    #endregion

    #region Calculations

    protected override void RecalculateInitPosition()
    {
        InitPosition = screenRect.center;
    }



    public void RecalculateDirection()
    {
        var randomDirection = Random.value > 0.5f ? Vector2.right : Vector2.left;    
        var randomAngle = Random.Range(-MaxStartRandomAngle, MaxStartRandomAngle);
        currentDirection = Quaternion.Euler(0, 0, randomAngle) * randomDirection;
    }

    protected override void RecalculateSize()
    {
        size.x = relativeSize.x * screenRect.width;
        size.y = relativeSize.y * screenRect.width;
    }

    private void ModifyDirection()
    {
        var randomAngle = Random.Range(-MaxRicochetRandomAngle, MaxRicochetRandomAngle);
        currentDirection = Quaternion.Euler(0, 0, randomAngle) * currentDirection;
    }


    #endregion

    #region Collision

    protected override void CheckForCollision()
    {
        var halfScreenSize = screenRect.size * 0.5f;
        var halfBallSize = size * 0.5f;
        

        if (Mathf.Abs(offsetPosition.x) + halfBallSize.x >= halfScreenSize.x)
        {
            var sign = Mathf.Sign(offsetPosition.x);
            offsetPosition.x = sign * (halfScreenSize.x - halfBallSize.x - contactOffset);

            var playerType = (int)sign == -1 ? PlayerSide.Left : PlayerSide.Right;
            OnHitOnPlayersGates?.Invoke(playerType);
        }
                
        if (Mathf.Abs(offsetPosition.y) + halfBallSize.y >= halfScreenSize.y)
        {
            var sign = Mathf.Sign(offsetPosition.y);
            offsetPosition.y = sign * (halfScreenSize.y - halfBallSize.y - contactOffset);
            currentDirection.y *= -1;
            ModifyDirection();
        }


        if (offsetPosition.x < 0)
        {
            if(LeftBorder <= player1.RightBorder)
                if (BottomBorder >= player1.TopBorder && TopBorder <= player1.BottomBorder)
                {
                    offsetPosition.x -= player1.RightBorder - LeftBorder - contactOffset;
                    currentDirection.x *= -1;
                    ModifyDirection();
                }
        }
        else
        {
            if (RightBorder >= player2.LeftBorder)
                if (BottomBorder >= player2.TopBorder && TopBorder <= player2.BottomBorder)
                {
                    offsetPosition.x -=  RightBorder - player2.LeftBorder + contactOffset;
                    currentDirection.x *= -1;
                    ModifyDirection();
                }
        }
    }

    #endregion
    
   
    
 

  



}
