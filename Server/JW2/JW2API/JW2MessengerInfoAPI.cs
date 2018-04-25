using System;
using System.Data;
using System.Text;
using GM_Server.JW2DataInfo;
using Common.Logic;
using Common.DataInfo;
using Domino;
using Common.NotesDataInfo;

namespace GM_Server.JW2API
{
	/// <summary>
	/// JW2MessengerInfoAPI ��ժҪ˵����
	/// </summary>
	public class JW2MessengerInfoAPI
	{
		Message msg = null;
		public JW2MessengerInfoAPI()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		#region ���캯��
		public JW2MessengerInfoAPI(byte[] packet)
		{
			msg = new Message(packet,(uint)packet.Length);
		}
		#endregion

		#region �鿴��Ҽ�����Ϣ
		/// <summary>
		/// �鿴��Ҽ�����Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_User_Family_Query()
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int type =-1;
			int userSN = -1;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+userSN+"������Ϣ!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+userSN+"������Ϣ!");
				ds = JW2DataInfo.JW2MessengerDataInfo.User_Family_Query(serverIP,userSN);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,0,ds.Tables[0].Rows.Count,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_User_Family_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�и���Ҽ�����Ϣ",Msg_Category.JW2_ADMIN,ServiceKey.JW2_User_Family_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("�鿴��Ҽ�����Ϣ->JW2_User_Family_Query->������IP->"+serverIP+"->�û�->"+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP("û�и���Ҽ�����Ϣ", Msg_Category.JW2_ADMIN, ServiceKey.JW2_User_Family_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �鿴������Ϣ
		/// <summary>
		/// �鿴������Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_FAMILYINFO_QUERY(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int type =-1;
			string FamilyName = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				FamilyName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_FAMILYNAME).m_bValueBuffer);
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���壺"+FamilyName+"��Ϣ!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���壺"+FamilyName+"��Ϣ!");
				ds = JW2DataInfo.JW2MessengerDataInfo.FAMILYINFO_QUERY(serverIP,FamilyName);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_FAMILYINFO_QUERY_RESP,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�иü�����Ϣ",Msg_Category.JW2_ADMIN,ServiceKey.JW2_FAMILYINFO_QUERY_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("�鿴������Ϣ->JW2_FAMILYINFO_QUERY->������IP->"+serverIP+"->����->"+FamilyName+"->"+ex.Message);
				return Message.COMMON_MES_RESP("û�иü�����Ϣ", Msg_Category.JW2_ADMIN, ServiceKey.JW2_FAMILYINFO_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �鿴���������Ϣ
		/// <summary>
		/// �鿴���������Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_FamilyPet_Query()
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int type =-1;
			int FamilyID = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_FAMILYID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_FAMILYID).m_bValueBuffer);
				FamilyID =(int)strut.toInteger();
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��Ҽ���"+FamilyID+"������Ϣ!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��Ҽ���"+FamilyID+"������Ϣ!");
				ds = JW2DataInfo.JW2MessengerDataInfo.FamilyPet_Query(serverIP,FamilyID);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,0,ds.Tables[0].Rows.Count,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_FamilyPet_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�м��������Ϣ",Msg_Category.JW2_ADMIN,ServiceKey.JW2_FamilyPet_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("�鿴���������Ϣ->JW2_User_Family_Query->������IP->"+serverIP+"->����ID->"+FamilyID+"->"+ex.Message);
				return Message.COMMON_MES_RESP("û�иü��������Ϣ", Msg_Category.JW2_ADMIN, ServiceKey.JW2_FamilyPet_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �鿴������Ϣ
		/// <summary>
		/// �鿴������Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_BasicInfo_Query(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int type =-1;
			int FamilyNameID = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_FAMILYID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_FAMILYID).m_bValueBuffer);
				FamilyNameID =(int)strut.toInteger();
				
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyNameID+"��Ϣ!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyNameID+"��Ϣ!");
				ds = JW2DataInfo.JW2MessengerDataInfo.BasicInfo_Query(serverIP,FamilyNameID);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_BasicInfo_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�иü�����Ϣ",Msg_Category.JW2_ADMIN,ServiceKey.JW2_BasicInfo_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("�鿴������Ϣ->JW2_BasicInfo_Query->������IP->"+serverIP+"->����->"+FamilyNameID+"->"+ex.Message);
				return Message.COMMON_MES_RESP("û�иû�����Ϣ", Msg_Category.JW2_ADMIN, ServiceKey.JW2_BasicInfo_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �鿴�����Ա��Ϣ
		/// <summary>
		/// �鿴�����Ա��Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_FAMILYMEMBER_QUERY(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int FamilyID = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_FAMILYID).m_bValueBuffer);
				FamilyID =(int)strut.toInteger();
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyID+"��Ա��Ϣ!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyID+"��Ա��Ϣ!");
				ds = JW2DataInfo.JW2MessengerDataInfo.FAMILYMEMBER_QUERY(serverIP,FamilyID);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_FAMILYMEMBER_QUERY_RESP,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�иü����Ա��Ϣ",Msg_Category.JW2_ADMIN,ServiceKey.JW2_FAMILYMEMBER_QUERY_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("�鿴�����Ա��Ϣ->JW2_FAMILYMEMBER_QUERY->������IP->"+serverIP+"->����->"+FamilyID+"->"+ex.Message);
				return Message.COMMON_MES_RESP("û�иü����Ա��Ϣ", Msg_Category.JW2_ADMIN, ServiceKey.JW2_FAMILYMEMBER_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �鿴���������Ϣ
		/// <summary>
		/// �鿴���������Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_FamilyItemInfo_Query(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int FamilyID = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_FAMILYID).m_bValueBuffer);
				FamilyID =(int)strut.toInteger();
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyID+"������Ϣ!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyID+"������Ϣ!");
				ds = JW2DataInfo.JW2MessengerDataInfo.FamilyItemInfo_Query(serverIP,FamilyID);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_FamilyItemInfo_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�иü��������Ϣ",Msg_Category.JW2_ADMIN,ServiceKey.JW2_FamilyItemInfo_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("�鿴���������Ϣ->JW2_FamilyItemInfo_Query->������IP->"+serverIP+"->����->"+FamilyID+"->"+ex.Message);
				return Message.COMMON_MES_RESP("û�иü��������Ϣ", Msg_Category.JW2_ADMIN, ServiceKey.JW2_FamilyItemInfo_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion
		
		#region �鿴���������г�Ա��Ϣ
		/// <summary>
		/// �鿴���������г�Ա��Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_FamilyMember_Applying(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int FamilyID = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_FAMILYID).m_bValueBuffer);
				FamilyID =(int)strut.toInteger();
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyID+"�����г�Ա��Ϣ!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyID+"�����г�Ա��Ϣ!");
				ds = JW2DataInfo.JW2MessengerDataInfo.FamilyMember_Applying(serverIP,FamilyID);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_FamilyMember_Applying_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�иü��������г�Ա��Ϣ",Msg_Category.JW2_ADMIN,ServiceKey.JW2_FamilyMember_Applying_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("�鿴���������г�Ա��Ϣ->JW2_FamilyMember_Applying->������IP->"+serverIP+"->����->"+FamilyID+"->"+ex.Message);
				return Message.COMMON_MES_RESP("û�иü��������г�Ա��Ϣ", Msg_Category.JW2_ADMIN, ServiceKey.JW2_FamilyMember_Applying_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �鿴����������Ϣ
		/// <summary>
		/// �鿴����������Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_BasicBank_Query(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int FamilyID = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_FAMILYID).m_bValueBuffer);
				FamilyID =(int)strut.toInteger();
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyID+"������Ϣ!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyID+"������Ϣ!");
				ds = JW2DataInfo.JW2MessengerDataInfo.BasicRank_Query(serverIP,FamilyID);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_BasicBank_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�иü���������Ϣ",Msg_Category.JW2_ADMIN,ServiceKey.JW2_BasicBank_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("�鿴����������Ϣ->JW2_BasicBank_Query->������IP->"+serverIP+"->����->"+FamilyID+"->"+ex.Message);
				return Message.COMMON_MES_RESP("û�иü���������Ϣ", Msg_Category.JW2_ADMIN, ServiceKey.JW2_BasicBank_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �鿴���Messenger�ƺ���Ϣ
		/// <summary>
		/// �鿴���Messenger�ƺ���Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_Messenger_Query(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int userSN = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ң�"+userSN+"Messenger�ƺ���Ϣ!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ң�"+userSN+"Messenger�ƺ���Ϣ!");
				ds = JW2DataInfo.JW2MessengerDataInfo.Messenger_Query(serverIP,userSN);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_Messenger_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�и����Messenger�ƺ���Ϣ",Msg_Category.JW2_ADMIN,ServiceKey.JW2_Messenger_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("�鿴���Messenger�ƺ���Ϣ->JW2_Messenger_Query->������IP->"+serverIP+"->�û�->"+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP("û�и����Messenger�ƺ���Ϣ", Msg_Category.JW2_ADMIN, ServiceKey.JW2_Messenger_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �鿴������߹�����־
		/// <summary>
		/// �鿴������߹�����־
		/// </summary>
		/// <returns></returns>
		public Message JW2_FamilyBuyLog_Query(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			string BeginTime = "";
			string EndTime = "";
			int FamilyID = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_FAMILYID).m_bValueBuffer);
				FamilyID =(int)strut.toInteger();
				BeginTime = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.BeginTime).m_bValueBuffer);
				EndTime = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.EndTime).m_bValueBuffer);
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyID+"���߹�����־!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyID+"���߹�����־!");
				ds = JW2DataInfo.JW2MessengerDataInfo.FamilyBuyLog_Query(serverIP,FamilyID,BeginTime,EndTime);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_FamilyBuyLog_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�иü�����߹�����־",Msg_Category.JW2_ADMIN,ServiceKey.JW2_FamilyBuyLog_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("�鿴������߹�����־->JW2_FamilyBuyLog_Query->������IP->"+serverIP+"->����->"+FamilyID+"->��ʼʱ��->"+BeginTime+"->����ʱ��->"+EndTime+"->"+ex.Message);
				return Message.COMMON_MES_RESP("û�иü�����߹�����־", Msg_Category.JW2_ADMIN, ServiceKey.JW2_FamilyBuyLog_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region ������־(0,����,1,����)
		/// <summary>
		/// ������־(0,����,1,����)
		/// </summary>
		/// <returns></returns>
		public Message JW2_FamilyFund_Log(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			string BeginTime = "";
			string EndTime = "";
			int type = -1;
			int FamilyID = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_FAMILYID).m_bValueBuffer);
				FamilyID =(int)strut.toInteger();
				BeginTime = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.BeginTime).m_bValueBuffer);
				EndTime = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.EndTime).m_bValueBuffer);
				strut = new TLV_Structure(TagName.JW2_GOODSTYPE,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_GOODSTYPE).m_bValueBuffer);
				type =(int)strut.toInteger();
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyID+"��������־!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����ID��"+FamilyID+"��������־!");
				ds = JW2DataInfo.JW2MessengerDataInfo.FamilyFund_Log(serverIP,FamilyID,BeginTime,EndTime,type);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_FamilyFund_Log_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�иü����������־",Msg_Category.JW2_ADMIN,ServiceKey.JW2_FamilyFund_Log_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������־(0,����,1,����)->JW2_FamilyFund_Log->������IP->"+serverIP+"->����->"+FamilyID+"->��ʼʱ��->"+BeginTime+"->����ʱ��->"+EndTime+"->"+ex.Message);
				return Message.COMMON_MES_RESP("û�иü����������־", Msg_Category.JW2_ADMIN, ServiceKey.JW2_FamilyFund_Log_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �����Ա��ȡС���ﵰ��Ϣ��ѯ
		/// <summary>
		/// �����Ա��ȡС���ﵰ��Ϣ��ѯ
		/// </summary>
		/// <returns></returns>
		public Message JW2_SmallPetAgg_Query()
		{
			string serverIP = "";
			DataSet ds = null;
			string BeginTime = "";
			string EndTime = "";
			int userSN = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				BeginTime = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.BeginTime).m_bValueBuffer);
				EndTime = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.EndTime).m_bValueBuffer);
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�����Ա��"+userSN+"��ȡС���ﵰ��Ϣ��ѯ!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�����Ա��"+userSN+"��ȡС���ﵰ��Ϣ��ѯ!");
				ds = JW2DataInfo.JW2MessengerDataInfo.SmallPetAgg_Query(serverIP,userSN,BeginTime,EndTime);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,0,ds.Tables[0].Rows.Count,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_SmallPetAgg_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�иü����������־",Msg_Category.JW2_ADMIN,ServiceKey.JW2_SmallPetAgg_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("�����Ա��ȡС���ﵰ��Ϣ��ѯ->JW2_SmallPetAgg_Query->������IP->"+serverIP+"->�����Ա->"+userSN+"->��ʼʱ��->"+BeginTime+"->����ʱ��->"+EndTime+"->"+ex.Message);
				return Message.COMMON_MES_RESP("�����Ա��ȡС���ﵰ��Ϣ��ѯ", Msg_Category.JW2_ADMIN, ServiceKey.JW2_SmallPetAgg_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion
		
		#region ������־
		/// <summary>
		/// ������־
		/// </summary>
		/// <returns></returns>
		public Message JW2_Item_Log(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			string BeginTime = "";
			string EndTime = "";
			int userSN = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				BeginTime = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.BeginTime).m_bValueBuffer);
				EndTime = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.EndTime).m_bValueBuffer);
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�Ñ�ID��"+userSN+"������־!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�Ñ�ID��"+userSN+"������־!");
				ds = JW2DataInfo.JW2MessengerDataInfo.Item_Log(serverIP,userSN,BeginTime,EndTime);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_Item_Log_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�и��û�������־",Msg_Category.JW2_ADMIN,ServiceKey.JW2_Item_Log_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������־->JW2_Item_Log->������IP->"+serverIP+"->�û�->"+userSN+"->��ʼʱ��->"+BeginTime+"->����ʱ��->"+EndTime+"->"+ex.Message);
				return Message.COMMON_MES_RESP("û�и��û�������־", Msg_Category.JW2_ADMIN, ServiceKey.JW2_Item_Log_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �鿴���ֽ������Ϣ
		/// <summary>
		/// �鿴���ֽ������Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_MailInfo_Query(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			string EndTime = "";
			string BeginTime = "";
			int userSN = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				BeginTime = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.BeginTime).m_bValueBuffer);
				EndTime = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.EndTime).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				
				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ң�"+userSN+"ֽ������Ϣ!");
				Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ң�"+userSN+"ֽ������Ϣ!");
				ds = JW2DataInfo.JW2MessengerDataInfo.MailInfo_Query(serverIP,userSN,BeginTime,EndTime);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_MailInfo_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�и����ֽ������Ϣ",Msg_Category.JW2_ADMIN,ServiceKey.JW2_MailInfo_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("�鿴���ֽ������Ϣ->JW2_MailInfo_Query->������IP->"+serverIP+"->�û�->"+userSN+"->��ʼʱ��->"+BeginTime+"->����ʱ��->"+EndTime+"->"+ex.Message);
				return Message.COMMON_MES_RESP("û�и����ֽ������Ϣ", Msg_Category.JW2_ADMIN, ServiceKey.JW2_MailInfo_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region �޸ļ�����
		/// <summary>
		/// �޸ļ�����
		/// </summary>
		/// <returns></returns>
		public Message JW2_UpdateFamilyName_Query()
		{
			string serverIP = "";
			int result = -1;
			int itemID = 0;
			int itemNo = 0;
			int type = -1;
			string itemName = "";
			string UserName = "";
			string OLD_FamilyName = "";
			string FamilyName = "";
			int familyID = 0;
			int userbyid = 0;
			try
			{
				//ip
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				//�ϼ���
				OLD_FamilyName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_OLD_FAMILYNAME).m_bValueBuffer);
				//������
				FamilyName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_FAMILYNAME).m_bValueBuffer);
				//����Աid
				TLV_Structure strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				userbyid =(int)strut.toInteger();
				//����id
				strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_FAMILYID).m_bValueBuffer);
				familyID =(int)strut.toInteger();

				SqlHelper.log.WriteLog("���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�޸ļ�����"+OLD_FamilyName+",Ϊ"+FamilyName);
				result = JW2DataInfo.JW2MessengerDataInfo.UpdateFamilyName_Query(serverIP,OLD_FamilyName,FamilyName,userbyid,familyID);
				if(result ==1)
				{
					Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�޸ļ�����"+OLD_FamilyName+",Ϊ"+FamilyName+"�ɹ�");
					return Message.COMMON_MES_RESP("SCUESS",Msg_Category.JW2_ADMIN,ServiceKey.JW2_UpDateFamilyName_Query_Resp);
				}
				else
				{
					Console.WriteLine(DateTime.Now+" - ���������2+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�޸ļ�����"+OLD_FamilyName+",Ϊ"+FamilyName+"ʧ��");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.JW2_ADMIN,ServiceKey.JW2_UpDateFamilyName_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("�޸ļ�����->JW2_UpdateFamilyName_Query->������IP->"+serverIP+"->�ʺ�->"+UserName+"->�ɼ�����"+OLD_FamilyName+"->������->"+FamilyName+"->"+ex.Message);
				return Message.COMMON_MES_RESP("�޸ļ�������" + OLD_FamilyName +"��Ϊ��" + FamilyName +"��ʧ�ܣ���ȷ�ϸü����Ƿ���ڣ�", Msg_Category.JW2_ADMIN, ServiceKey.JW2_UpDateFamilyName_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion		

	}
}
