using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InCorrect : MonoBehaviour {
    public GameObject text, TA;
    
    private void Start() {
        MainScript.StartVid = true;
        MainScript.GameStart = false;
        PlayerPrefs.SetInt("bestscore",Mathf.Max(PlayerPrefs.GetInt("bestscore"), MainScript.Score));
        text.GetComponent<Text>().text += MainScript.Score;

        var fi = JsonConvert.DeserializeObject<ArrayList>(MainScript.List[0][MainScript.Round][1].ToString());
        var first = JsonConvert.DeserializeObject<Dictionary<string, string> >(fi[0].ToString()); 
        TA.GetComponent<Text>().text = first["title_ru"];
        MainScript.Score = 0;
    }
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene("main");
        }
	}
}
