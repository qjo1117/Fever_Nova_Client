using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Pause : UI_Scene
{
    #region UI컴포넌트_ENUM
    enum Buttons
    {
        PauseButton             // 일시정지 버튼
    }


    enum Images
    {
        PauseImage              // 일시정지 버튼 이미지
    }
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
    }

   
}
