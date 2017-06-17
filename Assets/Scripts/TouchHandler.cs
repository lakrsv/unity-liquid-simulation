using UnityEngine;
using System.Collections;

public class TouchHandler : MonoBehaviour
{
    private TileManager _tileMan;
    //Debug
    CellularLiquid _celliq;
    private Vector3 _touchPoint = new Vector3();

    void Start()
    {
        _tileMan = GameObject.Find("TerrainGenerator").GetComponent<TileManager>();
        _celliq = _tileMan.gameObject.GetComponent<CellularLiquid>();
    }

	// Update is called once per frame
	void Update ()
    {
        HandleClick();

        //Debug
        if (Input.GetKeyDown(KeyCode.Z))
        {
            selectedAction++;
            if(selectedAction > 2)
            {
                selectedAction = 0;
            }

            switch (selectedAction)
            {
                case 0:
                    Debug.Log("Selected Action: Fill Water.");
                    break;
                case 1:
                    Debug.Log("Selected Action: Fill Oil.");
                    break;
                case 2:
                    Debug.Log("Selected Action: Fill Dirt.");
                    break;
                case 3:
                    Debug.Log("Selected Action: Fill Air.");
                    break;
            }
        }
	}

    //Debug
    int selectedAction = 0;
    void HandleClick()
    {
        //Debug
        if (Input.GetMouseButtonDown(1))
        {
            _touchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _touchPoint.Set(Mathf.FloorToInt(_touchPoint.x), Mathf.FloorToInt(_touchPoint.y), 0);
            Tile atPosR = _tileMan.GetTileAtWorldPos(_touchPoint);
            atPosR.ChangeTileType(Tile.TileType.Air, Tile.SubType.Air);
        }
        if (!Input.GetMouseButton(0)) { return; }
        _touchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Debug.Log(_touchPoint);
        //Debug.Log(Camera.main.transform.position.x - _touchPoint.x);

        _touchPoint.Set(Mathf.FloorToInt(_touchPoint.x), Mathf.FloorToInt(_touchPoint.y), 0);

        Tile atPos = _tileMan.GetTileAtWorldPos(_touchPoint);
        if (atPos == null) { return; }
        switch (selectedAction)
        {
            case 0:
                _celliq.AddLiquid(atPos, Tile.SubType.Water);
                break;
            case 1:
                _celliq.AddLiquid(atPos, Tile.SubType.Oil);
                break;
            case 2:
                atPos.ChangeTileType(Tile.TileType.Solid, Tile.SubType.Dirt);
                break;
        }
    }
}
