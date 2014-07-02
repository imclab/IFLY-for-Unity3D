using UnityEngine;

namespace Quibos.IFLY {
    /// <summary>
    /// ��̨����ʶ����
    /// ���ߣ�Quibos
    /// ���ڣ�2014.2.18
    /// </summary>
    public class IFLYRecognizer {

        private static IFLYRecognizer iFLYRecognizer;
        private IFLY iFLY;

        /// <summary>
        /// ˽�й��캯��
        /// </summary>
        private IFLYRecognizer ( ) {
            iFLY = IFLY.GetInstance ( );
            InitSpeechRecognizer ( );
        }

        /// <summary>
        /// ��ȡ��̨����ʶ��������
        /// </summary>
        /// <returns></returns>
        public static IFLYRecognizer GetInstance ( ) {
            if ( iFLYRecognizer == null ) {
                iFLYRecognizer = new IFLYRecognizer ( );
            }
            return iFLYRecognizer;
        }

        /// <summary>
        /// ��ʼ������ʶ��
        /// </summary>
        private void InitSpeechRecognizer ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "initSpeechRecognizer" );
                }
            }
        }

        /// <summary>
        /// ��������ʶ��
        /// </summary>
        public void Destroy ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "destroySpeechRecognizer" );
                }
            }
        }

        /// <summary>
        /// ֹͣ¼�����ȴ�����˷��ؽ��
        /// </summary>
        public void StopSpeechRecognizer ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "stopSpeechRecognizer" );
                }
            }
        }

        /// <summary>
        /// ȡ����ǰʶ��ֹͣ¼�����Ͽ������˵�����
        /// </summary>
        public void CancelSpeechRecognizer ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "cancelSpeechRecognizer" );
                }
            }
        }

        /// <summary>
        /// �Ƿ�����ʶ��
        /// </summary>
        /// <returns>true��ʾ���ڽ���ʶ��false��ʾ����</returns>
        public bool IsListening ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    return jo.Call<bool> ( "isListening" );
                }
            }
        }

        /// <summary>
        /// ������̨����ʶ��
        /// </summary>
        public void Start ( ) {
            if ( iFLY.CheckSpeechServiceInstalled ( ) ) {
                using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                    using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                        jo.Call ( "speechRecognizer" );
                    }
                }
            }
        }

    }
}
