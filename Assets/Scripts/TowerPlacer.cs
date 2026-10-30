using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerPlacer : MonoBehaviour
{
   public Tilemap placementMap;
   public Tilemap nonPlacebleMap;
   public GameObject GhostPrefab;
   public Button upgradeButton;
   

    private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();
    private GameObject ghostInstance;
    private Money money;

    void Awake()
    {
        money = GetComponent<Money>();
        if (upgradeButton != null)
        {
            upgradeButton.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        HandlePlacementHover();
        HandlePlacementClick();

    }

    void HandlePlacementHover()
    {
        if(TowerSelectionUI.selectedTowerPrefab == null)
        {
            if(ghostInstance != null)
            {
                Destroy(ghostInstance);
            }
            return;
        }
        if(ghostInstance == null)
        {
            ghostInstance = Instantiate(GhostPrefab);
        }
        ghostInstance.GetComponent<SpriteRenderer>().sprite = TowerSelectionUI.selectedTowerPrefab.GetComponent<SpriteRenderer>().sprite;
        Shoot selectedTower = TowerSelectionUI.selectedTowerPrefab.GetComponent<Shoot>();
        if (selectedTower == null)
        {
            Debug.LogError("Selected tower prefab must have a Shoot component.", TowerSelectionUI.selectedTowerPrefab);
            return;
        }
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3Int cellPosition = placementMap.WorldToCell(mouseWorldPos);

        Vector3 WorldCenter = placementMap.GetCellCenterWorld(cellPosition);

        WorldCenter.z = 0f;

        ghostInstance.transform.position = WorldCenter + new Vector3(0, placementMap.cellSize.y * 0.25f);

        bool valid = placementMap.HasTile(cellPosition) && !occupiedTiles.Contains(cellPosition) && money.CanAffordTower(selectedTower.towerCost);

        ghostInstance.GetComponent<GhostTower>().SetValid(valid);
    }
    

    void HandlePlacementClick()
    {
        if(!Input.GetMouseButtonDown(0)) return;
        if(TowerSelectionUI.selectedTowerPrefab == null) return;
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        Shoot selectedTower = TowerSelectionUI.selectedTowerPrefab.GetComponent<Shoot>();
        if (selectedTower == null)
        {
            Debug.LogError("Selected tower prefab must have a Shoot component.", TowerSelectionUI.selectedTowerPrefab);
            return;
        }
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector3Int cellPosition = placementMap.WorldToCell(mouseWorldPos);

        if(!placementMap.HasTile(cellPosition)) return;
        if(occupiedTiles.Contains(cellPosition)) return;
        if(!money.CanAffordTower(selectedTower.towerCost)) return;

        GameObject tower = Instantiate(TowerSelectionUI.selectedTowerPrefab, ghostInstance.transform.position, Quaternion.identity);
        Upgrade upgrade = tower.GetComponent<Upgrade>();
        if (upgrade != null)
        {
            upgrade.SetUpgradeButton(upgradeButton);
        }
        money.SpendMoney(selectedTower.towerCost);

        TowerSelectionUI.selectedTowerPrefab = null;

        occupiedTiles.Add(cellPosition);
    }



}