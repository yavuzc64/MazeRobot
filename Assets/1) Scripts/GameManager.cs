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

    public float[,,] updatedStatistics;
    public int[,] visitedMap;
    [SerializeField] private TMP_Text visitedText;

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


    public Stack<int[]> terminalPoint = new Stack<int[]>();
    public Stack<int[]> straigthPoint = new Stack<int[]>();

    private void Awake() => instance = this;

    //Position Functions
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
        printVisited();
    }
    public int[] GetPos()
    {
        return new int[] { playerPos[0], playerPos[1] }; // Kopyasýný dondur
    }
    public bool isValidPos(int x, int y, bool visited=false)
    {
        if (x < 0 || x >= measures[1] || y < 0 || y >= measures[2])
        {
            return false;
        }
        if (currentMap[x, y] == 1)
        {
            return false;
        }
        if (visited && visitedMap[x, y] > 0)
        {
            return false;
        }
        return true;
    }


    private void printVisited()
    {
        string s = "";
        for (int i = 0; i < measures[1]; i++)
        {
            for (int j = 0; j < measures[2]; j++)
            {
                s += visitedMap[i, j] + " ";
            }
            s += "\n";
        }
        visitedText.text = s;
    }


    //Load Functions
    public void LoadMap(int[,] map, int[] measures)
    {
        currentMap = map;
        this.measures = measures;
        visitedMap = new int[measures[1], measures[2]];
        if (directionStatistics != null)
        {
            AssignToUpdatedStatstics(measures);
        }
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
            AssignToUpdatedStatstics(measures);
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

    //other functions
    private void AssignToUpdatedStatstics(int[] measures)
    {
        for (int i = 0; i < measures[1]; i++)
        {
            for (int j = 0; j < measures[2]; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    updatedStatistics[i, j, k] = (float)directionStatistics[i, j, k];
                }
            }
        }
    }
    public bool IsValidThisDirection(int i, int j, int direction, bool visitedCtrl=false)
    {
        //yukari asagi sol sag
        //  0      1    3   2
        if (direction == 0)
        {
            return isValidPos(i - 1, j, visitedCtrl);
        }
        else if (direction == 1)
        {
            return isValidPos(i + 1, j, visitedCtrl);
        }
        else if (direction == 2)
        {
            return isValidPos(i, j - 1, visitedCtrl);
        }
        else if (direction == 3)
        {
            return isValidPos(i, j + 1, visitedCtrl);
        }
        return false;
    }
    public int[] ChangeArrByDirection(int currX, int currY, int direction)
    {
        //yukari asagi sol sag
        //  0      1    3   2
        if (direction == 0)
        {
            return new int[] { currX - 1, currY };
        }
        else if (direction == 1)
        {
            return new int[] { currX + 1, currY };
        }
        else if (direction == 2)
        {
            return new int[] { currX, currY - 1 };
        }
        else if (direction == 3)
        {
            return new int[] { currX, currY + 1 };
        }
        return new int[] { currX ,  currY};
    }
    public Vector3 ChangeV3ByDirection( int direction)
    {
        /*  adim uzunlugu sabit verildi degistir    */
        Vector3 c = player.transform.position;
        if(direction == 0)
        {
            c.z += 1;
        }
        else if (direction == 1)
        {
            c.z -= 1;
        }
        else if (direction == 2)
        {
            c.x -= 1;
        }
        else if (direction == 3)
        {
            c.x += 1;
        }
        return c;
    }
    public Vector3 ConvertToGamePos(int x, int y)
    {
        return Map.transform.position + new Vector3(y, 0.4f, -x);
    }

}
