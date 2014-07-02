using UnityEngine;
using System.Collections;
using Quibos.IFLY;

public class Test : MonoBehaviour {

    private IFLYSynthesizer iFLYSynthesizer;
    private IFLYRecognizerHasUI iFLYRecognizerHasUI;
    private string strSRMessageHasUI;

    void Start ( ) {
        iFLYSynthesizer = IFLYSynthesizer.GetInstance ( );
        iFLYRecognizerHasUI = IFLYRecognizerHasUI.GetInstance ( );
        strSRMessageHasUI = string.Empty;
        IFLYListener.eSRMessageHasUI += getSRMessageHasUI;
    }

    void OnGUI ( ) {
        GUI.Label ( new Rect ( 200 , 100 , 100 , 50 ) , strSRMessageHasUI );
        if ( GUI.Button ( new Rect ( 10 , 10 , 80 , 30 ) , "语音合成" ) ) {
            iFLYSynthesizer.Start ( "你好，今天天气不错哦" );
        }
        if ( GUI.Button ( new Rect ( 100 , 10 , 80 , 30 ) , "语音识别" ) ) {
            iFLYRecognizerHasUI.Start ( );
        }
        if ( GUI.Button ( new Rect ( 10 , 50 , 80 , 30 ) , "退出" ) ) {
            Application.Quit ( );
        }
    }

    void getSRMessageHasUI ( string s ) {
        strSRMessageHasUI = s;
    }
}
