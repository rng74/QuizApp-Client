using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlurControl : MonoBehaviour {
    public RawImage img;
	void Start () {
        float resolution = Screen.width/130.0f;
        img.material.SetFloat("_Size", resolution);
	}
}
