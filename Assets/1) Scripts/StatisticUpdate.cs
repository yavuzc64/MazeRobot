using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticUpdate : MonoBehaviour
{
    public float[,,] ratedStatistics; //reward-punishment result. bunu genelde tum islemler bittikten sonra(hedefe ulasma veya cikmaza girmede) almak icin sakliyoruz

    public void UpdateStatics(float[,,] updatedStatistics, int[] measures)
    {
        for (int i = 0; i < measures[1]; i++)
        {
            for (int j = 0; j < measures[2]; j++)
            {
                bool[] validDirections = new bool[4];
                float[] rate = new float[4];
                float sum = 0;

                for (int k = 0; k < 4; k++)
                {
                    validDirections[k] = GameManager.instance.IsValidThisDirection(i, j, k);
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
                        else
                        {
                            updatedStatistics[i, j, k] = 0;
                        }
                    }
                }
                

                // DEBUG
                if (i == 0 && j == 2)
                {
                    print($"0,2: {updatedStatistics[i, j, 0]} {updatedStatistics[i, j, 1]} {updatedStatistics[i, j, 2]} {updatedStatistics[i, j, 3]} (Sum: {sum})");
                }
            }
        }
    }

}
