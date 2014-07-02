package com.quibos.iflyforunity;

import java.util.ArrayList;
import java.util.List;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.ResolveInfo;
import android.os.Bundle;
import android.os.RemoteException;
import android.speech.RecognizerIntent;
import android.widget.Toast;

import com.iflytek.speech.ErrorCode;
import com.iflytek.speech.ISpeechModule;
import com.iflytek.speech.InitListener;
import com.iflytek.speech.RecognizerListener;
import com.iflytek.speech.RecognizerResult;
import com.iflytek.speech.SpeechConstant;
import com.iflytek.speech.SpeechRecognizer;
import com.iflytek.speech.SpeechSynthesizer;
import com.iflytek.speech.SpeechUtility;
import com.iflytek.speech.SynthesizerListener;
import com.iflytek.speech.UtilityConfig;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

/**
 * 该插件基于讯飞语音+开发
 * 用于实现Unity安卓平台下的语音功能
 * @author Quibos
 * @since  2014.2.19
 */
public class MainActivity extends UnityPlayerActivity {
	
	//[start]讯飞语音识别
	
	//[start]带UI识别
	
	/**语音识别结果的返回个数*/
	private int returnVoiceNumber = 1;
	private static final String ACTION_INPUT = "com.iflytek.speech.action.voiceinput";
	/**外部设置的弹出框完成按钮文字*/
	public static final String TITLE_DONE = "title_done";
	/**外部设置的弹出框取消按钮文字*/
	public static final String TITLE_CANCEL = "title_cancel";
	private static final int REQUEST_CODE_SEARCH = 1099;
	
	/**
	 * 获取带UI语音识别结果的返回个数
	 */
	public int getReturnVoiceNumber(){
		return returnVoiceNumber;
	}
	
	/**
	 * 设置带UI语音识别结果的返回个数
	 * @param number个数
	 */
	public void setReturnVoiceNumber(int number){
		if(number > 0 && number < 10){
			returnVoiceNumber = number;
		}
	}
	
	
	/**
	 * 带UI语音识别
	 */
	public void speechRecognizerHasUI(){
		if(isActionSupport(this)){
			Intent intent = new Intent();
			intent.setAction(ACTION_INPUT);
			intent.putExtra(SpeechConstant.PARAMS, "asr_ptt=1");
			intent.putExtra(SpeechConstant.VAD_EOS, "1000");
			//设置弹出框的两个按钮名称
			intent.putExtra(TITLE_DONE, "确定");
			intent.putExtra(TITLE_CANCEL, "取消");
			startActivityForResult(intent, REQUEST_CODE_SEARCH);
		} else {
			showTip("请先安装讯飞语音+");
		}			
	}
	
	/**
	 * 判断是否存在匹配的活动
	 * @param context
	 * @return
	 */
	private boolean isActionSupport(Context context) {
        final PackageManager packageManager = context.getPackageManager();
        final Intent intent = new Intent(ACTION_INPUT);
        //检索所有可用于进行给定意图的活动。如果没有匹配的活动，则返回一个空列表。
        List<ResolveInfo> list = packageManager.queryIntentActivities(intent,PackageManager.MATCH_DEFAULT_ONLY);
        return list.size() > 0;
    }
	
