using UnityEngine;

using System.Collections;

using UnityEngine.UI;

public class MovementScript : MonoBehaviour
{
    public Slider healthBarSlider;  //reference for slider
    public Text gameOverText;   //reference for text
    private bool isGameOver = false; //flag to see if game is over

    void Start()
    {
        gameOverText.enabled = false; //disable GameOver text on start
    }
    // Update is called once per frame

    void Update()
    {
        //check if game is over i.e., health is greater than 0
        if (!isGameOver)

            transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * 10f, 0, 0); //get input
    }
    //Check if player enters/stays on the fire

    void OnTriggerStay(Collider other)
    {
        //if player triggers fire object and health is greater than 0
        if (other.gameObject.name == "Fire" && healthBarSlider.value > 0)
        {
            healthBarSlider.value -= .011f;  //reduce health
        }
        else
        {
            isGameOver = true;    //set game over to true
            gameOverText.enabled = true; //enable GameOver text
        }
    }
}
/*
 * - GameOverText는 Health가 0일때만 보여야 되기 때문에, Start 함수에서 GameOverText를 disable시킨다.



- Update함수에서 isGameOver가 false인 경우, 키보드로부터 입력을 받는다. 왼쪽 화살표키를 움직이면 왼쪽으로, 오른쪽 화살표키를 누르면 오른쪽으로 움직인다.



- OnTriggerStay함수는 Slider의 Value값이 0보다 클 경우에, Player와 Fire객체의 접촉을 감지한다. 만약 Player가 Fire와 접촉할 경우, Health는 프레임당 0.01씩 감소할 것이다. 이 값은 Slider에 나타난다. 만약 Value가 0일경우, GameOverText가 enable 된다.



(Note: Slider Value의 기본 값은 1로 설정된다.)


출처: http://unityindepth.tistory.com/57 [UNITY IN DEPTH]
*/
