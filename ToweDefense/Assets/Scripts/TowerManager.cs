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
    GameObject towerInfo;
    [SerializeField]
    GameObject choiceTower;
    [SerializeField]
    GameObject textChoiceTower;

    [SerializeField]
    Text damageLabel;
    [SerializeField]
    Text radiusLabel;
    [SerializeField]
    Image towerImage;
    [SerializeField]
    public Button backBtn;
    private List<TowerControl> TowerList = new List<TowerControl>();
    private List<Collider2D> BuildList = new List<Collider2D>();
    private Collider2D buildTile;
    private Collider2D buildTileOld = null;
    private RaycastHit2D hitTile;
    private TowerControl selectTower;
    
    // Start is called before the first frame update
    void Start()
    {
        panelIsOpen=false;
        towerPanel.SetActive(false);
        backBtn.gameObject.SetActive(false);
        buildTile = GetComponent<Collider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
			Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);
            if (hit.collider && (hit.collider.tag == "TowerSide"  || hit.collider.tag == "TowerFull"))
            {
                //buildTile.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                towerPanel.SetActive(true);
                backBtn.gameObject.SetActive(true);
                buildTile = hit.collider;
                if (buildTileOld != null && buildTile != buildTileOld)
                {
                    buildTileOld.GetComponent<SpriteRenderer>().color = Color.white;
                }
                buildTile.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                buildTileOld = buildTile;
                hitTile = hit;
                if(hit.collider.tag == "TowerSide")
                {

                    if(selectTower != null){
                        selectTower.DisableRange();
                    }
                    towerInfo.SetActive(false);
                    choiceTower.SetActive(true);
                    textChoiceTower.SetActive(true);
                }
                else if(hit.collider.tag == "TowerFull")
                {
                    choiceTower.SetActive(false);
                    textChoiceTower.SetActive(false);
                    towerInfo.SetActive(true);
                    foreach(TowerControl tower in TowerList)
                    {
                        if(tower.transform.position == hit.transform.position){
                            if (selectTower != null)
                            {
                                selectTower.DisableRange();
                            }
                            selectTower=tower;
                            break;
                        }
                    }
                    if(selectTower!=null){
                        ViewTowerInfo();
                    }
                }
            }
            
		}
    }
    public void ViewTowerInfo()
    {
        radiusLabel.text="Radius: "+selectTower.attackRadius.ToString();
        damageLabel.text="Damage: "+selectTower.projectile.AttackDamage;
        SpriteRenderer m_SpriteRenderer = selectTower.GetComponent<SpriteRenderer>();
        Sprite sprite=m_SpriteRenderer.sprite;
        towerImage.sprite=sprite;
        selectTower.EnableRange();
    }
    public void ClocePanel()
    {
        if (selectTower != null)
        {
            selectTower.DisableRange();
        }
        towerPanel.SetActive(false);
        backBtn.gameObject.SetActive(false);
        if (buildTileOld != null)
        {
            buildTileOld.GetComponent<SpriteRenderer>().color = Color.white;
        }
        //backBtn.SetActive(false);
        //DestrTower();
    }

    public void DestrTower()
    {
        Manager.Instance.TotalMoney += selectTower.sellPrice / 2;
        TowerList.Remove(this.selectTower);
        Collider2D selectBuild = null;
        foreach (Collider2D buildTag in BuildList)
        {
            if (buildTag.transform.position == selectTower.transform.position)
            {
                selectBuild = buildTag;
                buildTag.tag = "TowerSide";
                break;
            }
        }
        BuildList.Remove(selectBuild);
        Destroy(selectTower.gameObject);
        selectTower = null;
        towerInfo.SetActive(false);
        choiceTower.SetActive(true);
        textChoiceTower.SetActive(true);
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
            newTower.sellPrice = towerBtnPressed.TowerPrice;
            newTower.transform.position = hit.transform.position;
            BuyTower(towerBtnPressed.TowerPrice);
            Manager.Instance.AudioSrc.PlayOneShot(SoundManager.Instance.TowerBuilt);
            RegisterTower(newTower);
            if(selectTower != null){
                selectTower.DisableRange();
            }
            selectTower=newTower;
            choiceTower.SetActive(false);
            textChoiceTower.SetActive(false);
            towerInfo.SetActive(true);
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
            ViewTowerInfo();
        }
        
        
        
    }
}
