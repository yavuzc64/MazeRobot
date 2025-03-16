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
                float[] rate = new float[4]; // Her d�ng�de s�f�rlanmal�
                float sum = 0;

                for (int k = 0; k < 4; k++)
                {
                    validDirections[k] = IsValidThisDirection(i, j, k);
                    if (validDirections[k])
                    {
                        rate[k] = updatedStatistics[i, j, k]; // Ge�erli y�nlerin oranlar�n� al
                        sum += rate[k];
                    }
                    else
                    {
                        rate[k] = 0; // Ge�ersiz y�nleri s�f�rla
                    }
                }

                if (sum > 0)
                {
                    float normalizationFactor = 100f / sum;
                    for (int k = 0; k < 4; k++)
                    {
                        if (validDirections[k])
                        {
                            updatedStatistics[i, j, k] = rate[k] * normalizationFactor; // Sadece ge�erli y�nleri g�ncelle
                        }
                        else
                        {
                            updatedStatistics[i, j, k] = 0; // Ge�ersiz y�nleri s�f�r b�rak
                        }
                    }
                }
                else
                {
                    // E�er hi� ge�erli y�n yoksa, t�m y�nleri s�f�r yap
                    for (int k = 0; k < 4; k++)
                    {
                        updatedStatistics[i, j, k] = 0;
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


    private bool IsValidThisDirection(int i, int j, int direction)
    {
        //yukari asagi sol sag
        //  0      1    3   2
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
            return GameManager.instance.isValidPos(i, j - 1);
        }
        else if (direction == 3)
        {
            return GameManager.instance.isValidPos(i, j + 1);
        }
        return false;
    }
}
