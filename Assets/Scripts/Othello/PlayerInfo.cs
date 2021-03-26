using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WYUN;

public class PlayerInfo : MonoBehaviour
{
    Text playerName;
    Image back;
    public string pName{get;private set;}
    public bool turn{get;private set;}
    Color red,white;
    public void setName(string n)
    {
        pName = n;
    }
    public void ChangeTurn()
    {
        turn = !turn;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerName = transform.Find("Name").gameObject.GetComponent<Text>();
        back = GetComponent<Image>();
        turn = false;
        red=new Color(1,0,0,0.4f);
        white=new Color(1,1,1,0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        if (pName != null)
        {
            playerName.text = pName;
            playerName.fontSize = 40;
        }
        back.color = turn ? red : white;
    }
}
