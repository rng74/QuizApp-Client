using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
public class SomeV : MonoBehaviour {
    public RawImage player;
    private AudioSource audioSource;
    private VideoPlayer myVideoPlayer;
    private UnityWebRequest _videoRequest;
    byte[] bytes = new byte[4096];

    private void Update() {
        if (MainScript.StartVid == true) {
            StartCoroutine(this.loadVideoFromThisURL(MainScript.List[4][MainScript.Round]));
            MainScript.StartVid = false;
        }
    }
    
    private IEnumerator loadVideoFromThisURL(string _url) {
        string _pathToFile = Path.Combine (Application.persistentDataPath, "movie.mp4");
        _videoRequest = UnityWebRequest.Get (_url);

        _videoRequest.downloadHandler = new CustomWebRequest(bytes, _pathToFile);
        yield return _videoRequest.Send();

        if (_videoRequest.isDone == false || _videoRequest.error != null)
        {   Debug.Log ("Request = " + _videoRequest.error );}

        Debug.Log ("Video Done - " + _videoRequest.isDone);
        MainScript.Done=true;

        Debug.Log (_pathToFile);
        StartCoroutine(this.playThisURLInVideo (_pathToFile));
        yield return null;
    }

    private IEnumerator playThisURLInVideo(string _url) {
        audioSource = gameObject.AddComponent<AudioSource>();
        myVideoPlayer = gameObject.AddComponent<VideoPlayer>();
        myVideoPlayer.url = _url;
        myVideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        myVideoPlayer.EnableAudioTrack(0, true);
        myVideoPlayer.SetTargetAudioSource(0, audioSource);
        myVideoPlayer.Prepare ();
        MainScript.GameStart = false;
        while (myVideoPlayer.isPrepared == false)
        {   yield return null;}
        MainScript.GameStart = true;
        player.texture = myVideoPlayer.texture;
        Debug.Log ("Video should play");
        
        Color color;
        color = Color.white;
        color.a = 255;
        player.color = color;

        audioSource.Play();
        myVideoPlayer.Play ();
    }
}