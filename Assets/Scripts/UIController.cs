using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Toggle BedTableToggle;
    public Toggle MainToggle;
    public Toggle SofaToggle;
    public Toggle DecorateToggle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        

    }

    public void lightControl(int LightNum) {
        switch (LightNum) {
            case 0:
                break;
            case 1:
                GameController.isBedTableLightEnable = BedTableToggle.isOn;
                print("isBedTableLightEnable"+GameController.isBedTableLightEnable);
                break;
            case 2:
                GameController.isBedMainLightEnable = MainToggle.isOn;
                print("isBedMainLightEnable" + GameController.isBedMainLightEnable);

                break;
            case 3:
                GameController.isBedSofaLightEnable = SofaToggle.isOn;
                print("isBedSofaLightEnable" + GameController.isBedSofaLightEnable);

                break;
            case 4:
                GameController.isBedDecorateLightEnable = DecorateToggle.isOn;
                print("isBedDecorateLightEnable" + GameController.isBedDecorateLightEnable);
                break;
            default:
                break;
        }
    }
}
