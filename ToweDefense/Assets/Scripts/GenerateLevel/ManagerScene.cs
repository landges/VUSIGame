using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.Text;
using System.IO;

public class ManagerScene : Loader<ManagerScene>
{
    [SerializeField]
    private GameObject[] tilePrefabs;
    private XmlElement xRoot;

    [SerializeField]
    private Transform map;
    public Point spawnPoint;
    public Point finishPoint;
    public TileScript spawn;
    private Stack<Node> path;
    public Stack<Node> Path
    {
        get
        {
            if (path == null)
            {
                GeneratePath();
            }
            return new Stack<Node>(new Stack<Node>(path));
        }
    }
    private Point mapSize;
    public Dictionary<Point,TileScript> Tiles{get;set;}
    public float TileSize
    {
        get
        {
            return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        ReadXML();
        CreateLevel();
        SetLevelParam();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CreateLevel()
    {
        // Dictionary<string,int> newdict=new Dictionary<string,int>();
        // newdict.Add("S",1);
        // Debug.Log(newdict['S']);
        Tiles = new Dictionary<Point,TileScript>();
        string[] mapData= ReadLevel();

        mapSize = new Point(mapData[0].ToCharArray().Length,mapData.Length);

        int mapX=mapData[0].ToCharArray().Length;
        int mapY=mapData.Length;
        Vector3 worldStart= Camera.main.ScreenToWorldPoint(new Vector3(0,Screen.height));
        for(int y=0;y<mapY;y++)
        {
            char[] newTiles=mapData[y].ToCharArray();
            for(int x=0;x<mapX;x++)
            {
                PlaceTile(newTiles[x].ToString(),x,y,worldStart);
            }
        }
        // Instantiate(tile);
    }
    public bool DefWalAble(int indexTile)
    {
        int[] array = { 0,7 };
        if (!Array.Exists(array, v => v == indexTile))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void PlaceTile(string tileType,int x,int y,Vector3 worldStart)
    {
        int tileIndex=int.Parse(tileType);
        bool WalkAble=DefWalAble(tileIndex);
        TileScript newTile=Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();
        newTile.Setup(new Point(x,y),new Vector3(worldStart.x+ (TileSize*x),worldStart.y - (TileSize*y),0),map,WalkAble);
        if (tileIndex==8)
        {
            spawnPoint=new Point(x,y);
            spawn=newTile;
        } 
        if(tileIndex == 9)
        {
            finishPoint=new Point(x,y);
        }
    }
    private string[] ReadLevel()
    {
        
        string data =  xRoot.SelectSingleNode("map").InnerText;
        data = data.Replace("\t",string.Empty).Replace("\n",string.Empty).Replace("\r",string.Empty);
        return data.Split('-');
    }
    private void ReadXML()
    {
        
        TextAsset textAsset = (TextAsset) Resources.Load(@"level2");
        string xmlData=textAsset.text;
        string msg_xml = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        if (xmlData.StartsWith(msg_xml))
        {
            xmlData = xmlData.Remove(0, msg_xml.Length-1);
        }
        XmlDocument xmldoc = new XmlDocument ();
        xmldoc.LoadXml(xmlData);
        xRoot = xmldoc.DocumentElement;
    }
    private void SetLevelParam()
    {
        int health=int.Parse(xRoot.SelectSingleNode("int[@name='health']").Attributes["value"].Value);
        Manager.Instance.TotalHealth=health;
        Manager.Instance.Health=health;
        int money=int.Parse(xRoot.SelectSingleNode("int[@name='money']").Attributes["value"].Value);
        Manager.Instance.TotalMoney=money;
    }
    public bool InBounds(Point position)
    {
        return position.X>=0 && position.Y >=0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }
    public void GeneratePath()
    {
        path=Astar.GetPath(spawnPoint,finishPoint);
    }
}
