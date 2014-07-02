using UnityEngine;

namespace Quibos.IFLY {
    /// <summary>
    /// 讯飞语音插件类
    /// 本插件应用与Unity下的安卓开发，底层使用讯飞语音+插件
    /// 该类主要包括讯飞插件系统的参数设置和APK的检测与安装
    /// 作者：Quibos
    /// 日期：2014.2.18
    /// </summary>

    public class IFLY {

        /// <summary>
        /// 讯飞语音+APK安装委托
        /// </summary>
        public delegate void InstallApkHandler ( );

        /// <summary>
        /// 讯飞语音+APK安装事件
        /// </summary>
        public event InstallApkHandler InstallApkEvent;
        private GameObject iFLYListener;
        private static IFLY iFLY;

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private IFLY ( ) {
            iFLYListener = new GameObject ( "iFLYListener" );
            iFLYListener.AddComponent<IFLYListener> ( );
        }

        /// <summary>
        /// 获取讯飞单例对象
        /// </summary>
        /// <returns>讯飞实例</returns>
        public static IFLY GetInstance ( ) {
            if ( iFLY == null ) {
                iFLY = new IFLY ( );
            }
            return iFLY;
        }

        #region APK的检测与安装

        /// <summary>
        /// 检测讯飞语音+APK是否已经安装
        /// </summary>
        /// <returns></returns>
        public bool CheckSpeechServiceInstalled ( ) {
            bool Installed;
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    Installed = jo.Call<bool> ( "checkSpeechServiceInstalled" );
                }
            }
            if ( !Installed && InstallApkEvent != null ) {
                InstallApkEvent ( );
            }
            return Installed;
        }

       /// <summary>
       /// 安装讯飞语音+APK
       /// </summary>
       /// <param name="type">安装方式</param>
        public void InstallApk ( InstallApkType type ) {
            switch ( type ) {
                case InstallApkType.Local:
                    using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                        using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                            jo.Call ( "installAPK" , "local" );
                        }
                    }
                    break;
                case InstallApkType.Web:
                    using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                        using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                            jo.Call ( "installAPK" , "web" );
                        }
                    }
                    break;
                case InstallApkType.None:
                    break;
            }
        }

        #endregion

        #region 系统参数设置

        /// <summary>
        /// 设置AppID
        /// 讯飞官方称日后会要求程序使用AppID
        /// 实际使用讯飞插件时最好设置AppID
        /// </summary>
        /// <param name="AppID">讯飞官网申请的AppID</param>
        public void SetAppID ( string AppID ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "getIsShowToast" , AppID );
                }
            }
        }

        /// <summary>
        /// 获取提示信息显示开关状态
        /// </summary>
        /// <returns>Toast的开关状态</returns>
        public bool GetIsShowToast ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    return  jo.Call<bool> ( "getIsShowToast" );
                }
            }
        }

        /// <summary>
        /// 设置提示信息显示状态
        /// </summary>
        /// <param name="isShowToast">Toast的开关状态</param>
        public void SetIsShowToast ( bool isShowToast ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "setIsShowToast" );
                }
            }
        }

	#endregion
    }
}
