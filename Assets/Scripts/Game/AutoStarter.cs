using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStarter : MonoBehaviour
{

    public static AutoStarter Instance { get; private set; }

    public float startTime = 5f;
    private float localTime = 5f;
    public bool isStart = false;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            localTime -= Time.deltaTime;
            if (localTime <= 0)
            {
                localTime = startTime;
                isStart = false;
                GameLogicManager.Instance.OnIdleButtonPressed();
            }
        }
    }

    public void StartTimer()
    {
        isStart = true;
    }

}
