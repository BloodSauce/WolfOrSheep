using UnityEngine;
using System.Collections;

public class ReturnButtonPos : MonoBehaviour {
    RectTransform rect;
    void Start() {
        rect = GameObject.Find("Canvas").GetComponent<RectTransform>();

    }
    void Update() {
        float pos_x = 0-((rect.sizeDelta.x / 2) - 100);
        if (this.transform.position.x != pos_x)
            this.transform.position = new Vector3(pos_x / 100, transform.position.y, transform.position.z);
    }
}
