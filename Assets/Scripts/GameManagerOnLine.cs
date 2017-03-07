using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Text;

public class GameManagerOnLine :GameManager {
    public static GameManagerOnLine gmOnLine = null;
    public GameObject wolfImage;
    public GameObject sheepImage;
    public GameObject connectPanel;     //连接服务器
    public GameObject searchPanel;      //匹配对手
    public GameObject breakPanel;       //断开连接
    public GameObject playerLeavePanel; //对手离开
    public string role;                 //角色
    private OnLineControl client;
    private bool ableToPlay=false;
    void Awake() {   
        if (gmOnLine == null)
            gmOnLine = this;
        else if (gmOnLine != this)
            Destroy(gameObject);
    }
    // Use this for initialization
    protected override void Start () {
        base.Start();
        client = new OnLineControl();
        connectPanel.SetActive(true);
        //连接服务器
        if (client.Connect()) {
            connectPanel.SetActive(false);
            searchPanel.SetActive(true);    
        }
        else {
            connectPanel.SetActive(false);
            breakPanel.SetActive(true);
        }       
    }
    protected override void Update() {
        if (client.isConnect) {
            if (ableToPlay) {
                searchPanel.SetActive(false);    
                //回合变换          
                if (role == "sheep" && round == Round.WolfRound)
                    OpponentMove();              
                else if (role == "wolf" && round == Round.SheepRound)
                    OpponentMove();
                else
                    base.Update();
            }
            else {
                //匹配对手并确定角色
                if (client.msgLength > 0) {
                    try {
                        string msg = Encoding.ASCII.GetString(client.msgRec, 0, client.msgLength);
                                               
                        if (msg[1] == 'w') {
                            role = "wolf";
                            wolfImage.SetActive(true);
                        }
                        else if (msg[1] == 's') {
                            role = "sheep";
                            sheepImage.SetActive(true);
                        }
                        if (msg[0] == 'y')
                            ableToPlay = true;
                        StartCoroutine(FirstRoundShow());
                    }
                    catch (Exception e) {
                        
                    }
                }
            }            
        }    
        else
            breakPanel.SetActive(true);
    }
    //增加向服务器发送消息
    protected override void AddSheep(Transform pos) {
        if (hasAdd == false) {
            hasAdd = true;
            GameObject newOb = Instantiate(sheep.gameObject, new Vector3(pos.position.x, pos.position.y, -1), Quaternion.identity) as GameObject;
            newOb.transform.parent = pos.transform;
            AudioSource.PlayClipAtPoint(moveAudio, newOb.transform.position);
            leftSheep--;
            leftSheepNum_UP.text = leftSheep.ToString();
            leftSheepNum_Down.text = leftSheep.ToString();
            client.SendMsg("a" + pos.transform.name);
            StartCoroutine(ChangeRound());
        }
    }
    protected override void Move(GameObject now, Transform pos) {
        int r_now = int.Parse(now.transform.parent.name.Substring(0, 1));
        int c_now = int.Parse(now.transform.parent.name.Substring(1));
        int r_pos = int.Parse(pos.transform.name.Substring(0, 1));
        int c_pos = int.Parse(pos.transform.name.Substring(1));
        float dis = Vector2.Distance(new Vector2(r_now, c_now), new Vector2(r_pos, c_pos));
        if (dis == firstDis || dis == secondDis) {
            string dieSheepPos = ((r_now + r_pos) * 0.5).ToString() + ((c_now + c_pos) * 0.5).ToString();
            if (GameObject.Find(dieSheepPos).transform.childCount != 0) {
                Destroy(GameObject.Find(dieSheepPos).transform.GetChild(0).gameObject);
                totalSheep--;
            }
        }
        now.transform.position = new Vector3(pos.position.x, pos.position.y, -1);
        client.SendMsg("m" + now.transform.parent.name + pos.transform.name);
        now.transform.parent = pos.transform;
        AudioSource.PlayClipAtPoint(moveAudio, now.transform.position);
        nowObject = null;
        clickPos = null;
        StartCoroutine(ChangeRound());       
    }
    //等待对方移动
    void OpponentMove() {
        if(round==Round.WolfRound)
            hasAdd = false;
        bool hadMove = false;
        string msg = Encoding.ASCII.GetString(client.msgRec,0,client.msgLength);
        if (client.msgLength > 0) {
            if (msg[0] == 'r')
                playerLeavePanel.SetActive(true);
            if (msg[0] == 'a') {
                base.AddSheep(GameObject.Find(msg.Substring(1, 2)).transform);

                client.Clean();
                hadMove = true;
            }
            else if (msg[0] == 'm') {
                base.Move(GameObject.Find(msg.Substring(1, 2)).transform.GetChild(0).gameObject, GameObject.Find(msg.Substring(3, 2)).transform);
                client.Clean();
                hadMove = true;
            }
        }
        if (hadMove) {
            StartCoroutine(ChangeRound());
            hadMove = false;
        }
    }
    void OnDestroy() {
        client.Close();
    }

}
