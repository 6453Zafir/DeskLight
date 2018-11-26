using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LampLayout : MonoBehaviour {
    public RenderTexture RT;
    public GameObject Ground;
    public GameObject Table1,Table2;
    public GameObject Bed;
    public GameObject Wall;
    public GameObject Sofa;
    public GameObject Ceiling;
    // Use this for initialization
    void Start () {
        GenerateSpotLight(GetTableLightPos(Table1, Bed));
        GenerateSpotLight(GetTableLightPos(Table2, Bed));
        GeneratePointLight(GetMainLightPos());
        GeneratePointLight(GetLampPos(Sofa));
    }

    // Update is called once per frame
    void Update () {
		
	}

    //单个物体包围盒
    public Bounds CalcAABB(List<MeshRenderer> meshes)
    {
        var boxColliders = meshes.Select(x => x.bounds).ToList();
        var aabb = boxColliders[0];
        foreach (var obj in boxColliders)
        {
            aabb.Encapsulate(obj);
        }
        return aabb;
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
        else {
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

    //计算俯视图二维平面的重心
    public void Face() {
        var go = GameObject.Find("gBufferCamera");
        Camera gBufferCamera = null;
        if (go == null)
            gBufferCamera = new GameObject("GBufferCamera").AddComponent<Camera>();
        else
            gBufferCamera = go.GetComponent<Camera>();
        var aabb = CalcBounds(Table1);
        var maxExtend = aabb.extents.y;
        gBufferCamera.orthographic = true;
        gBufferCamera.cullingMask = 1 << 8;
        print(aabb.extents.x / aabb.extents.z);
        gBufferCamera.aspect = aabb.extents.x/ aabb.extents.z;
        gBufferCamera.orthographicSize = aabb.extents.z;
        var m_ViewWidth = 1;
        var m_ViewHeight = 1;
        gBufferCamera.rect = new Rect(aabb.extents.x, aabb.extents.z, m_ViewWidth, m_ViewHeight);
        gBufferCamera.allowMSAA = false;
        gBufferCamera.allowHDR = false;

       // gBufferCamera.enabled = false;
        gBufferCamera.clearFlags = CameraClearFlags.SolidColor;
        gBufferCamera.backgroundColor = Color.clear;
        gBufferCamera.farClipPlane = maxExtend * 3;
        var cameraTransform = gBufferCamera.transform;
        cameraTransform.position = aabb.center;
        cameraTransform.forward = new Vector3(0, -1, 0);
        cameraTransform.position = aabb.center - cameraTransform.forward *2* maxExtend;
        gBufferCamera.targetTexture = RT;
        //Shader.SetGlobalTexture("_Lightmap", Lightmap);
        ////相应修改Getsinglelightmapshader
        //Shader.SetGlobalVector("_LightmspST", ObToLightmap.GetComponent<MeshRenderer>().lightmapScaleOffset);
        //gBufferCamera.RenderWithShader(GetSingleLightmapShader, "");
        //return ConvertRTtoT2D(RT);
    }

    //得到模型中间最上方位置
    public Vector3 GetLampPos(GameObject GO) {
        var aabb = CalcBounds(GO);
        return new Vector3(aabb.center.x,  aabb.center.y+aabb.extents.y, aabb.center.z);
    }

    //以床头为高，桌子中央为位置，初始化床头灯光
    public Vector3 GetTableLightPos(GameObject Fun, GameObject Bed) {
        var aabbTable = CalcBounds(Fun);
        var aabbBed = CalcBounds(Bed);
        return new Vector3(aabbTable.center.x, aabbBed.center.y + aabbBed.extents.y, aabbTable.center.z);

    }

    //以房间中心为xz，床高加2m为高
    public Vector3 GetMainLightPos()
    {
        var aabbCeiling = CalcBounds(Ceiling);
        var aabbBed = CalcBounds(Bed);
        return new Vector3(aabbCeiling.center.x, aabbBed.center.y+2f, aabbCeiling.center.z);

    }

    //以某扶手为中心，沙发高度加1m为高
    public Vector3 GetSofaLightPos()
    {
        var aabbCeiling = CalcBounds(Ceiling);
        var aabbBed = CalcBounds(Bed);
        return new Vector3(aabbCeiling.center.x, aabbBed.center.y + 2f, aabbCeiling.center.z);

    }

    //得到三维所有子物体合集的伪重心
    public Vector3 GetVertex(GameObject GO) {
        float TOTAL_X = 0, TOTAL_Z = 0;
        float TOTAL_Vert = 0;
        float AVG_X, AVG_Z, MAX_Y = 0;
        if (GO.GetComponent<MeshFilter>() != null)
        {
            Vector3[] vertices = GO.GetComponent<MeshFilter>().mesh.vertices;
            foreach (Vector3 vertex in vertices)
            {
                TOTAL_X += vertex.x;
                TOTAL_Z += vertex.z;
                if (vertex.y > MAX_Y)
                    MAX_Y = vertex.y;
            }
            AVG_X = TOTAL_X / vertices.Length;
            AVG_Z = TOTAL_Z / vertices.Length;
            return new Vector3(AVG_X, MAX_Y, AVG_Z);
        }
        else {
            MeshFilter[] childmeshs = GO.GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter mf in childmeshs)
            {
                Vector3[] vertices = mf.mesh.vertices;
                TOTAL_Vert += vertices.Length;
                foreach (Vector3 vertex in vertices)
                {
                    TOTAL_X += vertex.x;
                    TOTAL_Z += vertex.z;
                    if (vertex.y > MAX_Y)
                        MAX_Y = vertex.y;
                }
            }
            AVG_X = TOTAL_X / TOTAL_Vert;
            AVG_Z = TOTAL_Z / TOTAL_Vert;
            return new Vector3(AVG_X, MAX_Y, AVG_Z);
        }
       
    }

    //计算模型高度
    public float CalculateHeightFromGround(GameObject GO)
    {
        return (GO.transform.position.y - Ground.transform.position.y);
    }
    
    public void GenerateSpotLight(Vector3 vec) {
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
