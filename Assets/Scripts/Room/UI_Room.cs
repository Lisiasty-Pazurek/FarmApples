using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class UI_Room : MonoBehaviour
{
    public void BackToLobby()
    {
        SceneManager.LoadSceneAsync("LobbySample");
    }


}
