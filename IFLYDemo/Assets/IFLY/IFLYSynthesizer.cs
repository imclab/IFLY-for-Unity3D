using UnityEngine;
using System;

namespace Quibos.IFLY {
    /// <summary>
    /// 讯飞语音合成类
    /// 作者：Quibos
    /// 日期：2014.2.18
    /// </summary>
    public class IFLYSynthesizer {

        private static IFLYSynthesizer iFLYSynthesizer;

        private IFLY iFLY;

        /// <summary>
        /// 语速  值的范围：0 ~ 100
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
        /// 音调  值的范围：0 ~ 100
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
        /// 音量  值的范围：0 ~ 100
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
        /// 私有构造函数
        /// </summary>
        private IFLYSynthesizer ( ) {
            iFLY = IFLY.GetInstance ( );
            InitSpeechSynthesizer ( );
        }

        /// <summary>
        /// 获取语音合成单例对象
        /// </summary>
        /// <returns>语音合成对象</returns>
        public static IFLYSynthesizer GetInstance ( ) {
            if ( iFLYSynthesizer == null ) {
                iFLYSynthesizer = new IFLYSynthesizer ( );
            }
            return iFLYSynthesizer;
        }

        /// <summary>
        /// 初始化讯飞语音合成
        /// </summary>
        private void InitSpeechSynthesizer ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "initSpeechSynthesizer" );
                }
            }
        }

        /// <summary>
        /// 销毁语音合成
        /// </summary>
        public void Destroy ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "destroySpeechSynthesizer" );
                }
            }
        }

        /// <summary>
        ///  获取合成参数
        ///  范围包括引擎类型、语言、语言区域、领域、发音人等，另外包括支持的发音人列表
        ///  参数在不断更新中，目前支持以下参数：
        ///  engine_type,language,accent,domain,vad_bos,vad_eos，sample_rate，local_speakers，voice_name，speed，volume
        /// </summary>
        /// <param name="key">参数名称</param>
        /// <returns>返回值</returns>
        public string GetParameter ( string key ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    return jo.Call<string> ( "destroySpeechSynthesizer" , key );
                }
            }
        }

        /// <summary>
        /// 设置合成方式
        /// </summary>
        /// <param name="type">合成方式</param>
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
        /// 设置发音人
        /// 云端支持发音人：
        /// 中英文普通话：小燕（xiaoyan）、小宇（xiaoyu）、小研（vixy）、小琪（vixq）、 小峰（vixf）
        /// 英文：凯瑟琳（Catherine）、 亨利（henry）、玛丽（vimary）
        /// 中英文粤语：小梅（vixm）
        /// 中英文台湾话：小莉（vixl）
        /// 四川话：小蓉（vixr） 
        /// 东北话：小芸（vixyun）
        /// 河南话：小坤（vixk）
        /// 湖南话：小强（vixqa）
        /// 陕西话：小莹（vixying）
        /// 普通话：小新（vixx）、楠楠（vinn）老孙（vils）
        /// 本地支持发音人:小燕（xiaoyan）
        /// 若要添加本地发音人，需从讯飞语音+客户端下载
        /// </summary>
        /// <param name="person">发音人</param>
        public void SetSpeakingPerson ( string person ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "setSpeakingPerson" , person );
                }
            }
        }

        /// <summary>
        /// 设置扩展参数
        /// 合成支持：
        /// tts_buffer_time：播放缓冲时间，即缓冲多少秒音频后开始播放，如tts_buffer_time=5000
        /// tts_audio_path：保存音频路径，如tts_audio_path=/sdcard/tts.pcm
        /// </summary>
        /// <param name="param">扩展参数</param>
        public void SetParams ( string param ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "setParams" , param );
                }
            }
        }

        /// <summary>
        /// 开启语音合成
        /// </summary>
        /// <param name="text">语音合成内容</param>
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
        /// 暂停语音合成
        /// </summary>
        public void Pause ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "pauseSpeaking" );
                }
            }
        }

        /// <summary>
        /// 继续语音合成
        /// </summary>
        public void Resume ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "resumeSpeaking" );
                }
            }
        }

        /// <summary>
        /// 结束语音合成
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
