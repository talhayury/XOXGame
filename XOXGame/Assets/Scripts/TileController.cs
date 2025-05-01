using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour, IPointerDownHandler
{

    public TileState MyState{get;set;}
    [SerializeField] private SpriteRenderer _mySpriteRenderer;

    [SerializeField] private Sprite _xSprite;
    [SerializeField] private Sprite _oSprite;
    [SerializeField] private Color _xColor;
    [SerializeField] private Color _yColor;
    
    public Vector2 coordinate ;
    public void OnPointerDown(PointerEventData eventData)
    {
        if(MyState != TileState.None){
            return;
        }


        if(!GameManager.Instance.IsStartGame)
            GameManager.Instance.OnStartGame();


        var state = GameManager.Instance.Turn%2==0? TileState.X : TileState.O;
        SetState(state);
        GameManager.Instance.Turn++;

        var result = GameManager.Instance.HasWinner();
        var hasWinner = result.Item1;

        if(hasWinner){
            Debug.Log($"Winner => {result.Item2}");
            GameManager.Instance.OnGameOver(result.Item2);
        }else{
            if(!GameManager.Instance.HasNoneTile())
            GameManager.Instance.OnGameOver(TileState.None);
        }
    }

    public void SetState(TileState state){
        MyState =state;
        _mySpriteRenderer.color = state==TileState.X? _xColor : _yColor;
        _mySpriteRenderer.sprite = state==TileState.X? _xSprite : _oSprite;

        if(state == TileState.None) _mySpriteRenderer.sprite = null;
    }

    public TileController GetNextTile(Direction direction){
        var nextTileCoordinate = coordinate;
        switch (direction){
            case Direction.Up:
                nextTileCoordinate.y++;
                break;
            case Direction.UpRigt:
                nextTileCoordinate.y++;
                nextTileCoordinate.x++;
                break;
            case Direction.Right:
                nextTileCoordinate.x++;
                break;
            case Direction.DownRight:
                nextTileCoordinate.y--;
                nextTileCoordinate.x++;
                break;
            case Direction.Down:
                nextTileCoordinate.y--;
                break;
            case Direction.LeftDown:
                nextTileCoordinate.y--;
                nextTileCoordinate.x--;
                break;
            case Direction.Left:
                nextTileCoordinate.x--;
                break;
            case Direction.UpLeft:
                nextTileCoordinate.y++;
                nextTileCoordinate.x--;
                break;
        }
        return GameManager.Instance.ListTileController.Find(tile=> tile.coordinate==nextTileCoordinate);
    }
}


public enum TileState{
    None,
    X,
    O
}

public enum Direction{
    Up,
    UpRigt,
    Right,
    DownRight,
    Down,
    LeftDown,
    Left,
    UpLeft,

    
}
    

