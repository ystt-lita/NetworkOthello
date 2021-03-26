using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WYUN;
using WYUN.Deserialization;

public enum GameState
{
    Uninit,
    Init,
    WaitingPlayer,
    EncounterClicked,
    Num
}

public class OthelloGame : MonoBehaviour
{
    [SerializeField]
    PlayerInfo myInfo, encounterInfo;
    [SerializeField]
    DiskCount counter;
    [SerializeField]
    GameObject cellPrefab;
    OthelloCell[,] cells;
    public int clickedx,clickedy;
    // Start is called before the first frame update

    public GameState state;
    void Start()
    {
        state = GameState.Uninit;
        cells = new OthelloCell[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                cells[i, j] = Instantiate(cellPrefab, transform).GetComponent<OthelloCell>();
                cells[i, j].x = i; cells[i, j].y = j;
            }
        }
        counter.SetCount(2, 4);
        //自分が置いてひっくり返す時、counter.SetCount(counter.count+ひっくり返す数,counter.allcount+1)とかやれば良さそう

        //cells[3,3]は黒
        //cells[4,4]は黒
        //cells[3,4]は白
        //cells[4,3]は白
    }
    public void init()
    {
        cells[3, 3].Put(true);
        cells[4, 4].Put(true);
        cells[3, 4].Put(false);
        cells[4, 3].Put(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.Init:
                init();
                state = GameState.WaitingPlayer;
                break;
                case GameState.EncounterClicked:
                EncounterClick(clickedx,clickedy);
                state=GameState.WaitingPlayer;
                break;
        }
    }
    int Check(int x, int y, bool color)
    {
        Debug.Log(String.Format("{0},{1},{2}", x, y, color));
        int count = 0, tmpCount = 0;
        for (int i = x - 1, j = y - 1; i > 0 && j > 0; i--, j--)
        {
            //左上確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("左上空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("左上同色");
                count += tmpCount;
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x - 1, j = y; i > 0; i--)
        {
            //左確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("上空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("上同色");
                count += tmpCount;
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x - 1, j = y + 1; i > 0 && j < 8; i--, j++)
        {
            //左下確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("右上空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("右上同色");
                count += tmpCount;
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x, j = y + 1; j < 8; j++)
        {
            //下確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("右空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("右同色");
                count += tmpCount;
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x + 1, j = y + 1; i < 8 && j < 8; i++, j++)
        {
            //右下確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("右下空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("右下同色");
                count += tmpCount;
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x + 1, j = y; i < 8; i++)
        {
            //右確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("下空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("下同色");
                count += tmpCount;
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x + 1, j = y - 1; i < 8 && j > 0; i++, j--)
        {
            //右上確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("左下空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("左下同色");
                count += tmpCount;
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x, j = y - 1; j > 0; j--)
        {
            //上確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("左空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("左同色");
                count += tmpCount;
                break;
            }
            tmpCount++;
        }
        return count;
    }
    void Flip(int x, int y, bool color)
    {
        int tmpCount = 0;
        for (int i = x - 1, j = y - 1; i > 0 && j > 0; i--, j--)
        {
            //左上確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("左上空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("左上同色");
                for (i++, j++; i != x || j != y; i++, j++)
                {
                    cells[i, j].Reverse();
                }
                cells[x, y].Put(color);
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x - 1, j = y; i > 0; i--)
        {
            //左確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("上空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("上同色");
                for (i++; i != x; i++)
                {
                    cells[i, j].Reverse();
                }
                cells[x, y].Put(color);
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x - 1, j = y + 1; i > 0 && j < 8; i--, j++)
        {
            //左下確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("右上空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("右上同色");
                for (i++, j--; i != x || j != y; i++, j--)
                {
                    cells[i, j].Reverse();
                }
                cells[x, y].Put(color);
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x, j = y + 1; j < 8; j++)
        {
            //下確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("右空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("右同色");
                for (j--; j != y; j--)
                {
                    cells[i, j].Reverse();
                }
                cells[x, y].Put(color);
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x + 1, j = y + 1; i < 8 && j < 8; i++, j++)
        {
            //右下確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("右下空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("右下同色");
                for (i--, j--; i != x || j != y; i--, j--)
                {
                    cells[i, j].Reverse();
                }
                cells[x, y].Put(color);
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x + 1, j = y; i < 8; i++)
        {
            //右確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("下空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("下同色");
                for (i--; i != x; i--)
                {
                    cells[i, j].Reverse();
                }
                cells[x, y].Put(color);
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x + 1, j = y - 1; i < 8 && j > 0; i++, j--)
        {
            //右上確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("左下空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("左下同色");
                for (i--, j++; i != x || j != y; i--, j++)
                {
                    cells[i, j].Reverse();
                }
                cells[x, y].Put(color);
                break;
            }
            tmpCount++;
        }
        tmpCount = 0;
        for (int i = x, j = y - 1; j > 0; j--)
        {
            //上確認
            if (!cells[i, j].isAlive())
            {
                Debug.Log("左空白");
                break;
            }
            if (cells[i, j].isBlack == color)
            {
                Debug.Log("左同色");
                for (j++; j != y; j++)
                {
                    cells[i, j].Reverse();
                }
                cells[x, y].Put(color);
                break;
            }
            tmpCount++;
        }
    }

    public void Click(OthelloCell clicked)
    {
        if (state != GameState.WaitingPlayer) return;
        int count = Check(clicked.x, clicked.y, counter.isBlack);
        if (count > 0)
        {
            Flip(clicked.x, clicked.y, counter.isBlack);
            Core.Tell(encounterInfo.pName, string.Format("{0},{1}", clicked.x, clicked.y));
        }
        Debug.Log(count);
    }
    public void EncounterClick(int x, int y)
    {
        Debug.Log(string.Format("{0},{1}",x,y));
        int count = Check(x, y, !counter.isBlack);
        if (count > 0)
        {
            Flip(x, y, !counter.isBlack);
        }
        Debug.Log(count);
    }
}
