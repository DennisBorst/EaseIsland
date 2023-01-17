using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private KeyCode pauseMenuButton;
    [SerializeField] private GameObject normalPanel;
    [SerializeField] private GameObject confirmPanel;

    public void EnterPauseScreen()
    {
        normalPanel.SetActive(true);
        confirmPanel.SetActive(false);
        StartCoroutine(CheckForInput());
    }

    public void Resume()
    {
        StopAllCoroutines();
        GameManger.Instance.PauseGame(false);
        this.gameObject.SetActive(false);
        CharacterMovement.Instance.FreezePlayer(false);
        CharacterMovement.Instance.CloseMenu();
    }

    public void Restart()
    {

    }

    public void Menu()
    {
        StopAllCoroutines();
        GameManger.Instance.PauseGame(false);
        GameManger.Instance.Fade(0);
    }

    public void Quit()
    {
        StopAllCoroutines();
        Application.Quit();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(pauseMenuButton))
        {
            Resume();
        }
    }

    private IEnumerator CheckForInput()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        while (true)
        {
            yield return wait;
            CheckInput();
        }
    }
}
