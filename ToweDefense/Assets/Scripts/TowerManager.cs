﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : Loader<TowerManager>
{
    public TowerButton towerBtnPressed{get; set;}
    SpriteRenderer spriteRenderer;
    [SerializeField]
    public Transform towerPanel;
    [SerializeField]
    public GameObject towerInfo;
    [SerializeField]
    public GameObject choiceTower;

    [SerializeField]
    Text infoLabel;
    [SerializeField]
    Text upgradePriceLabel;
    [SerializeField]
    Text sellPriceLabel;
    [SerializeField]
    Image towerImage;
    [SerializeField]
    public Button upgradeBtn;

    private List<TowerControl> TowerList = new List<TowerControl>();
    private List<Collider2D> BuildList = new List<Collider2D>();
    private Collider2D buildTile;
    private Collider2D buildTileOld = null;
    private RaycastHit2D hitTile;
    private TowerControl selectTower;
    // Start is called before the first frame update
    void Start()
    {
        towerPanel.gameObject.SetActive(false);
        buildTile = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        // if (Input.GetMouseButtonDown(0) && Input.touchCount > 0)
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);
            if (hit.collider && (hit.collider.tag == "TowerSide"  || hit.collider.tag == "TowerFull") && !EventSystem.current.IsPointerOverGameObject())
            // if (hit.collider && (hit.collider.tag == "TowerSide"  || hit.collider.tag == "TowerFull") && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                //buildTile.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                towerPanel.gameObject.SetActive(true);
                buildTile = hit.collider;
                SelectTile();
                hitTile = hit;
                if(hit.collider.tag == "TowerSide")
                {

                    if(selectTower != null){
                        selectTower.DisableRange();
                    }
                    DisableChilds();
                    choiceTower.SetActive(true);
                }
                else if(hit.collider.tag == "TowerFull")
                {
                    DisableChilds();
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
            else if(hit.collider && hit.collider.tag == "Respawn" && !EventSystem.current.IsPointerOverGameObject())
            // else if(hit.collider && hit.collider.tag == "Respawn" && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                buildTile = hit.collider;
                SelectTile();
                if(selectTower != null)
                {
                    selectTower.DisableRange();
                }
                Manager.Instance.ShowWavesInfo();
            }
            else if(!EventSystem.current.IsPointerOverGameObject())
            // else if(!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                ClocePanel();
            }
		}
        if(selectTower != null)
        {
            if(selectTower.NextUpgrade != null)
            {
                if (selectTower.NextUpgrade.Price <= Manager.Instance.TotalMoney)
                {
                    upgradeBtn.interactable=true;
                }
                else{
                    upgradeBtn.interactable=false;
                }
            }
            
        }
    }
    public void SelectTile()
    {
        if (buildTileOld != null && buildTile != buildTileOld)
        {
            buildTileOld.GetComponent<SpriteRenderer>().color = Color.white;
        }
        buildTile.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.9843137f, 0.4823529f, 1, 1); ;
        buildTileOld = buildTile;
    }
    public void DisableChilds() // Вызывешь где-нибудь, например, через по нажатию UI кнопки
    {
        for (int i = 0; i < towerPanel.childCount; i++)
        {
            towerPanel.GetChild(i).gameObject.SetActive(false); // Выключаем или включаем каждого полученного ребёнка по порядку.
        }
        towerPanel.Find("ClosePanel").gameObject.SetActive(true);

    }
    public void ViewTowerInfo()
    {
        infoLabel.text=selectTower.GetStats();
        if (selectTower.NextUpgrade ==null)
        {
            upgradeBtn.interactable=false;
            upgradePriceLabel.text="Max lvl!";
            // upgradeBtn.GetChild(1).SetActive(false);
        }
        else
        {
            upgradeBtn.interactable=true;
            upgradePriceLabel.text="for "+selectTower.NextUpgrade.Price; 
        }
        sellPriceLabel.text=selectTower.sellPrice.ToString();
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
        towerPanel.gameObject.SetActive(false);
        if (buildTileOld != null)
        {
            buildTileOld.GetComponent<SpriteRenderer>().color = Color.white;
        }
        //backBtn.SetActive(false);
        //DestrTower();
    }
    public void TowerUpgrade()
    {
        if(selectTower != null)
        {
            if(selectTower.Level <= selectTower.Upgrades.Length && Manager.Instance.TotalMoney >= selectTower.NextUpgrade.Price)
            {
                selectTower.Upgrade();
            }
            ViewTowerInfo();
        }
    }
    public void DestrTower()
    {
        Manager.Instance.TotalMoney += selectTower.sellPrice;
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
        DisableChilds();
        choiceTower.SetActive(true);
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
            newTower.Start();
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
