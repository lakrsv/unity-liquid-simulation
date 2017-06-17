using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour
{
    private TerrainGenerator _terrainGen;
	// Use this for initialization
	void Start ()
    {
        _terrainGen = GetComponent<TerrainGenerator>();
	}

    public Tile GetTileAtWorldPos(Vector2 position)
    {
        return _terrainGen.TileWorldPosDict.ContainsKey(position) ? _terrainGen.TileWorldPosDict[position] : null;
    }
}
