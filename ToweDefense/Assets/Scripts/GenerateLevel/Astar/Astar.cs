using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class Astar
{
    private static Dictionary<Point,Node> nodes;

    private static void CreateNodes()
    {
        nodes = new Dictionary<Point,Node>();
        foreach (TileScript tile in ManagerScene.Instance.Tiles.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }

    public static Stack<Node> GetPath(Point start,Point goal)
    {
        if (nodes == null)
        {
            CreateNodes();
        }
        HashSet<Node> openList = new HashSet<Node>();
        HashSet<Node> closedList = new HashSet<Node>();
        Stack<Node> finalPath=new Stack<Node>();
        Node currentNode = nodes[start];
        openList.Add(currentNode);
        while(openList.Count > 0)
        {
            for(int x=-1;x<=1;x++)
            {
                for(int y=-1;y<=1;y++)
                {
                    Point neighbourPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y-y);
                    if(ManagerScene.Instance.InBounds(neighbourPos) && ManagerScene.Instance.Tiles[neighbourPos].WalkAble && neighbourPos!=currentNode.GridPosition)
                    {
                        int gCost = 0;
                        if(Math.Abs(x-y)==1)
                        {
                            gCost=10;
                        }
                        else
                        {
                            if(!ConnectedDiagonally(currentNode,nodes[neighbourPos]))
                            {
                                continue;
                            }
                            gCost=14;
                        }
                        Node neighbour = nodes[neighbourPos];

                        if (openList.Contains(neighbour))
                        {
                            if (currentNode.G +gCost<neighbour.G)
                            {
                                neighbour.CalcValues(currentNode,nodes[goal],gCost);
                            }
                        }
                        else if(!closedList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                            neighbour.CalcValues(currentNode,nodes[goal],gCost);

                        }
                    }
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (openList.Count>0)
            {
                currentNode=openList.OrderBy(n=>n.F).First();
            }
            if(currentNode == nodes[goal])
            {
                while (currentNode.GridPosition != start)
                {
                    finalPath.Push(currentNode);
                    currentNode=currentNode.Parent;
                }
                
                break;
            }
        }
        
        // onlu debug
        // GameObject.Find("Debugger").GetComponent<AstarDebugger>().DebugPath(openList,closedList,finalPath);
        return finalPath;
    }
    private static bool ConnectedDiagonally(Node currentNode,Node neighbour)
    {
        Point direction = neighbour.GridPosition - currentNode.GridPosition;
        Point first = new Point(currentNode.GridPosition.X - direction.X,currentNode.GridPosition.Y+direction.Y);
        Point second = new Point(currentNode.GridPosition.X,currentNode.GridPosition.Y+direction.Y);
        Point three = new Point(currentNode.GridPosition.X,currentNode.GridPosition.Y-direction.Y);
        if(ManagerScene.Instance.InBounds(first) && !ManagerScene.Instance.Tiles[first].WalkAble)
        {
            return false;
        }
        if(ManagerScene.Instance.InBounds(second) && !ManagerScene.Instance.Tiles[second].WalkAble)
        {
            return false;
        }
        if(ManagerScene.Instance.InBounds(three) && !ManagerScene.Instance.Tiles[three].WalkAble)
        {
            return false;
        }
        return true;
    }
}
