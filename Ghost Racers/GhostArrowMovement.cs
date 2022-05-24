using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GhostArrowMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 360, 0), 5f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1); // tweens the rotation from 0 to 360 and beyond
    }
}
