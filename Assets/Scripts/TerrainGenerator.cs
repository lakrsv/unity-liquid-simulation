using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//Is now a mess, clean up.
public class TerrainGenerator : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right}
    TileMap _tileMap = new TileMap();
    private float _tileSize = 0.16f;
    public float TileSize { get { return _tileSize; } }
    [SerializeField]
    private int _width = 100;
    [SerializeField]
    private int _height = 100;

    public int MapWidth { get { return _width; } }
    public int MapHeight { get { return _height; } }

    private GameObject[,] _tiles;
    public GameObject[,] Tiles
    {
        get
        {
            return _tiles;
        }
        set
        {
            Tiles = value;
        }
    }

    private Dictionary<GameObject, Tile> _tileDict = new Dictionary<GameObject, Tile>();
    public Dictionary<GameObject, Tile> TileDict { get { return _tileDict; } }
    private Dictionary<Vector2, Tile> _tilePosDict = new Dictionary<Vector2, Tile>();
    public Dictionary<Vector2, Tile> TilePosDict { get { return _tilePosDict; } }

    private Dictionary<Vector2, Tile> _tileWorldPosDict = new Dictionary<Vector2, Tile>();
    public Dictionary<Vector2, Tile> TileWorldPosDict { get { return _tileWorldPosDict; } }

    private GameObject _chunkholder;
    private Dictionary<Vector2, GameObject> _chunks = new Dictionary<Vector2, GameObject>();
    private GameObject _currentChunk;

    private int _chunkWidth = 8, _chunkHeight = 8;

    //[SerializeField]
    //private GameObject _solid;
    //[SerializeField]
    //private GameObject _empty;
    //[SerializeField]
    //private GameObject _liquid;

    [SerializeField]
    private GameObject _tile;

    public Sprite WaterSprite, OilSprite, DirtSprite;

    private bool _cameraPosSet = false;

    public static int[,] MapLayout;

    public static TerrainGenerator Instance;

    public Vector2 CollOffset = new Vector2(0.5f, 0.5f);

	// Use this for initialization
	void Start ()
    {
        if(Instance == null) { Instance = this; }

        _chunkholder = new GameObject();
        _chunkholder.name = "Level";
        int[,] layout = _tileMap.GetLayout(_width, _height);
        MapLayout = layout;
        _tiles = new GameObject[_width, _height];
        for(int i = 0; i < _width; i++)
        {
            for(int k = _height-1; k > 0; k--)
            {
               //Solid
                if(layout[i,k] == 1)
                {
                    GameObject tile = (GameObject)Instantiate(_tile, new Vector2(i-Mathf.RoundToInt(_width/2), k-Mathf.RoundToInt(_height/2)), Quaternion.identity);
                    _tiles[i, k] = tile;
                    Tile tileScript = tile.GetComponent<Tile>();

                    var coll = tile.GetComponent<BoxCollider2D>();
                    coll.enabled = true;

                    tileScript.Initialize(Tile.TileType.Solid, Tile.SubType.Dirt, new Vector2(i, k));
                    _tileDict.Add(tile, tileScript);
                    _tilePosDict.Add(tileScript.GridPos, tileScript);
                    _tileWorldPosDict.Add(tile.transform.position, tileScript);
                    
                    if (!_cameraPosSet && i == _width / 2)
                    {
                        _cameraPosSet = true;
                        Camera.main.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y+6, Camera.main.transform.position.z);
                    }
                }
                //Empty
                else
                {
                    GameObject tile = (GameObject)Instantiate(_tile, new Vector2(i - Mathf.RoundToInt(_width / 2), k - Mathf.RoundToInt(_height / 2)), Quaternion.identity);
                    _tiles[i, k] = tile;

                    Tile tileScript = tile.AddComponent<Tile>();

                    tileScript.Initialize(Tile.TileType.Air, Tile.SubType.Air, new Vector2(i, k));
                    _tileDict.Add(tile, tileScript);
                    _tilePosDict.Add(tileScript.GridPos, tileScript);
                    _tileWorldPosDict.Add(tile.transform.position, tileScript);
                }

                _tiles[i, k].name = string.Format("Tile: {0}.{1}", i, k);
                AddToChunk(_tiles[i, k], i, k);
            }
        }

	}

    private Vector2 posVector = new Vector2();
    void AddToChunk(GameObject tile, int xPos, int yPos)
    {

        double x = Math.Round((double)(xPos/_chunkWidth), 0, MidpointRounding.AwayFromZero);
        double y = Math.Round((double)(yPos/_chunkHeight), 0, MidpointRounding.AwayFromZero);

        posVector.Set(Mathf.RoundToInt((float)x), Mathf.RoundToInt((float)y));
        if (_chunks.ContainsKey(posVector))
        {
            tile.transform.SetParent(_chunks[posVector].transform.GetChild(0), true);
        }
        else
        {
            GameObject chunk = new GameObject();
            GameObject chunkRender = new GameObject();
            chunkRender.transform.SetParent(chunk.transform);
            chunkRender.name = "Chunk Render";
            BoxCollider2D chunkcoll = chunk.AddComponent<BoxCollider2D>();
            chunkcoll.size = new Vector2(_chunkWidth, _chunkHeight);
            chunkcoll.isTrigger = true;

            chunk.name = string.Format("Chunk: {0}.{1}", posVector.x, posVector.y);
            _chunks.Add(posVector, chunk);
            chunk.transform.position = new Vector2(tile.transform.position.x + _chunkWidth / 2, tile.transform.position.y+1 - _chunkHeight / 2);
            tile.transform.SetParent(_chunks[posVector].transform.GetChild(0), true);

            chunk.AddComponent<Chunk>();
            chunk.transform.SetParent(_chunkholder.transform, true);

        }
    }

    private Vector2 _neighbourPos = new Vector2();
    public Tile GetNeighbourTile(Tile tile, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                _neighbourPos.Set(tile.GridPos.x, tile.GridPos.y + 1);
                break;
            case Direction.Down:
                _neighbourPos.Set(tile.GridPos.x, tile.GridPos.y - 1);
                break;
            case Direction.Left:
                _neighbourPos.Set(tile.GridPos.x - 1, tile.GridPos.y);
                break;
            case Direction.Right:
                _neighbourPos.Set(tile.GridPos.x + 1, tile.GridPos.y);
                break;
            default:
                return null;
        }

        return _tilePosDict.ContainsKey(_neighbourPos) ? _tilePosDict[_neighbourPos] : null;
    }
}
