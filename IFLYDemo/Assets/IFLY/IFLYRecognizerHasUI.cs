using UnityEngine;

namespace Quibos.IFLY {
    /// <summary>
    /// 带UI语音识别类
    /// 作者：Quibos
    /// 日期：2014.2.18
    /// </summary>
    public class IFLYRecognizerHasUI {

        private static IFLYRecognizerHasUI iFLYRecognizerHasUI;
        private IFLY iFLY;

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private IFLYRecognizerHasUI ( ) {
            iFLY = IFLY.GetInstance ( );
        }

        /// <summary>
        /// 获取带UI语音识别单例对象
        /// </summary>
        /// <returns>带UI语音识别对象</returns>
        public static IFLYRecognizerHasUI GetInstance ( ) {
            if ( iFLYRecognizerHasUI == null ) {
                iFLYRecognizerHasUI = new IFLYRecognizerHasUI ( );
            }
            return iFLYRecognizerHasUI;
        }

        /// <summary>
        /// 启动带UI语音识别
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
        /// 获取带UI语音识别结果的返回个数
        /// </summary>
        /// <returns>返回结果个数</returns>
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
        /// 设置带UI语音识别结果的返回个数
        /// </summary>
        /// <param name="number">返回结果个数</param>
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
