using System;
using UnityEngine;
using PrimeTween;
using UnityEngine.UI;

public class CalbackTest : MonoBehaviour
{
    public Transform Player;
    public Transform Target;
    public Text TxtLog;
    public int RightTween = 1;

    // Start is called before the first frame update
    void Start()
    {
        TxtLog.text = string.Empty;
        if (RightTween == 1)
        {
            DoRightTween();
        }
        else if(RightTween == 2)
        {
            DoWrongTween1();
        }
        else if(RightTween == 3)
        {
            DoWrongTween2();
        }
    }


    private void DoRightTween()
    {
        TxtLog.text = "Right:";
        var startTime = DateTime.Now;
        Action callBack1 = () =>
        {
            Player.rotation = new Quaternion(0, 0, 90, 0);

            var endTime = DateTime.Now;
            var timeSpend = endTime - startTime;
            TxtLog.text += "\nCallBack 1 timeSpend: " + timeSpend.TotalSeconds;
        };
        Action callBack2 = () =>
        {
            Player.rotation = new Quaternion(0, 0, 180, 0);

            var endTime = DateTime.Now;
            var timeSpend = endTime - startTime;
            TxtLog.text += "\nCallBack 2 timeSpend: " + timeSpend.TotalSeconds;
        };

        var seq = Sequence.Create();
        seq.Chain(Tween.LocalPosition(Player, new Vector3(0, Target.localPosition.y, 0), 1f));
        seq.ChainCallback(callBack1);
        seq.Chain(Tween.LocalPosition(Player, Target.localPosition, 1f));
        seq.ChainCallback(callBack2);
    }

    private void DoWrongTween1()
    {
        TxtLog.text = "Wrong 1:";
        var startTime = DateTime.Now;
        Action callBack1 = () =>
        {
            Player.rotation = new Quaternion(0, 0, 90, 0);

            var endTime = DateTime.Now;
            var timeSpend = endTime - startTime;
            TxtLog.text += "\nCallBack 1 timeSpend: " + timeSpend.TotalSeconds;
        };
        Action callBack2 = () =>
        {
            Player.rotation = new Quaternion(0, 0, 180, 0);

            var endTime = DateTime.Now;
            var timeSpend = endTime - startTime;
            TxtLog.text += "\nCallBack 2 timeSpend: " + timeSpend.TotalSeconds;
        };

        var seq = Sequence.Create();
        seq.Chain(Tween.LocalPosition(Player, new Vector3(0, Target.localPosition.y, 0), 1f));
        seq.Chain(Tween.LocalPosition(Player, Target.localPosition, 1f));
        seq.Group(Tween.Delay(1f, callBack1));
        seq.Group(Tween.Delay(2f, callBack2));
    }

    private void DoWrongTween2()
    {
        TxtLog.text = "Wrong 2:";
        var startTime = DateTime.Now;
        Action callBack1 = () =>
        {
            Player.rotation = new Quaternion(0, 0, 90, 0);

            var endTime = DateTime.Now;
            var timeSpend = endTime - startTime;
            TxtLog.text += "\nCallBack 1 timeSpend: " + timeSpend.TotalSeconds;
        };
        Action callBack2 = () =>
        {
            Player.rotation = new Quaternion(0, 0, 180, 0);

            var endTime = DateTime.Now;
            var timeSpend = endTime - startTime;
            TxtLog.text += "\nCallBack 2 timeSpend: " + timeSpend.TotalSeconds;
        };

        var seq = Sequence.Create();
        seq.Group(Tween.Delay(1f, callBack1));
        seq.Group(Tween.Delay(2f, callBack2));
        seq.Chain(Tween.LocalPosition(Player, new Vector3(0, Target.localPosition.y, 0), 1f));
        seq.Chain(Tween.LocalPosition(Player, Target.localPosition, 1f));
    }
}
