using System;
using System.Text;
using System.Data;
using Common.Logic;
using STONE.HU.HELPER.UTIL;
using Common.DataInfo;
using lg = Common.API.LanguageAPI;
using SDGOManageBrokerLib;
using System.Runtime.InteropServices;

using GM_Server.SDOnlineDataInfo;

namespace GM_Server.SDOnlineAPI
{
	/// <summary>
	/// SDLogInfoAPI ��ժҪ˵����
	/// </summary>
	
	public class SDLogInfoAPI
	{
		[DllImport("SDOManager.dll")]
		private static extern int ManageKickOut(int ServerGroup, int Account,byte[] NickName);

		Message msg = null;
		public static Log log = null;
		public SDLogInfoAPI(byte[] packet)
		{
			msg = new Message(packet,(uint)packet.Length);
			
		}
		public SDLogInfoAPI()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}	
		#region lg.SDAPI_SDItemShopAPI_Integral+lg.SDAPI_SDItemLogInfoAPI_Account+lg.SDAPI_SDItemMsG9+��Ϣ
		/// <summary>
		/// lg.SDAPI_SDItemShopAPI_Integral+lg.SDAPI_SDItemLogInfoAPI_Account+lg.SDAPI_SDItemMsG9+��Ϣ
		/// </summary>
		/// <returns></returns>
		public Message SD_UserLoginfo_Query(int index,int pageSize)
		{
			string serverIP = null;
			//int uid = 0;
			DataSet ds = null;
			string strnick = null;
			string username = null;
			DateTime BeginTime ;
			DateTime EndTime ;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();

				strut = new TLV_Structure(TagName.SD_StartTime, 6, msg.m_packet.m_Body.getTLVByTag(TagName.SD_StartTime).m_bValueBuffer);
				BeginTime  =strut.toTimeStamp();

				strut = new TLV_Structure(TagName.SD_EndTime, 6, msg.m_packet.m_Body.getTLVByTag(TagName.SD_EndTime).m_bValueBuffer);
				EndTime  =strut.toTimeStamp();

				
				SqlHelper.log.WriteLog(lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+lg.SDAPI_SDItemLogInfoAPI_Account+username+lg.SDAPI_SDItemMsG9+lg.SDAPI_SDChallengeDataAPI_ProbabilityList);
				Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+lg.SDAPI_SDItemLogInfoAPI_Account+username+lg.SDAPI_SDItemMsG9+lg.SDAPI_SDChallengeDataAPI_ProbabilityList);
				ds = SDLogDataInfo.UserLoginfo_Query(serverIP,userid,BeginTime,EndTime);		
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,index,pageSize,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.SD_ADMIN,ServiceKey.SD_UserLoginfo_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemLogInfoAPI_Account+lg.SDAPI_SDItemMsG9+lg.SDAPI_SDChallengeDataAPI_ProbabilityList,Msg_Category.SD_ADMIN,ServiceKey.SD_UserLoginfo_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemLogInfoAPI_Account+lg.SDAPI_SDItemMsG9+lg.SDAPI_SDChallengeDataAPI_ProbabilityList, Msg_Category.SD_ADMIN, ServiceKey.SD_UserLoginfo_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region lg.SDAPI_SDItemLogInfoAPI_Account+����\�ʼ�lg.SDAPI_SDItemShopAPI_Integral+
		/// <summary>
		/// lg.SDAPI_SDItemLogInfoAPI_Account+����\�ʼ�lg.SDAPI_SDItemShopAPI_Integral+
		/// </summary>
		/// <returns></returns>
		public Message SD_UserGrift_Query(int index,int pageSize)
		{
			string serverIP = null;
			//int uid = 0;
			DataSet ds = null;
			int type =-1;
			string strnick = null;
			string username = null;
			int userid = 0;
			DateTime BeginTime;
			DateTime EndTime;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();

				strut = new TLV_Structure(TagName.SD_StartTime, 6, msg.m_packet.m_Body.getTLVByTag(TagName.SD_StartTime).m_bValueBuffer);
				BeginTime  =strut.toTimeStamp();

				strut = new TLV_Structure(TagName.SD_EndTime, 6, msg.m_packet.m_Body.getTLVByTag(TagName.SD_EndTime).m_bValueBuffer);
				EndTime  =strut.toTimeStamp();

				strut = new TLV_Structure(TagName.SD_Type, 4, msg.m_packet.m_Body.getTLVByTag(TagName.SD_Type).m_bValueBuffer);
				type  =(byte)strut.toInteger();
				
				SqlHelper.log.WriteLog(lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+lg.SDAPI_SDItemLogInfoAPI_Account+username+lg.SDAPI_SDItemShopAPI_NoIntegral+lg.SDAPI_SDChallengeDataAPI_ProbabilityList);
				Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+lg.SDAPI_SDItemLogInfoAPI_Account+username+lg.SDAPI_SDItemShopAPI_NoIntegral+lg.SDAPI_SDChallengeDataAPI_ProbabilityList);
				ds = SDLogDataInfo.UserGrift_Query(serverIP,userid,BeginTime,EndTime,type);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,index,pageSize,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.SD_ADMIN,ServiceKey.SD_UserGrift_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemLogInfoAPI_Account+lg.SDAPI_SDItemShopAPI_NoIntegral+lg.SDAPI_SDChallengeDataAPI_ProbabilityList,Msg_Category.SD_ADMIN,ServiceKey.SD_UserGrift_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemLogInfoAPI_Account+lg.SDAPI_SDItemShopAPI_NoIntegral+lg.SDAPI_SDChallengeDataAPI_ProbabilityList, Msg_Category.SD_ADMIN, ServiceKey.SD_UserGrift_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region �û�������
		/// <summary>
		/// �û�������
		/// </summary>
		/// <returns></returns>
		public Message SD_KickUser_Query()
		{
			string serverIP = null;
			//int uid = 0;
			int result = -1;
			string strnick = null;
			string username = null;
			int UserByID = 0;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();
				//GM ����Ա
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();

				strnick = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.f_pilot).m_bValueBuffer);
				byte[] bytNick = System.Text.Encoding.Unicode.GetBytes(strnick);
				int servergroup = int.Parse(CommonInfo.SD_GameDBInfo_Query(serverIP)[0].ToString());
				result = ManageKickOut(servergroup,10,bytNick);
				if(result ==0)
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_KickUser_Query","�û�"+username.ToString()+"�������߳ɹ�");
					Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"���û�"+username+"���߳ɹ�!");
					return Message.COMMON_MES_RESP("SCUESS",Msg_Category.SD_ADMIN,ServiceKey.SD_KickUser_Query_Resp);
				}
				else
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_KickUser_Query","�û�"+username.ToString()+"��������ʧ��");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.SD_ADMIN,ServiceKey.SD_KickUser_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.SD_ADMIN, ServiceKey.SD_KickUser_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region lg.SDAPI_SDMemberInfoAPI_AccountLock+
		/// <summary>
		/// lg.SDAPI_SDMemberInfoAPI_AccountLock+
		/// </summary>
		/// <returns></returns>
		public Message SD_BanUser_Ban()
		{
			string serverIP = null;
			//int uid = 0;
			int result = -1;
			string strnick = null;
			string username = null;
			string content = null;
			string serverName = null;
			int UserByID = 0;
			DateTime EndTime;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				serverName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerName).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				content = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_Content).m_bValueBuffer);

				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();
				//GM ����Ա
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();

				strut = new TLV_Structure(TagName.SD_EndTime, 6, msg.m_packet.m_Body.getTLVByTag(TagName.SD_EndTime).m_bValueBuffer);
				EndTime  =strut.toTimeStamp();

				strnick = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.f_pilot).m_bValueBuffer);
				byte[] bytNick = System.Text.Encoding.Unicode.GetBytes(strnick);

				int servergroup = int.Parse(CommonInfo.SD_GameDBInfo_Query(serverIP)[0].ToString());
				result = ManageKickOut(servergroup,10,bytNick);
				if(result ==0)
				{
					result = SDLogDataInfo.BanUser_Ban(serverIP,serverName,UserByID,userid,username,content,EndTime);
					if(result ==1)
					{
						SqlHelper.log.WriteLog("��ͣ--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ͣlg.SDAPI_SDItemLogInfoAPI_Account+"+username+"��"+EndTime.ToString()+"���ɹ�!");
						Console.WriteLine(DateTime.Now+" - ��ͣ--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ͣlg.SDAPI_SDItemLogInfoAPI_Account+"+username+"��"+EndTime.ToString()+"���ɹ�!");
						return Message.COMMON_MES_RESP("SCUESS",Msg_Category.SD_ADMIN,ServiceKey.SD_BanUser_Ban_Resp);
					}
					else
					{
						return Message.COMMON_MES_RESP("FAILURE",Msg_Category.SD_ADMIN,ServiceKey.SD_BanUser_Ban_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
					}
				}
				else
				{
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.SD_ADMIN,ServiceKey.SD_BanUser_Ban_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.SD_ADMIN, ServiceKey.SD_BanUser_Ban_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region ����û�
		/// <summary>
		/// ����û�
		/// </summary>
		/// <returns></returns>
		public Message SD_BanUser_UnBan()
		{
			string serverIP = null;
			//int uid = 0;
			int result = 0;
			string strnick = null;
			string username = null;
			string content = null;
			string serverName = null;
			int UserByID = 0;
			DateTime EndTime;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				serverName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerName).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				content = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_Content).m_bValueBuffer);

				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();
				//GM ����Ա
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();

				result = SDLogDataInfo.BanUser_UnBan(serverIP,serverName,UserByID,userid,username);
				if(result ==1)
				{
					SqlHelper.log.WriteLog("���--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����û�"+username+"�ɹ�!");
					Console.WriteLine(DateTime.Now+" - ���--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����û�"+username+"�ɹ�!");
					return Message.COMMON_MES_RESP("SCUESS",Msg_Category.SD_ADMIN,ServiceKey.SD_BanUser_UnBan_Resp);
				}
				else
				{
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.SD_ADMIN,ServiceKey.SD_BanUser_UnBan_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.SD_ADMIN, ServiceKey.SD_BanUser_UnBan_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region lg.SDAPI_SDMemberInfoAPI_AccountLock+lg.SDAPI_SDItemShopAPI_Integral+
		/// <summary>
		/// lg.SDAPI_SDMemberInfoAPI_AccountLock+lg.SDAPI_SDItemShopAPI_Integral+
		/// </summary>
		/// <returns></returns>
		public Message SD_BanUser_Query(int index,int pageSize)
		{
			string serverIP = null;
			//int uid = 0;
			DataSet ds = null;
			DateTime EndTime;
			string serverName = null;
			string userID = null;
			int type = -1;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				serverName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerName).m_bValueBuffer);

				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_Type).m_bValueBuffer);
				type =(int)strut.toInteger();
				if(type==1)
					userID = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				else
					userID = "";
				SqlHelper.log.WriteLog(lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+lg.SDAPI_SDMemberInfoAPI_AccountLock+lg.SDAPI_SDChallengeDataAPI_ProbabilityList);
				Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+lg.SDAPI_SDMemberInfoAPI_AccountLock+lg.SDAPI_SDChallengeDataAPI_ProbabilityList);
				ds = SDLogDataInfo.BanUser_Query(serverIP,serverName,type,userID);		
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,index,pageSize,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.SD_ADMIN,ServiceKey.SD_BanUser_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDMemberInfoAPI_AccountLock+lg.SDAPI_SDChallengeDataAPI_ProbabilityList,Msg_Category.SD_ADMIN,ServiceKey.SD_BanUser_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDMemberInfoAPI_AccountLock+lg.SDAPI_SDChallengeDataAPI_ProbabilityList, Msg_Category.SD_ADMIN, ServiceKey.SD_BanUser_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion
	
		#region �޸�lg.SDAPI_SDItemMsG6+
		/// <summary>
		/// �޸�lg.SDAPI_SDItemMsG6+
		/// </summary>
		/// <returns></returns>
		public Message SD_TmpPassWord_Query()
		{
			string serverIP = null;
			//int uid = 0;
			int result = 0;
			string strnick = null;
			string username = null;
			string content = null;
			string serverName = null;
			string TmpPWD = null;
			int UserByID = 0;
			DateTime EndTime;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				serverName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerName).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				TmpPWD = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_TmpPWD).m_bValueBuffer);

				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();
				//GM ����Ա
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();

				result = SDLogDataInfo.TmpPassWord_Query(serverIP,serverName,UserByID,userid,username,TmpPWD);
				if(result ==1)
				{
					SqlHelper.log.WriteLog("�޸�����--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�޸��û�"+username+"lg.SDAPI_SDItemMsG6+�ɹ�!");
					Console.WriteLine(DateTime.Now+" - �޸�����-SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�޸��û�"+username+"lg.SDAPI_SDItemMsG6+�ɹ�!");
					return Message.COMMON_MES_RESP("SCUESS",Msg_Category.SD_ADMIN,ServiceKey.SD_TmpPassWord_Query_Resp);
				}
				else if(result == 2)
				{
					return Message.COMMON_MES_RESP("�Ѿ��޸Ĺ�lg.SDAPI_SDItemMsG6+����ָ����޸ģ�",Msg_Category.SD_ADMIN,ServiceKey.SD_TmpPassWord_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				else
				{
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.SD_ADMIN,ServiceKey.SD_TmpPassWord_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.SD_ADMIN, ServiceKey.SD_TmpPassWord_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �ָ�lg.SDAPI_SDItemMsG6+
		/// <summary>
		/// �ָ�lg.SDAPI_SDItemMsG6+
		/// </summary>
		/// <returns></returns>
		public Message SD_ReTmpPassWord_Query()
		{
			string serverIP = null;
			//int uid = 0;
			int result = 0;
			string strnick = null;
			string username = null;
			string content = null;
			string serverName = null;
			string TmpPWD = null;
			int UserByID = 0;
			DateTime EndTime;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				serverName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerName).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);

				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();
				//GM ����Ա
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();

				result = SDLogDataInfo.ReTmpPassWord_Query(serverIP,serverName,UserByID,userid,username);
				if(result ==1)
				{
					SqlHelper.log.WriteLog("�ָ�����--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�ָ��û�"+username+"lg.SDAPI_SDItemMsG6+�ɹ�!");
					Console.WriteLine(DateTime.Now+" - �ָ�����--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�ָ��û�"+username+"lg.SDAPI_SDItemMsG6+�ɹ�!");
					return Message.COMMON_MES_RESP("SCUESS",Msg_Category.SD_ADMIN,ServiceKey.SD_ReTmpPassWord_Query_Resp);
				}
				else if(result ==2)
				{
					return Message.COMMON_MES_RESP("�û�û������lg.SDAPI_SDItemMsG6+",Msg_Category.SD_ADMIN,ServiceKey.SD_ReTmpPassWord_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				else
				{
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.SD_ADMIN,ServiceKey.SD_ReTmpPassWord_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.SD_ADMIN, ServiceKey.SD_ReTmpPassWord_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region lg.SDAPI_SDItemShopAPI_Integral+���һ��lg.SDAPI_SDItemMsG6+
		/// <summary>
		/// lg.SDAPI_SDItemShopAPI_Integral+���һ��lg.SDAPI_SDItemMsG6+
		/// </summary>
		/// <returns></returns>
		public Message SD_SearchPassWord_Query()
		{
			string serverIP = null;
			//int uid = 0;
			DataSet ds = null;
			string username = null;
			string serverName = null;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				serverName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerName).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();

				SqlHelper.log.WriteLog("���SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"lg.SDAPI_SDItemShopAPI_Integral+���һ��lg.SDAPI_SDItemMsG6+!");
				Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"lg.SDAPI_SDItemShopAPI_Integral+���һ��lg.SDAPI_SDItemMsG6+!");
				ds = SDLogDataInfo.SearchPassWord_Query(serverIP,serverName,userid,username);		
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,0,ds.Tables[0].Rows.Count,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.SD_ADMIN,ServiceKey.SD_SearchPassWord_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDItemMsG6,Msg_Category.SD_ADMIN,ServiceKey.SD_SearchPassWord_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDItemMsG6, Msg_Category.SD_ADMIN, ServiceKey.SD_SearchPassWord_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �޸�lg.SDAPI_SDItemLogInfoAPI_Account+�ȼ�
		/// <summary>
		/// �޸�lg.SDAPI_SDItemLogInfoAPI_Account+�ȼ�
		/// </summary>
		/// <returns></returns>
		public Message SD_UpdateExp_Query()
		{
			string serverIP = null;
			//int uid = 0;
			int result = 0;
			string strnick = null;
			string username = null;
			string content = null;
			string serverName = null;
			string TmpPWD = null;
			int UserByID = 0;
			int level = 0;
			DateTime EndTime;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);

				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();
				//GM ����Ա
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();

				//LEVEL
				strut = new TLV_Structure(TagName.f_level,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_level).m_bValueBuffer);
				level =(int)strut.toInteger();


				result = SDLogDataInfo.UpdateExp_Query(serverIP,UserByID,userid,username,level);
				if(result ==1)
				{
					SqlHelper.log.WriteLog("�޸ĵȼ�--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�޸��û�"+username+"�ȼ�Ϊ"+level.ToString()+"���ɹ�!");
					Console.WriteLine(DateTime.Now+" - �޸ĵȼ�--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�޸��û�"+username+"�ȼ�Ϊ"+level.ToString()+"���ɹ�!");
					return Message.COMMON_MES_RESP("SCUESS",Msg_Category.SD_ADMIN,ServiceKey.SD_UpdateExp_Query_Resp);
				}
				else
				{
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.SD_ADMIN,ServiceKey.SD_UpdateExp_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.SD_ADMIN, ServiceKey.SD_UpdateExp_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �޸�lg.SDAPI_SDItemLogInfoAPI_Account+����ȼ�
		/// <summary>
		/// �޸�lg.SDAPI_SDItemLogInfoAPI_Account+����ȼ�
		/// </summary>
		/// <returns></returns>
		public Message SD_UpdateUnitsExp_Query()
		{
			string serverIP = null;
			//int uid = 0;
			int result = 0;
			string strnick = null;
			string username = null;
			string content = null;
			string serverName = null;
			string TmpPWD = null;
			int UserByID = 0;
			int level = 0;
			int CustomLvMax = 0;
			int UnitNumber = 0;
			DateTime EndTime;
			string str = null;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);

				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();
				//GM ����Ա
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();

				//LEVEL
				strut = new TLV_Structure(TagName.f_level,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_ItemID).m_bValueBuffer);
				UnitNumber =(int)strut.toInteger();


				//����ȼ�
				strut = new TLV_Structure(TagName.f_level,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_UnitLevelNumber).m_bValueBuffer);
				level =(int)strut.toInteger();

				//ǿ���ȼ�
				strut = new TLV_Structure(TagName.f_level,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_CustomLvMax).m_bValueBuffer);
				CustomLvMax =(int)strut.toInteger();

				str = SDLogDataInfo.UpdateUnitsExp_Query(serverIP,UserByID,userid,username,level,CustomLvMax,UnitNumber);
					SqlHelper.log.WriteLog("�޸Ļ���ȼ�--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�޸��û�"+username+"����ȼ�Ϊ"+level.ToString()+"���ɹ�!");
					Console.WriteLine(DateTime.Now+" - �޸Ļ���ȼ�--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�޸��û�"+username+"����ȼ�Ϊ"+level.ToString()+"���ɹ�!");
					return Message.COMMON_MES_RESP(str,Msg_Category.SD_ADMIN,ServiceKey.SD_UpdateUnitsExp_Query_Resp);
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.SD_ADMIN, ServiceKey.SD_UpdateUnitsExp_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region ��ӵ���
		/// <summary>
		/// ��ӵ���
		/// </summary>
		/// <returns></returns>
		public Message SD_UserAdditem_Add()
		{
			string serverIP = null;
			//int uid = 0;
			int result = 0;
			string strnick = null;
			string username = null;
			string content = null;
			string serverName = null;
			string ItemName = null;
			string sendUser = null;
			int UserByID = 0;
			DateTime EndTime;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);

				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();
				//GM ����Ա
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();
				//itemname
				ItemName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ItemName).m_bValueBuffer);

				sendUser = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_Title).m_bValueBuffer);
				content  = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_Content).m_bValueBuffer);
				strnick = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.f_pilot).m_bValueBuffer);
				result = SDLogDataInfo.UserAdditem_Add(serverIP,UserByID,userid,strnick,ItemName,sendUser,content);
				if(result !=-1)
				{
					SqlHelper.log.WriteLog("��ӵ���--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ӵ��ߣ�"+ItemName.ToString()+"���ɹ�!");
					Console.WriteLine(DateTime.Now+" - ��ӵ���--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ӵ��ߣ�"+ItemName.ToString()+"���ɹ�!");
					return Message.COMMON_MES_RESP("SCUESS",Msg_Category.SD_ADMIN,ServiceKey.SD_UserAdditem_Add_Resp);
				}
				else
				{
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.SD_ADMIN,ServiceKey.SD_UserAdditem_Add_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.SD_ADMIN, ServiceKey.SD_UserAdditem_Add_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region ��ӵ��ߣ�������
		/// <summary>
		/// ��ӵ��ߣ�������
		/// </summary>
		/// <returns></returns>
		public Message SD_UserAdditem_Add_All()
		{
			string serverIP = null;
			//int uid = 0;
			string result = null;
			string strnick = null;
			string username = null;
			string content = null;
			string serverName = null;
			string ItemName = null;
			int UserByID = 0;
			DateTime EndTime;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);

				//GM ����Ա
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();
				//itemname
				ItemName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ItemName).m_bValueBuffer);

				result = SDLogDataInfo.UserAdditem_Add_All(serverIP,UserByID,ItemName);
				SqlHelper.log.WriteLog("��ӵ���--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ӵ���!");
				Console.WriteLine(DateTime.Now+" - ��ӵ���--SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ӵ���!");
				Console.WriteLine(result);
				return Message.COMMON_MES_RESP("SCUESS",Msg_Category.SD_ADMIN,ServiceKey.SD_UserAdditem_Add_All_Resp);
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.SD_ADMIN, ServiceKey.SD_UserAdditem_Add_All_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion
		
		#region ���lg.SDAPI_SDItemMsG7+
		/// <summary>
		/// ���lg.SDAPI_SDItemMsG7+
		/// </summary>
		/// <returns></returns>
		public Message SD_GetItemList_Query()
		{
			string serverIP = null;
			//int uid = 0;
			DataSet ds = null;
			int type = 0;
			string username = null;
			string serverName = null;
			string ItemName = "";
			int userid = 0;
			try
			{
				TLV_Structure tlvStrut = new TLV_Structure(TagName.UserByID,3,msg.m_packet.m_Body.getTLVByTag(TagName.SD_Type).m_bValueBuffer);
				type =(int)tlvStrut.toInteger();

				if(type==1)
				{
					ItemName = "";
				}
				else
				{
					ItemName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ItemName).m_bValueBuffer);
				}
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);

				SqlHelper.log.WriteLog("���SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���lg.SDAPI_SDItemMsG7+!");
				Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"���lg.SDAPI_SDItemMsG7+!");
				ds = SDLogDataInfo.GetItemList_Query(serverIP,type,ItemName);		
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,0,ds.Tables[0].Rows.Count,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.SD_ADMIN,ServiceKey.SD_ItemList_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDItemMsG7,Msg_Category.SD_ADMIN,ServiceKey.SD_ItemList_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDItemMsG7, Msg_Category.SD_ADMIN, ServiceKey.SD_ItemList_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region ������������Ϣlg.SDAPI_SDItemShopAPI_Integral+
		/// <summary>
		/// ������������Ϣlg.SDAPI_SDItemShopAPI_Integral+
		/// </summary>
		/// <returns></returns>
		public Message SD_Grift_FromUser_Query(int index,int pageSize)
		{
			string serverIP = null;
			//int uid = 0;
			DataSet ds = null;
			string  username = null;
			int  fromidx = 0;
			int  toidx = 0;
			DateTime Time;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);

				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_FromUser).m_bValueBuffer);
				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.SD_FromIdx,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_FromIdx).m_bValueBuffer);
				fromidx =(int)strut.toInteger();

				//�û�
				strut = new TLV_Structure(TagName.SD_ToIdx,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_ToIdx).m_bValueBuffer);
				toidx =(int)strut.toInteger();

				strut = new TLV_Structure(TagName.SD_SendTime, 6, msg.m_packet.m_Body.getTLVByTag(TagName.SD_SendTime).m_bValueBuffer);
				Time  =strut.toTimeStamp();
				
				SqlHelper.log.WriteLog(lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"������"+username+"������Ϣlg.SDAPI_SDItemShopAPI_Integral+!");
				Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"������"+username+"������Ϣlg.SDAPI_SDItemShopAPI_Integral+!");
				ds = SDLogDataInfo.Grift_FromUser_Query(serverIP,toidx,fromidx,Time);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,index,pageSize,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.SD_ADMIN,ServiceKey.SD_Grift_FromUser_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemShopAPI_Integral,Msg_Category.SD_ADMIN,ServiceKey.SD_Grift_FromUser_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemShopAPI_Integral, Msg_Category.SD_ADMIN, ServiceKey.SD_Grift_FromUser_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region ������������Ϣlg.SDAPI_SDItemShopAPI_Integral+
		/// <summary>
		/// ������������Ϣlg.SDAPI_SDItemShopAPI_Integral+
		/// </summary>
		/// <returns></returns>
		public Message SD_Grift_ToUser_Query(int index,int pageSize)
		{
			string serverIP = null;
			//int uid = 0;
			DataSet ds = null;
			int s_id = -1;
			string  username = null;
			string  item = null;
			int  toidx = 0;
			DateTime Time;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ToUser).m_bValueBuffer);
				//�û�
				item = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ItemName).m_bValueBuffer);

				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.SD_ToIdx,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_ToIdx).m_bValueBuffer);
				toidx =(int)strut.toInteger();

				//����ID
				strut = new TLV_Structure(TagName.SD_ToIdx,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_ID).m_bValueBuffer);
				s_id =(int)strut.toInteger();

				strut = new TLV_Structure(TagName.SD_SendTime, 6, msg.m_packet.m_Body.getTLVByTag(TagName.SD_SendTime).m_bValueBuffer);
				Time  =strut.toTimeStamp();
				
				SqlHelper.log.WriteLog(lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"������"+username+"������Ϣlg.SDAPI_SDItemShopAPI_Integral+!");
				Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"������"+username+"������Ϣlg.SDAPI_SDItemShopAPI_Integral+!");
				ds = SDLogDataInfo.Grift_ToUser_Query(serverIP,toidx,item,Time,s_id);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,index,pageSize,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.SD_ADMIN,ServiceKey.SD_Grift_ToUser_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemShopAPI_Integral,Msg_Category.SD_ADMIN,ServiceKey.SD_Grift_ToUser_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemShopAPI_Integral, Msg_Category.SD_ADMIN, ServiceKey.SD_Grift_ToUser_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion
		
		#region ɾ������
		/// <summary>
		/// ɾ������
		/// </summary>
		/// <returns></returns>
		public Message SD_UserAdditem_Del()
		{
			string serverIP = null;
			//int uid = 0;
			int result = -1;
			int type=0;
			string strnick = null;
			string username = null;
			string content = null;
			string serverName = null;
			string ItemName = null;
			int UserByID = 0;
			int itemID = 0;
			DateTime EndTime;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);

				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();
				//GM ����Ա
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();

				//itemid
				strut = new TLV_Structure(TagName.SD_ItemID,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_ID).m_bValueBuffer);
				itemID =(int)strut.toLong();

				//itemid
				strut = new TLV_Structure(TagName.SD_ItemID,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_Type).m_bValueBuffer);
				type =(int)strut.toInteger();

				//itemname
				ItemName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ItemName).m_bValueBuffer);

				result = SDLogDataInfo.UserAdditem_Del(serverIP,UserByID,userid,username,itemID,ItemName,type);
				if(result ==0)
				{
					SqlHelper.log.WriteLog(lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"ɾ�����ߣ�"+ItemName.ToString()+"���ɹ�!");
					Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"ɾ�����ߣ�"+ItemName.ToString()+"���ɹ�!");
					return Message.COMMON_MES_RESP("SCUESS",Msg_Category.SD_ADMIN,ServiceKey.SD_UserAdditem_Del_Resp);
				}
				else
				{
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.SD_ADMIN,ServiceKey.SD_UserAdditem_Del_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.SD_ADMIN, ServiceKey.SD_UserAdditem_Del_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region ���͹���
		public Message SD_SendNotes_Query()
		{
			string serverIP = null;
			string gsseverIP = null;
			string boardMessage = null;
			DateTime beginTime;
			DateTime endTime;
			int userbyID = 0;
			int interval = 0;
			int noticeType = 0;
			int Type = 0;
			int result = -1;
			try
			{
				serverIP = System.Text.Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				//����ԱID
				TLV_Structure tlvStrut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				userbyID =(int)tlvStrut.toInteger();
				tlvStrut = new TLV_Structure(TagName.AU_BeginTime,6,msg.m_packet.m_Body.getTLVByTag(TagName.SD_StartTime).m_bValueBuffer);
				beginTime =tlvStrut.toTimeStamp();
				tlvStrut = new TLV_Structure(TagName.AU_EndTime,6,msg.m_packet.m_Body.getTLVByTag(TagName.SD_EndTime).m_bValueBuffer);
				endTime =tlvStrut.toTimeStamp();
				boardMessage = System.Text.Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_Content).m_bValueBuffer);
				//���ͼ��
				tlvStrut = new TLV_Structure(TagName.AU_Interval,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_Limit).m_bValueBuffer);
				interval =(int)tlvStrut.toInteger();

				//��������
				tlvStrut = new TLV_Structure(TagName.AU_Interval,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_Type).m_bValueBuffer);
				noticeType =(int)tlvStrut.toInteger();

				//��������
				tlvStrut = new TLV_Structure(TagName.AU_Interval,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_SendType).m_bValueBuffer);
				Type =(int)tlvStrut.toInteger();
				
				result = SDLogDataInfo.BoardTask_Insert(userbyID,serverIP,boardMessage,beginTime,endTime,interval,noticeType,Type);
				if(result ==1)
				{
					SqlHelper.log.WriteLog("���SD�Ҵ�+>����Ϊ"+boardMessage.ToString()+"�Ĺ���ɹ�!");
					Console.WriteLine(DateTime.Now+" - ���SD�Ҵ�+>����Ϊ"+boardMessage.ToString()+"�Ĺ���ɹ�!");
					return Message.COMMON_MES_RESP("SCUESS",Msg_Category.SD_ADMIN,ServiceKey.SD_SendNotes_Query_Resp);
				}
				else
				{
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.SD_ADMIN,ServiceKey.SD_SendNotes_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("���ʧ��",Msg_Category.SD_ADMIN,ServiceKey.SD_SendNotes_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}
            
		}
		#endregion

		#region lg.SDAPI_SDItemShopAPI_Integral+����
		public Message SD_SeacrhNotes_Query()
		{
			System.Data.DataSet ds = null;
			try
			{
				SqlHelper.log.WriteLog("���SD�Ҵ�+>lg.SDAPI_SDItemShopAPI_Integral+�����б�!");
				Console.WriteLine(DateTime.Now+" - ���SD�Ҵ�+>lg.SDAPI_SDItemShopAPI_Integral+�����б�");
				//�����ݿ����潫Ƶ���б������
				ds = SDLogDataInfo.BoardTask_Query();
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,0,ds.Tables[0].Rows.Count,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.SD_ADMIN,ServiceKey.SD_BanUser_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_ProbabilityList,Msg_Category.SD_ADMIN,ServiceKey.SD_BanUser_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+"SD_SeacrhNotes_Query"+ex.Message);
				return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_ProbabilityList, Msg_Category.SD_ADMIN, ServiceKey.SD_BanUser_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
            
		}
		#endregion 

		#region �޸Ĺ���
		public Message SD_TaskList_Update()
		{
			string serverIP = "";
			int userbyID = 0;
			int taskID = 0;
			DateTime beginTime = DateTime.Now;
			DateTime endTime = DateTime.Now;
			int interval = 0;
			int status = 0;
			int noticeType = 0;
			string boardMessage = "";
			int result = -1;
			try
			{
				//����ԱID
				TLV_Structure tlvStrut = new TLV_Structure(TagName.UserByID,3,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				userbyID =(int)tlvStrut.toInteger();
				//����״̬
				tlvStrut = new TLV_Structure(TagName.AU_Status,3,msg.m_packet.m_Body.getTLVByTag(TagName.SD_Status).m_bValueBuffer);
				status =(int)tlvStrut.toInteger();
				//����ID
				tlvStrut = new TLV_Structure(TagName.AU_TaskID,3,msg.m_packet.m_Body.getTLVByTag(TagName.SD_ID).m_bValueBuffer);
				taskID =(int)tlvStrut.toInteger();

				boardMessage  = System.Text.Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_Content).m_bValueBuffer);
				if(status==0)
				{
					tlvStrut = new TLV_Structure(TagName.AU_BeginTime,6,msg.m_packet.m_Body.getTLVByTag(TagName.SD_StartTime).m_bValueBuffer);
					beginTime =tlvStrut.toTimeStamp();

					tlvStrut = new TLV_Structure(TagName.AU_EndTime,6,msg.m_packet.m_Body.getTLVByTag(TagName.SD_EndTime).m_bValueBuffer);
					endTime =tlvStrut.toTimeStamp();

					//���ͼ��
					tlvStrut = new TLV_Structure(TagName.AU_Interval,3,msg.m_packet.m_Body.getTLVByTag(TagName.SD_Limit).m_bValueBuffer);
					interval =(int)tlvStrut.toInteger();

					boardMessage  = System.Text.Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_Content).m_bValueBuffer);

					//��������
					tlvStrut = new TLV_Structure(TagName.AU_TaskID,3,msg.m_packet.m_Body.getTLVByTag(TagName.SD_Type).m_bValueBuffer);
					noticeType =(int)tlvStrut.toInteger();
				}
				

				result = SDLogDataInfo.BoardTask_Update(serverIP,userbyID,taskID,beginTime,endTime,interval,noticeType,status,boardMessage);
				if(result ==1)
				{
					SqlHelper.log.WriteLog("����SD�Ҵ�+>IDΪ"+taskID+"�Ĺ���ɹ�!");
					Console.WriteLine(DateTime.Now+" - ����SD�Ҵ�+>IDΪ"+taskID+"�Ĺ���ɹ�!");
					return Message.COMMON_MES_RESP("SCUESS",Msg_Category.SD_ADMIN,ServiceKey.SD_TaskList_Update_Resp);
				}
				else
				{
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.SD_ADMIN,ServiceKey.SD_TaskList_Update_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��",Msg_Category.SD_ADMIN,ServiceKey.SD_TaskList_Update_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region lg.SDAPI_SDItemShopAPI_Integral+lg.SDAPI_SDItemLogInfoAPI_Account+�����¼
		/// <summary>
		/// lg.SDAPI_SDItemShopAPI_Integral+lg.SDAPI_SDItemLogInfoAPI_Account+�����¼
		/// </summary>
		/// <returns></returns>
		public Message SD_BuyLog_Query(int index,int pageSize)
		{
			string serverIP = null;
			//int uid = 0;
			DataSet ds = null;
			string strnick = null;
			string username = null;
			DateTime BeginTime ;
			DateTime EndTime ;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();

				strut = new TLV_Structure(TagName.SD_StartTime, 6, msg.m_packet.m_Body.getTLVByTag(TagName.SD_StartTime).m_bValueBuffer);
				BeginTime  =strut.toTimeStamp();

				strut = new TLV_Structure(TagName.SD_EndTime, 6, msg.m_packet.m_Body.getTLVByTag(TagName.SD_EndTime).m_bValueBuffer);
				EndTime  =strut.toTimeStamp();


				
				SqlHelper.log.WriteLog(lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+lg.SDAPI_SDItemLogInfoAPI_Account+username+"�����¼lg.SDAPI_SDItemShopAPI_Integral+!");
				Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+lg.SDAPI_SDItemLogInfoAPI_Account+username+"�����¼lg.SDAPI_SDItemShopAPI_Integral+!");
				ds = SDLogDataInfo.BuyLog_Query(serverIP,userid,BeginTime,EndTime);		
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,index,pageSize,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.SD_ADMIN,ServiceKey.SD_BuyLog_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemLogInfoAPI_Account+lg.SDAPI_SDItemShopAPI_Integral,Msg_Category.SD_ADMIN,ServiceKey.SD_BuyLog_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemLogInfoAPI_Account+lg.SDAPI_SDItemShopAPI_Integral, Msg_Category.SD_ADMIN, ServiceKey.SD_BuyLog_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region lg.SDAPI_SDItemShopAPI_Integral+����ɾ����¼
		/// <summary>
		/// lg.SDAPI_SDItemShopAPI_Integral+����ɾ����¼
		/// </summary>
		/// <returns></returns>
		public Message SD_Delete_ItemLog_Query(int index,int pageSize)
		{
			string serverIP = null;
			//int uid = 0;
			DataSet ds = null;
			string strnick = null;
			string username = null;
			DateTime BeginTime ;
			DateTime EndTime ;
			int type = 0;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();
				//��������
				strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_Type).m_bValueBuffer);
				type =(int)strut.toInteger();

				strut = new TLV_Structure(TagName.SD_StartTime, 6, msg.m_packet.m_Body.getTLVByTag(TagName.SD_StartTime).m_bValueBuffer);
				BeginTime  =strut.toTimeStamp();

				strut = new TLV_Structure(TagName.SD_EndTime, 6, msg.m_packet.m_Body.getTLVByTag(TagName.SD_EndTime).m_bValueBuffer);
				EndTime  =strut.toTimeStamp();

				
				SqlHelper.log.WriteLog(lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+lg.SDAPI_SDItemLogInfoAPI_Account+username+"����ɾ����¼!");
				Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+lg.SDAPI_SDItemLogInfoAPI_Account+username+"����ɾ����¼!");
				ds = SDLogDataInfo.Delete_ItemLog_Query(serverIP,userid,type,BeginTime,EndTime);		
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,index,pageSize,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.SD_ADMIN,ServiceKey.SD_Delete_ItemLog_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemLogInfoAPI_Account+"����ɾ����¼",Msg_Category.SD_ADMIN,ServiceKey.SD_Delete_ItemLog_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemLogInfoAPI_Account+"����ɾ����¼", Msg_Category.SD_ADMIN, ServiceKey.SD_Delete_ItemLog_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region lg.SDAPI_SDItemShopAPI_Integral+lg.SDAPI_SDItemLogInfoAPI_Account+��־��Ϣ
		/// <summary>
		/// lg.SDAPI_SDItemShopAPI_Integral+lg.SDAPI_SDItemLogInfoAPI_Account+��־��Ϣ
		/// </summary>
		/// <returns></returns>
		public Message SD_LogInfo_Query(int index,int pageSize)
		{
			string serverIP = null;
			//int uid = 0;
			DataSet ds = null;
			string strnick = null;
			string username = null;
			DateTime BeginTime ;
			DateTime EndTime ;
			int type = 0;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();
				//��������
				strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_Type).m_bValueBuffer);
				type =(int)strut.toInteger();

				strut = new TLV_Structure(TagName.SD_StartTime, 6, msg.m_packet.m_Body.getTLVByTag(TagName.SD_StartTime).m_bValueBuffer);
				BeginTime  =strut.toTimeStamp();

				strut = new TLV_Structure(TagName.SD_EndTime, 6, msg.m_packet.m_Body.getTLVByTag(TagName.SD_EndTime).m_bValueBuffer);
				EndTime  =strut.toTimeStamp();

				
				SqlHelper.log.WriteLog(lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+lg.SDAPI_SDItemLogInfoAPI_Account+username+"��־��¼lg.SDAPI_SDItemShopAPI_Integral+!");
				Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+lg.SDAPI_SDItemLogInfoAPI_Account+username+"��־��¼lg.SDAPI_SDItemShopAPI_Integral+!");
				ds = SDLogDataInfo.LogInfo_Query(serverIP,userid,type,BeginTime,EndTime);		
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,index,pageSize,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.SD_ADMIN,ServiceKey.SD_LogInfo_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemLogInfoAPI_Account+lg.SDAPI_SDItemShopAPI_Integral,Msg_Category.SD_ADMIN,ServiceKey.SD_LogInfo_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+lg.SDAPI_SDChallengeDataAPI_NoChallengeScene+lg.SDAPI_SDItemLogInfoAPI_Account+lg.SDAPI_SDItemShopAPI_Integral, Msg_Category.SD_ADMIN, ServiceKey.SD_LogInfo_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region ���GM�˺�
		/// <summary>
		/// ���GM�˺�
		/// </summary>
		/// <returns></returns>
		public Message SD_GetGmAccount_Query()
		{
			string serverIP = null;
			//int uid = 0;
			DataSet ds = null;
			int type = 0;
			string username = null;
			string serverName = null;
			string userName = "";
			int userid = 0;
			try
			{
				TLV_Structure tlvStrut = new TLV_Structure(TagName.UserByID,3,msg.m_packet.m_Body.getTLVByTag(TagName.SD_Type).m_bValueBuffer);
				type =(int)tlvStrut.toInteger();

				if(type==1)
				{
					userName = "";
				}
				else
				{
					userName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				}
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);

				SqlHelper.log.WriteLog("���SD�ߴ�+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���GM�˺��б�!");
				Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"���GM�˺��б�!");
				ds = SDLogDataInfo.GetGmAccount_Query(serverIP,type,userName);		
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,0,ds.Tables[0].Rows.Count,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.SD_ADMIN,ServiceKey.SD_GetGmAccount_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+"GM�˺��б�",Msg_Category.SD_ADMIN,ServiceKey.SD_GetGmAccount_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP(lg.SDAPI_SDItemMsG5+"GM�˺��б�", Msg_Category.SD_ADMIN, ServiceKey.SD_GetGmAccount_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �޸�GM�˺�
		/// <summary>
		/// �޸�GM�˺�
		/// </summary>
		/// <returns></returns>
		public Message SD_UpdateGmAccount_Query()
		{
			string serverIP = null;
			//int uid = 0;
			int result = -1;
			int type=0;
			string strnick = null;
			string username = null;
			string oldUserName = null;
			string content = null;
			string serverName = null;
			string passWd = null;
			int UserByID = 0;
			int itemID = 0;
			string pilotName = null;
			DateTime EndTime;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				oldUserName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName_Old).m_bValueBuffer);
				pilotName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.f_pilot).m_bValueBuffer);

				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();

				//GM ����Ա
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();

				//itemname
				passWd = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_passWd).m_bValueBuffer);

				result = SDLogDataInfo.UpdateGmAccount_Query(serverIP,UserByID,userid,username,passWd,oldUserName,pilotName);
				if(result ==1)
				{
					SqlHelper.log.WriteLog(lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"�޸�GM�˺ţ��ɹ�!");
					Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"�޸�GM�˺ţ��ɹ�!");
					return Message.COMMON_MES_RESP("SCUESS",Msg_Category.SD_ADMIN,ServiceKey.SD_UpdateGmAccount_Query_Resp);
				}
				
				else
				{
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.SD_ADMIN,ServiceKey.SD_UpdateGmAccount_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.SD_ADMIN, ServiceKey.SD_UpdateGmAccount_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �޸Ľ�Ǯ
		/// <summary>
		/// �޸Ľ�Ǯ
		/// </summary>
		/// <returns></returns>
		public Message SD_UpdateMoney_Query()
		{
			string serverIP = null;
			//int uid = 0;
			int result = -1;
			int type=0;
			string strnick = null;
			string username = null;
			int Money = 0;
			int oldMoney = 0;
			string content = null;
			string serverName = null;
			string passWd = null;
			int UserByID = 0;
			int itemID = 0;
			DateTime EndTime;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				

				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();

				//GM ����Ա
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();

				//Ǯ���޸�֮ǰ��
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_GC).m_bValueBuffer);
				Money =(int)strut.toInteger();

				//Ǯ���޸�֮��
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_Money_Old).m_bValueBuffer);
				oldMoney =(int)strut.toInteger();


				result = SDLogDataInfo.UpdateMoney_Query(serverIP,UserByID,userid,username,Money,oldMoney);
				if(result ==1)
				{
					SqlHelper.log.WriteLog(lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"�޸��˺Ž�Ǯ���ɹ�!");
					Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+"�޸��˺Ž�Ǯ���ɹ�!");
					return Message.COMMON_MES_RESP("SCUESS",Msg_Category.SD_ADMIN,ServiceKey.SD_UpdateMoney_Query_Resp);
				}
				else
				{
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.SD_ADMIN,ServiceKey.SD_UpdateMoney_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.SD_ADMIN, ServiceKey.SD_UpdateMoney_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion
		
		#region �ָ�����
		/// <summary>
		/// �ָ�����
		/// </summary>
		/// <returns></returns>
		public Message SD_ReGetUnits_Query()
		{
			string serverIP = null;
			//int uid = 0;
			string result = null;
			string username = null;
			string beginDate = null;
			int UserByID = 0;
			int SDID = 0;
			int itemID = 0;
			string itemName = null;
			int userid = 0;
			try
			{
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ServerIP).m_bValueBuffer);
				username = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_UserName).m_bValueBuffer);
				itemName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.SD_ItemName).m_bValueBuffer);
				beginDate = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.BeginTime).m_bValueBuffer);

				//�û�
				TLV_Structure strut = new TLV_Structure(TagName.f_idx,4,msg.m_packet.m_Body.getTLVByTag(TagName.f_idx).m_bValueBuffer);
				userid =(int)strut.toInteger();

				//GM ����Ա
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();

				//���к�
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_ID).m_bValueBuffer);
				SDID =(int)strut.toInteger();

				//SD ����ID
				strut = new TLV_Structure(TagName.SD_ID,4,msg.m_packet.m_Body.getTLVByTag(TagName.SD_ItemID).m_bValueBuffer);
				itemID =(int)strut.toInteger();
				
				result = SDLogDataInfo.ReGetUnits_Query(serverIP,UserByID,userid,username,SDID,itemID,itemName,beginDate);
				SqlHelper.log.WriteLog(lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+result);
				Console.WriteLine(DateTime.Now+" - "+lg.SDAPI_SDItemMsG+lg.SDAPI_SD+"+>"+lg.SDAPI_SDItemMsG1+CommonInfo.serverIP_Query(serverIP)+result);
				return Message.COMMON_MES_RESP(result,Msg_Category.SD_ADMIN,ServiceKey.SD_ReGetUnits_Query_Resp);
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.SDAPI_SDItemMsG1+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.SD_ADMIN, ServiceKey.SD_ReGetUnits_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

	}
}
