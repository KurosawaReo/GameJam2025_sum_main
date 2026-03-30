using KR.Unity.Animation;

public class EffectBreakBlock : AnimUnityKR
{
    private void Start()
    {
        InitAnim(); //初期化.
    }

    private void Update()
    {
        //アニメーションが終了したら消去.
        if (IsFinished())
        {
            Delete();
        }
    }
}
