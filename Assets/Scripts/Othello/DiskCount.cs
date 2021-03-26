using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiskCount : MonoBehaviour
{
    [SerializeField]
    Image myDisks, encounterDisks;
    RectTransform md, ed;
    [SerializeField]
    public bool isBlack{get;private set;}
    public int count { get; private set; }
    public int allCount { get; private set; }
    public void SetColor(bool c){
        isBlack=c;
    }
    public void SetCount(int c, int m)
    {
        count = c;
        allCount = m;
        md.offsetMax=new Vector2(500*(float)count/allCount,0);
        ed.offsetMin=new Vector2(-500*(1-(float)count/allCount),0);
    }
    // Start is called before the first frame update
    void Start()
    {
        md = myDisks.gameObject.GetComponent<RectTransform>();
        ed = encounterDisks.gameObject.GetComponent<RectTransform>();
        if (isBlack)
        {
            myDisks.color = Color.black;
            encounterDisks.color = Color.white;
        }
        else
        {
            encounterDisks.color = Color.black;
            myDisks.color = Color.white;
        }
        SetCount(2, 4);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
