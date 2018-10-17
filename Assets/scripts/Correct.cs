using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Correct : MonoBehaviour {
    public GameObject text;
    
    private void Start() {
        MainScript.StartVid = true;
        MainScript.GameStart = false;
        text.GetComponent<Text>().text += MainScript.Score;
    }

    private void Update () {
		if(Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene("playground");
        }
	}
}
