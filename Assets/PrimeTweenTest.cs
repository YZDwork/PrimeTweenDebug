using UnityEngine;
using PrimeTween;

public class PrimeTweenTest : MonoBehaviour
{
    public Transform Player;
    public Transform Target;

    // Start is called before the first frame update
    void Start()
    {
        RunPrimeTween();
    }

    private void RunPrimeTween()
    {
        var seq = Sequence.Create();
        seq.Chain(Tween.LocalPosition(Player, new Vector3(0, Target.localPosition.y, 0), 1f));
        seq.Chain(Tween.LocalPosition(Player, new Vector3(Target.localPosition.x, 0, 0), 1f));
        seq.Chain(Tween.LocalPosition(Player, Target.localPosition, 1f));
        seq.Group(Tween.Scale(Player, Vector3.one * 3f, 1.5f, startDelay:0));
        seq.Group(Tween.Scale(Player, Vector3.one, 1.5f, startDelay:1.5f));
    }
}
