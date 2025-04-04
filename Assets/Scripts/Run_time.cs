using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Run_time : MonoBehaviour
{
    public float current_time;
    public Text Seconds, minutes;
    public int late ;
    public GameObject player1, player2;
    public GameObject gameOver;

    public Vector2 V_pl1, V_pl2;
    private bool Over_Time = false;
    private void Start()
    {
       Seconds.text = current_time.ToString();
        minutes.text = "0" + late.ToString() + ":";

        V_pl1 = player1.transform.position;

        V_pl2 = player2.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Over_Time && (player1.GetComponent<Rigidbody2D>().position.x != V_pl1.x || player2.GetComponent<Rigidbody2D>().position.x != V_pl2.x))
        {
            current_time -= Time.deltaTime;
            if (current_time < 10)
            {


                Seconds.text = "0" + current_time.ToString();

            }
            else
            {
                Seconds.text = current_time.ToString();
            }


            if (current_time < 0.1f)
            {
                late--;
                minutes.text = "0" + late.ToString() + ":";
                current_time = 60;
            }
        }
        EndTime();
    }

    void EndTime(){
        if (late == 0f && current_time < 1  )
        {
            Over_Time = true;
            gameOver.SetActive(true);
        }
    }
}
