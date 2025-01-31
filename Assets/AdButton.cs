using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdButton : MonoBehaviour
{
    gameManager manager;
   
    void Start()
    {
        manager =GameObject.Find("GameCanvas").GetComponent<gameManager>();
     
    }
    public void OnAdButtonPressed()
    {
       AdsManager.Instance.rewardedAds.ShowRewardedAd();
       manager.AdWatched();
       
    }
    
}
