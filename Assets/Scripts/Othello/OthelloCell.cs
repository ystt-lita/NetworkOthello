using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OthelloCell : MonoBehaviour, IPointerClickHandler
{
    OthelloGame board;
    public bool isBlack { get; private set; }
    Image cell;
    public int x, y;
    public int[] reversible;
    Text[] revText;
    // Start is called before the first frame update
    void Awake()
    {
        reversible = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
    }
    void Start()
    {
        board = transform.parent.GetComponent<OthelloGame>();
        cell = transform.Find("Disk").GetComponent<Image>();
        isBlack = false;
        revText = transform.GetComponentsInChildren<Text>();
    }
    public bool isAlive()
    {
        return cell.enabled;
    }
    public void Put(bool b)
    {
        Debug.Log(string.Format("{0},{1}:put {2}", x, y, isBlack ? "black" : "white"));
        cell.enabled = true;
        isBlack = b;
        cell.color = isBlack ? Color.black : Color.white;
    }
    public void Reverse()
    {
        isBlack = !isBlack;
        cell.color = isBlack ? Color.black : Color.white;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SetRevText()
    {
        for (int i = 0; i < 8; i++) revText[i].text = reversible[i].ToString();
    }
    public void OnPointerClick(PointerEventData p)
    {
        Debug.Log("Clicked");
        if (p.button == PointerEventData.InputButton.Left)
        {
            if (cell.enabled)
            {
                //already put;
                return;
            }
            board.Click(this);
        }
    }
}
