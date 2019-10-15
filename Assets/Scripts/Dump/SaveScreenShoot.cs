using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveScreenShoot : MonoBehaviour
{
    public bool isProcessing;

    public void ScreenShoot()
    {
        Debug.Log("Screen shoot");
        StartCoroutine(C_TakePhoto());

    }

    IEnumerator C_TakePhoto()
    {
        Debug.Log("C_TakePhoto");
        isProcessing = true;
        //yield return new WaitForEndOfFrame();


        //string file = "Screen_" + GetTimestamp(DateTime.Now) + ".png";
        //ScreenCapture.CaptureScreenshot(file);

        //file = Application.persistentDataPath + "/"+ file;

        //Debug.Log("file: " + file);
        //int count = 0;
        //while (!System.IO.File.Exists(file) || count > 4)
        //{
        //    yield return null;
        //    Debug.Log(System.IO.File.Exists(file));
        //}

        //if (count > 4)
        //{
        //    Debug.Log("Error too much time taking photo");
        //}
        //else
        //{
        //    Debug.Log("Call native share");
        //    NativeShare nativeShare = new NativeShare();
        //    nativeShare.AddFile(file);
        //    nativeShare.Share();
        //}

        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "Trophies.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath).Share();

        isProcessing = false;
    }

    public static String GetTimestamp(DateTime value)
    {
        return value.ToString("yyyyMMddHHmmssffff");
    }
}
