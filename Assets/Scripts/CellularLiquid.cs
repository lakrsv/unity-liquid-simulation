using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class CellularLiquid : MonoBehaviour
{
    public const float MaxMass = 1.0f;
    private const float MaxCompress = 0.02f;
    public const float Minmass = 0.0001f;
    private const float maxspeed = 10.0f;
    private const float minflow = 0.1f;
    public const float mindraw = 0.01f;
    private const float maxdraw = 1.1f;
    //public List<Tile> LiquidTiles = new List<Tile>();
    public Dictionary<int, Tile> LiquidTiles = new Dictionary<int, Tile>();
    List<Tile> _toRemove = new List<Tile>();

    private TerrainGenerator _terrainGen;

    void Start()
    {
        _terrainGen = GameObject.Find("TerrainGenerator").GetComponent<TerrainGenerator>();
        //StartCoroutine(SimulateLiquids());
    }

    bool simulate = true;
    bool liquidSettled = false;
    void Update()
    {
        if (simulate)
            SimulateLiquids();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            simulate = !simulate;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log(LiquidTiles.Count);
        }
    }
    float timesince;

    Dictionary<int, Tile> _liquidToCheck = new Dictionary<int, Tile>();
    public void AddLiquid(Tile tile, Tile.SubType type)
    {
        tile.ChangeTileType(Tile.TileType.Liquid, type);
        tile.CurrentVolume = 0;
        tile.NewVolume = 1;
        tile.SaveVolume(_terrainGen, this);
    }

    Tile.SubType liquidTypeSimulated = Tile.SubType.None;
    void SimulateLiquids()
    {
        if (LiquidTiles.Count == 0) { return; }

        var dictToList = LiquidTiles.Values.ToList();
        for (int i = 0; i < dictToList.Count; i++)
        {
            Tile liquid = dictToList[i];

            if (!VolumeChanged(liquid.CurrentVolume, liquid.NewVolume))
            {
                if (liquid.NoChangeCounter > 99)
                {
                    liquid.Settled = true;
                }
                else
                {
                    liquid.NoChangeCounter++;
                }
            }
            else
            {
                liquid.NoChangeCounter = 0;
                liquid.Settled = false;
            }

            if (liquid.Settled) continue;

            float flow = 0;
            float remainingmass = liquid.CurrentVolume;
            if (remainingmass <= 0) { continue; }

            //Below
            //tilePos.Set(liquid.GridPos.x, liquid.GridPos.y - 1);
            Tile downneighbour = _terrainGen.GetNeighbourTile(liquid, TerrainGenerator.Direction.Down);
            if(downneighbour == null) { continue; }

            if (downneighbour.TType != Tile.TileType.Solid)
            {
                if (liquid.NoChangeCounter == 0)
                {
                    downneighbour.NoChangeCounter = 0;
                    downneighbour.Settled = false;
                }

                ////Stop different liquids from interacting?
                //if (downneighbour.TType == Tile.TileType.Liquid && downneighbour.SType != liquid.SType) continue;
                flow = getstable(remainingmass + downneighbour.CurrentVolume) - downneighbour.CurrentVolume;
                if (flow > minflow) { flow *= 0.5f; }

                flow = Mathf.Clamp(flow, 0, Math.Min(maxspeed, remainingmass));

                //if (flow == 0) { noflowcount++; } else { downneighbour.Settled = false; }

                liquid.NewVolume -= flow;
                downneighbour.NewVolume += flow;
                remainingmass -= flow;

                if (!_liquidToCheck.ContainsKey(downneighbour.HashKey))
                {
                    _liquidToCheck.Add(downneighbour.HashKey, downneighbour);
                }
            }

            if (remainingmass <= 0) { continue; }

            //Left

            //tilePos.Set(liquid.GridPos.x - 1, liquid.GridPos.y);
            Tile leftneighbour = _terrainGen.GetNeighbourTile(liquid, TerrainGenerator.Direction.Left);
            if (leftneighbour == null) { continue; }

            if (leftneighbour.TType != Tile.TileType.Solid)
            {
                if (liquid.NoChangeCounter == 0)
                {
                    leftneighbour.NoChangeCounter = 0;
                    leftneighbour.Settled = false;
                }
                ////Stop different liquids from interacting?
                //if (leftneighbour.TType == Tile.TileType.Liquid && leftneighbour.SType != liquid.SType) continue;

                flow = (liquid.CurrentVolume - leftneighbour.CurrentVolume) / 4;
                if (flow > minflow) { flow *= 0.5f; }
                flow = Mathf.Clamp(flow, 0, remainingmass);

                //if (flow == 0) { noflowcount++; } else { leftneighbour.Settled = false; }

                liquid.NewVolume -= flow;
                leftneighbour.NewVolume += flow;
                remainingmass -= flow;

                if (!_liquidToCheck.ContainsKey(leftneighbour.HashKey))
                {
                    _liquidToCheck.Add(leftneighbour.HashKey, leftneighbour);
                }
            }

            if (remainingmass <= 0) { continue; }

            //Right
            //tilePos.Set(liquid.GridPos.x + 1, liquid.GridPos.y);
            Tile rightneighbour = _terrainGen.GetNeighbourTile(liquid, TerrainGenerator.Direction.Right);
            if(rightneighbour == null) { continue; }

            if (rightneighbour.TType != Tile.TileType.Solid)
            {
                if (liquid.NoChangeCounter == 0)
                {
                    rightneighbour.NoChangeCounter = 0;
                    rightneighbour.Settled = false;
                }
                ////Stop different liquids from interacting?
                //if (rightneighbour.TType == Tile.TileType.Liquid && rightneighbour.SType != liquid.SType) continue;
                flow = (liquid.CurrentVolume - rightneighbour.CurrentVolume) / 4;
                if (flow > minflow) { flow *= 0.5f; }
                flow = Mathf.Clamp(flow, 0, remainingmass);

                //if (flow == 0) { noflowcount++; } else { leftneighbour.Settled = false; }

                liquid.NewVolume -= flow;
                rightneighbour.NewVolume += flow;
                remainingmass -= flow;

                if (!_liquidToCheck.ContainsKey(rightneighbour.HashKey))
                {
                    _liquidToCheck.Add(rightneighbour.HashKey, rightneighbour);
                }
            }

            if (remainingmass <= 0) { continue; }

            //tilePos.Set(liquid.GridPos.x, liquid.GridPos.y + 1);
            Tile upneighbour = _terrainGen.GetNeighbourTile(liquid, TerrainGenerator.Direction.Up);
            if(upneighbour == null) { continue; }

            if (upneighbour.TType != Tile.TileType.Solid)
            {
                if (liquid.NoChangeCounter == 0)
                {
                    upneighbour.NoChangeCounter = 0;
                    upneighbour.Settled = false;
                }
                ////Stop different liquids from interacting?
                //if (upneighbour.TType == Tile.TileType.Liquid && upneighbour.SType != liquid.SType) continue;

                flow = remainingmass - getstable(remainingmass + upneighbour.CurrentVolume);

                if (flow > minflow) { flow *= 0.5f; }
                flow = Mathf.Clamp(flow, 0, Math.Min(maxspeed, remainingmass));

                //if (flow == 0) { noflowcount++; } else { leftneighbour.Settled = false; }

                liquid.NewVolume -= flow;
                upneighbour.NewVolume += flow;
                remainingmass -= flow;

                if (!_liquidToCheck.ContainsKey(upneighbour.HashKey))
                {
                    _liquidToCheck.Add(upneighbour.HashKey, upneighbour);
                }
            }
        }

        dictToList = _liquidToCheck.Values.ToList();
        for (int i = 0; i < dictToList.Count; i++)
        {
            Tile liquid = dictToList[i];

            if (liquid.Settled) continue;

            if (liquid.NewVolume > Minmass)
            {
                liquid.ChangeTileType(Tile.TileType.Liquid, Tile.SubType.Water);
                liquid.SaveVolume(_terrainGen, this);
            }
            else
            {
                liquid.ChangeTileType(Tile.TileType.Air, Tile.SubType.Air);
                LiquidTiles.Remove(liquid.HashKey);
                liquid.SaveVolume(_terrainGen, this);
            }
        }

        _liquidToCheck.Clear();

        dictToList = LiquidTiles.Values.ToList();
        for (int i = 0; i < dictToList.Count; i++)
        {
            Tile liquid = dictToList[i];

            if (liquid.Settled) continue;

            liquid.CurrentVolume = liquid.NewVolume;
            if (liquid.CurrentVolume > Minmass)
            {
                if (i != 0) { liquidTypeSimulated = dictToList[i - 1].SType; } else { liquidTypeSimulated = liquid.SType; }
                liquid.ChangeTileType(Tile.TileType.Liquid, Tile.SubType.Water);
                liquid.SaveVolume(_terrainGen, this);
            }
            else
            {
                liquid.ChangeTileType(Tile.TileType.Air, Tile.SubType.Air);
                _toRemove.Add(liquid);
                liquid.SaveVolume(_terrainGen, this);
            }
        }
        for (int i = 0; i < _toRemove.Count; i++)
        {
            LiquidTiles.Remove(_toRemove[i].HashKey);
        }

        _toRemove.Clear();
    }

    float getstable(float totalmass)
    {
        if (totalmass <= 1)
        {
            return 1;
        }
        else if (totalmass < 2 * MaxMass + MaxCompress)
        {
            return (MaxMass * MaxMass + totalmass * MaxCompress) / (MaxMass + MaxCompress);
        }
        else
        {
            return (totalmass + MaxCompress) / 2;
        }
    }

    private bool VolumeChanged(float a, float b)
    {
        bool change = Mathf.Abs(a - b) < 0.001f;
        return !change;
    }
}
