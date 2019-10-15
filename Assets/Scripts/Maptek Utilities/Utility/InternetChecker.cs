using System;
using System.Collections;
using UnityEngine;

namespace Trophies.Maptek
{
    public class InternetChecker : MonoBehaviour
    {
        IEnumerator checkInternetConnection(Action<bool> action)
        {
            WWW www = new WWW("http://google.com");
            yield return www;
            if (www.error != null)
            {
                action(false);
            }
            else
            {
                action(true);
            }
        }

        public void IsConnected(Action<bool> action)
        {
            StartCoroutine(checkInternetConnection(action));
        }
    }
}