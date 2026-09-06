using System.Text;
using Sproto;
using SprotoType;
using UnityEngine;

public class AccountVersionCheckRootLogic : SingletonUnity<AccountVersionCheckRootLogic>
{
	public GameObject CheckAccountRoot;

	public GameObject CheckVersionRoot;

	public UISprite ProgressLinePic;

	private int mProgressLineWidth = -1;

	private int mProgressTargetWidth;

	public UILabel CheckVersionInfoLabel;

	public UILabel ConnectLabel;

	private string mCurLocalVersion = string.Empty;

	private string mCurApkVersion = string.Empty;

	private FileUpdateHelper updateHelper;

	private UPDATE_STEP mLastUpdateStep;

	public UILabel FaceBookLabel;

	public UISprite BindSpriteIcon;

	public static int FaceBookState;

	public GameObject FackbookBtn;

	public GameObject VisitorBtn;

	public GameObject LineAnimaObj;

	private int curAutoReDownloadTimes;

	public UIInput AccountInput;

	public UIInput PasswordInput;

	private string mNoticeStr;

	private string mNoticeVersion;

	private bool mIsVerifyDownload;

	private bool isAutoDownload;

	private float mNeedDownLoadSize;

	private string mDownLoadUnit;

	private bool isDownloadData;

	private float curProgress;

	private StringBuilder downloadSb = new StringBuilder(512);

	private string downloadFormat = "{0:N1}/{1:N1}{2}";

	private int ChristmasVersionMin = 132;

	private int ChristmasVersionMax = 140;

	private string curVerifyId = string.Empty;

	private string curVerifyKey = string.Empty;

	private long cutBindType;

	public bool IsVerifyDownload => mIsVerifyDownload;

	private new void Awake()
	{
		base.Awake();
		curAutoReDownloadTimes = 0;
		NGUITools.SetActive(CheckAccountRoot, state: false);
		NGUITools.SetActive(CheckVersionRoot, state: false);
		NGUITools.SetActive(ConnectLabel.gameObject, state: true);
		ConnectLabel.text = StrDictionary.GetDictionaryString("#{102211}");
		ProgressLinePic.width = UIWidgetControl.GetFitWidth(ProgressLinePic.width);
		ProgressLinePic.transform.localPosition = new Vector3(-ProgressLinePic.width / 2, 0f, 0f);
		mProgressLineWidth = ProgressLinePic.width;
		SetProgressLineWidth(0);
		updateHelper = base.gameObject.GetComponent<FileUpdateHelper>();
		if (updateHelper == null)
		{
			updateHelper = base.gameObject.AddComponent<FileUpdateHelper>();
		}
	}

	private void SetProgressLineWidth(int width)
	{
		ProgressLinePic.width = width;
		LineAnimaObj.transform.localPosition = new Vector3(width, 0f, 0f);
	}

	public void StartVerifyAccount()
	{
		if (FaceBookState != 0)
		{
			ReLoginAndBindFacebook();
			FaceBookState = 0;
			return;
		}
		string playerAccountId = PlayerData.GetPlayerAccountId();
		if (string.IsNullOrEmpty(playerAccountId))
		{
			ResetAccountPage();
			return;
		}
		verfiy.request request = new verfiy.request();
		request.id = playerAccountId;
		request.key = PlayerData.GetPlayerAccountKey();
		curVerifyId = playerAccountId;
		curVerifyKey = request.key;
		request.versionCode = GameSettingData.GameVersion;
		NetLogic.GetInstance().Send<Protocol.verfiy>(request, OnVerifyResponse);
	}

