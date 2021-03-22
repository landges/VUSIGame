using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : Loader<TowerManager>
{
    public TowerButton towerBtnPressed{get; set;}
    SpriteRenderer spriteRenderer;
    bool panelIsOpen;
    [SerializeField]
    GameObject towerPanel;
    [SerializeField]
    public Button backBtn;
    private List<TowerControl> TowerList = new List<TowerControl>();
    private List<Collider2D> BuildList = new List<Collider2D>();
    private Collider2D buildTile;
    private RaycastHit2D hitTile;
    // Start is called before the first frame update
    void Start()
    {
        panelIsOpen=false;
        towerPanel.SetActive(false);
        // backBtn=GetComponent<Button>();
        backBtn.gameObject.SetActive(false);
        buildTile = GetComponent<Collider2D>();
        // hitTile = GetComponent<RaycastHit2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
			Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);
            if (hit.collider && hit.collider.tag == "TowerSide")
            {
                towerPanel.SetActive(true);
                backBtn.gameObject.SetActive(true);
                buildTile = hit.collider;
                hitTile = hit;
                // buildTile.tag = "TowerFull";
                
                // RegisterBuildSite(buildTile);
                // PlaceTower(hit);
            }
		
		}
    }
    public void ClocePanel()
    {
        towerPanel.SetActive(false);
        backBtn.gameObject.SetActive(false);
        // backBtn.SetActive(false);
    }
    public void RegisterBuildSite(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }
    // покупка башни: вычет денег из общего счета игрока
    public void BuyTower(int price)
    {
        Manager.Instance.SubtractMoney(price);
    }
    public void RegisterTower(TowerControl tower)
    {
        TowerList.Add(tower);
    }
    public void RenameTagBuildSite()
    {
        foreach(Collider2D buildTag in BuildList)
        {
            buildTag.tag = "TowerSide";
        }
        BuildList.Clear();
    }
    public void DestroyAllTowers()
    {
        foreach(TowerControl tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }
    public void PlaceTower(RaycastHit2D hit)
    {
        if (towerBtnPressed !=null)
        {
            TowerControl newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            BuyTower(towerBtnPressed.TowerPrice);
            Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.TowerBuilt);
            RegisterTower(newTower);
            // DisableDrag();
        }
    }
    public void SelectTower(TowerButton towerSelected)
    {
        if (towerSelected.TowerPrice <= Manager.Instance.TotalMoney)
        {
            towerBtnPressed = towerSelected;
            // EnableDrag(towerBtnPressed.DragSprite);
            buildTile.tag = "TowerFull";
                
            RegisterBuildSite(buildTile);
            PlaceTower(hitTile);
        }
        
        
        
    }
}
