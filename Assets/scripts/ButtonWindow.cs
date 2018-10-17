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

public class ButtonWindow : MonoBehaviour {
    public GameObject playButton, header;
    public GameObject text, back;

    private bool clicked = false;
    private bool ready = false;
    
    public Animation animation;

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
    
     public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
     bool isOk = true;
     // If there are errors in the certificate chain, look at each error to determine the cause.
     if (sslPolicyErrors != SslPolicyErrors.None) {
         for (int i=0; i<chain.ChainStatus.Length; i++) {
             if (chain.ChainStatus [i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
                 chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                 chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                 chain.ChainPolicy.UrlRetrievalTimeout = new System.TimeSpan (0, 1, 0);
                 chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                 bool chainIsValid = chain.Build ((X509Certificate2)certificate);
                 if (!chainIsValid) {
                     isOk = false;
                 }
             }
         }
     }
     return isOk;
 }


    private string GET(string Url) {
        WebRequest req = WebRequest.Create(Url);
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        WebResponse resp = req.GetResponse();
        Stream stream = resp.GetResponseStream();
        StreamReader sr = new StreamReader(stream);
        string Out = sr.ReadToEnd();
        sr.Close();
        return Out;
    }

    private void Start() {
        /*if (MainScript.CurSize != 0) {
            animation = header.GetComponent<Animation>();
            if (!header.GetComponent<Animator>().enabled) header.GetComponent<Animator>().enabled = true;
            else header.GetComponent<Animator>().SetTrigger("Out");
        }TODO*/
        text.GetComponent<Text>().text+=PlayerPrefs.GetInt("bestscore");
        new Thread(() => 
        {
            Thread.CurrentThread.IsBackground = true; 
            MainScript.List = JsonConvert.DeserializeObject<string[][]>(GET("https://ninja-server.herokuapp.com/api/v1/shuffle"));
            //Debug.Log("LIST OF ANIME IS READY");
            ready = true;
        }).Start();
    }
}
