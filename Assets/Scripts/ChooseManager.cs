using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public enum GameType {
    PVE,
    PVPLocal,
    PVPOnLine
}
public class ChooseManager : MonoBehaviour {
    public GameObject rulePanel;
    public AudioClip btnAudio;
    public static GameType gameType;
    // Use this for initialization
    void Awake () {
        rulePanel.SetActive(false);
	}
    public void StartPVE() {
        AudioSource.PlayClipAtPoint(btnAudio, transform.position);
        SceneManager.LoadScene("GameScene");
        gameType = GameType.PVE;
    }
    public void StartPVPLocal() {
        AudioSource.PlayClipAtPoint(btnAudio, transform.position);
        SceneManager.LoadScene("GameScene");
        gameType = GameType.PVPLocal;
    }
    public void StartPVPOnLine() {
        AudioSource.PlayClipAtPoint(btnAudio, transform.position);
        SceneManager.LoadScene("GameScene");
        gameType = GameType.PVPOnLine;
    }
    public void ShowGameRule() {
        AudioSource.PlayClipAtPoint(btnAudio, transform.position);
        rulePanel.SetActive(true);
    }
    public void RuleReturn() {
        AudioSource.PlayClipAtPoint(btnAudio, transform.position);
        rulePanel.SetActive(false);
    }
    public void Exit() {
        AudioSource.PlayClipAtPoint(btnAudio, transform.position);
        Application.Quit();
    }
}
