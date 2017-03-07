using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Round {
    WolfRound,
    SheepRound
}

    
public class GameManager : MonoBehaviour {
    public GameObject sheep;            
    public GameObject[] wolf;
    public Material defaultMaterial;         //默认材质
    public Material wolfClickMaterial;         //选中时材质
    public Material sheepClickMaterial;
    public Text leftSheepNum_UP;            //显示未落下的羊个数
    public Text leftSheepNum_Down;
    public GameObject wolfRoundPanel;        //回合提示
    public GameObject sheepRoundPanel;
    public GameObject wolfWinPanel;         //胜利提示
    public GameObject sheepWinPanel;
    public GameObject choosePanel;         //选择角色
    public float showTimeDel;
    public Round round = Round.WolfRound;
    public Transform clickPos;
    public GameObject nowObject;
    public int leftSheep = 16;
    public int totalSheep = 24;
    protected bool hasAdd = false;
    protected float firstDis = Mathf.Sqrt(8);
    protected float secondDis =2;
    protected float thirdDis = Mathf.Sqrt(2);
    protected float fourthDis = 1;

    protected AudioSource audioSource;

    public AudioClip winAudio;          //音效
    public AudioClip loseAudio;
    public AudioClip moveAudio;

    protected virtual void Start() {
        if(ChooseManager.gameType==GameType.PVE)
            choosePanel.SetActive(true);
        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        leftSheepNum_UP.text ="16";
        leftSheepNum_Down.text = "16";
    }

