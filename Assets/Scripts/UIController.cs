using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public GameObject ControllerList;
    //public Toggle BedTableToggle;
    //public Toggle MainToggle;
    //public Toggle SofaToggle;
    //public Toggle DecorateToggle;

    int PanelGenerated = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


    }

    public void InitList() {
        //GameController.TableLightCount
        foreach (GameObject lit in GameController.lights) {
            if (lit.gameObject.tag == "PointGe")
            {
                GeneratePanel(1,lit); 
                PanelGenerated += 1;
            }
            else if (lit.gameObject.tag == "SpotGroupGe")
            {
                GeneratePanel(2, lit);
                PanelGenerated += 1;
            }
            else {

            }
        }

    }


    /// <summary>
    /// 根据灯光类型生成相应的控制面板
    /// </summary>
    /// <param name="panelType">1.point 2.spotgroup 3.spot</param>
    /// <param name="PosY"></param>
    public void GeneratePanel(int panelType,GameObject Lit) {
        GameObject ConPanel =Instantiate(Resources.Load("LightControlPanel")) as GameObject;

        ConPanel.transform.SetParent(ControllerList.transform);
        ConPanel.transform.localPosition = Vector3.zero;
        ConPanel.transform.localScale = Vector3.one;
        ConPanel.transform.localPosition += new Vector3(0, -52f * PanelGenerated, 0);

        ConPanel.GetComponentInChildren<Text>().text = Lit.name;

        Toggle m_Toggle = ConPanel.GetComponentInChildren<Toggle>();
        m_Toggle.onValueChanged.AddListener(delegate {
        ToggleValueChanged(m_Toggle, Lit);
              });
    }

    void ToggleValueChanged(Toggle change, GameObject Lit)
    {
        Lit.SetActive(change.isOn);
    }


    public void ClearAllController() {
        
    }
}
