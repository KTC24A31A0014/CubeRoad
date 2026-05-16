using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++++++++++
//キューブを回転させるディスクをコントロールするクラス
//++++++++++++++++++++++++++++++++++++++
public class DiscController : MonoBehaviour
{
    //このディスクが回転する軸
    public bool rotateX;
    public bool rotateY;
    public bool rotateZ;

    Transform baseTransform;    //キューブとディスクの親クラス
    bool inRotate = false;      //回転中フラグ
    bool inChkCube = false;     //キューブチェック中フラグ

    private void Awake()
    {
        GetComponent<BoxCollider>().enabled = false;        //当たりを無効化
    }

    // Start is called before the first frame update
    void Start()
    {
        baseTransform = transform.parent;       //親クラス取得
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cube")
        {
            //キューブに接触
            if (!inRotate && !inChkCube)
            {
                other.gameObject.transform.SetParent(transform);
            }
        }
    }

    //
    public void CheckCube(bool b)
    {
        inChkCube = b;
        GetComponent<BoxCollider>().enabled = b;
    }

    //回転
    public void RotateCube(float angle)
    {
        if (!inRotate)
        {
            inChkCube = false;
            GetComponent<BoxCollider>().enabled = true;
            StartCoroutine(Rotate(angle));
        }
    }

    //回転コルーチン
    IEnumerator Rotate(float angle)
    {
        Debug.Log("回転:" + gameObject.name);

        yield return new WaitForSeconds(0.1f);
        GetComponent<BoxCollider>().enabled = false;
        inRotate = true;
        Vector3 from = transform.eulerAngles;
        Vector3 to = transform.eulerAngles;
        if (rotateX) { to.x += angle; }
        if (rotateY) { to.y += angle; }
        if (rotateZ) { to.z += angle; }
        float d = 0;
        while (d < 1.0f)
        {
            d += 4.0f * Time.deltaTime;
            if (d > 1.0f)
            {
                d = 1.0f;
            }
            transform.eulerAngles = Vector3.Lerp(from, to, d);
            yield return null;
        }
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform t = transform.GetChild(i);
            t.SetParent(baseTransform);
        }
        yield return new WaitForSeconds(0.1f);
        transform.eulerAngles = Vector3.zero;
        inRotate = false;
    }
}
