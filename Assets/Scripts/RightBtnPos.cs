using UnityEngine;
using System.Collections;

public class RightBtnPos : MonoBehaviour {
    public float offset;
    private RectTransform rect;
    void Start() {
        rect = GameObject.Find("Canvas").GetComponent<RectTransform>();

    }
    void Update() {
        float pos_x = (rect.sizeDelta.x / 2) - offset;
        if (this.transform.position.x != pos_x)
            this.transform.position = new Vector3(pos_x / 100, transform.position.y, transform.position.z);
    }
}
