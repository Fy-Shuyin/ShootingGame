using UnityEngine;
using System.Collections;

public class SystemHour : MonoBehaviour
{
    public GameObject sun;
    public GameObject moon;

    GameObject sunObject, moonObject;

    float sunHight, sunLeight, sunWidth, moonHight, moonLeight,moonWidth;
    float sunDeltaX, sunDeltaY, sunDeltaZ, moonDeltaX, moonDeltaY, moonDeltaZ;
    float sunAngle,moonAngle;

    int hour = 0;
    float minute = 0;

	void Awake ()
    {
        hour = System.DateTime.Now.Hour;
        minute = System.DateTime.Now.Minute;
   
        moonAngle = Random.Range(-60, 60);
    }

    void Start()
    {
        sunHight = sunLeight = 250;
        moonHight = moonLeight = 220;
        sunWidth = 100;
        moonWidth = 150;
    }
	
	void Update ()
    {
        minute += 10 * Time.deltaTime;
        sunAngle = (hour * 60 + minute  ) /4 ;
        if (minute >= 60)
        {
            hour++;
            minute = 0;
            if (hour == 24)
            hour = 0;
        }
        Sun();
        Moon();
    }

    void Sun()
    {
        sunDeltaX = sunLeight * Mathf.Sin(sunAngle * Mathf.PI / 180);
        sunDeltaY = sunHight * Mathf.Cos(sunAngle * Mathf.PI / 180);
        sunDeltaZ = sunWidth * Mathf.Cos(sunAngle * Mathf.PI / 180);
        sun.transform.position = new Vector3(transform.position.x - sunDeltaX,transform.position.y - sunDeltaY,transform.position.z - sunDeltaZ);
        sun.transform.LookAt(transform.position);
    }

    void Moon()
    {
        moonDeltaX = moonLeight * Mathf.Sin(moonAngle * Mathf.PI / 180);
        moonDeltaY = moonHight * Mathf.Cos(moonAngle * Mathf.PI / 180);
        moonDeltaZ = moonWidth * Mathf.Cos(moonAngle * Mathf.PI / 180);
        moon.transform.position = new Vector3(transform.position.x + moonDeltaX, transform.position.y + moonDeltaY, transform.position.z + moonDeltaZ);
        moon.transform.LookAt(transform.position);

        if (hour == 18)
        {
            moon.SetActive(true);
        }
        else
            if (hour == 6)
        {
            moon.SetActive(false);
            moonAngle += 10;
            if (moonAngle >= 80)
            {
                moonAngle = -60;
            }
        }
    }
}
