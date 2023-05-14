using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayButton : NetworkBehaviour
{

    private NetworkManager networkManager ;
    private void Start() {
        networkManager=NetworkManager.singleton;
        GameObject canvasObject = GameObject.Find("PlayButton");
        canvasObject.SetActive(true);

    }
    public void JoinRoomButton() {
        networkManager.StartHost();
    }
    
}