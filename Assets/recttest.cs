using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recttest : MonoBehaviour
{
    RectTransform t;
    public float a,b,c,d;
    // Start is called before the first frame update
    void Start()
    {
        t=GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

        t.offsetMin=new Vector2(a,b);
        t.offsetMax=new Vector2(c,d);
    }
}
