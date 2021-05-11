using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public Point GridPosition{get;set;}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Setup(Point gridPos,Vector3 worldPos){
        this.GridPosition=gridPos;
        transform.position=worldPos;
    }
}
