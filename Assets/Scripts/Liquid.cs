//using UnityEngine;
//using System.Collections;
//using System;
//using System.Collections.Generic;
//public class Liquid : MonoBehaviour
//{
//    private List<GameObject> _liquidCells = new List<GameObject>();
//    TerrainGenerator _terrainGen;

//    private float _cellSize = 0.04f;
//    private int _mapMod;

//    public List<Tile> ActiveFluidTiles = new List<Tile>();
//    public List<Tile> IdleFluidTiles = new List<Tile>();

//    [SerializeField]
//    private GameObject _liquidParticle;

//    private bool _checkFluids = true;

//    //Transparency level change, Level 1-8
//    //Multiplier is 31.875

//    [Flags]
//    enum FlowDirection
//    {
//        None,
//        Right,
//        Left,
//        Both = Left | Right,
//    }

//    public enum CellMatrix
//    {
//        None = 0,
//        Down = 1,
//        Left = 2,
//        Right = 4,
//        RightLeft = Left | Right,
//        All = Down | Left | Right,
//    }

//    public int[,] tileLiquidGrid = new int[8, 8];

//    void Start()
//    {
//        _terrainGen = GameObject.Find("TerrainGenerator").GetComponent<TerrainGenerator>();
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.LeftShift))
//        {
//            AddLiquid(_terrainGen.TileDict[_terrainGen.Tiles[21, 133]]);
//        }
//    }

//    public void StartCalculate(Tile tile)
//    {
//        CellMatrix matrix = GetMatrix(tile);
//        Debug.Log("Tile: " + tile.name + " reports direction: " + matrix);
//        StartCoroutine(MoveLiquid(tile, matrix));
//    }

//    private List<Tile> _toRemove = new List<Tile>();
//    private List<Tile> _toAdd = new List<Tile>();

//    public CellMatrix GetMatrix(Tile tile)
//    {
//        if (tile == null) { return CellMatrix.None; }
//        bool down = false, right = false, left = false;
//        //Check Down.
//        down = _terrainGen.TileDict[_terrainGen.Tiles[Mathf.RoundToInt(tile.GridPos.x), Mathf.RoundToInt(tile.GridPos.y - 1)]].TType == Tile.TileType.Air;
//        //Check Right
//        right = _terrainGen.TileDict[_terrainGen.Tiles[Mathf.RoundToInt(tile.GridPos.x + 1), Mathf.RoundToInt(tile.GridPos.y)]].TType == Tile.TileType.Air;
//        //Check Left
//        left = _terrainGen.TileDict[_terrainGen.Tiles[Mathf.RoundToInt(tile.GridPos.x - 1), Mathf.RoundToInt(tile.GridPos.y)]].TType == Tile.TileType.Air;

//        if (down && right && left)
//        {
//            //return CellMatrix.All;
//            return CellMatrix.Down;
//        }
//        else if (right && left)
//        {
//            return CellMatrix.RightLeft;
//            //return CellMatrix.Right;
//        }
//        else if (down)
//        {
//            return CellMatrix.Down;
//        }
//        else if (left)
//        {
//            return CellMatrix.Left;
//        }
//        else if (right)
//        {
//            return CellMatrix.Right;
//        }
//        else
//        {
//            return CellMatrix.None;
//        }
//    }

//    public void AddLiquid(Tile tile)
//    {
//        for (int i = 0; i < 8; i++)
//        {
//            for (int k = 0; k < 8; k++)
//            {
//                tile.FluidGridLevel[i, k] = 8;
//                tile.FluidParticles[i, k] = (GameObject)Instantiate(_liquidParticle, new Vector2(tile.gameObject.transform.position.x + i * 0.125f, tile.gameObject.transform.position.y + k * 0.125f), Quaternion.identity);
//            }
//        }
//        tile.Level = 512;
//        ActiveFluidTiles.Add(tile);
//        tile.Calculate = true;
//        StartCalculate(tile);
//    }

//    Vector2 moveVector = new Vector3();
//    Tile lastNeighbour;
//    IEnumerator MoveLiquid(Tile tile, CellMatrix direction)
//    {
//        if (tile == null) { yield break; }
//        if (tile.Level == 0) { tile.Calculate = false; yield break; }

//        //Vertical
//        for (int k = 0; k < 8; k++)
//        {
//            //Horizontal
//            for (int i = 0; i < 8; i++)
//            {
//                if (tile.FluidParticles[i, k] == null) { continue; }

