using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

//responsible to manage Units Data Detais while add and remove
public class UnitsManager : MonoBehaviour
{
    public static UnitsManager Instance;
    public LevelTroops _LevelTroopData;
    public int _TotalTroopCount = 0;
    public Dictionary<UnitItem, int> _UnitItemsList = new Dictionary<UnitItem, int>();
    public List<GameObject> _MirrorPlacableTiles = new List<GameObject>();
    public List<UnitDetails> _UnitItemsTempList = new List<UnitDetails>(); // which controls the count of units in both main and selected units

    [SerializeField] private TextMeshProUGUI m_Totalslot;
    [SerializeField] private TextMeshProUGUI m_slot;

    [SerializeField] private Transform m_SelectionUnitParent;
    [SerializeField] private Transform m_SelectedUnitParent;
    [SerializeField] private Transform m_DefaultUnitParent;
    [SerializeField] private float _OtherMirrorInRadius;
    [SerializeField] private Tilemap Tilemap;

    private void Awake()
    {
        Instance = this;

        InitializeUnitsDataList();

        UpdateTroopSlot();

        EventActions._DropUnitOnGround += DropUnit; // will be invoke while drop in ground and remove selected units of wave
        EventActions._AddUnit += AddUnit; // will be invoke while select units for a wave
    }

    //to initalize unitsdata
    private void InitializeUnitsDataList()
    {

        int index = 0;
        foreach (UnitItem item in _LevelTroopData.DefaultUnits)
        {
            _TotalTroopCount += item._UnitData.SlotCost;
            if (_UnitItemsList.ContainsKey(item))
            {
                _UnitItemsList[item]++;
            }
            else
            {
                _UnitItemsList.Add(item, 1);
                index++;
            }
        }
        foreach (var item in _UnitItemsList)
        {
            UnitDetails udet = new UnitDetails(item.Key._UnitData,item.Value);
            _UnitItemsTempList.Add(udet);
        }
    }

    //to update count and total slot count
    public void UpdateTroopSlot()
    {
        m_Totalslot.text = _LevelTroopData.maxTroopsSlot.ToString();
        m_slot.text = _TotalTroopCount.ToString();
    }

    public void DropUnit(TroopType trooptype)
    {
        int slotcost = 0;
        foreach (Transform unititem in m_SelectedUnitParent)
        {
            if (unititem.GetComponent<UnitUIItem>() && unititem.GetComponent<UnitUIItem>()._TroopType == trooptype)
            {
                unititem.GetComponent<UnitUIItem>().UpdateUnitItemCount(true);
                slotcost = unititem.GetComponent<UnitUIItem>().m_UnitSlotCost;

            }
        }
        foreach (Transform unititem in m_DefaultUnitParent)
        {
            if (unititem.GetComponent<UnitUIItem>() && unititem.GetComponent<UnitUIItem>()._TroopType == trooptype)
            {
                unititem.GetComponent<UnitUIItem>().UpdateUnitItemCount(true);
            }
        }
        if ((_TotalTroopCount - slotcost) >= 0)
        {
            _TotalTroopCount -= slotcost;
            UpdateTroopSlot();
        }
    }

    // while select Units
    public void AddUnit(TroopType trooptype)
    {
        int slotcost = 0;
        foreach (Transform unititem in m_SelectedUnitParent)
        {
            if (unititem.GetComponent<UnitUIItem>() && unititem.GetComponent<UnitUIItem>()._TroopType == trooptype)
            {
                unititem.GetComponent<UnitUIItem>().UpdateUnitItemCount(false);
                slotcost = unititem.GetComponent<UnitUIItem>().m_UnitSlotCost;

            }
        }

        foreach (Transform unititem in m_DefaultUnitParent)
        {
            if (unititem.GetComponent<UnitUIItem>() && unititem.GetComponent<UnitUIItem>()._TroopType == trooptype)
            {
                unititem.GetComponent<UnitUIItem>().UpdateUnitItemCount(false);

            }
        }

        if ((_TotalTroopCount + slotcost) <= _LevelTroopData.maxTroopsSlot)
        {
            _TotalTroopCount += slotcost;
            UpdateTroopSlot();
        }
    }

    //to check already any mirror there in given radius
    public bool IsMirrorDetected(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapSphere(pos,_OtherMirrorInRadius);
        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.gameObject.tag == "mirror")
                {
                    return false;
                }
            }
        }
        return true;
    }

    public GameObject IsMirrorPlacable(GameObject tileobj)
    {
        if (_MirrorPlacableTiles.Count > 0)
        {
            foreach (var tile in _MirrorPlacableTiles)
            {
                if(tile == tileobj)
                {
                    return tile;
                }
            }
        }
        return null;
    }

    public void HighlightMirrorTiles(bool h)
    {
        if (_MirrorPlacableTiles.Count > 0)
        {
            foreach (var tile in _MirrorPlacableTiles)
            {
                tile.gameObject.GetComponent<TileObject>().HighLightTile(h ? 3f : 0f);
            }
        }

    }
}
public class UnitDetails
{
    public UnitsData UnitsData;
    public int Count;

    public UnitDetails(UnitsData ud,int count) 
    {
        this.UnitsData = ud;
        this.Count = count;
    }
}
