using UnityEngine;
using System.Collections;

public class RandomWalk
{

    public int[,] AddCaves(int width, int height, int[,] tilemap, int seed)
    {
        int caveEntrances = (width+height) / 50;
        System.Random rand = new System.Random(seed);
        for(int i = 0; i < caveEntrances; i++)
        {
            int startX = Random.Range(20, width - 1);
            int startY = Random.Range(20, height / 2);
            int numTiles = Random.Range(50, 1000);

            int x = startX;
            int y = startY;
            x = Mathf.Clamp(x, 20, width - 1);
            y = Mathf.Clamp(y, 20, Mathf.RoundToInt(height / 1.5f));
            tilemap[x, y] = 0;
            for (int k = 0; k < numTiles; k++)
            {
                int left = rand.Next(-1, 2);
                int up = rand.Next(-1, 2);

                x += left;
                y += up;

                x = Mathf.Clamp(x, 20, width - 1);
                y = Mathf.Clamp(y, 20, Mathf.RoundToInt(height / 1.5f));
                tilemap[x, y] = 0;

                int nextX = x - 1;
                int nextY = y - 1;
                nextX = Mathf.Clamp(nextX, 20, width - 1);
                nextY = Mathf.Clamp(nextY, 20, Mathf.RoundToInt(height / 1.5f));
                tilemap[nextX, nextY] = 0;

                nextX = x + 1;
                nextY = y + 1;
                nextX = Mathf.Clamp(nextX, 20, width - 1);
                nextY = Mathf.Clamp(nextY, 20, Mathf.RoundToInt(height / 1.5f));
                tilemap[nextX, nextY] = 0;

                nextX = x + 1;
                nextY = y - 1;
                nextX = Mathf.Clamp(nextX, 20, width - 1);
                nextY = Mathf.Clamp(nextY, 20, Mathf.RoundToInt(height / 1.5f));
                tilemap[nextX, nextY] = 0;

                nextX = x - 1;
                nextY = y + 1;
                nextX = Mathf.Clamp(nextX, 20, width - 1);
                nextY = Mathf.Clamp(nextY, 20, Mathf.RoundToInt(height / 1.5f));
                tilemap[nextX, nextY] = 0;

                nextX = x + 1;
                nextY = y;
                nextX = Mathf.Clamp(nextX, 20, width - 1);
                nextY = Mathf.Clamp(nextY, 20, Mathf.RoundToInt(height / 1.5f));
                tilemap[nextX, nextY] = 0;

                if (Random.Range(0, 1) > 0.6f)
                {
                    nextX = x - 1;
                    nextY = y;
                    nextX = Mathf.Clamp(nextX, 20, width - 1);
                    nextY = Mathf.Clamp(nextY, 20, Mathf.RoundToInt(height / 1.5f));
                    tilemap[nextX, nextY] = 0;

                    nextX = x;
                    nextY = y + 1;
                    nextX = Mathf.Clamp(nextX, 20, width - 1);
                    nextY = Mathf.Clamp(nextY, 20, Mathf.RoundToInt(height / 1.5f));
                    tilemap[nextX, nextY] = 0;

                    nextX = x;
                    nextY = y - 1;
                    nextX = Mathf.Clamp(nextX, 20, width - 1);
                    nextY = Mathf.Clamp(nextY, 20, Mathf.RoundToInt(height / 1.5f));
                    tilemap[nextX, nextY] = 0;
                }

            }
        }
        return tilemap;
    }
}
