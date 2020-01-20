using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int rowIndex;
    public int colIndex;

    public bool mergedThisTurn = false;

    Text tileText;
    Image tileImage;

    private int number;
    public int Number
    {
        get
        {
            return number;
        }
        set
        {
            number = value;
            if (number == 0)
            {
                SetVisible(false);
            }
            else
            {
                ApplyStyle(number);
                SetVisible(true);
            }
        }
    }


    private void Awake()
    {
        tileText = GetComponentInChildren<Text>();
        tileImage = transform.Find("Cell_Number").GetComponent<Image>();
    }

    void ApplyStyleFromController(int index)
    {
        tileText.text= TileStyleController.Instance.tileStyles[index].number.ToString();
        tileText.color = TileStyleController.Instance.tileStyles[index].textColor;
        tileImage.color = TileStyleController.Instance.tileStyles[index].tileColor;
    }

    void ApplyStyle(int num)
    {
        switch (num)
        {
            case 2:
                ApplyStyleFromController(0);
                break;
            case 4:
                ApplyStyleFromController(1);
                break;
            case 8:
                ApplyStyleFromController(2);
                break;
            case 16:
                ApplyStyleFromController(3);
                break;
            case 32:
                ApplyStyleFromController(4);
                break;
            case 64:
                ApplyStyleFromController(5);
                break;
            case 128:
                ApplyStyleFromController(6);
                break;
            case 256:
                ApplyStyleFromController(7);
                break;
            case 512:
                ApplyStyleFromController(8);
                break;
            case 1024:
                ApplyStyleFromController(9);
                break;
            case 2048:
                ApplyStyleFromController(10);
                break;
            case 4096:
                ApplyStyleFromController(11);
                break;
            default:
                Debug.LogError("Incorrect Number Argument for style application");
                break;

        }
    }

    void SetVisible(bool isVisible)
    {
        tileImage.enabled = isVisible;
        tileText.enabled = isVisible;
    }

}
