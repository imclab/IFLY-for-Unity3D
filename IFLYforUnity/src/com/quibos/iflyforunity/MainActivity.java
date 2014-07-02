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
 * �ò������Ѷ������+����
 * ����ʵ��Unity��׿ƽ̨�µ���������
 * @author Quibos
 * @since  2014.2.19
 */
public class MainActivity extends UnityPlayerActivity {
	
	//[start]Ѷ������ʶ��
	
	//[start]��UIʶ��
	
	/**����ʶ�����ķ��ظ���*/
	private int returnVoiceNumber = 1;
	private static final String ACTION_INPUT = "com.iflytek.speech.action.voiceinput";
	/**�ⲿ���õĵ�������ɰ�ť����*/
	public static final String TITLE_DONE = "title_done";
	/**�ⲿ���õĵ�����ȡ����ť����*/
	public static final String TITLE_CANCEL = "title_cancel";
	private static final int REQUEST_CODE_SEARCH = 1099;
	
	/**
	 * ��ȡ��UI����ʶ�����ķ��ظ���
	 */
	public int getReturnVoiceNumber(){
		return returnVoiceNumber;
	}
	
	/**
	 * ���ô�UI����ʶ�����ķ��ظ���
	 * @param number����
	 */
	public void setReturnVoiceNumber(int number){
		if(number > 0 && number < 10){
			returnVoiceNumber = number;
		}
	}
	
	
	/**
	 * ��UI����ʶ��
	 */
	public void speechRecognizerHasUI(){
		if(isActionSupport(this)){
			Intent intent = new Intent();
			intent.setAction(ACTION_INPUT);
			intent.putExtra(SpeechConstant.PARAMS, "asr_ptt=1");
			intent.putExtra(SpeechConstant.VAD_EOS, "1000");
			//���õ������������ť����
			intent.putExtra(TITLE_DONE, "ȷ��");
			intent.putExtra(TITLE_CANCEL, "ȡ��");
			startActivityForResult(intent, REQUEST_CODE_SEARCH);
		} else {
			showTip("���Ȱ�װѶ������+");
		}			
	}
	
	/**
	 * �ж��Ƿ����ƥ��Ļ
	 * @param context
	 * @return
	 */
	private boolean isActionSupport(Context context) {
        final PackageManager packageManager = context.getPackageManager();
        final Intent intent = new Intent(ACTION_INPUT);
        //�������п����ڽ��и�����ͼ�Ļ�����û��ƥ��Ļ���򷵻�һ�����б�
        List<ResolveInfo> list = packageManager.queryIntentActivities(intent,PackageManager.MATCH_DEFAULT_ONLY);
        return list.size() > 0;
    }
	
