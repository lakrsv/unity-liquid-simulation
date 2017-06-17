using UnityEngine;
using System.Collections;

public class OldTileTypeGenerator : MonoBehaviour
{

    public Sprite

        DirtDefaultSprite,
        RockDefaultSprite,
        IronDefaultSprite,
        GoldDefaultSprite,
        TitaniumDefaultSprite,
        CoalDefaultSprite;

    public Sprite[] DamageOverlay;

    public OldTileData GetTileData(int tiletype)
    {
        OldTileData data = null;
        switch (tiletype)
        {
            case 1:
                data = new OldTileData()
                {
                    Name = "Dirt",
                    Health = 2,
                    Type = 1,
                    DefaultSprite = DirtDefaultSprite,
                    DefaultDamageOverlay = DamageOverlay
                };
                break;
            case 2:
                data = new OldTileData()
                {
                    Name = "Iron",
                    Health = 10,
                    Type = 2,
                    DefaultSprite = IronDefaultSprite,
                    DefaultDamageOverlay = DamageOverlay
                };
                break;
            case 3:
                data = new OldTileData()
                {
                    Name = "Coal",
                    Health = 10,
                    Type = 3,
                    DefaultSprite = CoalDefaultSprite,
                    DefaultDamageOverlay = DamageOverlay
                };
                break;
            case 4:
                data = new OldTileData()
                {
                    Name = "Gold",
                    Health = 20,
                    Type = 4,
                    DefaultSprite = GoldDefaultSprite,
                    DefaultDamageOverlay = DamageOverlay
                };
                break;
            case 5:
                data = new OldTileData()
                {
                    Name = "Titanium",
                    Health = 40,
                    Type = 5,
                    DefaultSprite = TitaniumDefaultSprite,
                    DefaultDamageOverlay = DamageOverlay
                };
                break;
            default:
                data = new OldTileData()
                {
                    Name = "Rock",
                    Health = 5,
                    Type = 0,
                    DefaultSprite = RockDefaultSprite,
                    DefaultDamageOverlay = DamageOverlay
                };
                break;
        }
        return data;
    }
}
