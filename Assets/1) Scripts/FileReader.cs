using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileReader : MonoBehaviour
{
    public static int[] LoadMeasures(string path)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            int[] measures = new int[3];
            measures[0] = reader.ReadInt32();//count
            measures[1] = reader.ReadInt32();//rows
            measures[2] = reader.ReadInt32();//cols
            //print("Measures: " + measures[0] + " " + measures[1] + " " + measures[2]);
            return measures;
        }
    }
    public static int[,] LoadNthMatrix(string path, int index)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            int count = reader.ReadInt32();
            int rows = reader.ReadInt32();
            int cols = reader.ReadInt32();

            if (index >= count)
            {
                throw new IndexOutOfRangeException("Dosyada bu sýrada matris yok.");
            }

            long offset = (long)index * rows * cols * sizeof(int);

            reader.BaseStream.Seek(offset, SeekOrigin.Current);

            int[,] matrix = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = reader.ReadInt32();
                }
            }

            return matrix;
        }
    }
    public static int[,,] LoadDirectionStatics(string path)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            int count = reader.ReadInt32(); //bos
            int rows = reader.ReadInt32();
            int cols = reader.ReadInt32();
            int[,,] matrix = new int[rows, cols, 4];
            for (int k = 0; k < rows; k++)
            {
                for (int i = 0; i < cols; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        matrix[k, i, j] = reader.ReadInt32();
                    }
                }
            }
            return matrix;
        }
    }
}
