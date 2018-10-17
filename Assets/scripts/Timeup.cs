using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Timeup : MonoBehaviour {
    public GameObject text;
    
    private void Start() {
        MainScript.StartVid = true;
        MainScript.GameStart = false;
        PlayerPrefs.SetInt("bestscore",Mathf.Max(PlayerPrefs.GetInt("bestscore"), MainScript.Score));
        text.GetComponent<Text>().text += MainScript.Score;
        MainScript.Score = 0;
    }
    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene("main");
        }
    }
}
