using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PressButton : MonoBehaviour
{
    public Vector3Int[] positionArray;
    public TileBase[] tileArrayOn;
    public TileBase[] tileArrayOff;
    public Tilemap tileMap;
    public Sprite pressedSprite;

    private Sprite _defaultSprite;
    private TileBase[] nullTiles;
    private SpriteRenderer _renderer;

    // Start is called before the first frame 
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _renderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ThrowableLadder")
        {
            _renderer.sprite = pressedSprite;
            tileMap.SetTiles(positionArray, tileArrayOn);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ThrowableLadder")
        {
            _renderer.sprite = _defaultSprite;
            tileMap.SetTiles(positionArray, tileArrayOff);
        }
    }
}
