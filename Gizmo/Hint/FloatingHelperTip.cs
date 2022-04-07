using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingHelperTip : MonoBehaviour
{
    private TextMeshPro textMesh; // the text mesh
    private HelperTipContainer helperTipContainer; // the helper tip container
    public Transform target0; // the floating target 0
    public Transform target1; // the floating target 1
    public Transform target2; // the floating target 2
    public Sequence movementSequence; // the dotween sequence to oscillate position between [target0 -> target1 -> target2 -> ... ]
    private void Start()
    {
        textMesh = GetComponent<TextMeshPro>(); // store a local reference to the text mesh
        helperTipContainer = FindObjectOfType<HelperTipContainer>(); // store a local reference to the helper tip container
        textMesh.text = helperTipContainer.GetRandomHelperTip(); // get a random helper tip from the helper tip container
        movementSequence = DOTween.Sequence(); // start a new dotween sequence for movement
        movementSequence.Append(transform.DOMove(target0.position, 1.0f).SetEase(Ease.Linear)); // append to the sequence a DOMove to target 0 position in 1 second
        movementSequence.Append(transform.DOMove(target1.position, 2.0f).SetEase(Ease.Linear)); // append to the sequence a DOMove to target 1 position in 2 second
        movementSequence.Append(transform.DOMove(target2.position, 1.0f).SetEase(Ease.Linear)); // append to the sequence a DOMove to target 2 position in 1 second
        movementSequence.SetLoops(50); // loop the sequence 50 times
    }
}