	/**
	 * ��UI����ʶ�𷵻���Ϣ
	 * ���ض��ʶ����ʱ���������"|"���Ÿ���
	 */
	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		if(requestCode == REQUEST_CODE_SEARCH && resultCode == RESULT_OK)
		{
			//ȡ��ʶ����ַ���
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
	
	//[start]��̨ʶ��
	
	//����ʶ�����
	private SpeechRecognizer mIat;
	
	/**
	 * ��ʼ������ʶ��
	 */
	public void initSpeechRecognizer(){
		if(mIat == null){
			mIat = new SpeechRecognizer(this, mInitListener);
		}
	}
	
	/**
	 * ��������ʶ��
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
	 * ����UI����ʶ��
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
	 * ֹͣ¼�����ȴ�����˷��ؽ��
	 */
	public void stopSpeechRecognizer(){
		mIat.stopListening(mRecognizerListener);
	}
	
	/**
	 * ȡ����ǰʶ��ֹͣ¼�����Ͽ������˵�����
	 */
	public void cancelSpeechRecognizer(){
		mIat.cancel(mRecognizerListener);
	}
	
	/**
	 * �Ƿ�����ʶ��
	 * @return true��ʾ���ڽ���ʶ��false��ʾ����
	 */
	public boolean isListening(){
		return mIat.isListening();
	}
	
    /**
     * ��ʼ����ɻص��ӿ�
     */
    private InitListener mInitListener = new InitListener() {
    	//��ʼ����ɻص�
		@Override
		public void onInit(ISpeechModule module, int code) {
			printCodeMessage(code);
		}
    };
	
    /**
     * ʶ��ص��ӿ�
     */
    private RecognizerListener mRecognizerListener = new RecognizerListener.Stub() {
        //¼�������ص�
        @Override
        public void onBeginOfSpeech() throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SpeechRecognizerStatus","onBeginOfSpeech");
        }
        //¼���Զ�ֹͣ�ص�
        @Override
        public void onEndOfSpeech() throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SpeechRecognizerStatus","onEndOfSpeech");
        }
        //ʶ�����ص�
        @Override
        public void onError(int errorCode) throws RemoteException {
        	printCodeMessage(errorCode);
        }
        //ʶ�����ص�
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
        //�����仯�ص�
        @Override
        public void onVolumeChanged(int v) throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SRVolumeChanged",Integer.toString(v));
        }
    };
	
    //[end]
    
	//[end]
	
	//[start]Ѷ�������ϳ�
	
	//�����ϳɶ���
	private SpeechSynthesizer mTts;
	
	/**
	 * ��ʼ��Ѷ�������ϳ�
	 */
	public void initSpeechSynthesizer(){
		if(mTts == null){
			mTts = new SpeechSynthesizer(this, mTtsInitListener);
		}
	}
	
	/**
	 * ���������ϳ�
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
 	 * ��ȡ�ϳɲ���
 	 * ��Χ�����������͡����ԡ������������򡢷����˵ȣ��������֧�ֵķ������б� 
 	 * �����ڲ��ϸ����У�Ŀǰ֧�����²�����
 	 * engine_type,language,accent,domain,vad_bos,vad_eos��sample_rate��local_speakers��voice_name��speed��volume
 	 * @param key��������
 	 * @return ����ֵ
 	 */
 	public String getParameter(String key){
 		return mTts.getParameter(key);
 	}
	
	
 	/**
 	 * ���úϳɷ�ʽ
 	 * @param type�ϳɷ�ʽ
 	 * ENGINE_TYPE��ѡ��local��cloud��auto��Ĭ�ϣ�auto
 	 */
 	public void setSynthesizerType(String type){
 		mTts.setParameter(SpeechConstant.ENGINE_TYPE, type);
 	}
 
 	/**
 	 * ���÷�����
 	 * @param person������
 	 * �ƶ�֧�ַ����ˣ�С�ࣨxiaoyan����С�xiaoyu����
 	 * ��ɪ�գ�Catherine���� ������henry����������vimary����
 	 * С�У�vixy����С����vixq���� С�壨vixf����С÷��vixm����
 	 * С��vixl����С�أ��Ĵ������� Сܿ��vixyun����
 	 * С����vixk����Сǿ��vixqa����СӨ��vixying���� 
 	 * С�£�vixx�����骣�vinn�����vils��
 	 * ����֧�ַ�����:С�ࣨxiaoyan��
 	 * ��Ҫ��ӱ��ط����ˣ����Ѷ������+�ͻ�������
 	 */
 	public void setSpeakingPerson(String person){
 		mTts.setParameter(SpeechSynthesizer.VOICE_NAME,	person);
 	}
 	
 	/**
 	 * ��������
 	 * @param speed���٣�0~100��
 	 */
 	public void setSpeed(String speed){
		mTts.setParameter(SpeechSynthesizer.SPEED, speed);
 	}
 	
 	/**
 	 * ��ȡ����
 	 * @return���٣�0~100��
 	 */
	public String getSpeed(){
 		return SpeechSynthesizer.SPEED;
 	}
 	
 	/**
 	 * ��������
 	 * @param pitch������0~100��
 	 */
 	public void setPitch(String pitch){
 		mTts.setParameter(SpeechSynthesizer.PITCH, pitch);
 	}
 	
 	/**
 	 * ��ȡ����
 	 * @return������0~100��
 	 */
 	public String getPitch(){
 		return SpeechSynthesizer.PITCH;
 	}
 	
 	/**
 	 * ��������
 	 * @param volume������0~100��
 	 */
 	public void setVolume(String volume){
 		mTts.setParameter(SpeechSynthesizer.VOLUME, volume);
 	}
 	
 	/**
 	 * ��ȡ����
 	 * @return������0~100��
 	 */
 	public String getVolume(){
 		return SpeechSynthesizer.VOLUME;
 	}
 	
 	
 	/**
 	 * ������չ����
 	 * @param params��չ����
 	 * �ϳ�֧�֣�
 	 * tts_buffer_time�����Ż���ʱ�䣬�������������Ƶ��ʼ���ţ���tts_buffer_time=5000
 	 * tts_audio_path��������Ƶ·������tts_audio_path=/sdcard/tts.pcm
 	 */
 	public void setParams(String params){
 		mTts.setParameter(SpeechConstant.PARAMS, params);
 	}
 	
 	/**
 	 * ��ʼ�����ϳ�
 	 * @param text�����ϳ�����
 	 */
 	public void startSpeaking(String text){
 		int code = mTts.startSpeaking(text, mTtsListener);
 		printCodeMessage(code);
 	}
 	
 	/**
 	 * ��ͣ�����ϳ�
 	 */
 	public void pauseSpeaking(){
 		mTts.pauseSpeaking(mTtsListener);
 	}
 	
 	/**
 	 * ���������ϳ�
 	 */
 	public void resumeSpeaking(){
 		mTts.resumeSpeaking(mTtsListener);
 	}

 	/**
 	 * ���������ϳ�
 	 */
 	public void stopSpeaking(){
 		mTts.stopSpeaking(mTtsListener);
 	}
 	
	/**
     * ��ʼ����ɻص��ӿ�
     */
    private InitListener mTtsInitListener = new InitListener() {
    	//��ʼ����ɻص�
		@Override
		public void onInit(ISpeechModule arg0, int code) {
        	printCodeMessage(code);
		}
    };
        
    /**
     * �����ϳɲ��Żص��ӿ�
     */
    private SynthesizerListener mTtsListener = new SynthesizerListener.Stub() {
    	//������Ȼص�
        @Override
        public void onBufferProgress(int progress) throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SynthesizerBufferProgress",Integer.toString(progress));
        }
        //��������ص�
        @Override
        public void onCompleted(int code) throws RemoteException {
            if(ErrorCode.ERROR_LOCAL_RESOURCE == code){
            	UnityPlayer.UnitySendMessage("iFLYListener","SynthesizerStatus","noSpeeker");
            }else{
            	printCodeMessage(code);
            }
        }
        //��ʼ���Żص�
        @Override
        public void onSpeakBegin() throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SynthesizerStatus","onSpeakBegin");
        }
        //��ͣ���Żص�
        @Override
        public void onSpeakPaused() throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SynthesizerStatus","onSpeakPaused");
        }
        //���Ž��Ȼص�
        @Override
        public void onSpeakProgress(int progress) throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SynthesizerSpeakProgress",Integer.toString(progress));
        }
        //���²��Żص�
        @Override
        public void onSpeakResumed() throws RemoteException {
        	UnityPlayer.UnitySendMessage("iFLYListener","SynthesizerStatus","onSpeakResumed");
        }
    };

    //[end]

    //[start]APK����밲װ
    
	private String type;
	
	/**
	 * ��װѶ������+APK
	 * @param type ��װ��ʽ���С�local���͡�web�����ַ�ʽ
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
	 * ����������û�а�װ�������ְ�װ��ʽ��
	 * 1.web:ֱ�Ӵ����������������ҳ�棬�������غ�װ��
	 * 2.local:�ѷ������apk��װ������assets�У�Ϊ�˱��ⱻ����ѹ�����޸ĺ�׺��Ϊmp3��Ȼ��copy��SDcard�н��а�װ
	 */
	private boolean processInstall(Context context ,String url,String assetsApk){
		if(type.equals("web")){
			// ֱ�����ط�ʽ
			ApkInstaller.openDownloadWeb(context, url);
		}else if(type.equals("local")){
			// ���ذ�װ��ʽ
			if(!ApkInstaller.installFromAssets(context, assetsApk)){
				Toast.makeText(MainActivity.this, "��װʧ��", Toast.LENGTH_SHORT).show();
				return false;
			}
		}
		return true;
	}
	
	/**
	 * ���Ѷ������+APK�Ƿ��Ѿ���װ
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
    
	//[start]ϵͳ���
	
	//��ʾ��Ϣ����
	private Toast mToast;
	private boolean isShowToast;

	/**
	 * Unity��׿�����ʼ��
	 */
	@SuppressLint("ShowToast")
	public void onCreate(Bundle savedInstanceState){
 		super.onCreate(savedInstanceState);
 		mToast = Toast.makeText(this,"",Toast.LENGTH_LONG);  		
 	}
	
	/**
	 * ����AppID
	 * @param id
	 */
	public void setAppID(String AppID){
		SpeechUtility.getUtility(MainActivity.this).setAppid(AppID);
	}
	
	/**
	 * ��ȡ��ʾ��Ϣ��ʾ����״̬
	 * @return
	 */
    public boolean getIsShowToast(){
    	return isShowToast;
    }
    
    /**
     * ������ʾ��Ϣ��ʾ����״̬
     * @param state 
     */
    public void setIsShowToast(boolean state){
    	isShowToast = state;
    }
    
    /**
     * ��ʾ��ʾ��Ϣ
     * @param str��ʾ����
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
	 * ��ʾ��������Ϣ
	 * @param code������
	 */
	private void printCodeMessage(int code){
		switch(code){
		case ErrorCode.ERROR_AUDIO_RECORD:
			showTip("¼��ʧ��");
			break;
		case ErrorCode.ERROR_COMPONENT_NOT_INSTALLED:
			showTip("û�а�װ�������");
			break;
		case ErrorCode.ERROR_EMPTY_UTTERANCE:
			showTip("��Ч���ı�����");
			break;
		case ErrorCode.ERROR_ENGINE_BUSY:
			showTip("���深æ");
			break;
		case ErrorCode.ERROR_ENGINE_CALL_FAIL:
			showTip("����ʧ��");
			break;
		case ErrorCode.ERROR_ENGINE_INIT_FAIL:
			showTip("��ʼ��ʧ��");
			break;
		case ErrorCode.ERROR_ENGINE_NOT_SUPPORTED:
			showTip("���治֧��");
			break;
		case ErrorCode.ERROR_FILE_ACCESS:
			showTip("�ļ���дʧ��");
			break;
		case ErrorCode.ERROR_INTERRUPT:
			showTip("���쳣���");
			break;
		case ErrorCode.ERROR_INVALID_DATA:
			showTip("��Ч����");
			break;
		case ErrorCode.ERROR_INVALID_PARAM:
			showTip("��Ч�Ĳ���");
			break;
		case ErrorCode.ERROR_INVALID_RESULT:
			showTip("����Ч�Ľ��");
			break;
		case ErrorCode.ERROR_LOCAL_ENGINE:
			showTip("���������ڲ�����");
			break;
		case ErrorCode.ERROR_LOCAL_NO_INIT:
			showTip("��������δ��ʼ��");
			break;
		case ErrorCode.ERROR_LOCAL_RESOURCE:
			showTip("������������Դ");
			break;
		case ErrorCode.ERROR_LOGIN:
			showTip("�û�δ��¼");
			break;
		case ErrorCode.ERROR_NET_EXPECTION:
			showTip("�����쳣");
			break;
		case ErrorCode.ERROR_NETWORK_TIMEOUT:
			showTip("�������ӳ�ʱ");
			break;
		case ErrorCode.ERROR_NO_MATCH:
			showTip("��ƥ����");
			break;
		case ErrorCode.ERROR_NO_NETWORK:
			showTip("����Ч����������");
			break;
		case ErrorCode.ERROR_NO_SPPECH:
			showTip("δ��⵽����");
			break;
		case ErrorCode.ERROR_PERMISSION_DENIED:
			showTip("��Ч��Ȩ");
			break;
		case ErrorCode.ERROR_PLAY_MEDIA:
			showTip("��Ƶ����ʧ��");
			break;
		case ErrorCode.ERROR_SPEECH_TIMEOUT:
			showTip("��Ƶ���볬ʱ");
			break;
		case ErrorCode.ERROR_TEXT_OVERFLOW:
			showTip("�ı����");
			break;
		case ErrorCode.ERROR_UNKNOWN:
			showTip("δ֪����");
			break;
		case ErrorCode.SUCCESS:
			break;
		}
	}
	//[end]
	
}