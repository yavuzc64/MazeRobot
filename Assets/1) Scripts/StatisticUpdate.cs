using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticUpdate : MonoBehaviour
{
    public float[,,] ratedStatistics; //reward-punishment result. bunu genelde tum islemler bittikten sonra(hedefe ulasma veya cikmaza girmede) almak icin sakliyoruz

    public void UpdateStatics(float[,,] updatedStatistics, int[] measures)
    {
        float[] rate = new float[4];
        float sum = 0;
        for (int i = 0; i < measures[1]; i++)
        {
            for (int j = 0; j < measures[2]; j++)
            {
                bool[] validDirections = new bool[4];

                for (int k = 0; k < 4; k++)
                {
                    validDirections[k] = IsValidThisDirection(i, j, k);
                    if (validDirections[k])
                    {
                        rate[k] = updatedStatistics[i, j, k];
                        sum += rate[k];
                    }
                    else
                    {
                        rate[k] = 0;
                    }
                }

                if (sum > 0)
                {
                    float normalizationFactor = 100f / sum;
                    for (int k = 0; k < 4; k++)
                    {
                        if (validDirections[k])
                        {
                            updatedStatistics[i, j, k] = rate[k] * normalizationFactor;
                        }
                    }
                }
            }
        }
    }

    private bool IsValidThisDirection(int i, int j, int direction)
    {
        //yukari asagi sol sag
        //  0      1    2   3
        if (direction == 0) 
        {
            return GameManager.instance.isValidPos(i - 1, j);
        }
        else if (direction == 1)
        {
            return GameManager.instance.isValidPos(i + 1, j);
        }
        else if (direction == 2)
        {
            return GameManager.instance.isValidPos(i, j + 1);
        }
        else if (direction == 3)
        {
            return GameManager.instance.isValidPos(i, j - 1);
        }
        return false;
    }
}
