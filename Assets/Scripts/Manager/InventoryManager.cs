using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] GameObject UI_Parent;
    [SerializeField] private InventoryData m_InvData;
    private List<GameObject> m_InventoryUIList = new List<GameObject>();
    void Start()
    {
        EventActions._SelectInv += GetMirror;
        EventActions._DropMirror += UpdateInvItemCount;
        ShowInvItems();
    }


    public void ShowInvItems()
    {
        foreach (var Invlist in m_InvData.MirrorInventoryList)
        {
          GameObject invui = Instantiate(Invlist.value._UIMirrorPrefab, UI_Parent.transform);
           m_InventoryUIList.Add(invui);
        }
    }

    public void GetMirror(MirrorVariation type)
    {
         EventActions._SelectedInvType = type;
    }

    public void UpdateInvItemCount(Mirror mirror)
    {

        foreach (var Invlist in m_InventoryUIList)
        {
            if(Invlist.GetComponent<Inventory>()._MirrorTypeData._MirrorType == mirror._MirrorType)
            {
                Invlist.GetComponent<Inventory>().UpdateInvCount();
            }
        }
    }
}
