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
        TA.GetComponent<Text>().text = MainScript.List[0][MainScript.Round];
        MainScript.Score = 0;
    }
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene("main");
        }
	}
}
