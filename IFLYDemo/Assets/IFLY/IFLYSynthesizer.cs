using UnityEngine;
using System;

namespace Quibos.IFLY {
    /// <summary>
    /// Ѷ�������ϳ���
    /// ���ߣ�Quibos
    /// ���ڣ�2014.2.18
    /// </summary>
    public class IFLYSynthesizer {

        private static IFLYSynthesizer iFLYSynthesizer;

        private IFLY iFLY;

        /// <summary>
        /// ����  ֵ�ķ�Χ��0 ~ 100
        /// </summary>
        public int Speed { 
            get{
                using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                    using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                        return Convert.ToInt32( jo.Call<string> ( "getSpeed" ));
                    }
                }
            }
            set{
                if ( value <= 100 && value >= 0 ) {
                    using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                        using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                            jo.Call ( "setSpeed" , value.ToString ( ) );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ����  ֵ�ķ�Χ��0 ~ 100
        /// </summary>
        public int Pitch {
            get {
                using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                    using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                        return Convert.ToInt32 ( jo.Call<string> ( "getPitch" ) );
                    }
                }
            }
            set {
                if ( value <= 100 && value >= 0 ) {
                    using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                        using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                            jo.Call ( "setPitch" , value.ToString ( ) );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ����  ֵ�ķ�Χ��0 ~ 100
        /// </summary>
        public int Volume {
            get {
                using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                    using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                        return Convert.ToInt32 ( jo.Call<string> ( "getVolume" ) );
                    }
                }
            }
            set {
                if ( value <= 100 && value >= 0 ) {
                    using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                        using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                            jo.Call ( "setVolume" , value.ToString ( ) );
                        }
                    }
                }
            }
        }


        /// <summary>
        /// ˽�й��캯��
        /// </summary>
        private IFLYSynthesizer ( ) {
            iFLY = IFLY.GetInstance ( );
            InitSpeechSynthesizer ( );
        }

        /// <summary>
        /// ��ȡ�����ϳɵ�������
        /// </summary>
        /// <returns>�����ϳɶ���</returns>
        public static IFLYSynthesizer GetInstance ( ) {
            if ( iFLYSynthesizer == null ) {
                iFLYSynthesizer = new IFLYSynthesizer ( );
            }
            return iFLYSynthesizer;
        }

        /// <summary>
        /// ��ʼ��Ѷ�������ϳ�
        /// </summary>
        private void InitSpeechSynthesizer ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "initSpeechSynthesizer" );
                }
            }
        }

        /// <summary>
        /// ���������ϳ�
        /// </summary>
        public void Destroy ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "destroySpeechSynthesizer" );
                }
            }
        }

        /// <summary>
        ///  ��ȡ�ϳɲ���
        ///  ��Χ�����������͡����ԡ������������򡢷����˵ȣ��������֧�ֵķ������б�
        ///  �����ڲ��ϸ����У�Ŀǰ֧�����²�����
        ///  engine_type,language,accent,domain,vad_bos,vad_eos��sample_rate��local_speakers��voice_name��speed��volume
        /// </summary>
        /// <param name="key">��������</param>
        /// <returns>����ֵ</returns>
        public string GetParameter ( string key ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    return jo.Call<string> ( "destroySpeechSynthesizer" , key );
                }
            }
        }

        /// <summary>
        /// ���úϳɷ�ʽ
        /// </summary>
        /// <param name="type">�ϳɷ�ʽ</param>
        public void SetSynthesizerType ( SynthesizerType type ) {
            switch ( type ) {
                case SynthesizerType.Auto:
                    using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                        using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                            jo.Call ( "setSynthesizerType" , "auto" );
                        }
                    }
                    break;
                case SynthesizerType.Cloud:
                    using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                        using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                            jo.Call ( "setSynthesizerType" , "cloud" );
                        }
                    }
                    break;
                case SynthesizerType.Local:
                    using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                        using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                            jo.Call ( "setSynthesizerType" , "local" );
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// ���÷�����
        /// �ƶ�֧�ַ����ˣ�
        /// ��Ӣ����ͨ����С�ࣨxiaoyan����С�xiaoyu����С�У�vixy����С����vixq���� С�壨vixf��
        /// Ӣ�ģ���ɪ�գ�Catherine���� ������henry����������vimary��
        /// ��Ӣ�����С÷��vixm��
        /// ��Ӣ��̨�廰��С��vixl��
        /// �Ĵ�����С�أ�vixr�� 
        /// ��������Сܿ��vixyun��
        /// ���ϻ���С����vixk��
        /// ���ϻ���Сǿ��vixqa��
        /// ��������СӨ��vixying��
        /// ��ͨ����С�£�vixx�����骣�vinn�����vils��
        /// ����֧�ַ�����:С�ࣨxiaoyan��
        /// ��Ҫ��ӱ��ط����ˣ����Ѷ������+�ͻ�������
        /// </summary>
        /// <param name="person">������</param>
        public void SetSpeakingPerson ( string person ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "setSpeakingPerson" , person );
                }
            }
        }

        /// <summary>
        /// ������չ����
        /// �ϳ�֧�֣�
        /// tts_buffer_time�����Ż���ʱ�䣬�������������Ƶ��ʼ���ţ���tts_buffer_time=5000
        /// tts_audio_path��������Ƶ·������tts_audio_path=/sdcard/tts.pcm
        /// </summary>
        /// <param name="param">��չ����</param>
        public void SetParams ( string param ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "setParams" , param );
                }
            }
        }

        /// <summary>
        /// ���������ϳ�
        /// </summary>
        /// <param name="text">�����ϳ�����</param>
        public void Start ( string text ) {
            if ( iFLY.CheckSpeechServiceInstalled ( ) ) {
                using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                    using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                        jo.Call ( "startSpeaking" , text );
                    }
                }
            }
        }

        /// <summary>
        /// ��ͣ�����ϳ�
        /// </summary>
        public void Pause ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "pauseSpeaking" );
                }
            }
        }

        /// <summary>
        /// ���������ϳ�
        /// </summary>
        public void Resume ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "resumeSpeaking" );
                }
            }
        }

        /// <summary>
        /// ���������ϳ�
        /// </summary>
        public void Stop ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "stopSpeaking" );
                }
            }
        }

    }
}
