namespace Quibos.IFLY {

    /// <summary>
    /// 讯飞语音+安装方式
    /// Web 网络安装
    /// Local 本地安装
    /// None 不安装
    /// </summary>
    public enum InstallApkType {
        Web,
        Local,
        None
    }

    /// <summary>
    /// 语音合成方式
    /// Cloud 云端合成
    /// Local 本地合成
    /// Auto 自动方式
    /// </summary>
     public enum SynthesizerType {
        Cloud,
        Local,
        Auto
    }


}