//                //Transfer to new tile.
//                if (k == 0 && direction == CellMatrix.Down)
//                {
//                    var particle = tile.FluidParticles[i, k];
//                    moveVector.Set(particle.transform.position.x, particle.transform.position.y - 0.125f);
//                    particle.transform.position = moveVector;

//                    Tile neighbour = GetNeighbour(tile, CellMatrix.Down);
//                    lastNeighbour = neighbour;
//                    neighbour.FluidParticles[i, 7] = tile.FluidParticles[i, k];
//                    neighbour.FluidGridLevel[i, 7] += tile.FluidGridLevel[i, k];
//                    neighbour.Level += tile.FluidGridLevel[i, k];
//                    tile.Level -= tile.FluidGridLevel[i, k];
//                    tile.FluidGridLevel[i, k] = 0;

//                    tile.FluidParticles[i, k] = null;

//                    var theneighbour = lastNeighbour;
//                    if (theneighbour != null && !theneighbour.Calculate)
//                    {
//                        StartCalculate(theneighbour);
//                        theneighbour.Calculate = true;
//                        lastNeighbour = null;
//                    }
//                }
//                //Move within tile
//                else if (k > 0 && direction == CellMatrix.Down)
//                {
//                    //if (tile.FluidParticles[i, k - 1] != null) { continue; }
//                    var particle = tile.FluidParticles[i, k];
//                    moveVector.Set(particle.transform.position.x, particle.transform.position.y - 0.125f);
//                    particle.transform.position = moveVector;

//                    tile.FluidParticles[i, k - 1] = tile.FluidParticles[i, k];
//                    tile.FluidGridLevel[i, k - 1] += tile.FluidGridLevel[i, k];
//                    tile.FluidParticles[i, k] = null;
//                    tile.FluidGridLevel[i, k] = 0;
//                }
//                else if (direction == CellMatrix.None)
//                {
//                    if(k > 0)
//                    {
//                        tile.Settled = true;
//                        if(tile.FluidParticles[i, k-1] == null)
//                        {
//                            tile.Settled = false;

//                            var particle = tile.FluidParticles[i, k];
//                            moveVector.Set(particle.transform.position.x, particle.transform.position.y - 0.125f);
//                            particle.transform.position = moveVector;

//                            tile.FluidParticles[i, k - 1] = tile.FluidParticles[i, k];
//                            tile.FluidGridLevel[i, k - 1] += tile.FluidGridLevel[i, k];
//                            tile.FluidParticles[i, k] = null;
//                            tile.FluidGridLevel[i, k] = 0;
//                        }
//                    }
//                }
//                else if(direction == CellMatrix.Left)
//                {
//                    if(k == 0 || tile.FluidParticles[i, k-1] != null)
//                    {
//                        //Move to the left.
//                        if(i == 0)
//                        {
//                            //Transfer to new tile
//                            var particle = tile.FluidParticles[i, k];
//                            moveVector.Set(particle.transform.position.x - 0.125f, particle.transform.position.y);
//                            particle.transform.position = moveVector;

//                            Tile neighbour = GetNeighbour(tile, CellMatrix.Left);
//                            lastNeighbour = neighbour;
//                            neighbour.FluidParticles[7, k] = tile.FluidParticles[i, k];
//                            neighbour.FluidGridLevel[7, k] += tile.FluidGridLevel[i, k];
//                            neighbour.Level += tile.FluidGridLevel[i, k];
//                            tile.Level -= tile.FluidGridLevel[i, k];
//                            tile.FluidGridLevel[i, k] = 0;

//                            tile.FluidParticles[i, k] = null;

//                            var theneighbour = lastNeighbour;
//                            if (theneighbour != null && !lastNeighbour.Calculate)
//                            {
//                                StartCalculate(theneighbour);
//                                theneighbour.Calculate = true;
//                                lastNeighbour = null;
//                            }
//                        }
//                        else 
//                        {
//                            //Move within tile
//                            var particle = tile.FluidParticles[i, k];
//                            moveVector.Set(particle.transform.position.x - 0.125f, particle.transform.position.y);
//                            particle.transform.position = moveVector;

