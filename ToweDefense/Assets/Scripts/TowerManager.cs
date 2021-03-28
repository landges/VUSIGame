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
    private RaycastHit2D hitTile;
    private TowerControl selectTower;

    // Start is called before the first frame update
    void Start()
    {
        panelIsOpen=false;
        towerPanel.SetActive(false);
        // backBtn=GetComponent<Button>();
        backBtn.gameObject.SetActive(false);
        buildTile = GetComponent<Collider2D>();
        selectTower=GetComponent<TowerControl>();
        // hitTile = GetComponent<RaycastHit2D>();
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
                towerPanel.SetActive(true);
                backBtn.gameObject.SetActive(true);
                buildTile = hit.collider;
                hitTile = hit;
                if(hit.collider.tag == "TowerSide")
                {
                    towerInfo.SetActive(false);
                    choiceTower.SetActive(true);
                }
                else if(hit.collider.tag == "TowerFull")
                {
                    choiceTower.SetActive(false);
                    towerInfo.SetActive(true);
                    foreach(TowerControl tower in TowerList)
                    {
                        if(tower.transform.position == hit.transform.position){
                            selectTower=tower;
                            break;
                        }
                    }
                    if(selectTower!=null){
                        ViewTowerInfo(selectTower);
                    }
                }
               
                // buildTile.tag = "TowerFull";
                
                // RegisterBuildSite(buildTile);
                // PlaceTower(hit);
            }
            // else{
            //     towerPanel.SetActive(false);
            // }		
		}
    }
    public void ViewTowerInfo(TowerControl tower)
    {
        radiusLabel.text="Radius: "+tower.attackRadius.ToString();
        damageLabel.text="Damage: "+tower.projectile.AttackDamage;
        // towerImage=tower.SpriteRenderer().Sprite;
        SpriteRenderer m_SpriteRenderer = tower.GetComponent<SpriteRenderer>();
        Sprite sprite=m_SpriteRenderer.sprite;
        towerImage.sprite=sprite;
    }
    public void ClocePanel()
    {
        towerPanel.SetActive(false);
        backBtn.gameObject.SetActive(false);
        // backBtn.SetActive(false);
    }
    public void DestroyTower()
    {
        Debug.Log("destroy");
        
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
