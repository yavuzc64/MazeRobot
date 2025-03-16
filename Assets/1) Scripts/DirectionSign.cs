using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DirectionSign : MonoBehaviour
{
    private int[,,] directionData;
    private int[] measures;
    private Transform player;
    [SerializeField] private float animDuration;
    [SerializeField] private List<GameObject> signs;
    [SerializeField] private List<TMP_Text> signTexts;
    [SerializeField] private List<Material> signMat;
    private void Start()
    {
        foreach (GameObject sign in signs)
        {
            Material quadMaterial = new Material(Shader.Find("Standard"));
            SetMaterialTransparent(quadMaterial);
            sign.GetComponent<Renderer>().material = quadMaterial;
            signMat.Add(quadMaterial);
            signTexts.Add( sign.GetComponentInChildren<TMP_Text>());
        }
    }
    void SetMaterialTransparent(Material mat)// hazir kod
    {
        mat.SetFloat("_Mode", 3); // Transparent Mode (0: Opaque, 1: Cutout, 2: Fade, 3: Transparent)
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000; // Transparent Queue
    }
    public void SetParameters(int[,,] directionData, int[] measures, Transform player)
    {
        this.directionData = directionData;
        this.measures = measures;
        this.player = player;
    }
    //yukari asagi sol sag
    private IEnumerator SignAnim(bool fadeIn, Material mat)
    {
        float duration = fadeIn ? animDuration : animDuration/4;
        duration = Mathf.Clamp01(duration);
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? mat.color.a : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            Color currentColor = mat.color;
            mat.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);

            yield return null;
        }
        yield return null;
    }
    public void UpdateSign(Vector3 targetPos, int direction)
    {
        StopAllCoroutines();
        /*for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                for (int k = 0; k < 4; k++)
                    Debug.Log("X: " + i + " Y: " + j + "  " + directionData[i, j, 0] + " " + directionData[i, j, 1] + " " + directionData[i, j, 2] + " " + directionData[i, j, 3]);
            }
        }*/
        int i = 0;
        int[] pos = GameManager.instance.GetPos();//= CalcNewPos(direction);
        foreach(Material m in signMat)
        {
            StartCoroutine(SignAnim(false, m));
        }
        for (i = 0; i < 4; i++)
        {
            Vector3 tPos = targetPos;
            signs[i].transform.position = CalcSingPos(i, tPos);
            int value = directionData[pos[0], pos[1], i];
            signTexts[i].text = (value==0) ? "" : value.ToString();
            signMat[i] = ChangeColor(value, signMat[i]);
            StartCoroutine(SignAnim(true, signMat[i]));


            /*if (pos[0] >= 0 && pos[0] < directionData.GetLength(0) &&       //DEBUG
                pos[1] >= 0 && pos[1] < directionData.GetLength(1) &&
               i >= 0 && i < directionData.GetLength(2))
            {
            }*/
        }
    }
    private int[] CalcNewPos(int dir)// CALISMIYOR
    {
        int[] newPos = GameManager.instance.GetPos();//= new int[] { originalPos[0], originalPos[1] }; // deneme
        Debug.Log("old X: " + newPos[0] + "  Y: " + newPos[1]);
        switch (dir)
        {
            case 0:
                newPos[0] += 1;
                break;
            case 1:
                newPos[0] -= 1;
                break;
            case 2:
                newPos[1] -= 1;
                break;
            case 3:
                newPos[1] += 1;
                break;
            default:
                
                break;

        }
        return newPos;
    }
    private Vector3 CalcSingPos(int a, Vector3 pos)
    {//yukari->1 asagi->2 sol->3 sag->4
        //Vector3 pos = new Vector3(pPos[1], 0.4f, pPos[0]);
        switch (a)
        {
            case 0:
                pos.z += 1;
                break;
            case 1:
                pos.z -= 1;
                break;
            case 2:
                pos.x -= 1;
                break;
            case 3:
                pos.x += 1;
                break;

        }
        pos.y = -0.4f;
        return pos;

    }
    
    private Material ChangeColor(int value, Material m)
    {
        value = Mathf.Clamp(value, 0, 100);
        float r, g, b, a = 255;
        if (value == 0)
        {
            r = 104; g = 97; b = 92; a = 0;                 // SIFIR islemi

        }
        else if (value <= 30)
        {
            r = 255; b = 0;
            g = 50+ (value / 30) * 205;
        }
        else
        {
            g = 255; b = 0;
            r = 255 - ((value - 30) / 70) * 255;
            if (value > 60 && value <90) r -= 40;
            if (value > 90) r = 0;
        }
        m.color = new Color(r/255, g/255, b/255, a/255);
        return m;
    }

}