	public void OnClickCreateAccountBtn()
	{
		MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{201023}"), StrDictionary.GetDictionaryString("#{100127}"), delegate
		{
			PlayerData.ClearAccount();
			WaitResponseUIRootLogic.OpenWaitBox(2, 0f, 0f);
			if (NetLogic.GetInstance().ConnectStatus == NetLogic.CONNECT_STATUS.CONNRCTED)
			{
				NetLogic.GetInstance().Send<Protocol.visitor>(null, VisitorResponse);
			}
			else
			{
				SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(PlayerData.CurLoginServerData.LoginIP, PlayerData.CurLoginServerData.LoginPort, delegate(bool success)
				{
					if (success)
					{
						NetLogic.GetInstance().Send<Protocol.visitor>(null, VisitorResponse);
					}
					else
					{
						SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TopMessageBoxUI, delegate
						{
							SingletonUnity<TopMessageBoxLogic>.Instance.OpenOKCancelBox("#{100142}", "#{100127}", OnYesClick, OnCancleClick, "#{100143}", "#{100144}");
						});
						NoticeLogic.AddNotifyData("#{200065}");
					}
				}, ConnectLost);
			}
		}, delegate
		{
			ResetAccountPage();
		});
	}

	public void StartCheckVerifyAccount()
	{
		if (FaceBookState != 0)
		{
			ReLoginAndBindFacebook();
			FaceBookState = 0;
		}
		else
		{
			VerifyAccount();
		}
	}

	public void ReLoginVerfiyAccount()
	{
		SingletonDontDestoryUnity<NetManager>.Instance.LeaveGame();
		ResetAccountPage();
	}

	public void ReLoginAndBindFacebook()
	{
		SingletonDontDestoryUnity<NetManager>.Instance.LeaveGame();
		ResetAccountPage();
		OnClickFaceBookBtn();
	}

	public static void CheckFaceBookBindTips(int type)
	{
		if (PlayerData.FaceBookBind > -1)
		{
			return;
		}
		if (type == 0 && LocalDataSaveManager.GetFaceBookPopTips(type))
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.FaceBookTipUIRoot, delegate
			{
				SingletonUnity<FaceBookTipUIRoot>.Instance.Reset(ClickBindFaceBook);
			});
			LocalDataSaveManager.SetFaceBookPopCount(0);
		}
		else if (type == 1 && LocalDataSaveManager.GetFaceBookPopTips(type) && LocalDataSaveManager.GetFaceBookNextPopDay() && !LocalDataSaveManager.IsFirstEnterGame())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.FaceBookTipUIRoot, delegate
			{
				SingletonUnity<FaceBookTipUIRoot>.Instance.Reset(ClickBindFaceBook);
			});
			LocalDataSaveManager.SetFaceBookPopCount(1);
			LocalDataSaveManager.SetFaceBookBindTime();
		}
	}

	public static void ClickBindFaceBook()
	{
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		if (!Application.loadedLevelName.Equals(GameDefine.LOGIN_SCENE_NAME))
		{
			SingletonDontDestoryUnity<NetManager>.Instance.LeaveGame();
			FaceBookState = 1;
			LoadingWindow.LoadScene(0);
		}
		else
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.AccountVersionCheckRootUI, delegate
			{
				SingletonUnity<AccountVersionCheckRootLogic>.Instance.ReLoginAndBindFacebook();
			});
		}
	}

	public void OnVerifyResponse(SprotoTypeBase req)
	{
		WaitResponseUIRootLogic.CloseBox();
		verfiy.response response = req as verfiy.response;
		mNoticeStr = response.notice;
		mNoticeVersion = response.notice_version;
		PlayerData.loginType = 1;
		if (response.HasFacebook_bind || response.HasFacebook_bind1 || response.HasGoogle_bind)
		{
			if (response.HasFacebook_bind1)
			{
				PlayerData.FaceBookBind2 = response.facebook_bind1;
				PlayerData.FaceBookBind = 100L;
			}
			else if (response.HasFacebook_bind1)
			{
				PlayerData.FaceBookBind2 = response.facebook_bind.ToString();
				PlayerData.FaceBookBind = 100L;
			}
			if (response.HasGoogle_bind)
			{
				if (PlayerData.FaceBookBind == 100)
				{
					PlayerData.FaceBookBind = 300L;
				}
				else
				{
					PlayerData.FaceBookBind = 200L;
				}
			}
		}
		else
		{
			PlayerData.FaceBookBind = -1L;
			PlayerData.FaceBookBind2 = string.Empty;
		}
		if (response.HasVersionCode && SingletonDontDestoryUnity<NetManager>.Instance.IsNeedQuitUpdate(response.versionCode, delegate
		{
			if (response.state == 0L)
			{
				PlayerData.session = response.session;
				PlayerData.loginType = 1;
				if (!int.TryParse(response.dataVersionCode, out PlayerData.ServerDataVersion))
				{
					PlayerData.ServerDataVersion = -1;
				}
				if (response.HasDownloadFlag)
				{
					PlayerData.downLoadFlag = (int)response.downloadFlag;
					ResetDownLoadPage(isVerifyDownload: true);
				}
				else
				{
					PlayerData.downLoadFlag = 0;
					ResetDownLoadPage(isVerifyDownload: true);
				}
				SingletonUnity<MenuSceneController>.Instance.SaveServerList(response);
				PlayerData.SavePlayerAccountId(curVerifyId);
				PlayerData.SavePlayerAccountKey(curVerifyKey);
			}
			else if (response.state == 2)
			{
				PlayerData.session = response.session;
				PlayerData.loginType = 2;
				if (!int.TryParse(response.dataVersionCode, out PlayerData.ServerDataVersion))
				{
					PlayerData.ServerDataVersion = -1;
				}
				if (response.HasDownloadFlag)
				{
					PlayerData.downLoadFlag = (int)response.downloadFlag;
					ResetDownLoadPage(isVerifyDownload: true);
				}
				else
				{
					PlayerData.downLoadFlag = 0;
					ResetDownLoadPage(isVerifyDownload: true);
				}
				SingletonUnity<MenuSceneController>.Instance.SaveServerList(response);
				PlayerData.SavePlayerAccountId(curVerifyId);
				PlayerData.SavePlayerAccountKey(curVerifyKey);
			}
			else if (response.state == 3)
			{
				NoticeLogic.AddNotifyData("#{200097}");
				ResetAccountPage();
			}
			else
			{
				MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{201022}"), StrDictionary.GetDictionaryString("#{100127}"), delegate
				{
					ResetAccountPage();
				});
			}
		}))
		{
			return;
		}
		if (response.state == 0L)
		{
			PlayerData.session = response.session;
			PlayerData.loginType = 1;
			if (!int.TryParse(response.dataVersionCode, out PlayerData.ServerDataVersion))
			{
				PlayerData.ServerDataVersion = -1;
			}
			if (response.HasDownloadFlag)
			{
				PlayerData.downLoadFlag = (int)response.downloadFlag;
				ResetDownLoadPage(isVerifyDownload: true);
			}
			else
			{
				PlayerData.downLoadFlag = 0;
				ResetDownLoadPage(isVerifyDownload: true);
			}
			SingletonUnity<MenuSceneController>.Instance.SaveServerList(response);
			PlayerData.SavePlayerAccountId(curVerifyId);
			PlayerData.SavePlayerAccountKey(curVerifyKey);
		}
		else if (response.state == 2)
		{
			PlayerData.session = response.session;
			PlayerData.loginType = 2;
			if (!int.TryParse(response.dataVersionCode, out PlayerData.ServerDataVersion))
			{
				PlayerData.ServerDataVersion = -1;
			}
			if (response.HasDownloadFlag)
			{
				PlayerData.downLoadFlag = (int)response.downloadFlag;
				ResetDownLoadPage(isVerifyDownload: true);
			}
			else
			{
				PlayerData.downLoadFlag = 0;
				ResetDownLoadPage(isVerifyDownload: true);
			}
			SingletonUnity<MenuSceneController>.Instance.SaveServerList(response);
			PlayerData.SavePlayerAccountId(curVerifyId);
			PlayerData.SavePlayerAccountKey(curVerifyKey);
		}
		else if (response.state == 3)
		{
			NoticeLogic.AddNotifyData("#{200097}");
			ResetAccountPage();
		}
		else
		{
			MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{201022}"), StrDictionary.GetDictionaryString("#{100127}"), delegate
			{
				ResetAccountPage();
			});
		}
	}

	public void RefreshAccount()
	{
		if (PlayerData.FaceBookBind > -1)
		{
			if (PlayerData.FaceBookBind == 100)
			{
				FaceBookLabel.text = "Unbind";
				BindSpriteIcon.spriteName = "CZ_faceBook";
			}
			else
			{
				FaceBookLabel.text = "Unbind";
				BindSpriteIcon.spriteName = "CZ_google";
			}
		}
		else
		{
			BindSpriteIcon.spriteName = "CZ_zhuangHaoGuanLian";
			FaceBookLabel.text = "Bind";
		}
	}

	public void ResetAccountPage()
	{
		NGUITools.SetActive(ConnectLabel.gameObject, state: false);
		NGUITools.SetActive(CheckAccountRoot, state: true);
		NGUITools.SetActive(CheckVersionRoot, state: false);
		AccountInput.value = PlayerData.GetPlayerAccountId();
		PasswordInput.value = PlayerData.GetPlayerAccountKey();
		RefreshAccount();
	}

	public void ResetDownLoadPage(bool isVerifyDownload)
	{
		int serverDataVersion = PlayerData.ServerDataVersion;
		if (serverDataVersion >= ChristmasVersionMin && serverDataVersion <= ChristmasVersionMax)
		{
			isAutoDownload = true;
			downloadFormat = "Downloading Christmas Resources: {0:N1}/{1:N1}{2}";
		}
		else
		{
			isAutoDownload = false;
			downloadFormat = "{0:N1}/{1:N1}{2}";
		}
		mIsVerifyDownload = isVerifyDownload;
		NGUITools.SetActive(ConnectLabel.gameObject, state: false);
		NGUITools.SetActive(CheckAccountRoot, state: false);
		NGUITools.SetActive(CheckVersionRoot, state: true);
		isDownloadData = false;
		updateHelper.StartCheckRes(updateHelper.mServerUrl, OnChangeUpdateStep, !isVerifyDownload, isNeedCopyRes: true);
	}

	public void OnChangeUpdateStep(UPDATE_STEP newStep)
	{
		switch (newStep)
		{
		case UPDATE_STEP.CHECK_VERSION:
			SetProgressLineVal(0.3f);
			CheckVersionInfoLabel.text = StrDictionary.GetDictionaryString("#{102203}");
			break;
		case UPDATE_STEP.GET_FILELIST:
			SetProgressLineVal(0.6f);
			CheckVersionInfoLabel.text = StrDictionary.GetDictionaryString("#{102204}");
			break;
		case UPDATE_STEP.COMPARE_RES:
			SetProgressLineVal(0.95f);
			CheckVersionInfoLabel.text = StrDictionary.GetDictionaryString("#{102205}");
			break;
		case UPDATE_STEP.CHECK_IS_DOWNLOAD:
			SetProgressLineVal(1f);
			if (updateHelper.NeedDownloadSize > 1048576)
			{
				mNeedDownLoadSize = (float)updateHelper.NeedDownloadSize / 1024f / 1024f;
				mDownLoadUnit = "MB";
			}
			else
			{
				mNeedDownLoadSize = (float)updateHelper.NeedDownloadSize / 1024f;
				mDownLoadUnit = "KB";
			}
			break;
		case UPDATE_STEP.CHECK_RES:
			SetProgressLineWidth(0);
			SetProgressLineVal(0.3f);
			CheckVersionInfoLabel.text = StrDictionary.GetDictionaryString("#{102207}");
			break;
		case UPDATE_STEP.COPY_RES:
			SetProgressLineVal(0.6f);
			break;
		case UPDATE_STEP.CLEAR_CACHE:
			SetProgressLineVal(0.93f);
			isDownloadData = true;
			break;
		case UPDATE_STEP.FINISH:
			if (updateHelper.CurUpdateResult == UPDATE_RESULT.SUCCESS)
			{
				if (mIsVerifyDownload)
				{
					if (!AnimationManager.initDownloadFlag)
					{
						AnimationManager.initDownloadAnimationData(SingletonDontDestoryUnity<GameManager>.Instance);
					}
					else if (isDownloadData)
					{
						SingletonDontDestoryUnity<GameManager>.Instance.ReImportAnima();
					}
					if (!DataManager.initFlag)
					{
						DataManager.InitData(SingletonDontDestoryUnity<GameManager>.Instance);
					}
					else if (isDownloadData)
					{
						SingletonDontDestoryUnity<GameManager>.Instance.ReImportData();
					}
				}
				else if (isDownloadData)
				{
					SingletonDontDestoryUnity<GameManager>.Instance.ReImportData();
					SingletonDontDestoryUnity<GameManager>.Instance.ReImportAnima();
				}
				SetProgressLineVal(0.95f);
			}
			else
			{
				SetProgressLineVal(0.3f);
				if (curAutoReDownloadTimes < GameDefine.AutoReDownloadTimes)
				{
					curAutoReDownloadTimes++;
					isAutoDownload = true;
					ReDownLoad();
				}
				else
				{
					isAutoDownload = false;
					MessageBoxLogic.OpenOKCancelBox("#{100156}", "#{100127}", ReDownLoad, OnClickExitGame, null, "#{100149}");
				}
			}
			break;
		}
		mLastUpdateStep = newStep;
	}

	private void OnClickExitGame()
	{
		SingletonDontDestoryUnity<NetManager>.Instance.LeaveGame();
		MessageBoxLogic.CloseBox();
		Application.Quit();
	}

	public void StartDownload()
	{
		CheckVersionInfoLabel.text = string.Format(downloadFormat, 0.0, mNeedDownLoadSize, mDownLoadUnit);
		updateHelper.DownloadFileList();
	}

	public void ContinueDownLoad()
	{
		updateHelper.ContinueDownload();
	}

	public void ReDownLoad()
	{
		SetProgressLineWidth(0);
		updateHelper.StartCheckRes(updateHelper.mServerUrl, OnChangeUpdateStep, !mIsVerifyDownload, isNeedCopyRes: true);
	}

	private void SetProgressLineVal(float val)
	{
		mProgressTargetWidth = (int)(val * (float)mProgressLineWidth);
		curProgress = val;
	}

	private void CheckDownloadRes()
	{
		CheckVersionInfoLabel.text = string.Empty;
		SetProgressLineVal(0f);
		SetProgressLineWidth(0);
		if (updateHelper.NeedDownloadSize > 1048576)
		{
			mNeedDownLoadSize = (float)updateHelper.NeedDownloadSize / 1024f / 1024f;
			mDownLoadUnit = "MB";
		}
		else
		{
			mNeedDownLoadSize = (float)updateHelper.NeedDownloadSize / 1024f;
			mDownLoadUnit = "KB";
		}
		if (isAutoDownload)
		{
			StartDownload();
			return;
		}
		MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{102209}", $"{mNeedDownLoadSize:N1}{mDownLoadUnit}"), StrDictionary.GetDictionaryString("#{102210}"), StartDownload, delegate
		{
			Application.Quit();
		});
	}

	private void Update()
	{
		if (mLastUpdateStep == UPDATE_STEP.DOWNLOAD_RES)
		{
			SetProgressLineVal(updateHelper.GetDownloadProgress());
		}
		if (mLastUpdateStep == UPDATE_STEP.FINISH && updateHelper.CurUpdateResult == UPDATE_RESULT.SUCCESS && DataManager.DataInitFinishFlag)
		{
			SetProgressLineVal(1f);
		}
		if (ProgressLinePic.width >= mProgressTargetWidth)
		{
			return;
		}
		int num = ProgressLinePic.width + 10;
		if (num > mProgressTargetWidth)
		{
			num = mProgressTargetWidth;
		}
		SetProgressLineWidth(num);
		if (mLastUpdateStep == UPDATE_STEP.DOWNLOAD_RES)
		{
			downloadSb.Length = 0;
			downloadSb.AppendFormat(downloadFormat, mNeedDownLoadSize * curProgress, mNeedDownLoadSize, mDownLoadUnit);
			CheckVersionInfoLabel.text = downloadSb.ToString();
		}
		if (ProgressLinePic.width < mProgressLineWidth)
		{
			return;
		}
		if (mLastUpdateStep == UPDATE_STEP.FINISH)
		{
			if (mIsVerifyDownload)
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.AccountVersionCheckRootUI);
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.LoginRootUI, delegate
				{
					SingletonUnity<LoginRootLogic>.Instance.Reset(mNoticeStr, mNoticeVersion);
				});
			}
			else
			{
				WaitResponseUIRootLogic.OpenWaitBox(0, 10f, 0f);
				SingletonDontDestoryUnity<NetManager>.Instance.CharacterListRequest();
			}
		}
		else if (mLastUpdateStep == UPDATE_STEP.CHECK_IS_DOWNLOAD)
		{
			CheckDownloadRes();
		}
	}

	public void VerifyAccount()
	{
		WaitResponseUIRootLogic.OpenWaitBox(2, 0f, 0f);
		if (NetLogic.GetInstance().ConnectStatus == NetLogic.CONNECT_STATUS.CONNRCTED)
		{
			string playerAccountId = PlayerData.GetPlayerAccountId();
			if (string.IsNullOrEmpty(playerAccountId))
			{
				NetLogic.GetInstance().Send<Protocol.visitor>(null, VisitorResponse);
				return;
			}
			verfiy.request request = new verfiy.request();
			request.id = playerAccountId;
			request.key = PlayerData.GetPlayerAccountKey();
			curVerifyId = playerAccountId;
			curVerifyKey = request.key;
			request.versionCode = GameSettingData.GameVersion;
			NetLogic.GetInstance().Send<Protocol.verfiy>(request, OnVerifyResponse);
		}
		else
		{
			SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(PlayerData.CurLoginServerData.LoginIP, PlayerData.CurLoginServerData.LoginPort, ConnectSuccess, ConnectLost);
		}
	}

	public void OnClickShowPasswordBtn()
	{
		if (PasswordInput.inputType == UIInput.InputType.Password)
		{
			PasswordInput.inputType = UIInput.InputType.Standard;
		}
		else if (PasswordInput.inputType == UIInput.InputType.Standard)
		{
			PasswordInput.inputType = UIInput.InputType.Password;
		}
		else
		{
			PasswordInput.inputType = UIInput.InputType.Password;
		}
		PasswordInput.UpdateLabel();
	}

	public void OnClickVisitorBtn()
	{
		WaitResponseUIRootLogic.OpenWaitBox(2, 0f, 0f);
		if (NetLogic.GetInstance().ConnectStatus == NetLogic.CONNECT_STATUS.CONNRCTED)
		{
			string value = AccountInput.value;
			string value2 = PasswordInput.value;
			if (string.IsNullOrEmpty(value))
			{
				WaitResponseUIRootLogic.CloseBox();
				MessageBoxLogic.OpenOKBox("#{201024}", "#{100127}");
				return;
			}
			if (string.IsNullOrEmpty(value2))
			{
				WaitResponseUIRootLogic.CloseBox();
				MessageBoxLogic.OpenOKBox("#{201025}", "#{100127}");
				return;
			}
			verfiy.request request = new verfiy.request();
			request.id = value;
			request.key = value2;
			curVerifyId = value;
			curVerifyKey = value2;
			request.versionCode = GameSettingData.GameVersion;
			NetLogic.GetInstance().Send<Protocol.verfiy>(request, OnVerifyResponse);
		}
		else
		{
			SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(PlayerData.CurLoginServerData.LoginIP, PlayerData.CurLoginServerData.LoginPort, ClickVisitorConnectSuccess, ConnectLost);
		}
	}

	private void ClickVisitorConnectSuccess(bool isSuccess)
	{
		WaitResponseUIRootLogic.CloseBox();
		if (isSuccess)
		{
			if (FaceBookState != 0)
			{
				ReLoginAndBindFacebook();
				FaceBookState = 0;
				return;
			}
			string value = AccountInput.value;
			string value2 = PasswordInput.value;
			if (string.IsNullOrEmpty(value))
			{
				MessageBoxLogic.OpenOKBox("#{201024}", "#{100127}");
				return;
			}
			if (string.IsNullOrEmpty(value2))
			{
				MessageBoxLogic.OpenOKBox("#{201025}", "#{100127}");
				return;
			}
			WaitResponseUIRootLogic.OpenWaitBox(3, 10f, 0f);
			verfiy.request request = new verfiy.request();
			request.id = value;
			request.key = value2;
			curVerifyId = value;
			curVerifyKey = value2;
			request.versionCode = GameSettingData.GameVersion;
			NetLogic.GetInstance().Send<Protocol.verfiy>(request, OnVerifyResponse);
		}
		else
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TopMessageBoxUI, delegate
			{
				SingletonUnity<TopMessageBoxLogic>.Instance.OpenOKCancelBox("#{100142}", "#{100127}", OnYesClick, OnCancleClick, "#{100143}", "#{100144}");
			});
			NoticeLogic.AddNotifyData("#{200065}");
		}
	}

	private void ConnectLost()
	{
		WaitResponseUIRootLogic.CloseBox();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TopMessageBoxUI, delegate
		{
			SingletonUnity<TopMessageBoxLogic>.Instance.OpenOKCancelBox("#{100142}", "#{100127}", OnYesClick, OnCancleClick, "#{100143}", "#{100144}");
		});
	}

	private void ConnectSuccess(bool isSuccess)
	{
		WaitResponseUIRootLogic.CloseBox();
		if (isSuccess)
		{
			StartVerifyAccount();
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TopMessageBoxUI, delegate
		{
			SingletonUnity<TopMessageBoxLogic>.Instance.OpenOKCancelBox("#{100142}", "#{100127}", OnYesClick, OnCancleClick, "#{100143}", "#{100144}");
		});
		NoticeLogic.AddNotifyData("#{200065}");
	}

	private void OnCancleClick()
	{
		Application.Quit();
	}

	private void OnYesClick()
	{
		WaitResponseUIRootLogic.OpenWaitBox(4, 0f, 0f);
		SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(PlayerData.CurLoginServerData.LoginIP, PlayerData.CurLoginServerData.LoginPort, ConnectSuccess, ConnectLost);
	}

	private void VisitorResponse(SprotoTypeBase rep)
	{
		if (rep is visitor.response { state: 0L } response)
		{
			PlayerData.SavePlayerAccountId(response.id);
			PlayerData.SavePlayerAccountKey(response.key);
			StartVerifyAccount();
		}
		else
		{
			WaitResponseUIRootLogic.CloseBox();
			NetLogic.GetInstance().DisconnectServer();
		}
	}

	public void OnClickFaceBookBtn()
	{
		if (PlayerData.FaceBookBind < 0)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.AccountBindUIRoot);
		}
		else if (PlayerData.FaceBookBind == 100)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.SignOutFacebook();
		}
		else
		{
			SingletonDontDestoryUnity<GameManager>.Instance.SignOutGoogle();
		}
	}

	public void SignInCheck(string id, string token, int type = 0)
	{
		WaitResponseUIRootLogic.OpenWaitBox(5, -1f, 0f);
		if (PlayerData.FaceBookBind > -1)
		{
			SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(PlayerData.CurLoginServerData.LoginIP, PlayerData.CurLoginServerData.LoginPort, delegate
			{
				FaceBookUnlink(id, token, type);
			}, ConnectLost);
		}
		else
		{
			SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(PlayerData.CurLoginServerData.LoginIP, PlayerData.CurLoginServerData.LoginPort, delegate
			{
				FaceBookSignInSuccess(id, token, type);
			}, ConnectLost);
		}
	}

	private void UseLocalAccount()
	{
		string faceBookId = PlayerData.FaceBookId;
		string facekBookToken = PlayerData.FacekBookToken;
		string playerAccountId = PlayerData.GetPlayerAccountId();
		string playerAccountKey = PlayerData.GetPlayerAccountKey();
		facebook_link.request request = new facebook_link.request();
		request.facebook_id = faceBookId;
		request.facebook_token = facekBookToken;
		request.id = playerAccountId;
		request.key = playerAccountKey;
		request.bindType = cutBindType;
		request.confirm = 1L;
		WaitResponseUIRootLogic.OpenWaitBox(5, 10f, 0f);
		NetLogic.GetInstance().Send<Protocol.facebook_link>(request, delegate(SprotoTypeBase rpc)
		{
			WaitResponseUIRootLogic.CloseBox();
			facebook_link.response response = rpc as facebook_link.response;
			if (response.state == 0L)
			{
				PlayerData.SavePlayerAccountId(response.id);
				PlayerData.SavePlayerAccountKey(response.key);
				StartVerifyAccount();
			}
			else
			{
				NoticeLogic.AddNotifyData("#{200093}");
			}
		});
	}

	private void FacebookLinkResponse(SprotoTypeBase rpcRsp)
	{
		WaitResponseUIRootLogic.CloseBox();
		facebook_link.response response = rpcRsp as facebook_link.response;
		if (response.state == 0L)
		{
			if (response.bindType == 0L)
			{
				if (PlayerData.FaceBookBind < 0)
				{
					PlayerData.FaceBookBind = 100L;
				}
				else if (PlayerData.FaceBookBind == 200)
				{
					PlayerData.FaceBookBind = 300L;
				}
			}
			else if (PlayerData.FaceBookBind < 0)
			{
				PlayerData.FaceBookBind = 200L;
			}
			else if (PlayerData.FaceBookBind == 100)
			{
				PlayerData.FaceBookBind = 300L;
			}
			PlayerData.SavePlayerAccountId(response.id);
			PlayerData.SavePlayerAccountKey(response.key);
			StartVerifyAccount();
		}
		else if (response.state == 1)
		{
			if (response.bindType == 0L)
			{
				if (PlayerData.FaceBookBind < 0)
				{
					PlayerData.FaceBookBind = 100L;
				}
				else if (PlayerData.FaceBookBind == 200)
				{
					PlayerData.FaceBookBind = 300L;
				}
			}
			else if (PlayerData.FaceBookBind < 0)
			{
				PlayerData.FaceBookBind = 200L;
			}
			else if (PlayerData.FaceBookBind == 100)
			{
				PlayerData.FaceBookBind = 300L;
			}
			string serverId = response.id;
			string serverKey = response.key;
			cutBindType = response.bindType;
			MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{200068}"), StrDictionary.GetDictionaryString("#{100127}"), UseLocalAccount, delegate
			{
				MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{200069}"), StrDictionary.GetDictionaryString("#{100127}"), delegate
				{
					PlayerData.SavePlayerAccountId(serverId);
					PlayerData.SavePlayerAccountKey(serverKey);
					StartVerifyAccount();
				});
			});
		}
		else
		{
			NoticeLogic.AddNotifyData("#{200093}");
		}
	}

	private void FacebookUnLinkResponse(SprotoTypeBase rpcRsp)
	{
		facebook_unlink.response response = rpcRsp as facebook_unlink.response;
		WaitResponseUIRootLogic.CloseBox();
		if (response == null)
		{
			return;
		}
		if (response.state == 0L)
		{
			if (response.bindType == 0L)
			{
				if (PlayerData.FaceBookBind == 100)
				{
					PlayerData.FaceBookBind = -1L;
				}
				else if (PlayerData.FaceBookBind == 300)
				{
					PlayerData.FaceBookBind = 200L;
				}
			}
			else if (PlayerData.FaceBookBind == 200)
			{
				PlayerData.FaceBookBind = -1L;
			}
			else if (PlayerData.FaceBookBind == 300)
			{
				PlayerData.FaceBookBind = 100L;
			}
		}
		else
		{
			NoticeLogic.AddNotifyData("#{200094}");
		}
		RefreshAccount();
	}

	public void FaceBookUnlink(string facebookId, string facebookToken, int type = 0)
	{
		string playerAccountId = PlayerData.GetPlayerAccountId();
		string playerAccountKey = PlayerData.GetPlayerAccountKey();
		facebook_unlink.request request = new facebook_unlink.request();
		request.id = playerAccountId;
		request.key = playerAccountKey;
		request.bindType = type;
		WaitResponseUIRootLogic.OpenWaitBox(6, 10f, 0f);
		NetLogic.GetInstance().Send<Protocol.facebook_unlink>(request, FacebookUnLinkResponse);
	}

	public void FaceBookSignInSuccess(string facebookId, string facebookToken, int type = 0)
	{
		string playerAccountId = PlayerData.GetPlayerAccountId();
		string playerAccountKey = PlayerData.GetPlayerAccountKey();
		facebook_link.request request = new facebook_link.request();
		request.facebook_id = facebookId;
		request.facebook_token = facebookToken;
		request.bindType = type;
		PlayerData.FaceBookId = facebookId;
		PlayerData.FacekBookToken = facebookToken;
		if (!string.IsNullOrEmpty(playerAccountId))
		{
			request.id = playerAccountId;
		}
		if (!string.IsNullOrEmpty(playerAccountKey))
		{
			request.key = playerAccountKey;
		}
		WaitResponseUIRootLogic.OpenWaitBox(5, 10f, 0f);
		NetLogic.GetInstance().Send<Protocol.facebook_link>(request, FacebookLinkResponse);
	}
}
