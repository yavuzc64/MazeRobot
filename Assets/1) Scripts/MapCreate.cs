using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    public string mapText;
    public TMP_Text mapTextObject;

    
    [SerializeField] private string pathTemp;
    private bool isMapAvailable = false;
    private int[] measures;

    [SerializeField] private GameObject mapIndexPanel;
    [SerializeField] private TMP_InputField indexInputBox;
    

    [SerializeField] private GameObject blockObj;
    [SerializeField] private Vector3 blockSize;

    [SerializeField] private GameObject mapObj;
    [SerializeField] private GameObject mapGround;
    private Dictionary<Vector2Int, GameObject> blocks = new Dictionary<Vector2Int, GameObject>();

    private void Start()
    {
        blockSize = blockObj.transform.localScale;
        mapIndexPanel.SetActive(false);
    }

    
    private void mapInstantiate(int rows, int cols)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                GameObject tmpBlock = Instantiate(blockObj, mapObj.transform.position + Vector3.Scale(blockSize , new Vector3(j, 0, -i)), Quaternion.identity, mapObj.transform);
                tmpBlock.SetActive(false);
                blocks.Add(new Vector2Int(i, j), tmpBlock);
            }
        }
        Vector3 groundScale = new Vector3(cols*blockSize.x, 0.01f, rows*blockSize.y);
        mapGround.transform.localScale = groundScale;
        Vector3 groundCenterPos = new Vector3((cols - 1) * (blockSize.x / 2), -0.5f, -(rows - 1) * (blockSize.x / 2));// konum yanlýssa rows ve cols yer degis
        mapGround.transform.localPosition = groundCenterPos;
    }
    private void mapUpdate(int[,] currentMap)
    {
        if(isMapAvailable == false){    return;     }
        for (int i = 0; i < measures[1]; i++)
        {
            for (int j = 0; j < measures[2]; j++)
            {
                if (currentMap[i, j] == 1)
                {
                    blocks[new Vector2Int(i, j)].SetActive(true);
                }
                else
                {
                    blocks[new Vector2Int(i, j)].SetActive(false);
                }
            }
        }
        GameManager.instance.LoadMap(currentMap, measures);
    }
    public void MapLoadButton()
    {
        if (isMapAvailable)
        {
            mapDestroyButton();
        }
        pathTemp = GameManager.instance.LoadMapInFile();
        measures = FileReader.LoadMeasures(pathTemp);
        print(measures[0]);
        mapInstantiate(measures[1], measures[2]);
        isMapAvailable = true;
        mapIndexPanel.SetActive(true);

        GameManager.instance.LoadStatisticsInFile(measures);
    }
    public void mapDestroyButton()
    {
        foreach (var block in blocks)
        {
            Destroy(block.Value);
        }
        blocks.Clear();
        isMapAvailable = false;
        mapIndexPanel.SetActive(false);
    }
    public void mapIndexLoadButton()
    {
        int input = int.Parse(indexInputBox.text);
        int[,] currentMap = FileReader.LoadNthMatrix(pathTemp, input);
        if (input >= measures[0])
        {
            print("Dosyada bu sýrada matris yok.");
            return;                                                         //  BURASI ICIN BIR METIN KUTUSU EKLENEBILIR
        }
        mapUpdate(currentMap);
        DisplayInScreen(input, pathTemp);
        GameManager.instance.initPos();
    }



    private void DisplayInScreen(int index, string path)
    {
        int[,] mapData = FileReader.LoadNthMatrix(path, index);

        mapText = "";
        for (int i = 0; i < mapData.GetLength(0); i++)
        {
            for (int j = 0; j < mapData.GetLength(1); j++)
            {
                if (mapData[i, j] == 1)
                {
                    mapText += "x";
                }
                else
                {
                    mapText += "o";
                }
                mapText += " ";
            }
            mapText += "\n";
        }
        mapTextObject.text = mapText;
    }

}
