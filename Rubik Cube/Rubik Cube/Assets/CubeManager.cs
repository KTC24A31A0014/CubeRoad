using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++++++++++
//キューブ全体を管理するクラス
//++++++++++++++++++++++++++++++++++++++
public class CubeManager : MonoBehaviour
{
    List<DiscController> canRotateDiscs;
    Vector3 cobeSafaceNormal;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canRotateDiscs != null && canRotateDiscs.Count > 0)
        {
            DiscController discX = null;
            DiscController discY = null;
            if (cobeSafaceNormal.x != 0)
            {
                discX = canRotateDiscs[1];
                discY = canRotateDiscs[2];
            }
            else if (cobeSafaceNormal.y != 0)
            {
                discX = canRotateDiscs[1];
                discY = canRotateDiscs[1];
            }
            else if (cobeSafaceNormal.z != 0)
            {
                discX = canRotateDiscs[1];
                discY = canRotateDiscs[0];
            }
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            Vector2 v = new Vector2(x, y).normalized;
            if(v.magnitude > 0.5f)
            {
                float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                if (angle > -45 && angle < 45 && discX != null)
                {
                    //右
                    discX.GetComponent<DiscController>().RotateCube(-90);
                }
                else if (angle > 45 && angle < 135 && discY != null)
                {
                    //上
                    discY.GetComponent<DiscController>().RotateCube(90);
                }
                else if (angle > -135 && angle < -45 && discY != null)
                {
                    //下
                    discY.GetComponent<DiscController>().RotateCube(-90);
                }
                else if(discX != null)
                {
                    //左
                    discX.GetComponent<DiscController>().RotateCube(90);
                }
                canRotateDiscs = null;
            }
        }
        else
        {
            //マウスでのキューブ選択
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject clickedObj = hit.collider.gameObject;
                    CubeController cube = clickedObj.GetComponent<CubeController>();
                    if (cube != null)
                    {
                        //選択した面の法線を取得
                        cobeSafaceNormal = hit.normal;
                        Debug.Log(cobeSafaceNormal);
                        //キューブと接触しているディスクチェック
                        StartCoroutine(DiscColliderCheck());
                        cube.CheckContactedDisc();
                    }
                }
            }
        }
    }

    //DiscControllerの当たりを時間差でオン／オフするコルーチン
    IEnumerator DiscColliderCheck()
    {
        GameObject[] discs = GameObject.FindGameObjectsWithTag("Disc");
        foreach (GameObject disc in discs)
        {
            disc.GetComponent<DiscController>().CheckCube(true);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject disc in discs)
        {
            disc.GetComponent<DiscController>().CheckCube(false);
        }
    }

    //回転可能な軸を設定
    public void SetCanRotateDiscs(List<DiscController> didcList)
    {
        if (didcList != null && didcList.Count > 0)
        {
            canRotateDiscs = didcList;
        }
    }

}
