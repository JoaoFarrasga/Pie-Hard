using UnityEngine;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager;

    [Header("UI")]
    [SerializeField] private List<GameObject> uiGO= new();
    private GameObject currentUI;


    private void Awake()
    {
        if (uiManager == null)
        {
            uiManager = this;
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading new scenes
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }

        currentUI = uiGO[0];
    }

    //Enables Current UI GameObjects
    public void EnableUIGO(int ui)
    {
        currentUI = uiGO[ui];
        currentUI.SetActive(true);
    }

    //Disables Current UI GameObjects
    public void DisableUIGO() { currentUI.SetActive(false); }

    //public void EnableUIScript() { currentUI.GetComponent<MonoBehaviour>().enabled = true; }

    //public void DisableUIScript() { currentUI.GetComponent<MonoBehaviour>().enabled = false; }

    //public List<GameObject> GetUIList() { return uiGO; }
}
