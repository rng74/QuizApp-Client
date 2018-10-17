using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameScript : MonoBehaviour {    
    public GameObject btns; // Buttons as gameobjects. to take animation and other components
    public Image timer; // timer at top bar
    private float waitTime = 30.0f; // amount of time
    
    private bool started = false; // is game started?
    private bool fill = false; // when fill true, timer bar filling in.
    
    private bool[] btnCorrect = new bool[4]; // correctness of each button
    private Button[] buttons; // Buttons

    public Image loading;

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

    // NOTE: var goodOne = JsonConvert.DeserializeObject<Dictionary<string, string>>(GET(url)); 
    private void setBtnText() { // Текст берем с сервака
        System.Random r = new System.Random();
        var numbers = Enumerable.Range(0, 4).OrderBy(i => r.Next()).ToList();

        buttons[numbers[0]].GetComponentInChildren<Text>().text = MainScript.List[1][MainScript.Round];
        buttons[numbers[1]].GetComponentInChildren<Text>().text = MainScript.List[2][MainScript.Round];
        buttons[numbers[2]].GetComponentInChildren<Text>().text = MainScript.List[3][MainScript.Round];
        buttons[numbers[3]].GetComponentInChildren<Text>().text = MainScript.List[0][MainScript.Round];
        btnCorrect[numbers[3]] = true;
        btnCorrect[numbers[0]]=btnCorrect[numbers[1]]=btnCorrect[numbers[2]]=false;
        
        /*buttons[0].onClick.AddListener(delegate { TaskOnChoose(0); } );
        buttons[1].onClick.AddListener(delegate { TaskOnChoose(1); } );
        buttons[2].onClick.AddListener(delegate { TaskOnChoose(2); } );
        buttons[3].onClick.AddListener(delegate { TaskOnChoose(3); } );*/
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
        if (timer.fillAmount == 1 &&  MainScript.GameStart) {
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
        if (!MainScript.Done) {
            loading.fillAmount = MainScript.CurSize / MainScript.ContentLength;
        }
        else if (loading.fillAmount>0) {
            loading.enabled = false;
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