    // Update is called once per frame
    protected virtual void Update() {
        if (nowObject != null || clickPos != null) {
            ClickJudge();    
        }
            
    }
    //点击判断
    void ClickJudge() {
        switch (round) {
            case Round.WolfRound:
                hasAdd = false;
                if (nowObject != null && clickPos != null) {
                    if (CanMove_Wolf(nowObject, clickPos)) {
                        Move(nowObject, clickPos);                      
                    }
                    else
                        clickPos = null;
                } break;
            case Round.SheepRound:
                if (leftSheep> 0) {
                    if (clickPos != null) {
                        AddSheep(clickPos);
                    }
                    nowObject = null;
                    clickPos = null;
                    break;
                }
                else {
                    if (nowObject != null && clickPos != null) {
                        if (CanMove_Sheep(nowObject, clickPos)) {
                            Move(nowObject, clickPos);                        
                        }    
                    } break;
                }

        }
    }
    //增加羊
    protected virtual void AddSheep(Transform pos) {
        if (hasAdd == false) {
            hasAdd = true;
            GameObject newOb = Instantiate(sheep.gameObject, new Vector3(pos.position.x, pos.position.y, -1), Quaternion.identity) as GameObject;
            newOb.transform.parent = pos.transform;
            AudioSource.PlayClipAtPoint(moveAudio, new Vector3(newOb.transform.position.x, newOb.transform.position.y, -5), 1);
            leftSheep--;
            leftSheepNum_UP.text = leftSheep.ToString();
            leftSheepNum_Down.text = leftSheep.ToString();
            StartCoroutine(ChangeRound());
        }
    }
    //狼移动判断
    protected bool CanMove_Wolf(GameObject now, Transform pos) {
        int r_now = int.Parse(now.transform.parent.name.Substring(0, 1));
        int c_now = int.Parse(now.transform.parent.name.Substring(1));
        int r_pos = int.Parse(pos.transform.name.Substring(0, 1));
        int c_pos = int.Parse(pos.transform.name.Substring(1));
        int nowSum = r_now + c_now;
        int posSum= r_pos + c_pos;
        float dis = Vector2.Distance(new Vector2(r_now, c_now), new Vector2(r_pos, c_pos));
        if (dis != firstDis && dis != secondDis && dis != thirdDis && dis != fourthDis)
            return false;
        if (r_now == 0 || r_now == 1) {
            if (r_pos == 0 || r_pos == 1 || (r_pos == 2 && c_pos == 2)) {
                if (dis != secondDis)
                    return true;
                else {
                    string dieSheepPos = ((r_now + r_pos) * 0.5).ToString() + ((c_now + c_pos) * 0.5).ToString();
                    GameObject obj = GameObject.Find(dieSheepPos);
                    if (obj.transform.childCount != 0 && obj.transform.GetChild(0).tag == "Sheep")
                        return true;
                    else
                        return false;
                }
            }
            else if ((((r_pos == 3 && c_pos == 1) || (r_pos == 3 && c_pos == 3)) && (dis == firstDis)) || (r_pos == 3 && c_pos == 2)) {
                string dieSheepPos = ((r_now + r_pos) * 0.5).ToString() + ((c_now + c_pos) * 0.5).ToString();
                GameObject obj = GameObject.Find(dieSheepPos);
                if (obj.transform.childCount != 0 && obj.transform.GetChild(0).tag == "Sheep")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        if (r_now == 7 || r_now == 8) {
            if (r_pos == 7 || r_pos == 8 || (r_pos == 6 && c_pos == 2)) {
                if (dis != secondDis)
                    return true;
                else {
                    string dieSheepPos = ((r_now + r_pos) * 0.5).ToString() + ((c_now + c_pos) * 0.5).ToString();
                    GameObject obj = GameObject.Find(dieSheepPos);
                    if (obj.transform.childCount != 0 && obj.transform.GetChild(0).tag == "Sheep")
                        return true;
                    else
                        return false;
                }
            }
                
            else if((((r_pos == 5 && c_pos == 1)  || (r_pos == 5 && c_pos == 3))&& (dis == firstDis)) || (r_pos == 5 && c_pos == 2)) {
                string dieSheepPos = ((r_now + r_pos) * 0.5).ToString() + ((c_now + c_pos) * 0.5).ToString();
                GameObject obj = GameObject.Find(dieSheepPos);
                if (obj.transform.childCount != 0 && obj.transform.GetChild(0).tag == "Sheep")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        if (r_now == 2 && (r_pos == 0 || r_pos == 1) && c_now != 2)
            return false;
        if(r_now == 6 && (r_pos == 7 || r_pos == 8) && c_now != 2)
            return false;
        if (dis == secondDis) {
            if (r_pos == 1 && c_now != 2)
                return false;
            if (r_pos == 7 && c_now != 2)
                return false;
            string dieSheepPos = ((r_now + r_pos) * 0.5).ToString() + ((c_now + c_pos) * 0.5).ToString();
            GameObject obj = GameObject.Find(dieSheepPos);
            if (obj.transform.childCount == 0)
                return false;
            else {
                if (obj.transform.GetChild(0).tag == "Wolf")
                    return false;
                else
                    return true;
            }
        }
        if (dis == firstDis) {
            if (nowSum % 2 != 0)
                return false;
            if (r_pos == 1 && (c_now != 1 && c_now != 3))
                return false;

            if (r_pos == 7  && (c_now != 1 && c_now != 3))
                return false;  
            string dieSheepPos = ((r_now + r_pos) * 0.5).ToString() + ((c_now + c_pos) * 0.5).ToString();
            GameObject obj = GameObject.Find(dieSheepPos);
            if (obj.transform.childCount == 0)
                return false;
            else {
                if (obj.transform.GetChild(0).tag == "Wolf")
                    return false;
                else
                    return true;
            }
        }
        if (nowSum % 2 == 0)
            return true;
        else {
            if (posSum % 2 == 0)
                return true;
            else
                return false;
        }
        
    }
    //羊移动判断
    protected bool CanMove_Sheep(GameObject now, Transform pos) {
        int r_now = int.Parse(now.transform.parent.name.Substring(0, 1));
        int c_now = int.Parse(now.transform.parent.name.Substring(1));
        int r_pos = int.Parse(pos.transform.name.Substring(0, 1));
        int c_pos = int.Parse(pos.transform.name.Substring(1));
        int nowSum = r_now + c_now;
        int posSum = r_pos + c_pos;
        float dis = Vector2.Distance(new Vector2(r_now, c_now), new Vector2(r_pos, c_pos));
        if (dis !=thirdDis && dis != fourthDis)
            return false;
        if (r_now == 0 || r_now == 1) {
            if (r_pos == 0 || r_pos == 1 || (r_pos == 2 && c_pos == 2))
                return true;
            else
                return false;
        }
        if (r_now == 7 || r_now == 8) {
            if (r_pos == 7 || r_pos == 8 || (r_pos == 6 && c_pos == 2))
                return true;
            else
                return false;
        }
        if (nowSum % 2 == 0)
            return true;
        else {
            if (posSum % 2 == 0)
                return true;
            else
                return false;
        }

    }
    //棋子移动
    protected virtual void Move(GameObject now,Transform pos) {
        int r_now = int.Parse(now.transform.parent.name.Substring(0, 1));
        int c_now = int.Parse(now.transform.parent.name.Substring(1,1));
        int r_pos = int.Parse(pos.transform.name.Substring(0, 1));
        int c_pos = int.Parse(pos.transform.name.Substring(1,1));
        float dis = Vector2.Distance(new Vector2(r_now, c_now), new Vector2(r_pos, c_pos));
        if (dis == firstDis || dis == secondDis) {
            string dieSheepPos = ((r_now + r_pos) * 0.5).ToString() + ((c_now + c_pos) * 0.5).ToString();
            if (GameObject.Find(dieSheepPos).transform.childCount != 0) {
                Destroy(GameObject.Find(dieSheepPos).transform.GetChild(0).gameObject);
                totalSheep--;
            }
        }
        now.transform.position = new Vector3(pos.position.x,pos.position.y,-1);
        now.transform.parent = pos.transform;
        AudioSource.PlayClipAtPoint(moveAudio,new Vector3(now.transform.position.x, now.transform.position.y,-5), 1);
        if(nowObject!=null)
        nowObject.GetComponent<Renderer>().material = defaultMaterial;
        nowObject = null;
        clickPos = null;
        StartCoroutine(ChangeRound());
    }
    //回合变换
    protected virtual IEnumerator ChangeRound() {
        isGameOver();
        if (round == Round.WolfRound) {
            sheepRoundPanel.gameObject.SetActive(true);
            yield return new WaitForSeconds(showTimeDel);
            sheepRoundPanel.gameObject.SetActive(false);
            round = Round.SheepRound;
        }
        else {
            wolfRoundPanel.gameObject.SetActive(true);
            yield return new WaitForSeconds(showTimeDel);
            wolfRoundPanel.gameObject.SetActive(false);
            round = Round.WolfRound;
        }
    }
    //开始提示
    protected IEnumerator FirstRoundShow() {
        wolfRoundPanel.SetActive(true);
        yield return new WaitForSeconds(showTimeDel);
        wolfRoundPanel.SetActive(false);
    }
    //游戏结束判断
    protected void isGameOver() {
        List<GameObject> points = new List<GameObject>();
        if (totalSheep >= 4) {
            for (int n = 0; n < 2; n++) {
                int r_now = int.Parse(wolf[n].transform.parent.name.Substring(0, 1));
                int c_now = int.Parse(wolf[n].transform.parent.name.Substring(1));
                if (r_now == 0 || r_now == 1) {
                    points.Add(GameObject.Find("22"));
                    points.Add(GameObject.Find("31"));
                    points.Add(GameObject.Find("32"));
                    points.Add(GameObject.Find("33"));
                    foreach (GameObject g in points) {
                        if (g.transform.childCount == 0) {
                            return;
                        }
                    }
                    points.Clear();
                }
                else if (r_now == 7 || r_now == 8) {
                    points.Add(GameObject.Find("62"));
                    points.Add(GameObject.Find("51"));
                    points.Add(GameObject.Find("52"));
                    points.Add(GameObject.Find("53"));
                    foreach (GameObject g in points) {
                        if (g.transform.childCount == 0) {
                            return;
                        }
                    }
                    points.Clear();
                }
                else {
                    for (int i = -2; i <= 2; i++) {
                        for (int j = -2; j <= 2; j++) {
                            if (i != 0 || j != 0) {
                                GameObject obj = GameObject.Find((r_now + i).ToString() + (c_now + j).ToString());
                                if (obj != null && obj.transform.childCount == 0) {
                                    if ((r_now + i == 0) || (r_now + i == 1) || (r_now + i == 7) || (r_now + i == 8)) {
                                        if (CanMove_Wolf(wolf[n], obj.transform)) {
                                            points.Add(obj);
                                        }
                                    }
                                    else {
                                        points.Add(obj);
                                    }
                                }
                            }       
                        }
                    }
                    foreach (GameObject g in points) {
                        if (CanMove_Wolf(wolf[n], g.transform)) {
                            return;
                        }
                    }
                    points.Clear();
                }
            }
            if (sheepWinPanel.activeInHierarchy == false) {
                audioSource.Pause();
                AudioSource.PlayClipAtPoint(winAudio,new Vector3(0,0,-5));
                sheepWinPanel.SetActive(true);
            }
        }
        else {
            audioSource.Pause();
            AudioSource.PlayClipAtPoint(winAudio, new Vector3(0, 0, -5));
            wolfWinPanel.SetActive(true);
        }
            
    }
   
}