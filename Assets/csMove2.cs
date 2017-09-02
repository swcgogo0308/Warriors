using System.Collections;
using UnityEngine;

public class csMove2 : MonoBehaviour {

    //충돌이 일어날때 발생하는 이벤트
   void csCollisionEnter(Collision clooision)
    {
        //여기에 충돌 처리 됫을때 발생하는거 할거임
        Debug.Log("OnCollisionEnter");
    }
}
