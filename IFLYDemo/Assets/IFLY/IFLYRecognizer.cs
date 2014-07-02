using UnityEngine;

namespace Quibos.IFLY {
    /// <summary>
    /// 后台语音识别类
    /// 作者：Quibos
    /// 日期：2014.2.18
    /// </summary>
    public class IFLYRecognizer {

        private static IFLYRecognizer iFLYRecognizer;
        private IFLY iFLY;

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private IFLYRecognizer ( ) {
            iFLY = IFLY.GetInstance ( );
            InitSpeechRecognizer ( );
        }

        /// <summary>
        /// 获取后台语音识别单例对象
        /// </summary>
        /// <returns></returns>
        public static IFLYRecognizer GetInstance ( ) {
            if ( iFLYRecognizer == null ) {
                iFLYRecognizer = new IFLYRecognizer ( );
            }
            return iFLYRecognizer;
        }

        /// <summary>
        /// 初始化语音识别
        /// </summary>
        private void InitSpeechRecognizer ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "initSpeechRecognizer" );
                }
            }
        }

        /// <summary>
        /// 销毁语音识别
        /// </summary>
        public void Destroy ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "destroySpeechRecognizer" );
                }
            }
        }

        /// <summary>
        /// 停止录音，等待服务端返回结果
        /// </summary>
        public void StopSpeechRecognizer ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "stopSpeechRecognizer" );
                }
            }
        }

        /// <summary>
        /// 取消当前识别，停止录音并断开与服务端的连接
        /// </summary>
        public void CancelSpeechRecognizer ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "cancelSpeechRecognizer" );
                }
            }
        }

        /// <summary>
        /// 是否正在识别
        /// </summary>
        /// <returns>true表示正在进行识别，false表示空闲</returns>
        public bool IsListening ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    return jo.Call<bool> ( "isListening" );
                }
            }
        }

        /// <summary>
        /// 开启后台语音识别
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
