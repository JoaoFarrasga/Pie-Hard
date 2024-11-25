using UnityEngine;
using Unity.Cinemachine;
using System;
using System.Collections.Generic;

public class CinemachineManager : MonoBehaviour
{ 
    private CinemachineCamera currentCamera;

    private void Awake()
    {
        currentCamera = transform.Find("CinemachineMainCamera").GetComponent<CinemachineCamera>();
        GameManager.OnGameStateChanged += OnGameEndState;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameEndState;
    }
    private void OnGameEndState(GameState state)
    {
        switch (state)
        {
            case GameState.InitialScreen:
                ChangeCamera("MainMenuCamera");
                break;

            case GameState.InGame:
                ChangeCamera("CinemachineMainCamera");
                break;

            case GameState.GameEnd:
                if (GameManager.gameManager.GetWinner() == null) return;
                    
                if (GameManager.gameManager.GetWinner()["PlayerID"] == 1)
                {
                    ChangeCamera("CameraLeftSide");
                }
                else
                {
                    ChangeCamera("CameraRightSide");
                }
                break;

            default:
                break;
        }      
    }

    private void ChangeCamera(string camera)
    {
        currentCamera.enabled = !currentCamera.enabled;
        transform.Find(camera).gameObject.GetComponent<CinemachineCamera>().enabled = true;
        currentCamera = transform.Find(camera).GetComponent<CinemachineCamera>();
    }
}
