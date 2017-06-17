using UnityEngine;
using System.Collections;

public class OldTile : MonoBehaviour {
    public int TileTypeID = -1;
    public string Name;
    public int TileType;
    public float Health;
    private float _healthPercentage;
    private int _startHealth;
    public Sprite[] DamageOverlaySprite;
    private GameObject _spriteOverlay;
    private SpriteRenderer _spriteOverlayRenderer;
    private OldTileTypeGenerator _gen;
    void Start()
    {
        _gen = GameObject.Find("TerrainGenerator").GetComponent<OldTileTypeGenerator>();
        var data = _gen.GetTileData(TileType);
        if (TileTypeID != -1)
        {
            data = _gen.GetTileData(TileTypeID);
        }
        var renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = data.DefaultSprite;
        Health = data.Health;
        Name = data.Name;
        DamageOverlaySprite = data.DefaultDamageOverlay;
        _startHealth = (int)Health;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        DamageOverlay(Health);
        switch ((int)Health)
        {
            case 0:
                var deathParticles = Resources.Load<GameObject>("Prefabs/TileDeathParticles");
                var deathParticlePos = new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f);
                Instantiate(deathParticles, deathParticlePos, Quaternion.identity);
                Destroy(gameObject);
                //var playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventoryManager>();
                //playerInventory.AddItem(Item.ItemType.Placeable, "PlaceableTile", Name);
                break;
        }
    }
    void DamageOverlay(float health)
    {
        _healthPercentage = health / _startHealth;
        var healthI = _healthPercentage * 10;
        var damageI = (int)(9 - healthI);
        if (_spriteOverlay == null)
        {
            _spriteOverlay = new GameObject();
            _spriteOverlay.transform.SetParent(this.gameObject.transform);
            _spriteOverlay.transform.localPosition = Vector2.zero;
            _spriteOverlayRenderer = _spriteOverlay.AddComponent<SpriteRenderer>();
            _spriteOverlayRenderer.sortingOrder = 1;
        }
        _spriteOverlayRenderer.sprite = DamageOverlaySprite[damageI];
    }
    public void ChangeBlockType(int tileID)
    {
        _gen = GameObject.Find("TerrainGenerator").GetComponent<OldTileTypeGenerator>();
        var data = _gen.GetTileData(tileID);
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = data.DefaultSprite;
        Health = data.Health;
        Name = data.Name;
        _startHealth = (int)Health;
    }
}
