using UnityEngine;
using System.Collections;

public class EmptyClick : MonoBehaviour {
    void OnMouseDown() {
        switch (ChooseManager.gameType) {
            case GameType.PVE:OnClick(GameManagerPVE.gmPVE);break;
            case GameType.PVPLocal:OnClick(GameManagerLocal.gmLocal);break;
            case GameType.PVPOnLine:OnClick(GameManagerOnLine.gmOnLine);break;
        }
            

    }
    void OnClick(GameManager gm) {
        //空位置点击判断
        if (gm.round == Round.WolfRound) {
            if (gm.nowObject != null)
                gm.clickPos = this.transform;
        }
        else {
            if (gm.leftSheep > 0) {
                gm.clickPos = this.transform;
            }
            else
                 if (gm.nowObject != null)
                gm.clickPos = this.transform;
        }
    }
}
