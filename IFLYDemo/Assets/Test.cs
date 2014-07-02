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
        if ( GUI.Button ( new Rect ( 10 , 10 , 80 , 30 ) , "�����ϳ�" ) ) {
            iFLYSynthesizer.Start ( "��ã�������������Ŷ" );
        }
        if ( GUI.Button ( new Rect ( 100 , 10 , 80 , 30 ) , "����ʶ��" ) ) {
            iFLYRecognizerHasUI.Start ( );
        }
        if ( GUI.Button ( new Rect ( 10 , 50 , 80 , 30 ) , "�˳�" ) ) {
            Application.Quit ( );
        }
    }

    void getSRMessageHasUI ( string s ) {
        strSRMessageHasUI = s;
    }
}
