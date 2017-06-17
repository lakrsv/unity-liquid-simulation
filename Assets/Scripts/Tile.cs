using UnityEngine;
using System.Collections;
using System;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        Air = 0,
        Solid = 1,
        Liquid = 2
    }

    public enum SubType
    {
        None = 0,
        Air = 1,
        Dirt = 2,
        Water = 3,
        Oil = 4
    }

    public TileType TType;
    public SubType SType;

    public int HashKey;

    //public GameObject[,] FluidParticles = new GameObject[8, 8];
    //public int[,] FluidGridLevel = new int[8, 8];

    //public bool Calculate;
    //public bool Settled = false;

    public Vector2 GridPos;

    //When A Tile's maxlevel is 8, only one 4x4 Liquidblock is present. When it is 512, all 64 are present.
    //Liquid will dynamically spread out based on the level, and transparency is directly linked to a tile's maxlevel, with 8 being one 4x4 on max.

    //Example: A tile's fluid level is 8, this means that there will be 8 liquid cells 4x4 at the lowest opacity in the tile when spread out.
    //public static int MaxLevel = 512;

    public float CurrentVolume = 0;
    public float NewVolume = 0;

    public void Initialize(TileType type, SubType subtype, Vector2 gridPos)
    {
        TType = type;
        SType = subtype;
        GridPos = gridPos;

        setSprite();
        HashKey = this.GetHashCode();
    }

    Vector2 tempPos = new Vector2();
    SpriteRenderer spriterenderer;
    public bool Settled = false;
    public int NoChangeCounter = 0;

    public void SaveVolume(TerrainGenerator terraingen, CellularLiquid celliq)
    {
        if (spriterenderer == null) { spriterenderer = GetComponent<SpriteRenderer>(); }

        CurrentVolume = NewVolume;

        if (CurrentVolume == 0 || CurrentVolume < CellularLiquid.Minmass)
        {
            //ChangeTileType(TileType.Air, SubType.Air);
        }
        else if (CurrentVolume > CellularLiquid.Minmass)
        {
            if (!celliq.LiquidTiles.ContainsKey(this.HashKey))
                celliq.LiquidTiles.Add(this.HashKey, this);

            //var liquidSprite = terraingen.WaterSprite;
            //spriterenderer.sprite = liquidSprite;

            //gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, Mathf.Clamp(CurrentVolume / 1, 0, 1), gameObject.transform.localScale.z);
            spriterenderer.color = new Color(1f, 1f, 1f, Mathf.Clamp(CurrentVolume / 1.12f, 0.08f, 1.0f));

            if (CurrentVolume < CellularLiquid.MaxMass)
            {
                tempPos.Set(this.GridPos.x, this.GridPos.y + 1);
                var neighbour = terraingen.TilePosDict[tempPos];
                if (neighbour.CurrentVolume >= CellularLiquid.mindraw)
                {
                    gameObject.transform.localScale = Vector3.one;
                }
                else
                {
                    tempPos.Set(gameObject.transform.localScale.x, Mathf.Clamp(CurrentVolume / 1, 0, 1));
                    gameObject.transform.localScale = tempPos;
                    //gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, Mathf.Clamp(CurrentVolume / 1, 0, 1), gameObject.transform.localScale.z);
                }
            }
            else
            {
                transform.localScale = Vector3.one;
            }
        }
    }

    public void ChangeTileType(TileType ttype, SubType stype = SubType.None)
    {
        if (TType == ttype && stype == SType) { return; }
        //if(TType != TileType.Liquid && ttype == TileType.Liquid && SType == SubType.Air || SType == SubType.None) { SType = stype; }
        TType = ttype;
        SType = stype;

        this.NoChangeCounter = 0;
        this.Settled = false;
        gameObject.transform.localScale = Vector3.one;
        spriterenderer.color = Color.white;

        if (TType != TileType.Solid)
        {
            var collider = gameObject.GetComponent<BoxCollider2D>();
            collider.enabled = false;
        }
        else
        {
            var collider = gameObject.GetComponent<BoxCollider2D>();

            collider.enabled = true;
            collider.isTrigger = false;
        }

        setSprite();

        //If this tiletype was changed to air, also check if any of it's neighbours are liquids and unsettle them.
        if (this.SType == SubType.Air)
        {
            var terrainGen = GameObject.Find("TerrainGenerator").GetComponent<TerrainGenerator>();
            for (int i = 0; i < 4; i++)
            {
                var neighbour = terrainGen.GetNeighbourTile(this, (TerrainGenerator.Direction)i);
                if(neighbour == null) { continue; }

                if(neighbour.TType == TileType.Liquid)
                {
                    neighbour.UnsettleAll(this, terrainGen);
                }
            }

        }
    }

    private void setSprite()
{
    if (spriterenderer == null) { spriterenderer = GetComponent<SpriteRenderer>(); }
    //Debug Oil Water
    switch (SType)
    {
        case SubType.Air:
            spriterenderer.sprite = null;
            break;
        case SubType.Dirt:
            spriterenderer.sprite = TerrainGenerator.Instance.DirtSprite;
            break;
        case SubType.Oil:
            spriterenderer.sprite = TerrainGenerator.Instance.OilSprite;
            break;
        case SubType.Water:
                spriterenderer.sprite = TerrainGenerator.Instance.OilSprite;
            break;
    }

}

    public void UnsettleAll(Tile originTile, TerrainGenerator terraingen)
    {
        NoChangeCounter = 0;
        Settled = false;

        for(int i = 0; i < 4; i++)
        {
            var tile = terraingen.GetNeighbourTile(this, (TerrainGenerator.Direction)i);
            if(tile == null) { continue; }

            if (tile == originTile || !tile.Settled || tile.NoChangeCounter == 0) continue;

            tile.UnsettleAll(this, terraingen);
            //yield return new WaitForSeconds(0.01f);
        }
    }
}
