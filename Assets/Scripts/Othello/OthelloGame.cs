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
    EncounterPassed,
    None
}

public class OthelloGame : MonoBehaviour
{
    [SerializeField]
    PlayerInfo myInfo, encounterInfo;
    [SerializeField]
    DiskCount counter;
    [SerializeField]
    GameObject cellPrefab, WinPanel, LosePanel;
    OthelloCell[,] cells;
    public int clickedx, clickedy;
    // Start is called before the first frame update

    public GameState state;
    void Start()
    {
        state = GameState.None;
        cells = new OthelloCell[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                cells[i, j] = Instantiate(cellPrefab, transform).GetComponent<OthelloCell>();
                cells[i, j].x = j; cells[i, j].y = i;
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
        cells[3, 3].Put(false);
        cells[4, 4].Put(false);
        cells[3, 4].Put(true);
        cells[4, 3].Put(true);
        counter.SetCount(2, 4);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.Uninit:
                init();
                if (myInfo.turn)
                {
                    counter.SetColor(true);
                }
                else
                {
                    counter.SetColor(false);
                }
                state = GameState.Init;
                break;
            case GameState.Init:
                CheckAll(counter.isBlack);
                state = GameState.WaitingPlayer;
                break;
            case GameState.EncounterClicked:
                EncounterClick(clickedx, clickedy);
                state = GameState.WaitingPlayer;
                break;
            case GameState.EncounterPassed:
                EncounterPass();
                state = GameState.WaitingPlayer;
                break;
        }
    }
    void Flip(int x, int y, bool color)
    {
        int flipped = 0;
        cells[y, x].Put(color);
        Debug.Log(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
        cells[y, x].reversible[0], cells[y, x].reversible[1], cells[y, x].reversible[2],
        cells[y, x].reversible[3], cells[y, x].reversible[4], cells[y, x].reversible[5],
        cells[y, x].reversible[6], cells[y, x].reversible[7]));
        for (int k = cells[y, x].reversible[0]; k > 0; k--)
        {
            flipped++;
            cells[y + k, x + k].Reverse();
        }
        for (int k = cells[y, x].reversible[1]; k > 0; k--)
        {
            flipped++;
            cells[y, x + k].Reverse();
        }
        for (int k = cells[y, x].reversible[2]; k > 0; k--)
        {
            flipped++;
            cells[y - k, x + k].Reverse();
        }
        for (int k = cells[y, x].reversible[3]; k > 0; k--)
        {
            flipped++;
            cells[y - k, x].Reverse();
        }
        for (int k = cells[y, x].reversible[4]; k > 0; k--)
        {
            flipped++;
            cells[y - k, x - k].Reverse();
        }
        for (int k = cells[y, x].reversible[5]; k > 0; k--)
        {
            flipped++;
            cells[y, x - k].Reverse();
        }
        for (int k = cells[y, x].reversible[6]; k > 0; k--)
        {
            flipped++;
            cells[y + k, x - k].Reverse();
        }
        for (int k = cells[y, x].reversible[7]; k > 0; k--)
        {
            flipped++;
            cells[y + k, x].Reverse();
        }
        if (color == counter.isBlack)
        {
            counter.SetCount(counter.count + flipped + 1, counter.allCount + 1);
        }
        else
        {
            counter.SetCount(counter.count - flipped, counter.allCount + 1);
        }
    }
    bool CheckAll(bool color)
    {
        int x = 0;
        int y = 0;
        int count;
        bool pass = true, c = false;
        foreach (var item in cells)
        {
            item.reversible = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        }
        for (; x < 8; x++)
        {//上の行
            count = 0; c = false;
            for (int i = y, j = x; i < 8; i++)
            {//下向き走査
                if (cells[i, j].isAlive())
                {
                    if (cells[i, j].isBlack == color)
                    {
                        c = true; count = 0;
                    }
                    else if (c) { count++; }
                }
                else
                {
                    if (c)
                    {
                        cells[i, j].reversible[3] = count;
                        if (count != 0)
                        {
                            pass = false;
                        }
                        c = false;
                    }
                    else { count = 0; }
                }
            }

            if (x == 7) break;
            count = 0; c = false;
            for (int i = y, j = x; i < 8 && j < 8; i++, j++)
            {//右下向き走査
                if (cells[i, j].isAlive())
                {
                    if (cells[i, j].isBlack == color)
                    {
                        c = true; count = 0;
                    }
                    else if (c) { count++; }
                }
                else
                {
                    if (c)
                    {
                        cells[i, j].reversible[4] = count;
                        if (count != 0)
                        {
                            pass = false;
                        }
                        c = false;
                    }
                    else { count = 0; }
                }
            }
            count = 0; c = false;
            for (int i = y, j = x; i < 8 && j >= 0; i++, j--)
            {//左下向き走査
                if (cells[i, j].isAlive())
                {
                    if (cells[i, j].isBlack == color)
                    {
                        c = true; count = 0;
                    }
                    else if (c) { count++; }
                }
                else
                {
                    if (c)
                    {
                        cells[i, j].reversible[2] = count;
                        if (count != 0)
                        {
                            pass = false;
                        }
                        c = false;
                    }
                    else { count = 0; }
                }
            }
        }
        for (x--; y < 8; y++)
        {//右の列
            count = 0; c = false;
            for (int i = y, j = x; j >= 0; j--)
            {//左向き走査
                if (cells[i, j].isAlive())
                {
                    if (cells[i, j].isBlack == color)
                    {
                        c = true; count = 0;
                    }
                    else if (c) { count++; }
                }
                else
                {
                    if (c)
                    {
                        cells[i, j].reversible[1] = count;
                        if (count != 0)
                        {
                            pass = false;
                        }
                        c = false;
                    }
                    else { count = 0; }
                }
            }
            if (y == 7) break;
            count = 0; c = false;
            for (int i = y, j = x; i < 8 && j >= 0; i++, j--)
            {//左下向き走査
                if (cells[i, j].isAlive())
                {
                    if (cells[i, j].isBlack == color)
                    {
                        c = true; count = 0;
                    }
                    else if (c) { count++; }
                }
                else
                {
                    if (c)
                    {
                        cells[i, j].reversible[2] = count;
                        if (count != 0)
                        {
                            pass = false;
                        }
                        c = false;
                    }
                    else { count = 0; }
                }
            }
            count = 0; c = false;
            for (int i = y, j = x; i >= 0 && j >= 0; i--, j--)
            {//左上向き走査
                if (cells[i, j].isAlive())
                {
                    if (cells[i, j].isBlack == color)
                    {
                        c = true; count = 0;
                    }
                    else if (c) { count++; }
                }
                else
                {
                    if (c)
                    {
                        cells[i, j].reversible[0] = count;
                        if (count != 0)
                        {
                            pass = false;
                        }
                        c = false;
                    }
                    else { count = 0; }
                }
            }
        }
        for (y--; x >= 0; x--)
        {//下の行
            count = 0; c = false;
            for (int i = y, j = x; i >= 0; i--)
            {//上向き走査
                if (cells[i, j].isAlive())
                {
                    if (cells[i, j].isBlack == color)
                    {
                        c = true; count = 0;
                    }
                    else if (c) { count++; }
                }
                else
                {
                    if (c)
                    {
                        cells[i, j].reversible[7] = count;
                        if (count != 0)
                        {
                            pass = false;
                        }
                        c = false;
                    }
                    else { count = 0; }
                }
            }
            if (x == 0) break;
            count = 0; c = false;
            for (int i = y, j = x; i >= 0 && j < 8; i--, j++)
            {//右上向き走査
                if (cells[i, j].isAlive())
                {
                    if (cells[i, j].isBlack == color)
                    {
                        c = true; count = 0;
                    }
                    else if (c) { count++; }
                }
                else
                {
                    if (c)
                    {
                        cells[i, j].reversible[6] = count;
                        if (count != 0)
                        {
                            pass = false;
                        }
                        c = false;
                    }
                    else { count = 0; }
                }
            }
            count = 0; c = false;
            for (int i = y, j = x; i >= 0 && j >= 0; i--, j--)
            {//左上向き走査
                if (cells[i, j].isAlive())
                {
                    if (cells[i, j].isBlack == color)
                    {
                        c = true; count = 0;
                    }
                    else if (c) { count++; }
                }
                else
                {
                    if (c)
                    {
                        cells[i, j].reversible[0] = count;
                        if (count != 0)
                        {
                            pass = false;
                        }
                        c = false;
                    }
                    else { count = 0; }
                }
            }
        }
        for (x++; y >= 0; y--)
        {//左の列
            count = 0; c = false;
            for (int i = y, j = x; j < 8; j++)
            {//右向き走査
                if (cells[i, j].isAlive())
                {
                    if (cells[i, j].isBlack == color)
                    {
                        c = true; count = 0;
                    }
                    else if (c) { count++; }
                }
                else
                {
                    if (c)
                    {
                        cells[i, j].reversible[5] = count;
                        if (count != 0)
                        {
                            pass = false;
                        }
                        c = false;
                    }
                    else { count = 0; }
                }
            }
            if (y == 0) break;
            count = 0; c = false;
            for (int i = y, j = x; i >= 0 && j < 8; i--, j++)
            {//右上向き走査
                if (cells[i, j].isAlive())
                {
                    if (cells[i, j].isBlack == color)
                    {
                        c = true; count = 0;
                    }
                    else if (c) { count++; }
                }
                else
                {
                    if (c)
                    {
                        cells[i, j].reversible[6] = count;
                        if (count != 0)
                        {
                            pass = false;
                        }
                        c = false;
                    }
                    else { count = 0; }
                }
            }
            count = 0; c = false;
            for (int i = y, j = x; i < 8 && j < 8; i++, j++)
            {//右下向き走査
                if (cells[i, j].isAlive())
                {
                    if (cells[i, j].isBlack == color)
                    {
                        c = true; count = 0;
                    }
                    else if (c) { count++; }
                }
                else
                {
                    if (c)
                    {
                        cells[i, j].reversible[4] = count;
                        if (count != 0)
                        {
                            pass = false;
                        }
                        c = false;
                    }
                    else { count = 0; }
                }
            }
        }
        Debug.Log("checked:" + (pass ? " " : "not ") + "pass");
        return pass;
    }

    public void Click(OthelloCell clicked)
    {
        if (state != GameState.WaitingPlayer) return;
        if (!myInfo.turn) return;
        if (clicked.reversible.Max() != 0)
        {
            Flip(clicked.x, clicked.y, counter.isBlack);
            Core.Tell(encounterInfo.pName, string.Format("{0},{1}", clicked.x, clicked.y));
            myInfo.ChangeTurn();
            encounterInfo.ChangeTurn();
        }
        Debug.Log(clicked.reversible.Max());
    }
    public void EncounterPass()
    {
        if (CheckAll(counter.isBlack))
        {
            Debug.Log("game set");
            Core.Tell(encounterInfo.pName, "check");
            WinLose();
        }
        myInfo.ChangeTurn();
        encounterInfo.ChangeTurn();
    }
    public void WinLose()
    {
        if (counter.count > counter.allCount / 2)
        {
            WinPanel.SetActive(true);
        }
        else
        {
            LosePanel.SetActive(true);
        }
    }
    public void EncounterClick(int x, int y)
    {
        Debug.Log(string.Format("{0},{1}", x, y));
        CheckAll(!counter.isBlack);
        Flip(x, y, !counter.isBlack);
        if (CheckAll(counter.isBlack))
        {
            Core.Tell(encounterInfo.pName, "pass");
        }
        else
        {
            myInfo.ChangeTurn();
            encounterInfo.ChangeTurn();
        }
    }
}
