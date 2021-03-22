using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerPanel : MonoBehaviour
{
    bool panelIsOpen;
    [SerializeField]
    GameObject towerPanel;
    [SerializeField]
    public Button backBtn;


    void Start()
    {
        panelIsOpen=false;
        towerPanel.SetActive(false);
        // backBtn=GetComponent<Button>();
        backBtn.gameObject.SetActive(false);
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
                // buildTile = hit.collider;
                // buildTile.tag = "TowerFull";
                Debug.Log("Towerside");
                towerPanel.SetActive(true);
                backBtn.gameObject.SetActive(true);
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
}
