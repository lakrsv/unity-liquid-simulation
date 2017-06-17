/*
* Perlin noise octave generator v1.0
* Created by Baggerboot; 3/5/2012
*
*/
using System;
using UnityEngine;

public class Perlin
{
    private long seed;
    private System.Random rand;
    private int octave;
    private int actualOctave;
    //BUGFREE
    public Perlin(long seed, int octave)
    {
        this.seed = seed;
        this.octave = octave;
        rand = new System.Random();
    }
    public double getNoiseLevelAtPosition(int x, int z)
    {
        //actualOctave = octave + UnityEngine.Random.Range(-1, 6);
        actualOctave = octave;
        int xmin = (int)(double)x / actualOctave;
        int xmax = xmin + 1;
        int zmin = (int)(double)z / actualOctave;
        int zmax = zmin + 1;
        Vector2 a = new Vector2(xmin, zmin);
        Vector2 b = new Vector2(xmax, zmin);
        Vector2 c = new Vector2(xmax, zmax);
        Vector2 d = new Vector2(xmin, zmax);
        double ra = getRandomAtPosition(a);
        double rb = getRandomAtPosition(b);
        double rc = getRandomAtPosition(c);
        double rd = getRandomAtPosition(d);
        double ret = cosineInterpolate( //Interpolate Z direction
                cosineInterpolate((float)ra, (float)rb, (float)(x - xmin * actualOctave) / actualOctave), //Interpolate X1
                cosineInterpolate((float)rd, (float)rc, (float)(x - xmin * actualOctave) / actualOctave), //Interpolate X2
                ((float)z - (float)zmin * (float)actualOctave) / (float)actualOctave);
        return ret;
    }
    private float cosineInterpolate(float a, float b, float x)
    {
        float ft = (float)(x * Math.PI);
        float f = (float)((1f - Math.Cos(ft)) * .5f);
        float ret = a * (1f - f) + b * f;
        return ret;
    }
    private double getRandomAtPosition(Vector2 coord)
    {
        double var = 10000 * (Math.Sin(coord.x) + Math.Cos(coord.y) + Math.Tan(seed));
        System.Random rand = new System.Random((int)(var*1000));
        double ret = rand.NextDouble();
        return ret;
    }
}