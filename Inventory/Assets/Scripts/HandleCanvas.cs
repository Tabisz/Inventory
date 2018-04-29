using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleCanvas : MonoBehaviour {
    private CanvasScaler scalar;
	// Use this for initialization
	void Start () {
        scalar = GetComponent<CanvasScaler>();

        scalar.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
	}
	

}
