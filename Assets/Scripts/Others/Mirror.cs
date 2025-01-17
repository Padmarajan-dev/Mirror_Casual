using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour,IIUnityItem
{

    float rotangle;

    public float _RotSpeed = 80f;

    public bool _IsSelected = false;

    public MirrorVariation _MirrorType;

    public float yPos;



    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (_IsSelected && Input.GetAxis("Horizontal")!=0)
        {
            RotateMirror();
        }
    }

    //to rotate mirror
    public void RotateMirror()
    {
        float clampedrot = rotangle + Input.GetAxis("Horizontal") * _RotSpeed * Time.deltaTime;
        rotangle = Mathf.Clamp(clampedrot, -30f, 30f);
        transform.localRotation = Quaternion.Euler(new Vector3(0, clampedrot, -4.221f));
    }

    public void DropItem(GameObject go, Vector3 p)
    {
        if (go == this.gameObject)
        {
            this.gameObject.SetActive(true);

            this.gameObject.transform.position = p;
            UnitsManager.Instance.DropUnit(EventActions._SelectedUnitType);
        }
    }
}

[Serializable]
public enum MirrorVariation
{
    low,
    medium,
    none
}

//public class MirrorData
//{
//    public string _MirrorType;

//    public int _MirrorCount;

//    public int _TypeId;

//    public MirrorData(string type,int count,int typeid)
//    {
//        _MirrorType = type;
//        _MirrorCount = count;
//        _TypeId = typeid;
//    }
//}
