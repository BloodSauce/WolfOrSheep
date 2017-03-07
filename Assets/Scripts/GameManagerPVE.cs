using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManagerPVE : GameManager {
    public static GameManagerPVE gmPVE = null;
    public string role;
    private float time = 0;
    private float delTime = 60.0f;
    void Awake() {
        if (gmPVE == null)
            gmPVE = this;
        else if (gmPVE != this)
            Destroy(gameObject);
    }
    protected override void Update() {
        if (role != null) {
            if (role == "sheep" && round == Round.WolfRound) {
                time++;
                if (time > delTime) {
                    WolfMove();
                    time = 0;
                }
            }
            else if (role == "wolf" && round == Round.SheepRound) {
                time++;
                if (time > delTime) {
                    SheepMove();
                    time = 0;
                }
            }
            else
                base.Update();
        }
    }
    //狼移动AI
    void WolfMove() {
        hasAdd = false;
        GameObject bestWolf = null;
        Transform bestPos = null;
        int bestPower = 0;
        for (int n = 0; n < 2; n++) {
            int r_now = int.Parse(wolf[n].transform.parent.name.Substring(0, 1));
            int c_now = int.Parse(wolf[n].transform.parent.name.Substring(1));
            GameObject nowParent = GameObject.Find(r_now.ToString() + c_now.ToString());

            for (int i = -2; i <= 2; i++) {
                for (int j = -2; j <= 2; j++) {
                    if (i != 0 || j != 0) {
                        int power = 0;
                        string Pos = (r_now + i).ToString() + (c_now + j).ToString();
                        GameObject obj = GameObject.Find(Pos);
                        if (obj != null && obj.transform.childCount == 0) {
                            if (base.CanMove_Wolf(wolf[n], obj.transform)) {
                                float dis = Vector2.Distance(Vector2.zero, new Vector2(i, j));
                                if (r_now == 0 || r_now == 1) {
                                    if (r_now + i > 1)
                                        power +=12;
                                }
                                if (r_now == 7 || r_now == 8) {
                                    if (r_now + i < 7)
                                        power += 12;
                                }
                                if (dis == firstDis || dis == secondDis)
                                    power += 10;
                                if (dis == thirdDis || dis == fourthDis)
                                    power += 1;
                                if (obj.transform.position.x > 6 || obj.transform.position.x < 2)
                                    power -= 1;
                                wolf[n].transform.parent = obj.transform;
                                for (int a = -2; a <= 2; a++) {
                                    for (int b = -2; b <= 2; b++) {
                                        if (a != 0 || b != 0) {
                                            string nextPos = (r_now + i + a).ToString() + (c_now + j + b).ToString();
                                            GameObject nextobj = GameObject.Find(nextPos);
                                            if (nextobj != null && nextobj.transform.childCount == 0) {
                                                if (base.CanMove_Wolf(wolf[n], nextobj.transform)) {
                                                    float nextDis = Vector2.Distance(Vector2.zero, new Vector2(a, b));
                                                    if (nextDis == firstDis || nextDis == secondDis)
                                                        power += 2;
                                                    if (dis == thirdDis || dis == fourthDis)
                                                        power += 1;
                                                    if (obj.transform.position.x > 6 || obj.transform.position.x < 2)
                                                        power -= 1;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (power >= bestPower) {
                                    bestPower = power;
                                    bestPos = obj.transform;
                                    bestWolf = wolf[n];
                                }
                                wolf[n].transform.parent = nowParent.transform;
                            }
                        }
                    }
                }
            }
        }
        try {
            Move(bestWolf, bestPos);
            StartCoroutine(ChangeRound());
        }
        catch { }
    }
    //羊移动AI
    void SheepMove() {
        GameObject bestSheep=null;
        Transform bestPos = null;
        int bestPower = 0;
        if (leftSheep > 0) {
            for (int n = 0; n < 2; n++) {
                GameObject oWolf;
                if (n == 0)
                    oWolf = wolf[1];
                else
                    oWolf = wolf[0];
                int r_now = int.Parse(wolf[n].transform.parent.name.Substring(0, 1));
                int c_now = int.Parse(wolf[n].transform.parent.name.Substring(1));
                int r_owolf = int.Parse(oWolf.transform.parent.name.Substring(0, 1));
                int c_owolf = int.Parse(oWolf.transform.parent.name.Substring(1));
                for (int i = -2; i <= 2; i += 2) {
                    for (int j = -2; j <= 2; j += 2) {
                        if (i != 0 || j != 0) {
                            int power = 0;
                            string Pos = (r_now + i).ToString() + (c_now + j).ToString();
                            GameObject obj = GameObject.Find(Pos);
                            if (obj != null && obj.transform.childCount == 0) {
                                if (base.CanMove_Wolf(wolf[n], obj.transform)) {
                                    power += 10;
                                    float odis = Vector2.Distance(new Vector2(r_now + i, c_now + j), new Vector2(r_owolf, c_owolf));
                                    if (odis == firstDis || odis == secondDis) {
                                        if (base.CanMove_Wolf(oWolf, obj.transform))
                                            power += 10;
                                    } 
                                    int r_pos = 2 * (r_now + i) - r_owolf;
                                    int c_pos = 2 * (c_now + j) - c_owolf;
                                    GameObject nobj = GameObject.Find(r_pos.ToString() + c_pos.ToString());
                                    if (nobj != null && nobj.transform.childCount == 0) {
                                        float dis = Vector2.Distance(new Vector2(r_pos, c_pos), new Vector2(r_owolf, c_owolf));
                                        if (dis == secondDis)
                                            power -= 2;
                                        else if (dis == firstDis) {
                                            if ((r_owolf + c_owolf) % 2 == 0)
                                                power -= 2;
                                        }

                                    }
                                }
                                else {
                                    power += 8;
                                    int r_pos = 2 * (r_now + i) - int.Parse(oWolf.transform.parent.name.Substring(0, 1));
                                    int c_pos = 2 * (c_now + j) - int.Parse(oWolf.transform.parent.name.Substring(1));
                                    GameObject nobj = GameObject.Find(r_pos.ToString() + c_pos.ToString());
                                    if (nobj != null && nobj.transform.childCount == 0) {
                                        float dis = Vector2.Distance(new Vector2(r_pos, c_pos), new Vector2(r_owolf, c_owolf));
                                        if (dis == secondDis)
                                            power -= 2;
                                        else if (dis == firstDis) {
                                            if ((r_owolf + c_owolf) % 2 == 0)
                                                power -= 2;
                                        }
                                    }
                                }
                                if (power > bestPower) {
                                    bestPower = power;
                                    bestPos = obj.transform;
                                }
                            }
                        }
                    }
                }
                for (int i = -1; i <= 1; i++) {
                    for (int j = -1; j <= 1; j++) {
                        if (i != 0 || j != 0) {
                            int power = 0;
                            string Pos = (r_now + i).ToString() + (c_now + j).ToString();
                            GameObject obj = GameObject.Find(Pos);
                            if (obj != null && obj.transform.childCount == 0) {
                                if (base.CanMove_Wolf(wolf[n], obj.transform)) {
                                    power += 7;
                                    int r_pos = 2 * (r_now + i) - r_now;
                                    int c_pos = 2 * (c_now + j) - c_now;
                                    GameObject nextobj = GameObject.Find(r_pos.ToString() + c_pos.ToString());
                                    if (nextobj != null) {
                                        if (nextobj.transform.childCount != 0)
                                            power += 2;
                                        else
                                            power -= 2;
                                        int or_pos = 2 * (r_now + i) - r_owolf;
                                        int oc_pos = 2 * (c_now + j) - c_owolf;
                                        GameObject nobj = GameObject.Find(or_pos.ToString() + oc_pos.ToString());
                                        if (nobj != null && nobj.transform.childCount == 0)
                                            power -= 2;
                                    }
                                    if (power > bestPower) {
                                        bestPower = power;
                                        bestPos = obj.transform;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            base.AddSheep(bestPos);
            StartCoroutine(ChangeRound());
        }
        else {
            for (int n = 0; n < 2; n++) {
                GameObject oWolf;
                if (n == 0)
                    oWolf = wolf[1];
                else
                    oWolf = wolf[0];
                int r_now = int.Parse(wolf[n].transform.parent.name.Substring(0, 1));
                int c_now = int.Parse(wolf[n].transform.parent.name.Substring(1));
                int r_owolf = int.Parse(oWolf.transform.parent.name.Substring(0, 1));
                int c_owolf = int.Parse(oWolf.transform.parent.name.Substring(1));
                for (int i = -2; i <= 2; i += 2) {
                    for (int j = -2; j <= 2; j += 2) {
                        if (i != 0 || j != 0) {                        
                            string Pos = (r_now + i).ToString() + (c_now + j).ToString();
                            GameObject obj = GameObject.Find(Pos);
                            if (obj != null && obj.transform.childCount == 0) {
                                if (base.CanMove_Wolf(wolf[n], obj.transform)) {
                                    for (int a = -1; a <= 1; a++) {
                                        for (int b = -1; b <= 1; b++) {
                                            if (a != 0 || b != 0) {
                                                int power = 0;
                                                string nextPos = (r_now + i + a).ToString() + (c_now + j + b).ToString();
                                                GameObject nextobj = GameObject.Find(nextPos);
                                                if (nextobj != null && nextobj.transform.childCount != 0) {
                                                    GameObject oSheep = nextobj.transform.GetChild(0).gameObject;
                                                    if (oSheep.tag == "Sheep") {
                                                        if (base.CanMove_Sheep(oSheep, obj.transform)) {
                                                            power += 10;
                                                            int or_pos = 2 * (r_now + i) - r_owolf;
                                                            int oc_pos = 2 * (c_now + j) - c_owolf;
                                                            GameObject onobj = GameObject.Find(or_pos.ToString() + oc_pos.ToString());
                                                            if (onobj != null) {
                                                                if (onobj.transform.childCount == 0)
                                                                    power -= 2;
                                                                else {
                                                                    if (onobj.transform.GetChild(0).gameObject == oSheep)
                                                                        power -= 2;
                                                                }
                                                            }               
                                                            if (power > bestPower) {
                                                                bestPower = power;
                                                                bestSheep = oSheep;
                                                                bestPos = obj.transform;
                                                            }
                                                        }
                                                    }   
                                                }
                                                else if (nextobj != null && nextobj.transform.childCount == 0) {
                                                    GameObject hSheep= GameObject.Find((r_now + i / 2).ToString() + (c_now + j / 2).ToString()).transform.GetChild(0).gameObject;
                                                    if (hSheep.tag == "Sheep") {
                                                        if (base.CanMove_Sheep(hSheep, nextobj.transform)) {
                                                            power += 8;
                                                            for (int c = -1; c <= 1; c++) {
                                                                for (int d = -1; d <= 1; d++) {
                                                                    if (c != 0 || d != 0) {
                                                                        string nPos = (r_now + i + a + c).ToString() + (c_now + b + j + d).ToString();
                                                                        GameObject nobj = GameObject.Find(nextPos);
                                                                        if (nobj != null && nobj.transform.childCount != 0) {
                                                                            if (nobj.transform.GetChild(0).tag == "Wolf")
                                                                                power -= 2;
                                                                        }

                                                                    }
                                                                }
                                                            }
                                                            if (power > bestPower) {
                                                                bestPower = power;
                                                                bestSheep = hSheep;
                                                                bestPos = nextobj.transform;
                                                            }
                                                        }
                                                    }
                                                    
                                                }
                                            }
                                        }
                                    }
                                }
                                else {
                                    for (int a = -1; a <= 1; a++) {
                                        for (int b = -1; b <= 1; b++) {
                                            if (a != 0 || b != 0) {
                                                int power = 0;
                                                string nextPos = (r_now + i + a).ToString() + (c_now + j + b).ToString();
                                                GameObject nextobj = GameObject.Find(nextPos);
                                                if (nextobj != null && nextobj.transform.childCount != 0) {
                                                    GameObject oSheep = nextobj.transform.GetChild(0).gameObject;
                                                    if (oSheep.tag == "Sheep") {
                                                        if (base.CanMove_Sheep(oSheep, obj.transform)) {
                                                            power += 8;
                                                            int or_pos = (r_now + i + a + r_owolf)/2;
                                                            int oc_pos = (c_now + j + b + c_owolf)/2;
                                                            GameObject onobj = GameObject.Find(or_pos.ToString() + oc_pos.ToString());
                                                            if (onobj != null && onobj.transform.childCount != 0)
                                                                power -= 2;
                                                            int r_pos = 2 * (r_now + i) - r_owolf;
                                                            int c_pos = 2 * (c_now + j) - c_owolf;
                                                            GameObject nobj = GameObject.Find(or_pos.ToString() + oc_pos.ToString());
                                                            if (nobj != null) {
                                                                if (nobj.transform.childCount == 0)
                                                                    power -= 2;
                                                                else if (nobj.transform.GetChild(0).gameObject == oSheep)
                                                                    power -= 2;
                                                            }
                                                               
                                                        }
                                                        if (power > bestPower) {                                                        
                                                            bestPower = power;
                                                            bestSheep = oSheep;
                                                            bestPos = obj.transform;
                                                        }
                                                    }    
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
                for(int i = -1; i <= 1; i++) {
                    for(int j = -1; j <= 1; j++) {
                        if (i != 0 || j != 0) {
                            string Pos = (r_now + i).ToString() + (c_now + j).ToString();
                            GameObject obj = GameObject.Find(Pos);
                            if (obj != null && obj.transform.childCount == 0) {
                                if (base.CanMove_Wolf(wolf[n], obj.transform)) {
                                    for (int a = -1; a <= 1; a++) {
                                        for (int b = -1; b <= 1; b++) {
                                            if (a != 0 || b != 0) {
                                                int power = 0;
                                                string nextPos = (r_now + i + a).ToString() + (c_now + j + b).ToString();
                                                GameObject nextobj = GameObject.Find(nextPos);
                                                if (nextobj != null && nextobj.transform.childCount != 0) {
                                                    GameObject oSheep = nextobj.transform.GetChild(0).gameObject;
                                                    if (oSheep.tag == "Sheep") {
                                                        if (base.CanMove_Sheep(oSheep, obj.transform)) {
                                                            power += 7;
                                                            int r_pos = 2 * (r_now + i) - r_now;
                                                            int c_pos = 2 * (c_now + j) - c_now;
                                                            GameObject nobj = GameObject.Find(r_pos.ToString() + c_pos.ToString());
                                                            if(nobj != null) {
                                                                if (nobj.transform.childCount == 0)
                                                                    power -= 2;
                                                                else {
                                                                    if (nobj.transform.GetChild(0).gameObject == oSheep)
                                                                        power -= 2;
                                                                }
                                                            }
                                                            float dis = Vector2.Distance(new Vector2(r_owolf, c_owolf), new Vector2(r_now + i + a, c_now + j + b));
                                                            if (dis == firstDis || dis == secondDis) {
                                                                if (base.CanMove_Wolf(oWolf, nextobj.transform))
                                                                    power -= 2;
                                                            }
                                                            int or_pos = 2 * (r_now + i) - r_owolf;
                                                            int oc_pos = 2 * (c_now + j) - c_owolf;
                                                            GameObject onobj = GameObject.Find(or_pos.ToString() + oc_pos.ToString());
                                                            if (nobj != null && nobj.transform.childCount == 0)
                                                                power -= 2;
                                                            if (power > bestPower) {
                                                                bestPower = power;
                                                                bestSheep = oSheep;
                                                                bestPos = obj.transform;
                                                            }
                                                        }
                                                    }        
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            base.Move(bestSheep, bestPos);
            StartCoroutine(ChangeRound());
        }
    }
}