//                            tile.FluidParticles[i-1, k] = tile.FluidParticles[i, k];
//                            tile.FluidGridLevel[i-1, k] += tile.FluidGridLevel[i, k];
//                            tile.FluidParticles[i, k] = null;
//                            tile.FluidGridLevel[i, k] = 0;
//                        }
//                    }
//                    else
//                    {
//                        //Else move down.
//                        var particle = tile.FluidParticles[i, k];
//                        moveVector.Set(particle.transform.position.x, particle.transform.position.y - 0.125f);
//                        particle.transform.position = moveVector;

//                        tile.FluidParticles[i, k - 1] = tile.FluidParticles[i, k];
//                        tile.FluidGridLevel[i, k - 1] += tile.FluidGridLevel[i, k];
//                        tile.FluidParticles[i, k] = null;
//                        tile.FluidGridLevel[i, k] = 0;
//                    }
//                }
//                else if(direction == CellMatrix.Right)
//                {
//                    if(k == 0 || tile.FluidParticles[i, k-1] != null)
//                    {
//                        //Move to the right.
//                        if(i == 7)
//                        {
//                            //Transfer to new tile
//                            var particle = tile.FluidParticles[i, k];
//                            moveVector.Set(particle.transform.position.x + 0.125f, particle.transform.position.y);
//                            particle.transform.position = moveVector;

//                            Tile neighbour = GetNeighbour(tile, CellMatrix.Right);
//                            lastNeighbour = neighbour;
//                            neighbour.FluidParticles[0, k] = tile.FluidParticles[i, k];
//                            neighbour.FluidGridLevel[0, k] += tile.FluidGridLevel[i, k];
//                            neighbour.Level += tile.FluidGridLevel[i, k];
//                            tile.Level -= tile.FluidGridLevel[i, k];
//                            tile.FluidGridLevel[i, k] = 0;

//                            tile.FluidParticles[i, k] = null;

//                            var theneighbour = lastNeighbour;
//                            if (theneighbour != null && !theneighbour.Calculate)
//                            {
//                                StartCalculate(theneighbour);
//                                theneighbour.Calculate = true;
//                                lastNeighbour = null;
//                            }
//                        }
//                        else
//                        {
//                            //Move within tile.
//                            //Move within tile
//                            var particle = tile.FluidParticles[i, k];
//                            moveVector.Set(particle.transform.position.x + 0.125f, particle.transform.position.y);
//                            particle.transform.position = moveVector;

//                            tile.FluidParticles[i + 1, k] = tile.FluidParticles[i, k];
//                            tile.FluidGridLevel[i + 1, k] += tile.FluidGridLevel[i, k];
//                            tile.FluidParticles[i, k] = null;
//                            tile.FluidGridLevel[i, k] = 0;
//                        }
//                    }
//                    else
//                    {
//                        //Else move down.
//                        var particle = tile.FluidParticles[i, k];
//                        moveVector.Set(particle.transform.position.x, particle.transform.position.y - 0.125f);
//                        particle.transform.position = moveVector;

//                        tile.FluidParticles[i, k - 1] = tile.FluidParticles[i, k];
//                        tile.FluidGridLevel[i, k - 1] += tile.FluidGridLevel[i, k];
//                        tile.FluidParticles[i, k] = null;
//                        tile.FluidGridLevel[i, k] = 0;
//                    }
//                }
//                else if(direction == CellMatrix.RightLeft)
//                {

//                }
//            }
//            yield return new WaitForSeconds(1 / 60f);
//        }

//        if (tile.Level > 0 && !tile.Settled)
//        {
//            StartCalculate(tile);
//            tile.Calculate = true;
//        }
//        else
//        {
//            Debug.Log("Dont calculate!");
//            tile.Calculate = false;
//        }
//    }

//    private Tile GetNeighbour(Tile tile, CellMatrix matrix)
//    {
//        switch (matrix)
//        {
//            case CellMatrix.Down:
//                return _terrainGen.TileDict[_terrainGen.Tiles[Mathf.RoundToInt(tile.GridPos.x), Mathf.RoundToInt(tile.GridPos.y - 1)]];
//            case CellMatrix.Left:
//                return _terrainGen.TileDict[_terrainGen.Tiles[Mathf.RoundToInt(tile.GridPos.x - 1), Mathf.RoundToInt(tile.GridPos.y)]];
//            case CellMatrix.Right:
//                return _terrainGen.TileDict[_terrainGen.Tiles[Mathf.RoundToInt(tile.GridPos.x + 1), Mathf.RoundToInt(tile.GridPos.y)]];
//            default:
//                return null;
//        }
//    }
//    void RenderUpdates()
//    {

//    }
//}
