using UnityEngine;

namespace Quibos.IFLY {
    /// <summary>
    /// ��UI����ʶ����
    /// ���ߣ�Quibos
    /// ���ڣ�2014.2.18
    /// </summary>
    public class IFLYRecognizerHasUI {

        private static IFLYRecognizerHasUI iFLYRecognizerHasUI;
        private IFLY iFLY;

        /// <summary>
        /// ˽�й��캯��
        /// </summary>
        private IFLYRecognizerHasUI ( ) {
            iFLY = IFLY.GetInstance ( );
        }

        /// <summary>
        /// ��ȡ��UI����ʶ��������
        /// </summary>
        /// <returns>��UI����ʶ�����</returns>
        public static IFLYRecognizerHasUI GetInstance ( ) {
            if ( iFLYRecognizerHasUI == null ) {
                iFLYRecognizerHasUI = new IFLYRecognizerHasUI ( );
            }
            return iFLYRecognizerHasUI;
        }

        /// <summary>
        /// ������UI����ʶ��
        /// </summary>
        public void Start ( ) {
            if ( iFLY.CheckSpeechServiceInstalled ( ) ) {
                using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                    using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                        jo.Call ( "speechRecognizerHasUI" );
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ��UI����ʶ�����ķ��ظ���
        /// </summary>
        /// <returns>���ؽ������</returns>
        public int GetReturnVoiceNumberHasUI ( ) {
            if ( iFLY.CheckSpeechServiceInstalled ( ) ) {
                using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                    using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                        return jo.Call<int> ( "getReturnVoiceNumber" );
                    }
                }
            } else {
                return 0;
            }
        }

        /// <summary>
        /// ���ô�UI����ʶ�����ķ��ظ���
        /// </summary>
        /// <param name="number">���ؽ������</param>
        public void SetReturnVoiceNumberHasUI ( int number ) {
            if ( iFLY.CheckSpeechServiceInstalled ( ) ) {
                using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                    using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                        jo.Call ( "setReturnVoiceNumber" , number );
                    }
                }
            }
        }

    }
}
