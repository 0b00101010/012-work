using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    [SerializeField]
    private Animator anima1;
    [SerializeField]
    private Animator anima2;
    [SerializeField]
    private Animator anima3;

    public RuntimeAnimatorController[] anims;

    private int highScore;

    private int num1;
    private int num2;
    private int num3;

    [SerializeField]
    private RectTransform num1Transform;
    [SerializeField]
    private RectTransform num2Transform;
    [SerializeField]
    private RectTransform num3Transform;

    // Start is called before the first frame update
    void Start()
    {
        highScore = Prefs.HighScore;
        Debug.Log(highScore);
        NumAnimSet();
        //SetPosition(num1, num1Transform, 1);
        //SetPosition(num2, num2Transform, 2);
        //SetPosition(num3, num3Transform, 3);
    }

    private void nanum()
    {
        num3 = highScore / 100;
        num2 = highScore % 100 / 10;
        num1 = highScore % 10;
        print("첫째자리 " + num1 + " 둘째자리 " + num2 + " 셋째자리 " + num3);
    }

    private void SetPosition(int num, RectTransform number, int digit)
    {
        float positionX = 0;

        if (digit == 3)
        {
            positionX = -200;
        }
        else if (digit == 1)
        {
            positionX = 200;
        }
        else
        {
            positionX = 0;
        }

        number.anchoredPosition = new Vector2(positionX, number.anchoredPosition.y);

        switch (num)
        {
            case 1:
                number.anchoredPosition += new Vector2(3, 0);
                break;
            case 2:
                number.anchoredPosition += new Vector2(-49, 0);
                break;
            case 3:
                number.anchoredPosition += new Vector2(-54, 0);
                break;
            case 4:
                number.anchoredPosition += new Vector2(-94, 0);
                break;
            case 5:
                number.anchoredPosition += new Vector2(-31, 0);
                break;
            case 6:
                number.anchoredPosition += new Vector2(48, 0);
                break;
            case 7:
                number.anchoredPosition += new Vector2(-40, 0);
                break;
            case 8:
                number.anchoredPosition += new Vector2(-84, 0);
                break;
            case 9:
                number.anchoredPosition += new Vector2(89, 0);
                break;
            default:
                break;
        }

    }

    private void NumAnimSet()
    {
        nanum();

        for (int i = 0; i < anims.Length; i++)
        {
            if (num1 == i)
            {
                anima1.runtimeAnimatorController = anims[i];
            }
        }

        for (int i = 0; i < anims.Length; i++)
        {
            if (num2 == i)
            {
                anima2.runtimeAnimatorController = anims[i];
            }
        }

        for (int i = 0; i < anims.Length; i++)
        {
            if (num3 == i)
            {
                anima3.runtimeAnimatorController = anims[i];
            }
        }
    }
}
