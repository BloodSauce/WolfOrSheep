using UnityEngine;
using System.Collections;

public class GameManagerLocal :GameManager{
    public static GameManagerLocal gmLocal = null;
    public GameObject ShowSheepLeft_Down;
    void Awake() {
        if (gmLocal == null)
            gmLocal = this;
        else if (gmLocal != this)
            Destroy(gameObject);
    }
    protected override void Start() {
        base.Start();
        ShowSheepLeft_Down.SetActive(true);
        StartCoroutine(FirstRoundShow());
    }
}
