using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    GameManager m => GameManager.instance;
    [SerializeField] private PlayerMovement pm;
    [SerializeField] private int straigthWayCount; //son terminalden sonra kac adim duz gidildigini tutar
    public void MainMove()
    {
        float max = 0;
        int selectedDirection = 0;
        int terminalCount=0; //burasi terminel mi (birden fazla secenek var mi) kontrolu icin
        int[] pos = m.GetPos();
        for (int k = 0; k < 4; k++)
        {
            if(m.IsValidThisDirection(pos[0], pos[1], k, true))
            {
                terminalCount++;
                if (m.updatedStatistics[pos[0], pos[1], k] > max )
                {
                    max = m.updatedStatistics[pos[0], pos[1], k];
                    selectedDirection = k;
                }
            }
            
        }
        if (m.IsValidThisDirection(pos[0], pos[1], selectedDirection,true))// (true) visited kontrolu yapiliyor
        {
            Debug.Log("Gidiyorum...");
            int[] newPos = m.ChangeArrByDirection(pos[0], pos[1], selectedDirection);
            Debug.Log("New Pos: " + newPos[0] + " " + newPos[1]);
            m.SetPos(newPos[0], newPos[1]);
            m.directionSign.UpdateSign(m.ChangeV3ByDirection(selectedDirection), selectedDirection);
            StartCoroutine( pm.MovePlayer(m.ChangeV3ByDirection(selectedDirection)));

            m.visitedMap[newPos[0], newPos[1]]++;
        }
        else
        {
            Debug.Log("Invalid Move");

            if (m.terminalPoint.Count > 0)
            {
                for (int i = 0; i < straigthWayCount; i++)
                {
                    m.straigthPoint.Pop();                  //  CEZA VERILECEK
                    print("Straigth Point Pop");
                }
                int[] lastTerminal = m.terminalPoint.Pop();
                print("Terminal Point Pop" + lastTerminal[0] + " - " + lastTerminal[1]);
                m.SetPos(lastTerminal[0], lastTerminal[1]);
                m.directionSign.UpdateSign(m.ConvertToGamePos(lastTerminal[0], lastTerminal[1]), 0);
                StartCoroutine(pm.MovePlayer(m.ConvertToGamePos(lastTerminal[0], lastTerminal[1])));
            }
            else
            {
                Debug.Log("You are stuck");
            }
        }

        if (terminalCount > 1)
        {
            m.terminalPoint.Push(new int[] { pos[0], pos[1] });
            print("Terminal Point: " + pos[0] + " " + pos[1]);
            straigthWayCount = 0;
        }
        else{

            m.straigthPoint.Push(new int[] { pos[0], pos[1] });
            print("Straigth Point: " + pos[0] + " " + pos[1]);
            straigthWayCount++;
        }

    }
}
