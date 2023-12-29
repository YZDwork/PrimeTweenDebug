using UnityEngine;
using DG.Tweening;

public class DotweenTest : MonoBehaviour
{
    public Transform Player;
    public Transform Target;

    void Start()
    {
        RunDoTween();
    }

    private void RunDoTween()
    {
        var seq = DOTween.Sequence();
        seq.Append(Player.DOLocalMove(new Vector3(0, Target.localPosition.y, 0), 1f));
        seq.Append(Player.DOLocalMove(new Vector3(Target.localPosition.x, 0, 0), 1f));
        seq.Append(Player.DOLocalMove(Target.localPosition, 1f));
        seq.Insert(0,Player.DOScale(Vector3.one * 3f, 1.5f));
        seq.Insert(1.5f,Player.DOScale(Vector3.one, 1.5f));
    }
}
