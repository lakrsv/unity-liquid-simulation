using UnityEngine;
using System.Collections;

public static class OldMineralGenerator
{

    //Set Ore quality based on weighted randomizing.
    public static int ChooseOre()
    {
        var orequality = 0;
        int random = (int)(Random.value * 100);

        if (random <= 60)
        {
            orequality = 0;
        }
        else if (random <= 97 && random >= 61)
        {
            orequality = 1;
        }
        else if (random <= 100 && random >= 98)
        {
            orequality = 2;
        }
        return ChooseOreType(orequality);
    }

    //Set Ore type based on the quality produced.
    static int ChooseOreType(int quality)
    {
        var oretype = 0;
        switch (quality)
        {
            case 0:
                oretype = Random.Range(0, 2);
                break;
            case 1:
                oretype = Random.Range(2, 4);
                break;
            case 2:
                oretype = Random.Range(4, 6);
                break;
        }
        return oretype;
    }
}
