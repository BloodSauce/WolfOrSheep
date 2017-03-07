using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsAct : MonoBehaviour {
    public GameObject audioBtn;
    public Sprite audioOn;
    public Sprite audioOff;
    public AudioClip btnAudio;
    public GameObject returnPanel;
    public GameObject choosePanel;
    void Awake() {
        returnPanel.SetActive(false);
    }
    //再来一局
    public void ReStart() {
        AudioSource.PlayClipAtPoint(btnAudio, Vector2.zero);
        SceneManager.LoadScene("GameScene");
    }
    //返回菜单
    public void ReturnMenu() {
        AudioSource.PlayClipAtPoint(btnAudio, Vector2.zero);
        SceneManager.LoadScene("StartScene");
    }
    //音乐开关
    public void BgmControl() {
        if (AudioListener.volume!=0) {
            AudioListener.volume=0;
            audioBtn.GetComponent<Image>().sprite = audioOff;
        }
        else {
            AudioListener.volume = 1;
            audioBtn.GetComponent<Image>().sprite = audioOn;
        }
    }
    //放弃本局
    public void Abandon() {
        AudioSource.PlayClipAtPoint(btnAudio, Vector2.zero);
        returnPanel.SetActive(true);
    }
    //取消
    public void Cancel() {
        AudioSource.PlayClipAtPoint(btnAudio, Vector2.zero);
        returnPanel.SetActive(false);
    }
    //角色选择-狼
    public void WolfBtn() {
        GameManagerPVE.gmPVE.role = "wolf";
        choosePanel.SetActive(false);
    }
    //角色选择-羊
    public void SheepBtn() {
        GameManagerPVE.gmPVE.role = "sheep";
        choosePanel.SetActive(false);
    }
}
