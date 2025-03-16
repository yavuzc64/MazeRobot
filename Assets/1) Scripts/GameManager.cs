using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int[,] currentMap;
    private int[] measures;
    private int[,,] directionStatistics;

    private float[,,] updatedStatistics;
    private int[,] visitedMap;

    public string MapPath;
    public string statisticsPath;

    public DirectionSign directionSign;
    [SerializeField] private StatisticUpdate statisticUpdate;

    [SerializeField] private TMP_InputField staticsPathInputBox;
    [SerializeField] private TMP_InputField mapPathInputBox;
    [SerializeField] private TMP_InputField xInputBox;
    [SerializeField] private TMP_InputField yInputBox;
    [SerializeField] private GameObject Map;

    private int[] playerPos = new int[2];

    [SerializeField] private GameObject player;
    private void Awake() => instance = this;

    public void initPos()
    {
        SetPos(int.Parse(xInputBox.text), int.Parse(yInputBox.text));
        player.transform.position = Map.transform.position + new Vector3(playerPos[1],0.4f, -playerPos[0]);
        directionSign.UpdateSign(player.transform.position, 4);
    }
    public void SetPos(int x, int y)
    {
        playerPos[0] = x;
        playerPos[1] = y;
        visitedMap[x, y]++;
        //print("Player Pos: " + playerPos[0] + " " + playerPos[1]);
    }
    public int[] GetPos()
    {
        return new int[] { playerPos[0], playerPos[1] }; // Kopyas�n� dondur
    }

    public bool isValidPos(int x, int y)
    {
        if (x < 0 || x >= measures[1] || y < 0 || y >= measures[2])
        {
            return false;
        }
        if (currentMap[x, y] == 1)
        {
            return false;
        }
        return true;
    }
    public void LoadMap(int[,] map, int[] measures)
    {
        currentMap = map;
        this.measures = measures;
        visitedMap = new int[measures[1], measures[2]];
        statisticUpdate.UpdateStatics(updatedStatistics, measures);
        directionSign.SetParameters(updatedStatistics, measures, player.transform);
    }
    public void LoadStatisticsInFile(int[] measures)
    {
        statisticsPath = statisticsPath != null ? statisticsPath : staticsPathInputBox.text;
        directionStatistics = FileReader.LoadDirectionStatics(statisticsPath);
        if (directionStatistics != null)
        {
            updatedStatistics = new float[measures[1], measures[2], 4];
            for (int i = 0; i < measures[1]; i++)
            {
                for (int j = 0; j < measures[2]; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        updatedStatistics[i, j, k] = (float)directionStatistics[i,j,k];
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Statistics not loaded");
        }
    }
    public string LoadMapInFile()
    {
        return MapPath != null ? MapPath : mapPathInputBox.text;
    }



}
