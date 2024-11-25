using UnityEngine;
using Unity.Cinemachine;
using System;
using System.Collections.Generic;

public class CinemachineManager : MonoBehaviour
{
    [SerializeField] private List<PlayerController> playersGO;
    [SerializeField] private List<CinemachineCamera> cameras;

    private CinemachineCamera currentCamera;

    private void Awake()
    {
        currentCamera = GetComponent<CinemachineCamera>();
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
            case GameState.GameEnd:
                if (GameManager.gameManager.GetWinner() == null) return;
                for (int i = 0; i < playersGO.Count; i++)
                {
                    if (GameManager.gameManager.GetWinner()["PlayerID"] == playersGO[i].GetID())
                    {
                        playersGO[i].GetComponentInChildren<CinemachineCamera>().enabled = true;
                        currentCamera.enabled = !currentCamera.enabled;
                        currentCamera = playersGO[i].GetComponentInChildren<CinemachineCamera>();
                    }
                }
                break;
            case GameState.InGame:
                {
                    currentCamera.enabled = !currentCamera.enabled;
                    GetComponent<CinemachineCamera>().enabled = true;
                    currentCamera = gameObject.GetComponent<CinemachineCamera>();
                }
                break;
            default:
                break;
    }
        
    }
}
