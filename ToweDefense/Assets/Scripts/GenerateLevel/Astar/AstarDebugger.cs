using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarDebugger : MonoBehaviour
{
    [SerializeField]
    private TileScript start,goal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // void Update()
    // {
    //     ClickTile();

    //     if(Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Astar.GetPath(start.GridPosition,goal.GridPosition);
    //     }
    // }
    private void ClickTile()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);
            if (hit.collider !=null)
            {
                TileScript tmp=hit.collider.GetComponent<TileScript>();
                if(tmp!=null)
                {
                    if(start ==null)
                    {
                        start=tmp;
                        start.SpriteRenderer.color=new Color32(255,132,0,255);
                    }
                    else if(goal ==null)
                    {
                        goal=tmp;
                        goal.SpriteRenderer.color=new Color32(255,0,0,255);
                    }
                }
            }
        }
    }
    public void DebugPath(HashSet<Node> openList,HashSet<Node> closedList,Stack<Node> path)
    {
        foreach(Node node in openList)
        {
            if (node.TileRef !=start)
            {
                node.TileRef.SpriteRenderer.color=Color.cyan;
            }
            // PointToParent(node,node.TileRef.WorldPosition);
        }
        // foreach(Node node in closedList)
        // {
        //     if (node.TileRef !=goal)
        //     {
        //         node.TileRef.SpriteRenderer.color=Color.red;
        //     }
        //     // PointToParent(node,node.TileRef.WorldPosition);
        // }
        foreach (Node node in path)
        {
            if (node.TileRef !=goal && node.TileRef !=start)
            {
                node.TileRef.SpriteRenderer.color=Color.green;
            }
        }
    }
    private void PointToParent(Node node,Vector2 position)
    {

    }
}
