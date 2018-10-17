using System.IO;
using UnityEngine;
using UnityEngine.Networking; 

public class CustomWebRequest : DownloadHandlerScript {
    private int cursize=0;
    private string _targetFilePath;
    private Stream _fileStream=null;
	// Standard scripted download handler - will allocate memory on each ReceiveData callback
    public CustomWebRequest()
        : base()
    {
        
    }

    // Pre-allocated scripted download handler
    // Will reuse the supplied byte array to deliver data.
    // Eliminates memory allocation.
    public CustomWebRequest(byte[] bytes, string targetFilePath)
        : base(bytes)
    {
        _targetFilePath = targetFilePath;
    }

    // Required by DownloadHandler base class. Called when you address the 'bytes' property.
    protected override byte[] GetData() { return null; }
    
    // Called once per frame when data has been received from the network.
    protected override bool ReceiveData(byte[] byteFromServer, int dataLength) {
        if (byteFromServer == null || byteFromServer.Length < 1) {
            Debug.Log("CustomWebRequest :: ReceiveData - received a null/empty buffer");
            return false;
        }
        if (_fileStream == null) {
             _fileStream = File.OpenWrite(_targetFilePath);
         }
 
         // write data to file
         _fileStream.Write(byteFromServer, 0, dataLength);
        cursize+=dataLength;
        Debug.Log("Size of peace: " + dataLength);
        MainScript.CurSize=cursize;

        //Do something with or Process byteFromServer here


        return true;
    }

    // Called when all data has been received from the server and delivered via ReceiveData
    protected override void CompleteContent() {
        Debug.Log("CustomWebRequest :: CompleteContent - DOWNLOAD COMPLETE!");
        _fileStream.Close();
    }

    // Called when a Content-Length header is received from the server.
    protected override void ReceiveContentLength(int contentLength) {
        Debug.Log(string.Format("CustomWebRequest :: ReceiveContentLength - length {0}", contentLength));
        MainScript.ContentLength = contentLength;
    }
}
