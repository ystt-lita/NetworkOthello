using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OthelloCell : MonoBehaviour,IPointerClickHandler
{
    OthelloGame board;
    bool isPut;
    public bool isBlack{get;private set;}
    Image cell;
    public int x,y;
    // Start is called before the first frame update
    void Start()
    {
        board=transform.parent.GetComponent<OthelloGame>();
        cell=transform.Find("Disk").GetComponent<Image>();
        isPut=false;
        isBlack=false;
    }
    public bool isAlive(){
        return cell.enabled;
    }
    public void Put(bool b){
        isPut=true;
        isBlack=b;
    }
    public void Reverse(){
        isBlack=!isBlack;
        cell.color=isBlack?Color.black:Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPut){
            Debug.Log(string.Format("{0},{1}:put {2}",x,y,isBlack?"black":"white"));
            cell.enabled=true;
            cell.color=isBlack?Color.black:Color.white;
            isPut=false;
        }
    }
    public void OnPointerClick(PointerEventData p){
            Debug.Log("Clicked");
        if(p.button==PointerEventData.InputButton.Left){
            if(cell.enabled){
                //already put;
                return;
            }
            board.Click(this);
        }
    }
}
