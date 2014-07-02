using UnityEngine;

namespace Quibos.IFLY {
    /// <summary>
    /// Ѷ�����������
    /// �����Ӧ����Unity�µİ�׿�������ײ�ʹ��Ѷ������+���
    /// ������Ҫ����Ѷ�ɲ��ϵͳ�Ĳ������ú�APK�ļ���밲װ
    /// ���ߣ�Quibos
    /// ���ڣ�2014.2.18
    /// </summary>

    public class IFLY {

        /// <summary>
        /// Ѷ������+APK��װί��
        /// </summary>
        public delegate void InstallApkHandler ( );

        /// <summary>
        /// Ѷ������+APK��װ�¼�
        /// </summary>
        public event InstallApkHandler InstallApkEvent;
        private GameObject iFLYListener;
        private static IFLY iFLY;

        /// <summary>
        /// ˽�й��캯��
        /// </summary>
        private IFLY ( ) {
            iFLYListener = new GameObject ( "iFLYListener" );
            iFLYListener.AddComponent<IFLYListener> ( );
        }

        /// <summary>
        /// ��ȡѶ�ɵ�������
        /// </summary>
        /// <returns>Ѷ��ʵ��</returns>
        public static IFLY GetInstance ( ) {
            if ( iFLY == null ) {
                iFLY = new IFLY ( );
            }
            return iFLY;
        }

        #region APK�ļ���밲װ

        /// <summary>
        /// ���Ѷ������+APK�Ƿ��Ѿ���װ
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
       /// ��װѶ������+APK
       /// </summary>
       /// <param name="type">��װ��ʽ</param>
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

        #region ϵͳ��������

        /// <summary>
        /// ����AppID
        /// Ѷ�ɹٷ����պ��Ҫ�����ʹ��AppID
        /// ʵ��ʹ��Ѷ�ɲ��ʱ�������AppID
        /// </summary>
        /// <param name="AppID">Ѷ�ɹ��������AppID</param>
        public void SetAppID ( string AppID ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    jo.Call ( "getIsShowToast" , AppID );
                }
            }
        }

        /// <summary>
        /// ��ȡ��ʾ��Ϣ��ʾ����״̬
        /// </summary>
        /// <returns>Toast�Ŀ���״̬</returns>
        public bool GetIsShowToast ( ) {
            using ( AndroidJavaClass jc = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ) ) {
                using ( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ( "currentActivity" ) ) {
                    return  jo.Call<bool> ( "getIsShowToast" );
                }
            }
        }

        /// <summary>
        /// ������ʾ��Ϣ��ʾ״̬
        /// </summary>
        /// <param name="isShowToast">Toast�Ŀ���״̬</param>
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
