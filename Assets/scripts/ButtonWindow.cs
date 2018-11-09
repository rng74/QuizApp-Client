using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ButtonWindow : MonoBehaviour {
    public GameObject playButton, header;
    public GameObject text, back;

    private bool clicked = false;
    private bool ready = false;

    private void buttonsReady() {
        StartCoroutine(forRequestReady());
    }
    public void onClickPlay() {
        if (clicked == true) {
            return;
        }
        clicked = true;
        if (!playButton.GetComponent<Animator>().enabled) playButton.GetComponent<Animator>().enabled = true;
        else playButton.GetComponent<Animator>().SetTrigger("In");
        if (!header.GetComponent<Animator>().enabled) header.GetComponent<Animator>().enabled = true;
        else header.GetComponent<Animator>().SetTrigger("In");
        StartCoroutine(FewSeconds());
    }

    IEnumerator FewSeconds() {
        yield return new WaitForSeconds(1.0f);
        buttonsReady();
    }

    IEnumerator forRequestReady() {
        yield return new WaitUntil( () => ready == true );
        SceneManager.LoadScene("playground");
    }
    
    private void Start() {
        /*if (MainScript.CurSize != 0) {
            animation = header.GetComponent<Animation>();
            if (!header.GetComponent<Animator>().enabled) header.GetComponent<Animator>().enabled = true;
            else header.GetComponent<Animator>().SetTrigger("Out");
        }TODO*/
        text.GetComponent<Text>().text+=PlayerPrefs.GetInt("bestscore");
        StartCoroutine(GetText());
    }
    IEnumerator GetText() {
        Debug.Log("Started");
        UnityWebRequest www = UnityWebRequest.Get("http://207.154.226.130/api/v1/get_hundred");
        yield return www.Send();
 
        if(www.isError) {
            Debug.Log(www.error);
        }
        else {
            var it = JsonConvert.DeserializeObject<ArrayList[][]>(www.downloadHandler.text);
            var prom = JsonConvert.DeserializeObject<ArrayList>(it[0][0][1].ToString());
            var info = JsonConvert.DeserializeObject<Dictionary<string, string> >(prom[0].ToString()); // it[variant][round][1] -> json file
            MainScript.List = it;
            Debug.Log(info["title_ru"]); // title_ru, title_orig, poster_link, song_link
            ready=true;
        }
    }
}
