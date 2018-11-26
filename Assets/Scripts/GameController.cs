using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static int TableLightCount = 0;
    public static int MainLightCount = 0;
    public static int SofaLightCount = 0;
    public static int DecorateLightCount = 0;
    public static bool isBedTableLightEnable = false;
    public static bool isBedMainLightEnable = false;
    public static bool isBedSofaLightEnable = false;
    public static bool isBedDecorateLightEnable = false;



    public GameObject Ground;
    public GameObject Table1, Table2;
    public GameObject Bed;
    public GameObject Wall;
    public GameObject Sofa;
    public GameObject Ceiling;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TempLayout() {
        ClearAllLight();
        GenerateSpotLight(GetTableLightPos(Table1, Bed));
        GenerateSpotLight(GetTableLightPos(Table2, Bed));
        GeneratePointLight(GetMainLightPos());
        GeneratePointLight(GetLampPos(Sofa));
    }


    public void ClearAllLight() {
        GameObject[] allLight = GameObject.FindGameObjectsWithTag("LightGe");
        if (allLight.Length > 0) {
            foreach (GameObject go in allLight)
                Destroy(go);
        }

    }

    //所有子物体包围盒
    public Bounds CalcBounds(GameObject GO)
    {
        if (GO.GetComponent<MeshRenderer>() != null)
        {
            Vector3 center = Vector3.zero;
            center = GO.GetComponent<MeshRenderer>().bounds.center;
            Bounds bounds = new Bounds(center, Vector3.zero);
            bounds.Encapsulate(GO.GetComponent<MeshRenderer>().bounds);
            return bounds;
        }
        else
        {
            Vector3 center = Vector3.zero;

            foreach (Transform child in GO.transform)
            {
                center += child.GetComponent<MeshRenderer>().bounds.center;
            }
            center /= GO.transform.childCount; //center is average center of children

            //Now you have a center, calculate the bounds by creating a zero sized 'Bounds', 
            Bounds bounds = new Bounds(center, Vector3.zero);

            foreach (Transform child in GO.transform)
            {
                bounds.Encapsulate(child.GetComponent<MeshRenderer>().bounds);
            }
            return bounds;
        }


    }


    //得到模型中间最上方位置
    public Vector3 GetLampPos(GameObject GO)
    {
        var aabb = CalcBounds(GO);
        return new Vector3(aabb.center.x, aabb.center.y + aabb.extents.y, aabb.center.z);
    }

    //以床头为高，桌子中央为位置，初始化床头灯光
    public Vector3 GetTableLightPos(GameObject Fun, GameObject Bed)
    {
        var aabbTable = CalcBounds(Fun);
        var aabbBed = CalcBounds(Bed);
        return new Vector3(aabbTable.center.x, aabbBed.center.y + aabbBed.extents.y, aabbTable.center.z);

    }

    //以房间中心为xz，床高加2m为高
    public Vector3 GetMainLightPos()
    {
        var aabbCeiling = CalcBounds(Ceiling);
        var aabbBed = CalcBounds(Bed);
        return new Vector3(aabbCeiling.center.x, aabbBed.center.y + 2f, aabbCeiling.center.z);

    }

    //以某扶手为中心，沙发高度加1m为高
    public Vector3 GetSofaLightPos()
    {
        var aabbCeiling = CalcBounds(Ceiling);
        var aabbBed = CalcBounds(Bed);
        return new Vector3(aabbCeiling.center.x, aabbBed.center.y + 2f, aabbCeiling.center.z);

    }

    public void GenerateSpotLight(Vector3 vec)
    {
        GameObject Spotgroup = Resources.Load("SpotGroup") as GameObject;
        GameObject sg = Instantiate(Spotgroup) as GameObject;
        sg.transform.position = vec;
    }

    public void GeneratePointLight(Vector3 vec)
    {
        GameObject Pointgroup = Resources.Load("PointGroup") as GameObject;
        GameObject pg = Instantiate(Pointgroup) as GameObject;
        pg.transform.position = vec;

    }
}
