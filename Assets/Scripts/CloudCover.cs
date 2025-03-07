using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCover : MonoBehaviour
{
    [Header("Inscribed")]
    public Sprite[] cloudSprites;
    public int numClouds = 30;
    public Vector3 minPos = new Vector3(-10, 5, 0);
    public Vector3 maxPos = new Vector3(100, 10, 0);
    [Tooltip("For scaleRange, x is the min value, and y is the max value.")]
    public Vector2 scaleRange = new Vector2(1, 4);

    void Start()
    {
        Transform parentTrans = this.transform;
        GameObject cloudGO;
        Transform cloudTrans;
        SpriteRenderer sRend;
        float scaleMulti;
        for (int i = 0; i < numClouds; i++) {
            cloudGO = new GameObject();
            cloudTrans = cloudGO.transform;
            sRend = cloudGO.AddComponent<SpriteRenderer>();

            int spriteNum = Random.Range(0, cloudSprites.Length);
            sRend.sprite = cloudSprites[spriteNum];

            cloudTrans.position = RandomPos();
            cloudTrans.SetParent(parentTrans, true);

            scaleMulti = Random.Range(scaleRange.x, scaleRange.y);
            cloudTrans.localScale = Vector3.one * scaleMulti;
        }
    }

    Vector3 RandomPos() {
        Vector3 pos = new Vector3();
        pos.x = Random.Range(minPos.x, minPos.y);
        pos.y = Random.Range(minPos.y, maxPos.y);
        pos.z = Random.Range(minPos.z, maxPos.z);

        return pos;
    }
}
