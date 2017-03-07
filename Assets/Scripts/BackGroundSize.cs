using UnityEngine;
using System.Collections;

public class BackGroundSize : MonoBehaviour {
    RectTransform rect;
    void Start() {
        rect = GameObject.Find("Canvas").GetComponent<RectTransform>();
    }
	void Update() {
        //宽度适配不同分辨率
        float scale=rect.sizeDelta.x / 600;
        if (this.transform.localScale.x != scale) {
            this.transform.localScale = new Vector3(scale, 1, 1);
        }
    }

}
