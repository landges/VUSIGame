using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public Point GridPosition{get;private set;}
    public bool IsEmpty{get;private set;}
    private Color32 fullColor=new Color32(255,118,118,255);
    private Color32 emptyColor=new Color32(96,255,90,255);
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer{get;set;}
    public bool WalkAble{get;set;}
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x,transform.position.y);
            // return new Vector2(transform.position.x-(GetComponent<SpriteRenderer>().bounds.size.x/2),transform.position.y-(GetComponent<SpriteRenderer>().bounds.size.y/2));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer=GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Setup(Point gridPos,Vector3 worldPos,Transform parent,bool walkAble)
    {
        WalkAble=walkAble;
        this.GridPosition=gridPos;
        transform.position=worldPos;
        transform.SetParent(parent);
        ManagerScene.Instance.Tiles.Add(gridPos,this);
    }
    public void ColorTile(Color newColor)
    {
        SpriteRenderer.color=newColor;
    }
}
