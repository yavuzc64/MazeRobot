using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float slowness ;
    [SerializeField] private float stepDist;
    [SerializeField] private GameObject player;
    [SerializeField] private int cuttOff;
    [SerializeField] private float angleBalance;
    private GameManager gameManager;

    private bool isMoving = false;
    private void Awake() => gameManager = GameManager.instance;

    
    private bool MoveControl(int x, int z)
    {
        int[] m = gameManager.GetPos();
        
        if (gameManager.isValidPos(m[0]+x, m[1]+z))
        {
            gameManager.SetPos(m[0] + x, m[1] + z);
            return true;
        }
        return false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 rayPos = hit.point;
                Vector3 pPos = player.transform.position;

                float diffX = Mathf.Abs(rayPos.x - pPos.x);
                float diffZ = Mathf.Abs(rayPos.z - pPos.z);
                if (diffX > cuttOff || diffZ > cuttOff || (Mathf.Abs(diffZ-diffX) < angleBalance))
                {
                    return;
                }
                int x = 0; int z=0; int direction = 0;
                Vector3 targetPos = pPos;
                if (diffX > diffZ )
                {
                    if (rayPos.x > pPos.x)          //sag
                    {
                        targetPos.x += stepDist;
                        x++;
                        direction = 2;
                    }
                    else                           //sol
                    {
                        targetPos.x -= stepDist;
                        x--;
                        direction = 3;
                    }
                }
                else if (diffZ > diffX)
                {
                    if (rayPos.z > pPos.z)         //yukari
                    {
                        targetPos.z += stepDist;
                        z++;
                        direction = 0;
                    }
                    else                           //asagi
                    {
                        targetPos.z -= stepDist;
                        z--;
                        direction = 1;
                    }
                }
                if(MoveControl(-z, x)) { 
                    gameManager.directionSign.UpdateSign(targetPos,direction);
                    StartCoroutine(MovePlayer(targetPos));
                }
            }
        }
    }

    private IEnumerator MovePlayer(Vector3 targetPos)
    {
        isMoving = true;
        Vector3 startPos = player.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < slowness)
        {
            player.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / slowness);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = targetPos;
        isMoving = false;
    }
}
