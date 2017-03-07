using UnityEngine;
using System.Collections;

public class SheepClick : MonoBehaviour {
    void OnMouseDown() {
        switch (ChooseManager.gameType) {
            case GameType.PVE:OnClick(GameManagerPVE.gmPVE);break;
            case GameType.PVPLocal: OnClick(GameManagerLocal.gmLocal); break;
            case GameType.PVPOnLine:
                if(GameManagerOnLine.gmOnLine.role=="sheep")
                    OnClick(GameManagerOnLine.gmOnLine);
                break;
        }
        
    }
    void OnClick(GameManager gm) {
        //点击判断，改变颜色
        if (gm.round == Round.SheepRound) {
            if(gm.nowObject!=null)
                gm.nowObject.GetComponent<Renderer>().material = gm.defaultMaterial;
            gm.nowObject = this.gameObject;
            if(gm.leftSheep==0)
                gm.nowObject.GetComponent<Renderer>().material = gm.sheepClickMaterial;
        }
    }
}
