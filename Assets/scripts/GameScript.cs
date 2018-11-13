using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GameScript : MonoBehaviour {    
    public GameObject btns; // Buttons as gameobjects. to take animation and other components
    public Image timer; // timer at top bar
    private float waitTime = 20.0f; // amount of time
    
    private bool started = false; // is game started?
    private bool fill = false; // when fill true, timer bar filling in.
    
    private bool[] btnCorrect = new bool[4]; // correctness of each button
    private Button[] buttons; // Buttons

    public RawImage blur;

    private void inCorrect() {
        //print ("wrong ans");
        btns.GetComponent<Animator>().SetTrigger("Out");
        started = false;
        SceneManager.LoadScene("wrong");
    }

    private void Correct() {
        //print("Grats!");
        MainScript.Round++;
        btns.GetComponent<Animator>().SetTrigger("Out");
        started = false;
        MainScript.Score+=Mathf.FloorToInt(timer.fillAmount*waitTime);
        SceneManager.LoadScene("correct");
    }

    private void outOfTime() {
        //print ("0 Time");
        MainScript.Done = false;
        btns.GetComponent<Animator>().SetTrigger("Out");
        //StartCoroutine(forAnimTime());
        started = false;
        SceneManager.LoadScene("timeup");
    }

    public void TaskOnChoose(int x) { // Запускаем соответствующую анимацию
        MainScript.Done = false;
        if (!btnCorrect[x]) inCorrect();
        else                Correct();
    }

    private void setBtnText() { // Текст берем с сервака
        System.Random r = new System.Random();
        var numbers = Enumerable.Range(0, 4).OrderBy(i => r.Next()).ToList();

        // Getting current variant's information
        var fi = JsonConvert.DeserializeObject<ArrayList>(MainScript.List[0][MainScript.Round][1].ToString());
        var first = JsonConvert.DeserializeObject<Dictionary<string, string> >(fi[0].ToString()); 

        var se = JsonConvert.DeserializeObject<ArrayList>(MainScript.List[1][MainScript.Round][1].ToString());
        var second = JsonConvert.DeserializeObject<Dictionary<string, string> >(se[0].ToString());

        var thi = JsonConvert.DeserializeObject<ArrayList>(MainScript.List[2][MainScript.Round][1].ToString());
        var third = JsonConvert.DeserializeObject<Dictionary<string, string> >(thi[0].ToString());

        var fou = JsonConvert.DeserializeObject<ArrayList>(MainScript.List[3][MainScript.Round][1].ToString());
        var forth = JsonConvert.DeserializeObject<Dictionary<string, string> >(fou[0].ToString());

        buttons[numbers[0]].GetComponentInChildren<Text>().text = first["title_ru"];
        buttons[numbers[1]].GetComponentInChildren<Text>().text = second["title_ru"];
        buttons[numbers[2]].GetComponentInChildren<Text>().text = third["title_ru"];
        buttons[numbers[3]].GetComponentInChildren<Text>().text = forth["title_ru"];

        btnCorrect[numbers[0]] = true;
        btnCorrect[numbers[3]]=btnCorrect[numbers[1]]=btnCorrect[numbers[2]]=false;
    }

    private void buttonsReady() {
        MainScript.StartVid = true;
        setBtnText();
        fill=true;
        btns.GetComponent<Animator>().SetTrigger("In");
    }
    
    private void Start() {
        buttons = btns.GetComponentsInChildren<Button>();
        btnCorrect[0]=btnCorrect[1]=btnCorrect[2]=btnCorrect[3]=false;
        buttonsReady();
    }

    private void Update() {
        if (timer.fillAmount == 1 && MainScript.GameStart) {
            fill = false;
            started = true;
        }
        if(fill) {
            timer.fillAmount += 1.0f / 0.7f*Time.deltaTime;
        }
        if(!fill && timer.fillAmount > 0 && started == true) {
            timer.fillAmount -= 1.0f / waitTime*Time.deltaTime;
        }
        if (started && timer.fillAmount == 0) {
            outOfTime();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) { 
            MainScript.StartVid = true;
            MainScript.GameStart = false;
            PlayerPrefs.SetInt("bestscore",Mathf.Max(PlayerPrefs.GetInt("bestscore"), MainScript.Score));
            MainScript.Score = 0;
            SceneManager.LoadScene("main");
        }
    }
}