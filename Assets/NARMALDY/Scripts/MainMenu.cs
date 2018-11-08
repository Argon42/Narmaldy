using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private GameObject AutorsPanel;
    [SerializeField] private GameObject Bomj;

    private void Awake()
    {
        MainPanel.SetActive(true);
        OptionsPanel.SetActive(false);
        AutorsPanel.SetActive(false);
    }

    public void OpenCloseOptions()
    {
        MainPanel.SetActive(!MainPanel.activeInHierarchy);
        OptionsPanel.SetActive(!OptionsPanel.activeInHierarchy);
    }

    public void OpenCloseAutors()
    {
        MainPanel.SetActive(!MainPanel.activeInHierarchy);
        AutorsPanel.SetActive(!AutorsPanel.activeInHierarchy);
    }

    public void NewGame()
    {
        StartCoroutine(TimerForStart());
    }
    IEnumerator TimerForStart()
    {
        Bomj.GetComponent<Animator>().Play("StartNewGame");
        MainPanel.SetActive(false);
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("Tutorial");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
