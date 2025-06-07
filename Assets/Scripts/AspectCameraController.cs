using UnityEngine;

public class MaintainAspectRatio : MonoBehaviour
{
    void Start()
    {
        SetCameraViewport();
    }

    void SetCameraViewport()
    {
        float targetAspect = 16.0f / 9.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            // Schwarze Balken oben und unten
            Camera.main.rect = new Rect(0, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
        }
        else
        {
            // Schwarze Balken links und rechts
            float scaleWidth = 1.0f / scaleHeight;
            Camera.main.rect = new Rect((1.0f - scaleWidth) / 2.0f, 0, scaleWidth, 1.0f);
        }
    }

    void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;
            SetCameraViewport();
        }
    }

    private int lastWidth, lastHeight;

    /*void OnApplicationQuit()
    {
        Application.Quit();
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }*/

}
