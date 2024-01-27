using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHandler:MonoBehaviour{
    public PlayerController playerController;

    [Header("Player Info")]
    public Slider playerHPSlider;

    void Start(){
        GetPlayerInfo();
    }

    void Update(){
        playerHPSlider.value=playerController.GetCurrentPlayerHP();
    }

    private void GetPlayerInfo(){
        playerHPSlider.maxValue=playerController.GetPlayerMaxHP();
    }
}
