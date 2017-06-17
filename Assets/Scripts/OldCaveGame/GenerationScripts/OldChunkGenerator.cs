using UnityEngine;
using System.Collections;

public class OldChunkGenerator
{
    public int ChunkWidth, ChunkHeight;

    public OldChunkGenerator()
    {
        ChunkWidth = 10;
        ChunkHeight = 10;
    }
    //Chunk starts at bottom-left
    public int[,] GenerateChunk()
    {
        var Tiles = new int[ChunkWidth, ChunkHeight];
        for (int i = 0; i < ChunkWidth; i++)
        {
            for (int j = 0; j < ChunkHeight; j++)
            {
                int baseOrePicker = (int)(Random.value * 100);
                var baseOre = 0;
                if (baseOrePicker < 70)
                {
                    baseOre = 0;
                }
                else
                {
                    baseOre = 1;
                }
                Tiles[i, j] = baseOre;
            }
        }
        AddOre(Tiles);
        return Tiles;
    }
    void AddOre(int[,] tiles)
    {
        var chosenOre = OldMineralGenerator.ChooseOre();
        var oreAmount = Random.Range(4, 50);
    

        var inWidth = Random.Range(1, ChunkWidth);
        var inHeight = Random.Range(1, ChunkHeight);

        var startWidth = inWidth;
        var startHeight = inHeight;


        while (oreAmount > 0)
        {
            oreAmount--;
            var direction = Random.Range(0, 4);

            switch (direction)
            {
                //up left
                case 0:
                    tiles[inWidth, inHeight] = chosenOre;
                    inWidth--;
                    inHeight++;
                    break;
                //up right
                case 1:
                    tiles[inWidth, inHeight] = chosenOre;
                    inWidth++;
                    inHeight++;
                    break;
                //down left
                case 2:
                    tiles[inWidth, inHeight] = chosenOre;
                    inWidth--;
                    inHeight--;
                    break;
                //down right
                case 3:
                    tiles[inWidth, inHeight] = chosenOre;
                    inWidth++;
                    inHeight--;
                    break;
            }
            if (inWidth == ChunkWidth || inWidth == 0)
            {
                inWidth = startWidth;
            }
            if (inHeight == ChunkHeight || inHeight == 0)
            {
                inHeight = startHeight;
            }
        }
    }
}
