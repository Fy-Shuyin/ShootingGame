using UnityEngine;
using System.Collections;

public class LoadStage : MonoBehaviour {

    //异步对象
    AsyncOperation async;

    //读取场景的进度，它的取值范围在0 - 1 之间。
    int progress = 0;

    void Start()
    {
        //在这里开启一个异步任务，
        //进入loadScene方法。
        StartCoroutine(loadScene());
    }

    //注意这里返回值一定是 IEnumerator
    IEnumerator loadScene()
    {
        //异步读取场景。
        //Globe.loadName 就是A场景中需要读取的C场景名称。
        async = Application.LoadLevelAsync(Globe.loadName);

        //读取完毕后返回， 系统会自动进入C场景
        yield return async;

    }

    void Update()
    {

        //在这里计算读取的进度，
        //progress 的取值范围在0.1 - 1之间， 但是它不会等于1
        //也就是说progress可能是0.9的时候就直接进入新场景了
        //所以在写进度条的时候需要注意一下。
        //为了计算百分比 所以直接乘以100即可
        progress = (int)(async.progress * 100);

        //有了读取进度的数值，大家可以自行制作进度条啦。
        Debug.Log("StageLoad" + progress + "%");
    }

    public class Globe
    {
        //在这里记录当前切换场景的名称
        public static int loadName = 1;
    }
}
