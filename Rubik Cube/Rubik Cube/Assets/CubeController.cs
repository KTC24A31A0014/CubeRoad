using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++++++++++
//キューブ１つをコントロールするクラス
//++++++++++++++++++++++++++++++++++++++
public class CubeController : MonoBehaviour
{
    List<DiscController> discList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Disc" && discList != null)
        {
            //接触したディスクをListに登録
            DiscController discCnt = other.gameObject.GetComponent<DiscController>();
            if (discCnt.rotateX) { discList[0] = discCnt; }
            if (discCnt.rotateY) { discList[1] = discCnt; }
            if (discCnt.rotateZ) { discList[2] = discCnt; }
            //discList.Add(discCnt);
            other.gameObject.GetComponent<BoxCollider>().enabled = false;   //接触確認したディスクは当たりをオフ
        }
    }

    //接触ディスクのチェック
    public void CheckContactedDisc()
    {
        discList = new List<DiscController>() {null, null , null };      //Listを初期化
        StartCoroutine(ContactDiscReturn());
    }
    //接触ディスクを返すコルーチン
    IEnumerator ContactDiscReturn()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject.FindFirstObjectByType<CubeManager>().SetCanRotateDiscs(discList);
        discList = null;
    }
}
