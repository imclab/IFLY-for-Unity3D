using UnityEngine;

namespace Quibos.IFLY {
    /// <summary>
    /// 讯飞语音消息监听
    /// 作者：Quibos
    /// 日期：2014.2.17
    /// </summary>
    public class IFLYListener : MonoBehaviour {

        void Awake ( ) {
            DontDestroyOnLoad ( gameObject );
        }

        public delegate void MessageHandler ( string message );

        /// <summary>
        /// 消息码事件
        /// </summary>
        public static event MessageHandler eCodeMessage;

        /// <summary>
        /// 带界面语音识别单个结果事件
        /// </summary>
        public static event MessageHandler eSRMessageHasUI;

        /// <summary>
        /// 带界面语音识别多个结果事件
        /// 识别结果间以“|”符号作为间隔
        /// </summary>
        public static event MessageHandler eSRMessagesHasUI;

        /// <summary>
        /// 后台语音识别状态事件
        /// </summary>
        public static event MessageHandler eSpeechRecognizerStatus;

        /// <summary>
        /// 后台语音识别结果事件
        /// </summary>
        public static event MessageHandler eSpeechRecognizerMessage;

        /// <summary>
        /// 后台语音识别音量变化事件
        /// </summary>
        public static event MessageHandler eSRVolumeChanged;

        /// <summary>
        /// 语音合成缓冲进度
        /// </summary>
        public static event MessageHandler eSynthesizerBufferProgress;

        /// <summary>
        /// 语音合成状态
        /// </summary>
        public static event MessageHandler eSynthesizerStatus;

        /// <summary>
        /// 语音合成播放进度
        /// </summary>
        public static event MessageHandler eSynthesizerSpeakProgress;

        void CodeMessage ( string message ) {
            if ( eCodeMessage != null ) {
                eCodeMessage ( message );
            }
        }

        void SRMessageHasUI ( string message ) {
            if ( eSRMessageHasUI != null ) {
                eSRMessageHasUI ( message );
            }
        }

        void SRMessagesHasUI ( string message ) {
            if ( eSRMessagesHasUI != null ) {
                eSRMessagesHasUI ( message );
            }
        }

        void SpeechRecognizerStatus ( string message ) {
            if ( eSpeechRecognizerStatus != null ) {
                eSpeechRecognizerStatus ( message );
            }
        }

        void SpeechRecognizerMessage ( string message ) {
            if ( eSpeechRecognizerMessage != null ) {
                eSpeechRecognizerMessage ( message );
            }
        }

        void SRVolumeChanged ( string message ) {
            if ( eSRVolumeChanged != null ) {
                eSRVolumeChanged ( message );
            }
        }

        void SynthesizerBufferProgress ( string message ) {
            if ( eSynthesizerBufferProgress != null ) {
                eSynthesizerBufferProgress ( message );
            }
        }

        void SynthesizerStatus ( string message ) {
            if ( eSynthesizerStatus != null ) {
                eSynthesizerStatus ( message );
            }
        }

        void SynthesizerSpeakProgress ( string message ) {
            if ( eSynthesizerSpeakProgress != null ) {
                eSynthesizerSpeakProgress ( message );
            }
        }
    }
}
