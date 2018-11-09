using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SomeV : MonoBehaviour {
    public RawImage player, bigPoster;

    private string url_song, url_poster;
    private AudioSource source;

    IEnumerator Start() {
        source = GetComponent<AudioSource>();
        
       // StartCoroutine(TakeAudio());
        var fi = JsonConvert.DeserializeObject<ArrayList>(MainScript.List[0][MainScript.Round][1].ToString());
        var first = JsonConvert.DeserializeObject<Dictionary<string, string> >(fi[0].ToString()); 
        url_song = "http://anison.fm"+first["song_link"];
        using (var www = new WWW(url_song)) {
            yield return www;
            source.clip = www.GetAudioClip();
        }
        
        url_poster = "http://anison.fm"+first["poster_link"];
        Texture2D tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        using (WWW www = new WWW(url_poster)) {
            yield return www;
            www.LoadImageIntoTexture(tex);
            player.texture = tex;
            bigPoster.texture = tex;
            Color color;
            color = Color.white;
            color.a = 255;
            player.color = color;
            bigPoster.color = color;
        }
    }

    void Update() {
        try {
            if (!source.isPlaying && source.clip.loadState == AudioDataLoadState.Loaded) {
                source.PlayDelayed(0.3f);
                MainScript.GameStart = true;
            }
        } catch {

        }
    }
}