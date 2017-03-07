using UnityEngine;
using System.Collections;

public class ChooseGameManager : MonoBehaviour {
    public GameObject gameManagerPVE;
    public GameObject gameManagerLocal;
    public GameObject gameManagerOnLine;

    void Awake () {
        gameManagerPVE.SetActive(false);
        gameManagerLocal.SetActive(false);
        gameManagerOnLine.SetActive(false);
        switch (ChooseManager.gameType) {
            case GameType.PVE:gameManagerPVE.SetActive(true);break;
            case GameType.PVPLocal:gameManagerLocal.SetActive(true);break;
            case GameType.PVPOnLine:gameManagerOnLine.SetActive(true);break;
        }
    }
}
