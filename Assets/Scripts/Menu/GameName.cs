using UnityEngine;
using DG.Tweening;

public class GameName : MonoBehaviour
{
    [SerializeField] private float _yDistance;
    [SerializeField] private float _animationTime;

    private void Awake()
    {
        transform.DOMoveY(transform.position.y - _yDistance, _animationTime).SetLoops(-1, LoopType.Yoyo);
    }
}