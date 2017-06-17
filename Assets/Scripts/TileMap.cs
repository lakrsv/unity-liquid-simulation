using UnityEngine;
using System.Collections;

public class TileMap
{
    Perlin surfacePerlin, surfacePerlin2, surfacePerlin3, surfacePerlin4;
    Perlin cavePerlin;
    RandomWalk _caveWalker = new RandomWalk();
    public int[,] GetLayout(int width, int height)
    {
        surfacePerlin = new Perlin((long)Random.Range(1414, 1243132473284), 24);
        surfacePerlin2 = new Perlin((long)Random.Range(1414, 1243132473284), 12);
        surfacePerlin3 = new Perlin((long)Random.Range(1414, 1243132473284), 6);
        surfacePerlin4 = new Perlin((long)Random.Range(1414, 1243132473284), 4);
        cavePerlin = new Perlin((long)Random.Range(1414, 12431324773284), 48);
        int[,] tileGrid = new int[width, height];

        for(int i = 0; i < width; i++)
        {
            for (int k = 0; k < Mathf.RoundToInt(height/2); k++)
            {
                double y = surfacePerlin.getNoiseLevelAtPosition(i, k) + surfacePerlin2.getNoiseLevelAtPosition(i, k)/2 + surfacePerlin3.getNoiseLevelAtPosition(i, k)/4 + surfacePerlin4.getNoiseLevelAtPosition(i, k)/8;
                int position = Mathf.RoundToInt(k + (float)(y*20));
                position = Mathf.Max(position, 0);
                tileGrid[i, position] = 1;
                while(position != 0)
                {
                    position--;
                    tileGrid[i, position] = 1;
                }
            }
        }

        tileGrid = _caveWalker.AddCaves(width, height, tileGrid, Random.Range(42, 56423));
        MapHandler mapHandler = new MapHandler(width + 20, Mathf.RoundToInt((height / 1.95f) + (height / 4)) - 10, 60);
        mapHandler.MakeCaverns();
        for (int i = 10; i < width - 10; i++)
        {
            int y = 0;
            for (int k = Mathf.RoundToInt((height / 1.95f) + Random.Range(-5, 5)); k > 10; k--)
            {
                if (mapHandler.Map[i, y] == 0)
                {
                    tileGrid[i, k] = mapHandler.Map[i, y];
                }
                y++;
            }
        }
        //CaveGen
        //for(int i = 10; i < width-10; i++)
        //{
        //    for(int k = Mathf.RoundToInt(height/4); k > height/4-3; k--)
        //    {
        //        double y = cavePerlin.getNoiseLevelAtPosition(i, k);
        //        int position = Mathf.RoundToInt(k + (float)y * 50);
        //        tileGrid[i, position] = 0;
        //    }
        //}

        return tileGrid;
    }
}