	/**
	 * 带UI语音识别返回信息
	 * 返回多个识别结果时，结果间以"|"符号隔开
	 */
	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		if(requestCode == REQUEST_CODE_SEARCH && resultCode == RESULT_OK)
		{
			//取得识别的字符串
			ArrayList<String> results = data.getStringArrayListExtra(RecognizerIntent.EXTRA_RESULTS);
			if(returnVoiceNumber == 1){
				UnityPlayer.UnitySendMessage("iFLYListener","SRMessageHasUI",results.get(0));
			}else{
				StringBuilder sb = new StringBuilder();
				for(int i = 0;i < results.size();i++){
					sb.append(results.get(i));
					sb.append("|");
				}
				UnityPlayer.UnitySendMessage("iFLYListener","SRMessagesHasUI",sb.toString());
			}
		}
		super.onActivityResult(requestCode, resultCode, data);
	}
	
	//[end]
	
	//[start]后台识别
	
	//语音识别对象
	private SpeechRecognizer mIat;
	
	/**
	 * 初始化语音识别
	 */
	public void initSpeechRecognizer(){
		if(mIat == null){
			mIat = new SpeechRecognizer(this, mInitListener);
		}
	}
	
	/**
	 * 销毁语音识别
	 */
	public void destroySpeechRecognizer(){
		if(mIat != null){
			mIat.cancel(mRecognizerListener);
			if(mIat.destory()){
				mIat = null;
			}		
		}
	}
	
	/**
	 * 不带UI语音识别
	 */
	public void speechRecognizer(){
		mIat.setParameter(SpeechConstant.LANGUAGE, "zh_cn");
		mIat.setParameter(SpeechConstant.ACCENT, "mandarin");
		mIat.setParameter(SpeechConstant.DOMAIN, "iat");
		mIat.setParameter(SpeechConstant.VAD_BOS, "10000");
		mIat.setParameter(SpeechConstant.VAD_EOS, "10000");
		mIat.setParameter(SpeechConstant.PARAMS, "asr_ptt=1,asr_audio_path=/sdcard/asr.pcm");
		mIat.startListening(mRecognizerListener);
	}
	
	/**
	 * 停止录音，等待服务端返回结果
	 */
	public void stopSpeechRecognizer(){
		mIat.stopListening(mRecognizerListener);
	}
	
	/**
	 * 取消当前识别，停止录音并断开与服务端的连接
	 */
	public void cancelSpeechRecognizer(){
		mIat.cancel(mRecognizerListener);
	}
	
	/**
	 * 是否正在识别
	 * @return true表示正在进行识别，false表示空闲
	 */
	public boolean isListening(){
		return mIat.isListening();
	}
	
    /**
     * 初始化完成回调接口
     */
    private InitListener mInitListener = new InitListener() {
    	//初始化完成回调
		@Override
		public void onInit(ISpeechModule module, int code) {
			printCodeMessage(code);
		}
    };
	
    /**
     * 识别回调接口
     */
    private RecognizerListener mRecognizerListener = new RecognizerListener.Stub() {
        //录音启动回调
        @Override
        public void onBeginOfSpeech() throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SpeechRecognizerStatus","onBeginOfSpeech");
        }
        //录音自动停止回调
        @Override
        public void onEndOfSpeech() throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SpeechRecognizerStatus","onEndOfSpeech");
        }
        //识别错误回调
        @Override
        public void onError(int errorCode) throws RemoteException {
        	printCodeMessage(errorCode);
        }
        //识别结果回调
        @Override
        public void onResult(final RecognizerResult result, boolean isLast)
                throws RemoteException {
        	runOnUiThread(new Runnable() {
				@Override
				public void run() {
					if (null != result) {
						String iattext = JsonParser.parseIatResult(result.getResultString());
						UnityPlayer.UnitySendMessage("iFLYListener","SpeechRecognizerMessage",iattext);
		            } else {    
		            	UnityPlayer.UnitySendMessage("iFLYListener","SpeechRecognizerStatus","noResult");
		            }	
				}
			});            
        }
        //音量变化回调
        @Override
        public void onVolumeChanged(int v) throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SRVolumeChanged",Integer.toString(v));
        }
    };
	
    //[end]
    
	//[end]
	
	//[start]讯飞语音合成
	
	//语音合成对象
	private SpeechSynthesizer mTts;
	
	/**
	 * 初始化讯飞语音合成
	 */
	public void initSpeechSynthesizer(){
		if(mTts == null){
			mTts = new SpeechSynthesizer(this, mTtsInitListener);
		}
	}
	
	/**
	 * 销毁语音合成
	 */
	public void destroySpeechSynthesizer(){
		if(mTts != null){
			 mTts.stopSpeaking(mTtsListener);
		     if(mTts.destory()){
		    	 mTts = null;
		     }
		}      		
	}
	
 	/**
 	 * 获取合成参数
 	 * 范围包括引擎类型、语言、语言区域、领域、发音人等，另外包括支持的发音人列表。 
 	 * 参数在不断更新中，目前支持以下参数：
 	 * engine_type,language,accent,domain,vad_bos,vad_eos，sample_rate，local_speakers，voice_name，speed，volume
 	 * @param key参数名称
 	 * @return 返回值
 	 */
 	public String getParameter(String key){
 		return mTts.getParameter(key);
 	}
	
	
 	/**
 	 * 设置合成方式
 	 * @param type合成方式
 	 * ENGINE_TYPE可选：local，cloud，auto，默认：auto
 	 */
 	public void setSynthesizerType(String type){
 		mTts.setParameter(SpeechConstant.ENGINE_TYPE, type);
 	}
 
 	/**
 	 * 设置发音人
 	 * @param person发音人
 	 * 云端支持发音人：小燕（xiaoyan）、小宇（xiaoyu）、
 	 * 凯瑟琳（Catherine）、 亨利（henry）、玛丽（vimary）、
 	 * 小研（vixy）、小琪（vixq）、 小峰（vixf）、小梅（vixm）、
 	 * 小莉（vixl）、小蓉（四川话）、 小芸（vixyun）、
 	 * 小坤（vixk）、小强（vixqa）、小莹（vixying）、 
 	 * 小新（vixx）、楠楠（vinn）老孙（vils）
 	 * 本地支持发音人:小燕（xiaoyan）
 	 * 若要添加本地发音人，需从讯飞语音+客户端下载
 	 */
 	public void setSpeakingPerson(String person){
 		mTts.setParameter(SpeechSynthesizer.VOICE_NAME,	person);
 	}
 	
 	/**
 	 * 设置语速
 	 * @param speed语速（0~100）
 	 */
 	public void setSpeed(String speed){
		mTts.setParameter(SpeechSynthesizer.SPEED, speed);
 	}
 	
 	/**
 	 * 获取语速
 	 * @return语速（0~100）
 	 */
	public String getSpeed(){
 		return SpeechSynthesizer.SPEED;
 	}
 	
 	/**
 	 * 设置音调
 	 * @param pitch音调（0~100）
 	 */
 	public void setPitch(String pitch){
 		mTts.setParameter(SpeechSynthesizer.PITCH, pitch);
 	}
 	
 	/**
 	 * 获取音调
 	 * @return音调（0~100）
 	 */
 	public String getPitch(){
 		return SpeechSynthesizer.PITCH;
 	}
 	
 	/**
 	 * 设置音量
 	 * @param volume音量（0~100）
 	 */
 	public void setVolume(String volume){
 		mTts.setParameter(SpeechSynthesizer.VOLUME, volume);
 	}
 	
 	/**
 	 * 获取音量
 	 * @return音量（0~100）
 	 */
 	public String getVolume(){
 		return SpeechSynthesizer.VOLUME;
 	}
 	
 	
 	/**
 	 * 设置扩展参数
 	 * @param params扩展参数
 	 * 合成支持：
 	 * tts_buffer_time：播放缓冲时间，即缓冲多少秒音频后开始播放，如tts_buffer_time=5000
 	 * tts_audio_path：保存音频路径，如tts_audio_path=/sdcard/tts.pcm
 	 */
 	public void setParams(String params){
 		mTts.setParameter(SpeechConstant.PARAMS, params);
 	}
 	
 	/**
 	 * 开始语音合成
 	 * @param text语音合成内容
 	 */
 	public void startSpeaking(String text){
 		int code = mTts.startSpeaking(text, mTtsListener);
 		printCodeMessage(code);
 	}
 	
 	/**
 	 * 暂停语音合成
 	 */
 	public void pauseSpeaking(){
 		mTts.pauseSpeaking(mTtsListener);
 	}
 	
 	/**
 	 * 继续语音合成
 	 */
 	public void resumeSpeaking(){
 		mTts.resumeSpeaking(mTtsListener);
 	}

 	/**
 	 * 结束语音合成
 	 */
 	public void stopSpeaking(){
 		mTts.stopSpeaking(mTtsListener);
 	}
 	
	/**
     * 初始化完成回调接口
     */
    private InitListener mTtsInitListener = new InitListener() {
    	//初始化完成回调
		@Override
		public void onInit(ISpeechModule arg0, int code) {
        	printCodeMessage(code);
		}
    };
        
    /**
     * 语音合成播放回调接口
     */
    private SynthesizerListener mTtsListener = new SynthesizerListener.Stub() {
    	//缓冲进度回调
        @Override
        public void onBufferProgress(int progress) throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SynthesizerBufferProgress",Integer.toString(progress));
        }
        //缓冲结束回调
        @Override
        public void onCompleted(int code) throws RemoteException {
            if(ErrorCode.ERROR_LOCAL_RESOURCE == code){
            	UnityPlayer.UnitySendMessage("iFLYListener","SynthesizerStatus","noSpeeker");
            }else{
            	printCodeMessage(code);
            }
        }
        //开始播放回调
        @Override
        public void onSpeakBegin() throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SynthesizerStatus","onSpeakBegin");
        }
        //暂停播放回调
        @Override
        public void onSpeakPaused() throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SynthesizerStatus","onSpeakPaused");
        }
        //播放进度回调
        @Override
        public void onSpeakProgress(int progress) throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SynthesizerSpeakProgress",Integer.toString(progress));
        }
        //重新播放回调
        @Override
        public void onSpeakResumed() throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SynthesizerStatus","onSpeakResumed");
        }
    };

    //[end]

    //[start]APK检测与安装
    
	private String type;
	
	/**
	 * 安装讯飞语音+APK
	 * @param type 安装方式，有“local”和“web”两种方式
	 */
	public void installAPK(String type){
		if(type.equals("local"))
			this.type = "local";
		else if(type.equals("web"))
			this.type = "web";
		else
			return;
		String url = SpeechUtility.getUtility(MainActivity.this).getComponentUrl();
		String assetsApk="SpeechService.apk";
		processInstall(MainActivity.this, url,assetsApk);
	}
	
	/**
	 * 如果服务组件没有安装，有两种安装方式。
	 * 1.web:直接打开语音服务组件下载页面，进行下载后安装。
	 * 2.local:把服务组件apk安装包放在assets中，为了避免被编译压缩，修改后缀名为mp3，然后copy到SDcard中进行安装
	 */
	private boolean processInstall(Context context ,String url,String assetsApk){
		if(type.equals("web")){
			// 直接下载方式
			ApkInstaller.openDownloadWeb(context, url);
		}else if(type.equals("local")){
			// 本地安装方式
			if(!ApkInstaller.installFromAssets(context, assetsApk)){
				Toast.makeText(MainActivity.this, "安装失败", Toast.LENGTH_SHORT).show();
				return false;
			}
		}
		return true;
	}
	
	/**
	 * 检测讯飞语音+APK是否已经安装
	 * @return
	 */
	public boolean checkSpeechServiceInstalled(){
		 String packageName = UtilityConfig.DEFAULT_COMPONENT_NAME;
		 List<PackageInfo> packages = getPackageManager().getInstalledPackages(0);
		 for(int i = 0; i < packages.size(); i++){
			 PackageInfo packageInfo = packages.get(i);
			 if(packageInfo.packageName.equals(packageName)){
				 return true;
			 }else{
				 continue;
			 }
		 }
		 return false;
	}
    //[end]
    
	//[start]系统相关
	
	//提示信息对象
	private Toast mToast;
	private boolean isShowToast;

	/**
	 * Unity安卓插件初始化
	 */
	@SuppressLint("ShowToast")
	public void onCreate(Bundle savedInstanceState){
 		super.onCreate(savedInstanceState);
 		mToast = Toast.makeText(this,"",Toast.LENGTH_LONG);  		
 	}
	
	/**
	 * 设置AppID
	 * @param id
	 */
	public void setAppID(String AppID){
		SpeechUtility.getUtility(MainActivity.this).setAppid(AppID);
	}
	
	/**
	 * 获取提示信息显示开关状态
	 * @return
	 */
    public boolean getIsShowToast(){
    	return isShowToast;
    }
    
    /**
     * 设置提示信息显示开关状态
     * @param state 
     */
    public void setIsShowToast(boolean state){
    	isShowToast = state;
    }
    
    /**
     * 显示提示信息
     * @param str提示内容
     */
	private void showTip(final String str){
		if(isShowToast){
			runOnUiThread(new Runnable() {
				@Override
				public void run() {
					mToast.setText(str);
					mToast.show();
			    }
			});
		}else{
			UnityPlayer.UnitySendMessage("iFLYListener","CodeMessage",str);
		}
	}

	/**
	 * 显示返回码信息
	 * @param code返回码
	 */
	private void printCodeMessage(int code){
		switch(code){
		case ErrorCode.ERROR_AUDIO_RECORD:
			showTip("录音失败");
			break;
		case ErrorCode.ERROR_COMPONENT_NOT_INSTALLED:
			showTip("没有安装语音组件");
			break;
		case ErrorCode.ERROR_EMPTY_UTTERANCE:
			showTip("无效的文本输入");
			break;
		case ErrorCode.ERROR_ENGINE_BUSY:
			showTip("引擎繁忙");
			break;
		case ErrorCode.ERROR_ENGINE_CALL_FAIL:
			showTip("调用失败");
			break;
		case ErrorCode.ERROR_ENGINE_INIT_FAIL:
			showTip("初始化失败");
			break;
		case ErrorCode.ERROR_ENGINE_NOT_SUPPORTED:
			showTip("引擎不支持");
			break;
		case ErrorCode.ERROR_FILE_ACCESS:
			showTip("文件读写失败");
			break;
		case ErrorCode.ERROR_INTERRUPT:
			showTip("被异常打断");
			break;
		case ErrorCode.ERROR_INVALID_DATA:
			showTip("无效数据");
			break;
		case ErrorCode.ERROR_INVALID_PARAM:
			showTip("无效的参数");
			break;
		case ErrorCode.ERROR_INVALID_RESULT:
			showTip("无有效的结果");
			break;
		case ErrorCode.ERROR_LOCAL_ENGINE:
			showTip("本地引擎内部错误");
			break;
		case ErrorCode.ERROR_LOCAL_NO_INIT:
			showTip("本地引擎未初始化");
			break;
		case ErrorCode.ERROR_LOCAL_RESOURCE:
			showTip("本地引擎无资源");
			break;
		case ErrorCode.ERROR_LOGIN:
			showTip("用户未登录");
			break;
		case ErrorCode.ERROR_NET_EXPECTION:
			showTip("网络异常");
			break;
		case ErrorCode.ERROR_NETWORK_TIMEOUT:
			showTip("网络连接超时");
			break;
		case ErrorCode.ERROR_NO_MATCH:
			showTip("无匹配结果");
			break;
		case ErrorCode.ERROR_NO_NETWORK:
			showTip("无有效的网络连接");
			break;
		case ErrorCode.ERROR_NO_SPPECH:
			showTip("未检测到语音");
			break;
		case ErrorCode.ERROR_PERMISSION_DENIED:
			showTip("无效授权");
			break;
		case ErrorCode.ERROR_PLAY_MEDIA:
			showTip("音频播放失败");
			break;
		case ErrorCode.ERROR_SPEECH_TIMEOUT:
			showTip("音频输入超时");
			break;
		case ErrorCode.ERROR_TEXT_OVERFLOW:
			showTip("文本溢出");
			break;
		case ErrorCode.ERROR_UNKNOWN:
			showTip("未知错误");
			break;
		case ErrorCode.SUCCESS:
			break;
		}
	}
	//[end]
	
}