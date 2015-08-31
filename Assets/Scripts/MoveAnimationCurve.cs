using UnityEngine;
using System.Collections;

public class MoveAnimationCurve : MonoBehaviour {

    public AnimationCurve animationCurve;

    public Transform moveTargetTrs;
    public float distance = 1.0f;

    public Transform targetTransform;
    public Transform [] targets;

    public float xValue = 10;
    public float yValue = 10.0f;

    public float fCurrentTime = 0.0f;

     // 设定AnimationCurve指定物体运动轨迹
    void Update()
    {
        fCurrentTime += Time.deltaTime * 0.25f;
        if (fCurrentTime >= 1.0f)
            fCurrentTime = 1.0f;

        float yEvaluate = animationCurve.Evaluate(fCurrentTime);
        float xTarget = xValue * fCurrentTime;
        float yTarget = yValue * yEvaluate;
        targetTransform.position = new UnityEngine.Vector3(xTarget, yTarget, 0.0f);

        MoveTargetLogic();
    }


    void Start()
    {
        for (int i = 0; i < targets.Length;i++ )
        {
            float xPercent = i / (float)(targets.Length - 1);
            float yPercent = animationCurve.Evaluate(xPercent);
            targets[i].transform.position = new Vector3(xValue * xPercent, yValue * yPercent, 0.0f);
        }
    }


    // 螺旋移动一个物体
    void MoveTargetLogic()
    {
        moveTargetTrs.Rotate(Vector3.forward, Time.deltaTime * 180.0f);
        moveTargetTrs.Translate(Time.deltaTime * Vector3.up * distance);
        distance += Time.deltaTime * 1.5f;
    }
}
