using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    GameManager m => GameManager.instance;
    [SerializeField] private PlayerMovement pm;
    public void MainMove()
    {
        float max = 0;
        int selectedDirection = 0;
        int terminalCount=0; //burasi terminel mi (birden fazla secenek var mi) kontrolu icin
        int[] pos = m.GetPos();
        for (int k = 0; k < 4; k++)
        {
            if(m.updatedStatistics[pos[0], pos[1], k]>max)
            {
                max = m.updatedStatistics[pos[0], pos[1], k];
                selectedDirection = k;
                terminalCount++;
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
        }
        else
        {
            Debug.Log("Invalid Move");
        }

        if (terminalCount > 1)
        {
            //terminal stack ine ekle
        }
        else
        {
            //straight stack ine ekle
        }

    }
}
