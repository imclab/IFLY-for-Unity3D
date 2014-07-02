using UnityEngine;

namespace Quibos.IFLY {
    /// <summary>
    /// Ѷ��������Ϣ����
    /// ���ߣ�Quibos
    /// ���ڣ�2014.2.17
    /// </summary>
    public class IFLYListener : MonoBehaviour {

        void Awake ( ) {
            DontDestroyOnLoad ( gameObject );
        }

        public delegate void MessageHandler ( string message );

        /// <summary>
        /// ��Ϣ���¼�
        /// </summary>
        public static event MessageHandler eCodeMessage;

        /// <summary>
        /// ����������ʶ�𵥸�����¼�
        /// </summary>
        public static event MessageHandler eSRMessageHasUI;

        /// <summary>
        /// ����������ʶ��������¼�
        /// ʶ�������ԡ�|��������Ϊ���
        /// </summary>
        public static event MessageHandler eSRMessagesHasUI;

        /// <summary>
        /// ��̨����ʶ��״̬�¼�
        /// </summary>
        public static event MessageHandler eSpeechRecognizerStatus;

        /// <summary>
        /// ��̨����ʶ�����¼�
        /// </summary>
        public static event MessageHandler eSpeechRecognizerMessage;

        /// <summary>
        /// ��̨����ʶ�������仯�¼�
        /// </summary>
        public static event MessageHandler eSRVolumeChanged;

        /// <summary>
        /// �����ϳɻ������
        /// </summary>
        public static event MessageHandler eSynthesizerBufferProgress;

        /// <summary>
        /// �����ϳ�״̬
        /// </summary>
        public static event MessageHandler eSynthesizerStatus;

        /// <summary>
        /// �����ϳɲ��Ž���
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
