using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesMoveController : MonoBehaviour
{

    public TreeMoveInstance[] leftTreesTransforms;
    public TreeMoveInstance[] rightTreesTransforms;

    public bool isStart = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            isStart = false;
            MoveLeftTrees();
            MoveRightTrees();
        }
    }

    public void MoveLeftTrees()
    {
        foreach (TreeMoveInstance tree in leftTreesTransforms)
        {
            tree.StartMoving();
        }
    }
    public void MoveRightTrees()
    {
        foreach (TreeMoveInstance tree in rightTreesTransforms)
        {
            tree.StartMoving();
        }
    }

}
