using System.Collections;
using UnityEngine;

public class statue_show : MonoBehaviour
{
    public GameObject tanhajirao1, Baji1, devi1;
    public GameObject tanhajirao, Baji, devi;
    public GameObject text1, text2, text3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tanhajirao.SetActive(false);
        Baji.SetActive(false);
        devi.SetActive(false);

        tanhajirao1.SetActive(true);
        Baji1.SetActive(true);
        devi1.SetActive(true);

        text1 .SetActive(false);
        text2.SetActive(false);
        text3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show1st()
    {
        tanhajirao1.SetActive(false);
        
        tanhajirao.SetActive(true);
        StartCoroutine(MyWaitRoutine());
        text1.SetActive(true);
    }
    public void show2nd()
    {
        Baji1.SetActive(false);

        Baji.SetActive(true);
        StartCoroutine(MyWaitRoutine());
        text2.SetActive(true);

    }
    public void show3rd()
    {
        devi1.SetActive(false);

        devi.SetActive(true);
        StartCoroutine(MyWaitRoutine());
        text3.SetActive(true);
    }


    IEnumerator MyWaitRoutine()
    {
        Debug.Log("Waiting for 2 seconds...");

        // This line pauses the routine, but NOT the whole game
        yield return new WaitForSeconds(2.0f);

        Debug.Log("Time is up!");
    }
}
