using UnityEngine;
using System.Collections;

public class WolfClick : MonoBehaviour {
    void OnMouseDown() {
        switch (ChooseManager.gameType) {
            case GameType.PVE:OnClick(GameManagerPVE.gmPVE);break;
            case GameType.PVPLocal:OnClick(GameManagerLocal.gmLocal); break;
            case GameType.PVPOnLine:
                if (GameManagerOnLine.gmOnLine.role == "wolf") {
                    OnClick(GameManagerOnLine.gmOnLine);
                }
                break;

        }
        
    }
    void OnClick(GameManager gm) {
        //点击判断，改变颜色
        if (gm.round == Round.WolfRound) {
            if (gm.nowObject != null)
                gm.nowObject.GetComponent<Renderer>().material = gm.defaultMaterial;
            gm.nowObject = this.gameObject;
            gm.nowObject.GetComponent<Renderer>().material = gm.wolfClickMaterial;
        }
    }
}
