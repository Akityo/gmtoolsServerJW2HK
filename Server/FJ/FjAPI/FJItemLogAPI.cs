using System;
using System.Data;
using System.Text;
using Common.Logic;
using Common.DataInfo;
using GM_Server.FJDataInfo;
using System.Collections;
namespace GM_Server.FjAPI
{
	/// <summary>
	/// FJItemLogAPI ��ժҪ˵����
	/// </summary>
	public class FJItemLogAPI
	{
		Message msg = null;
		public FJItemLogAPI()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		public FJItemLogAPI(byte[] packet)
		{
			msg = new Message(packet, (uint)packet.Length);
		}
		/// <summary>
		/// �鿴����������ͣ���ʺ�
		/// </summary>
		/// <returns></returns>
		public Message FJ_Gamesbanishment_Query(int index, int pageSize)
		{
			string serverIP = null;
			string city = null;
			string account = null;
			System.Data.DataSet ds = null;
			try
			{
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ" + serverIP + "��Ϸ����ͣ���ʺ���Ϣ");
				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + serverIP + "��Ϸ����ͣ���ʺ���Ϣ");
				ds = FJItemLogInfo.GamesUserBan_Query(serverIP, city, account);
				if (null != ds && ds.Tables[0].Rows.Count > 0)
				{
					//��ҳ��
					int pageCount = 0;
					pageCount = ds.Tables[0].Rows.Count % pageSize;
					if (pageCount > 0)
					{
						pageCount = ds.Tables[0].Rows.Count / pageSize + 1;
					}
					else
						pageCount = ds.Tables[0].Rows.Count / pageSize;

					if (index + pageSize > ds.Tables[0].Rows.Count)
					{
						pageSize = ds.Tables[0].Rows.Count - index;
					}
					Query_Structure[] structList = new Query_Structure[pageSize];
					for (int i = index; i < index + pageSize; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length + 1);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserID, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP, Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[1]));
						strut.AddTagKey(TagName.FJ_BanDate, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[2]);
						strut.AddTagKey(TagName.FJ_Reason, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[3]);
						strut.AddTagKey(TagName.FJ_Style, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						//��ҳ��
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));


						structList[i - index] = strut;
					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_GamesUserBan_Query_RESP, 5);
				}
				else
				{
					return Message.COMMON_MES_RESP("��Ϸ����û�б�ͣ����ʺ�", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GamesUserBan_Query_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);

				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("��Ϸ����û�б�ͣ����ʺ�", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GamesUserBan_Query_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		/// <summary>
		/// ��Ϸ��������ʺŽ��
		/// </summary>
		/// <returns></returns>
		public Message FJ_GamesAccountOpen_Update()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string city = null;
			string account = null;
			string reason = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				reason = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Reason).m_bValueBuffer);
				result = FJItemLogInfo.GamesUserBan_Open(operateUserID, serverIP, city, account, reason);
				if (result == -1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "��Ϸ�������" + account + "���ʺ��ѱ����");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "��Ϸ�������" + account + "�ʺŲ�����!");
					return Message.COMMON_MES_RESP("������ʺ��ѱ����!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GamesUserBan_Open_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}
				else if (result == 1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "��Ϸ�������" + account + "�ʺŽ��ɹ�!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "��Ϸ�������" + account + "�ʺŽ��ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GamesUserBan_Open_RESP);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "��Ϸ�������" + account + "�ʺŽ��ʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "��Ϸ�������" + account + "�ʺŽ��ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GamesUserBan_Open_RESP);
				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("���ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GamesUserBan_Open_RESP,TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		/// <summary>
		/// �鿴����������ͣ���ʺ�
		/// </summary>
		/// <returns></returns>
		public Message FJ_banishment_Query(int index, int pageSize)
		{
			string serverIP = null;
			string city = null;
			string account = null;
			System.Data.DataSet ds = null;
			try
			{
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ" + serverIP + "ͣ���ʺ���Ϣ");
				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + serverIP + "ͣ���ʺ���Ϣ");
				ds = FJItemLogInfo.UserBan_Query(serverIP, city, account);
				if (null != ds && ds.Tables[0].Rows.Count > 0)
				{
					//��ҳ��
					int pageCount = 0;
					pageCount = ds.Tables[0].Rows.Count % pageSize;
					if (pageCount > 0)
					{
						pageCount = ds.Tables[0].Rows.Count / pageSize + 1;
					}
					else
						pageCount = ds.Tables[0].Rows.Count / pageSize;

					if (index + pageSize > ds.Tables[0].Rows.Count)
					{
						pageSize = ds.Tables[0].Rows.Count - index;
					}
					Query_Structure[] structList = new Query_Structure[pageSize];
					for (int i = index; i < index + pageSize; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length + 1);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserID, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP, Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[1]));
						strut.AddTagKey(TagName.FJ_BanDate, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[2]);
						strut.AddTagKey(TagName.FJ_Reason, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						//�ж��Ƿ�����GM��ͣ,ͨ��IP�����������ʺ�
//						DataSet fjDS = FJItemLogInfo.FJGMBan_Query(serverIP,city,ds.Tables[0].Rows[i].ItemArray[0].ToString());
//						if(fjDS!=null && fjDS.Tables[0].Rows.Count>0)
//						{
//							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, "����GM��ͣ");
//						}
//						else
//						{
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[3]);
						//}
						strut.AddTagKey(TagName.FJ_Style, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						//��ҳ��
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));


						structList[i - index] = strut;
					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBan_Query_Resp, 5);
				}
				else
				{
					return Message.COMMON_MES_RESP("��ǰû�б�ͣ����ʺ�", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBan_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);

				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("��ǰû�б�ͣ����ʺ�", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBan_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		/// <summary>
		/// ����������ʺŽ��
		/// </summary>
		/// <returns></returns>
		public Message FJ_AccountOpen_Update()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string city = null;
			string account = null;
			string reason = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				reason = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Reason).m_bValueBuffer);
				result = FJItemLogInfo.UserBan_Open(operateUserID, serverIP, city, account, reason);
				if (result == -1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "���ʺ��ѱ����");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "�ʺŲ�����!");
					return Message.COMMON_MES_RESP("������ʺ��ѱ����!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBan_Open_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

				else if (result == 1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "�ʺŽ��ɹ�!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "�ʺŽ��ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBan_Open_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "�ʺŽ��ʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "�ʺŽ��ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBan_Open_Resp);
				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("���ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBan_Open_Resp,TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}

		/// <summary>
		/// ����������ʺŽ�� �۳�300����
		/// </summary>
		/// <returns></returns>
		public Message FJ_Account300_Update()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string city = null;
			string account = null;
			string reason = null;
			string desc = "";
			int Decresult = 0;
			ArrayList arrUserinfo;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				reason = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Reason).m_bValueBuffer);
				//���ýӿ�������ҺϼƳ�ֵ���
				arrUserinfo = CharacterInfo.FJUserCharge_Query(CommonInfo.FJdbid_Query(city),account,ref desc);
				//Decresult = FJItemLogInfo.FJ_UserChargeConsume(account,city, Convert.ToInt32(arrUserinfo[1])*10);
				
				//��ֵ���Ϊ RMB
				//1RMB = 1000���б�
				//1000���б� = 10 ����� 
				//�����Ҫ�۳�300�����
				//�����ж���ҵĽ���Ƿ�300����� ����300��ֱ�ӷ��� �޷�����ʺ�
				if(arrUserinfo!= null && arrUserinfo[1].ToString() !="" && Convert.ToInt32(arrUserinfo[1])*10 >= 300 )
				{	//�����>=300 ����ô洢���� ��¼��ǰ�۳��Ľ�� ����ʺ� ����
					Decresult = FJItemLogInfo.FJ_UserChargeConsume(city,account, Convert.ToInt32(arrUserinfo[1])*10);
					//��¼�ɹ��������
					if(Decresult == 1)
					{
						result = FJItemLogInfo.UserBan_Open(operateUserID, serverIP, city, account, reason);
						if (result == -1)
						{
							SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "���ʺ��ѱ����");
							Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "�ʺŲ�����!");
							return Message.COMMON_MES_RESP("������ʺ��ѱ����!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountGold_Update_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
						}

						else if (result == 1)
						{	
						
							SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "�ʺŽ��ɹ�!");
							Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "�ʺŽ��ɹ�!");
							return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountGold_Update_RESP);

						}
						else
						{
							SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "�ʺŽ��ʧ��!");
							Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "�ʺŽ��ʧ��!");
							return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountGold_Update_RESP);
						}
					}
					else
					{
						return Message.COMMON_MES_RESP("������ʺ��������ʽ���,�����ڽ��!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountGold_Update_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
					}
				}
				else
				{
					return Message.COMMON_MES_RESP("������ʺ��������ʽ���,�����ڽ��!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountGold_Update_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}
			}
			catch
			{
				return Message.COMMON_MES_RESP("������ʺ��������ʽ���,�����ڽ��", Msg_Category.FJ_ADMIN,ServiceKey.FJ_AccountGold_Update_RESP,TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		/// <summary>
		/// ������ͣ�����������ʺ�
		/// </summary>
		/// <returns></returns>
		public Message FJ_AccountTimeClose_Update()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string city = null;
			string account = null;
			string accountState = null;
			string reason = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				accountState = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Style).m_bValueBuffer);
				reason = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Reason).m_bValueBuffer);
				result = FJItemLogInfo.UserBanTime_Close(operateUserID, serverIP, city, account,Convert.ToInt32(accountState),reason);
				if (result == -1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "���ʺ��ѱ�ͣ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "���ʺŲ�����!");
					return Message.COMMON_MES_RESP("���ʺ��ѱ�ͣ�⣡", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBanTime_Create_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}
				else if (result == 1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "�ʺ�ͣ��ɹ�!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "�ʺ�ͣ��ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBanTime_Create_RESP);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "�ʺ�ͣ��ʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "�ʺ�ͣ��ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBanTime_Create_RESP);
				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("ͣ��ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBanTime_Create_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		/// <summary>
		/// ͣ�����������ʺ�
		/// </summary>
		/// <returns></returns>
		public Message FJ_AccountClose_Update()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string city = null;
			string account = null;
			string reason = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				reason = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Reason).m_bValueBuffer);
				result = FJItemLogInfo.UserBan_Close(operateUserID, serverIP, city, account, reason);
				if (result == -1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "���ʺ��ѱ�ͣ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "���ʺŲ�����!");
					return Message.COMMON_MES_RESP("���ʺ��ѱ�ͣ�⣡", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBan_Close_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}
				else if (result == 1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "�ʺ�ͣ��ɹ�!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "�ʺ�ͣ��ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBan_Close_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "�ʺ�ͣ��ʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "�ʺ�ͣ��ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBan_Close_Resp);
				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("ͣ��ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserBan_Close_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		/// <summary>
		/// ��ѯGS������
		/// </summary>
		/// <returns></returns>
		public Message FJ_GSName_Query()
		{
			DataSet ds = null;
			string serverIP = null;
			string city = null;
			try
			{
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "GS��������Ϣ!");
				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "GS��������Ϣ");
				ds = FJItemLogInfo.FJ_GS_Query(serverIP, city);
				Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_GSName, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						structList[i] = strut;
					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_GS_Query_Resp, 1);

				}
				else
				{
					return Message.COMMON_MES_RESP("û��GS��������Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GS_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP" + serverIP + ex.Message);
				return Message.COMMON_MES_RESP("û��GS��������Ϣ!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GS_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		/// <summary>
		/// ������Ϣ��ѯ
		/// </summary>
		/// <returns></returns>
		public Message FJ_BoardList_Query()
		{
			string serverIP = null;
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "������Ϣ!");
				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "������Ϣ!");
				ds = FJItemLogInfo.BoardList_Query(serverIP);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]));
						strut.AddTagKey(TagName.FJ_MsgID, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
						strut.AddTagKey(TagName.FJ_GSName, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][2].ToString());
						strut.AddTagKey(TagName.FJ_MsgContent, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[3]));
						strut.AddTagKey(TagName.FJ_BoardFlag, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[4]));
						strut.AddTagKey(TagName.FJ_StartTime, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[5]));
						strut.AddTagKey(TagName.FJ_Interval, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[6]));
						strut.AddTagKey(TagName.FJ_EndTime, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						structList[i] = strut;
					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Query_Resp, 7);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�й�����Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP" + serverIP + ex.Message);
				return Message.COMMON_MES_RESP("û�й�����Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		/// <summary>
		/// ��ӹ�������
		/// </summary>
		/// <returns></returns>
		public Message FJ_BoardList_Insert()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string city = null;
			string gsName = null;
			string gsDesc = null;
			int interval = 0;
			string msgContent = null;
			string startDate = "";
			string endDate = "";
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				gsName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_GSName).m_bValueBuffer);
				gsDesc = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Message).m_bValueBuffer);
				startDate = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_StartTime).m_bValueBuffer);
				endDate = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_EndTime).m_bValueBuffer);
				strut = new TLV_Structure(TagName.FJ_Interval, 4, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Interval).m_bValueBuffer);
				interval = (int)strut.toInteger();
				msgContent = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_MsgContent).m_bValueBuffer);
				result = FJItemLogInfo.FJBoardList_Insert(operateUserID, serverIP, city, gsName, gsDesc, msgContent, startDate, endDate, interval);
				if (result == 1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "������ӳɹ�!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "������ӳɹ�!");
					return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_BoardList_Insert_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "����ʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "�������ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_BoardList_Insert_Resp);
				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("�������ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_BoardList_Insert_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		/// <summary>
		/// ɾ����������
		/// </summary>
		/// <returns></returns>
		public Message FJ_BoardList_Delete()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string city = null;
			int taskID = 0;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				strut = new TLV_Structure(TagName.FJ_MsgID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_MsgID).m_bValueBuffer);
				taskID = (int)strut.toInteger();
				result = FJItemLogInfo.FJBoardList_Delete(operateUserID, serverIP, city, taskID);
				if (result == 1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "����ɾ���ɹ�!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "����ɾ���ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_BoardList_Delete_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "����ɾ��ʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "����ɾ��ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_BoardList_Delete_Resp);
				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("����ɾ��ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_BoardList_Delete_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		/// <summary>
		/// ��������ѯ
		/// </summary>
		/// <returns></returns>
		public Message Task_Query()
		{
			string serverIP = null;
			string charName = null;
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				charName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + charName + "������Ϣ!");
				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + charName + "������Ϣ!");

				ds = FJItemLogInfo.Task_Query(serverIP, charName);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserNick, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
						strut.AddTagKey(TagName.FJ_Quest_id, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][2].ToString());
						strut.AddTagKey(TagName.FJ_Quest_state, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[3]);
						strut.AddTagKey(TagName.FJ_Content, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						structList[i] = strut;
					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Query_Resp, 4);
				}
				else
				{
					return Message.COMMON_MES_RESP("�����û��������Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP" + serverIP + ex.Message);
				return Message.COMMON_MES_RESP("�����û��������Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		/// <summary>
		/// �����б��ѯ
		/// </summary>
		/// <returns></returns>
		public Message Quest_Query()
		{
			int level = 0;
			DataSet ds = null;
			try
			{
				TLV_Structure struts = new TLV_Structure(TagName.FJ_Level, 4, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Level).m_bValueBuffer);
				level = (int)struts.toInteger();
				SqlHelper.log.WriteLog("������֮��+>�����б���Ϣ!");
				Console.WriteLine(DateTime.Now + " - ������֮��+>�����б���Ϣ!");

				ds = FJItemLogInfo.Quest_Query(level);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]));
						strut.AddTagKey(TagName.FJ_GuidID, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
						strut.AddTagKey(TagName.FJ_GuildName, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						structList[i] = strut;
					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_QuestTable_Query_Resp, 2);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�������б���Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_QuestTable_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Message.COMMON_MES_RESP("û�������б���Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_QuestTable_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}

		/// <summary>
		/// ����������
		/// </summary>
		/// <returns></returns>
		public Message FJ_Task_Update()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string nickName = null;
			int taskID = 0;
			int taskState = 0;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				nickName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				taskID = int.Parse(System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Quest_id).m_bValueBuffer));
				taskState = int.Parse(System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Quest_state).m_bValueBuffer));
				result = FJItemLogInfo.Task_Update(operateUserID, serverIP, nickName, taskID, taskState);
				if (result == 1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + nickName + "������³ɹ�!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + nickName + "������³ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Update_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + nickName + "�������ʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + nickName + "�������ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Update_Resp);
				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("�������ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Update_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}

		/// <summary>
		/// ����������
		/// </summary>
		/// <returns></returns>
		public Message FJ_Task_Insert()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string nickName = null;
			int taskID = 0;
			int taskState = 0;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				nickName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				taskID = int.Parse(System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Quest_id).m_bValueBuffer));
				taskState = int.Parse(System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Quest_state).m_bValueBuffer));
				result = FJItemLogInfo.Task_Insert(operateUserID, serverIP, nickName, taskID, taskState);
				if (result == 1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + nickName + "������ӳɹ�!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + nickName + "������ӳɹ�!");
					return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Insert_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + nickName + "�������ʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + nickName + "�������ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Insert_Resp);
				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("�������ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Insert_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		/// <summary>
		/// �������ɾ��
		/// </summary>
		/// <returns></returns>
		public Message FJ_Task_Delete()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string nickName = null;
			int taskID = 0;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				nickName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				taskID = int.Parse(System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Quest_id).m_bValueBuffer));
				result = FJItemLogInfo.Task_Delete(operateUserID, serverIP, nickName, taskID);
				if (result == 1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + nickName + "����ɾ���ɹ�!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + nickName + "����ɾ���ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Delete_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + nickName + "����ɾ��ʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + nickName + "����ɾ��ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Delete_Resp);
				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("����ɾ��ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Task_Delete_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		/// <summary>
		/// һ���������ڣ�ͬһ��IP�£���½���˺�����10
		/// </summary>
		/// <returns></returns>
		public Message UserLoginCount_Query(int index,int pageSize)
		{
			string city = null;
			DateTime beginDate;
			DateTime endDate;
			DataSet ds = null;
			try
			{
				city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				TLV_Structure struts = new TLV_Structure(TagName.FJ_StartTime, 3, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_StartTime).m_bValueBuffer);
				beginDate = struts.toDate();
				struts = new TLV_Structure(TagName.FJ_EndTime, 3, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_EndTime).m_bValueBuffer);
				endDate = struts.toDate();
				SqlHelper.log.WriteLog("������֮��+>һ���������ڣ�ͬһ��IP�£���½���˺�����10!");
				Console.WriteLine(DateTime.Now + " - һ���������ڣ�ͬһ��IP�£���½���˺�����10!");

				ds = FJItemLogInfo.UserLoginCount_Query(city,beginDate,endDate);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					//��ҳ��
					int pageCount = 0;
					pageCount = ds.Tables[0].Rows.Count % pageSize;
					if (pageCount > 0)
					{
						pageCount = ds.Tables[0].Rows.Count / pageSize + 1;
					}
					else
						pageCount = ds.Tables[0].Rows.Count / pageSize;

					if (index + pageSize > ds.Tables[0].Rows.Count)
					{
						pageSize = ds.Tables[0].Rows.Count - index;
					}
					Query_Structure[] structList = new Query_Structure[pageSize];
					for (int i = index; i < index + pageSize; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[0]));
						strut.AddTagKey(TagName.FJ_UserID, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
						strut.AddTagKey(TagName.ServerInfo_City, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][2].ToString());
						strut.AddTagKey(TagName.FJ_LoginIP, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][3].ToString());
						strut.AddTagKey(TagName.FJ_LoginTime, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						//��ҳ��
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));
						structList[i-index] = strut;
					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserLoginCount_Query_RESP,5);
				}
				else
				{
					return Message.COMMON_MES_RESP("û������ʺ���Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserLoginCount_Query_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Message.COMMON_MES_RESP("û������ʺ���Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserLoginCount_Query_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		/// <summary>
		/// ��ѯ�����־����
		/// </summary>
		/// <returns></returns>
		public Message ItemLogType_Query()
		{
			DataSet ds = null;
			SqlHelper.log.WriteLog("������֮��+>��־����!");
			Console.WriteLine(DateTime.Now + " - ������֮��+>��־����!");
			try
			{
				ds = FJItemLogInfo.ItemLogType_Query();
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]));
						strut.AddTagKey(TagName.FJ_Type, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
						strut.AddTagKey(TagName.FJ_Content, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						structList[i] = strut;
					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_ItemLogType_Query_Resp, 2);
				}
				else
				{
					return Message.COMMON_MES_RESP("�����û����־����", Msg_Category.FJ_ADMIN, ServiceKey.FJ_ItemLogType_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Message.COMMON_MES_RESP("�����û����־����", Msg_Category.FJ_ADMIN, ServiceKey.FJ_ItemLogType_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}


		}
		/// <summary>
		/// ��ѯ�����־
		/// </summary>
		/// <returns></returns>
		public Message ItemLog_Query(int index, int pageSize)
		{
			string serverIP = null;
			string city = null;
			string charName = null;
			string itemName = null;
			int actionType = 0;
			int fjtype = 0;
			string tableName = null;
			DateTime beginDate = new DateTime();
			DateTime endDate = new DateTime();
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				charName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				itemName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ItemName).m_bValueBuffer);
				tableName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Relate).m_bValueBuffer);
				TLV_Structure tlvstrut = new TLV_Structure(TagName.FJ_ActionType, 4, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ActionType).m_bValueBuffer);
				actionType = (int)tlvstrut.toInteger();
				tlvstrut = new TLV_Structure(TagName.FJ_StartTime, 3, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_StartTime).m_bValueBuffer);
				beginDate = tlvstrut.toDate();
				tlvstrut = new TLV_Structure(TagName.FJ_EndTime, 3, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_EndTime).m_bValueBuffer);
				endDate = tlvstrut.toDate();
				tlvstrut = new TLV_Structure(TagName.FJ_Type, 4, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Type).m_bValueBuffer);
				fjtype = (int)tlvstrut.toInteger();
				SqlHelper.log.WriteLog("������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + charName + "��ϸ��־!");
				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + charName + "��ϸ��־!");
				ds = FJItemLogInfo.ItemLog_Query(serverIP, city, charName, tableName, itemName, actionType, fjtype, beginDate, endDate);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					//��ҳ��
					int pageCount = 0;
					pageCount = ds.Tables[0].Rows.Count % pageSize;
					if (pageCount > 0)
					{
						pageCount = ds.Tables[0].Rows.Count / pageSize + 1;
					}
					else
						pageCount = ds.Tables[0].Rows.Count / pageSize;

					if (index + pageSize > ds.Tables[0].Rows.Count)
					{
						pageSize = ds.Tables[0].Rows.Count - index;
					}
					Query_Structure[] structList = new Query_Structure[pageSize];
					for (int i = index; i < index + pageSize; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length + 6);
						if (fjtype == 100)
						{
							byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]));
							strut.AddTagKey(TagName.FJ_GuidID, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP, Convert.ToDateTime(ds.Tables[0].Rows[i][1].ToString()));
							strut.AddTagKey(TagName.FJ_SendTime, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][2].ToString());
							strut.AddTagKey(TagName.FJ_Sender, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][3].ToString());
							strut.AddTagKey(TagName.FJ_Receiver, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);

							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][4].ToString());
							strut.AddTagKey(TagName.FJ_Title, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[5]);
							strut.AddTagKey(TagName.FJ_Content, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							DataSet nameDS = FJItemShopInfo.FJ_ItemName_Query(Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[6]));
							string name = "";
							string color = "";
							if (nameDS != null && nameDS.Tables[0].Rows.Count > 0)
							{
								name = nameDS.Tables[0].Rows[0][0].ToString();
								color = nameDS.Tables[0].Rows[0][1].ToString();
							}
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, name);
							strut.AddTagKey(TagName.FJ_item_0_guid, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, color);
							strut.AddTagKey(TagName.FJ_Color0, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, ds.Tables[0].Rows[i].ItemArray[7]);
							strut.AddTagKey(TagName.FJ_item_0_Num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
							nameDS = FJItemShopInfo.FJ_ItemName_Query(Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[8]));
							name = "";
							color = "";
							if (nameDS != null && nameDS.Tables[0].Rows.Count > 0)
							{
								name = nameDS.Tables[0].Rows[0][0].ToString();
								color = nameDS.Tables[0].Rows[0][1].ToString();
							}
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, name);
							strut.AddTagKey(TagName.FJ_item_1_guid, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, color);
							strut.AddTagKey(TagName.FJ_Color1, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);

							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, ds.Tables[0].Rows[i].ItemArray[9]);
							strut.AddTagKey(TagName.FJ_item_1_Num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
							nameDS = FJItemShopInfo.FJ_ItemName_Query(Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[10]));
							name = "";
							color = "";
							if (nameDS != null && nameDS.Tables[0].Rows.Count > 0)
							{
								name = nameDS.Tables[0].Rows[0][0].ToString();
								color = nameDS.Tables[0].Rows[0][1].ToString();
							}
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, name);
							strut.AddTagKey(TagName.FJ_item_2_guid, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, color);
							strut.AddTagKey(TagName.FJ_Color2, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, ds.Tables[0].Rows[i].ItemArray[11]);
							strut.AddTagKey(TagName.FJ_item_2_Num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
							nameDS = FJItemShopInfo.FJ_ItemName_Query(Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[12]));
							name = "";
							color = "";
							if (nameDS != null && nameDS.Tables[0].Rows.Count > 0)
							{
								name = nameDS.Tables[0].Rows[0][0].ToString();
								color = nameDS.Tables[0].Rows[0][1].ToString();
							}
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, name);
							strut.AddTagKey(TagName.FJ_item_3_guid, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, color);
							strut.AddTagKey(TagName.FJ_Color3, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, ds.Tables[0].Rows[i].ItemArray[13]);
							strut.AddTagKey(TagName.FJ_item_3_Num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
							nameDS = FJItemShopInfo.FJ_ItemName_Query(Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[14]));
							name = "";
							color = "";
							if (nameDS != null && nameDS.Tables[0].Rows.Count > 0)
							{
								name = nameDS.Tables[0].Rows[0][0].ToString();
								color = nameDS.Tables[0].Rows[0][1].ToString();
							}
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, name);
							strut.AddTagKey(TagName.FJ_item_4_guid, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, color);
							strut.AddTagKey(TagName.FJ_Color4, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, ds.Tables[0].Rows[i].ItemArray[15]);
							strut.AddTagKey(TagName.FJ_item_4_Num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, ds.Tables[0].Rows[i].ItemArray[16]);
							strut.AddTagKey(TagName.FJ_iMoney, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
							//��ҳ��
							strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));
							structList[i - index] = strut;

						}
						else
						{

							byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[0]);
							strut.AddTagKey(TagName.FJ_GSName, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
							strut.AddTagKey(TagName.FJ_UserNick, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][2].ToString());
							strut.AddTagKey(TagName.FJ_ItemName, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i][3].ToString()));
							strut.AddTagKey(TagName.FJ_ItemCount, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
							int color = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[4]);
							string colorDesc = "";
							switch (color)
							{
								case 1:
									colorDesc = "��ɫ";
									break;
								case 2:
									colorDesc = "��ɫ";
									break;
								case 3:
									colorDesc = "��ɫ";
									break;
								case 4:
									colorDesc = "��ɫ";
									break;
								case 5:
									colorDesc = "��ɫ";
									break;
							}
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, colorDesc);
							strut.AddTagKey(TagName.FJ_Color, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP, Convert.ToDateTime(ds.Tables[0].Rows[i][5]));
							strut.AddTagKey(TagName.FJ_Act_Time, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[6]);
							strut.AddTagKey(TagName.FJ_RelateCHarName, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[7]));
							strut.AddTagKey(TagName.FJ_iMoney, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[8]));
							strut.AddTagKey(TagName.FJ_LeftMoney, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[9]));
							strut.AddTagKey(TagName.FJ_FactoryMark, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[10]));
							strut.AddTagKey(TagName.FJ_ConsumeCredit, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[11]));
							strut.AddTagKey(TagName.FJ_LeftCredit, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							//��ҳ��
							strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));
							structList[i - index] = strut;
						}
					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_ItemLog_Query_Resp, ds.Tables[0].Rows[0].ItemArray.Length + 1);
				}
				else
				{
					return Message.COMMON_MES_RESP("�����û����־��Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_ItemLog_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP" + serverIP + ex.Message);
				return Message.COMMON_MES_RESP("�����û����־��Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_ItemLog_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		/// <summary>
		/// GM�����ͣ�ʺ���Ϣ
		/// </summary>
		/// <returns></returns>
		public Message FJ_GMUserBan_Query(int index, int pageSize)
		{
			string serverIP = null;
			string city = null;
			string account = null;
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "GM�����ͣ�ʺ���Ϣ!");
				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "GM�����ͣ�ʺ���Ϣ!");
				//�������з�������б�
				ds = FJItemLogInfo.GMUserBanLog_Query(serverIP, city, account);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{

					//��ҳ��
					int pageCount = 0;
					pageCount = ds.Tables[0].Rows.Count % pageSize;
					if (pageCount > 0)
					{
						pageCount = ds.Tables[0].Rows.Count / pageSize + 1;
					}
					else
						pageCount = ds.Tables[0].Rows.Count / pageSize;

					if (index + pageSize > ds.Tables[0].Rows.Count)
					{
						pageSize = ds.Tables[0].Rows.Count - index;
					}
					Query_Structure[] structList = new Query_Structure[pageSize];
					for (int i = index; i < index + pageSize; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length + 1);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]));
						strut.AddTagKey(TagName.FJ_GuidID, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
						strut.AddTagKey(TagName.FJ_UserID, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP, Convert.ToDateTime(ds.Tables[0].Rows[i][2].ToString()));
						strut.AddTagKey(TagName.FJ_BanDate, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][3]);
						strut.AddTagKey(TagName.FJ_GMAccountName, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						//��ҳ��
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));

						structList[i - index] = strut;

					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_GMUserBan_Query_RESP, 5);
				}
				else
				{
					return Message.COMMON_MES_RESP("û��GM�����ͣ���ʺ���Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GMUserBan_Query_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}
			}
			catch (System.Exception ex)
			{
				return Message.COMMON_MES_RESP("û��GM�����ͣ���ʺ���Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GMUserBan_Query_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		/// <summary>
		/// GM�ʺŵ�log��¼��ѯ
		/// </summary>
		/// <returns></returns>
		public Message FJ_GMUserLog_Query(int index, int pageSize)
		{
			string serverIP = null;
			string city = null;
			string charName = null;
			DateTime beginDate = new DateTime();
			DateTime endDate = new DateTime();
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				charName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				TLV_Structure tlvstrut = new TLV_Structure(TagName.FJ_StartTime, 3, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_StartTime).m_bValueBuffer);
				beginDate = tlvstrut.toDate();
				tlvstrut = new TLV_Structure(TagName.FJ_EndTime, 3, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_EndTime).m_bValueBuffer);
				endDate = tlvstrut.toDate();
				tlvstrut = new TLV_Structure(TagName.FJ_ActionType, 4, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ActionType).m_bValueBuffer);
				int actionType = (int)tlvstrut.toInteger();
				SqlHelper.log.WriteLog("������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + charName + "GM�ʺŲ�������־!");
				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + charName + "GM�ʺŲ�������־!");
				//�������з�������б�
				ds = FJItemLogInfo.GMLog_Query(serverIP, city,charName,actionType, beginDate, endDate);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{

					//��ҳ��
					int pageCount = 0;
					pageCount = ds.Tables[0].Rows.Count % pageSize;
					if (pageCount > 0)
					{
						pageCount = ds.Tables[0].Rows.Count / pageSize + 1;
					}
					else
						pageCount = ds.Tables[0].Rows.Count / pageSize;

					if (index + pageSize > ds.Tables[0].Rows.Count)
					{
						pageSize = ds.Tables[0].Rows.Count - index;
					}
					Query_Structure[] structList = new Query_Structure[pageSize];
					for (int i = index; i < index + pageSize; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length + 1);
						//byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]));
						//strut.AddTagKey(TagName.FJ_GuidID, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][0].ToString());
						strut.AddTagKey(TagName.FJ_Server_Name, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
						strut.AddTagKey(TagName.FJ_UserNick, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP, Convert.ToDateTime(ds.Tables[0].Rows[i][2]));
						strut.AddTagKey(TagName.FJ_Act_Time, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[3]));
						strut.AddTagKey(TagName.FJ_XPosition, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[4]));
						strut.AddTagKey(TagName.FJ_YPosition, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[5]));
						strut.AddTagKey(TagName.FJ_ZPosition, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[6]);
						strut.AddTagKey(TagName.FJ_RelateCHarName, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[7]);
						strut.AddTagKey(TagName.FJ_Command_Content, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						//��ҳ��
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));

						structList[i - index] = strut;

					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_GMUser_Qyery, 9);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�и�GM�ʺŵ�LOG��־", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GMUser_Qyery, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}
			}
			catch (System.Exception ex)
			{
				return Message.COMMON_MES_RESP("û�и�GM�ʺŵ�LOG��־", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GMUser_Qyery, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		/// <summary>
		/// GM�ʺŵ����ɾ����Ȩ�޵ȼ��趨
		/// </summary>
		/// <returns></returns>
		public Message FJ_GMUser_Update()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string city = null;
			string account = null;
			int power = 0;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				power = int.Parse(System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Quest_id).m_bValueBuffer));
				result = FJItemLogInfo.GMAccount_Update(operateUserID, serverIP, city, account, power);
				if (result == 1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "���GM�ʺųɹ�!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "���GM�ʺųɹ�!");
					return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GMUser_Update_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "���GM�ʺ�ʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "���GM�ʺ�ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GMUser_Update_Resp);
				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("GM�ʺ����ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GMUser_Update_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		/// <summary>
		/// ���ʹ�������Ϣ��ѯ
		/// </summary>
		/// <returns></returns>
		public Message KillUser_Query(int index, int pageSize)
		{
			string serverIP = null;
			string city = null;
			string account = null;
			string tableName = null;
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				tableName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Title).m_bValueBuffer);
				city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "ʹ�������Ϣ!");
				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "ʹ�������Ϣ!");

				ds = FJItemLogInfo.UserKill_Query(serverIP, account, tableName, city);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					//��ҳ��
					int pageCount = 0;
					pageCount = ds.Tables[0].Rows.Count % pageSize;
					if (pageCount > 0)
					{
						pageCount = ds.Tables[0].Rows.Count / pageSize + 1;
					}
					else
						pageCount = ds.Tables[0].Rows.Count / pageSize;

					if (index + pageSize > ds.Tables[0].Rows.Count)
					{
						pageSize = ds.Tables[0].Rows.Count - index;
					}
					Query_Structure[] structList = new Query_Structure[pageSize];
					for (int i = index; i < index + pageSize; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length + 1);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP, Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[0]));
						strut.AddTagKey(TagName.FJ_LoginTime, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
						strut.AddTagKey(TagName.FJ_UserID, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i][2].ToString()));
						strut.AddTagKey(TagName.FJ_Level, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[3]));
						strut.AddTagKey(TagName.FJ_dwProcessID, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP, Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[4]));
						strut.AddTagKey(TagName.FJ_BanDate, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[5]));
						strut.AddTagKey(TagName.FJ_dwErrorID, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[6]));
						strut.AddTagKey(TagName.FJ_dwHandle, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[7]));
						strut.AddTagKey(TagName.FJ_GSName, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						//��ҳ��
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));
						structList[i - index] = strut;

					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserKill_Query_Resp, 9);
				}
				else
				{
					return Message.COMMON_MES_RESP("�����û��ʹ�����", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserKill_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP" + serverIP + ex.Message);
				return Message.COMMON_MES_RESP("�����û��ʹ�����", Msg_Category.FJ_ADMIN, ServiceKey.FJ_UserKill_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		/// <summary>
		/// ��ѯ�����ʹ�����
		/// </summary>
		/// <returns></returns>
		public Message FJ_AccountReward_Query()
		{
			string serverIP = null;
			string account = null;
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���Ӣ�ۿ���Ϣ!");
				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���Ӣ�ۿ���Ϣ!");
				ds = FJItemLogInfo.FJ_AccountReward__Query(serverIP, account);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]));
						strut.AddTagKey(TagName.FJ_UserID, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
						strut.AddTagKey(TagName.FJ_UserNick, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][2].ToString());
						strut.AddTagKey(TagName.FJ_GSName, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[3]));
						strut.AddTagKey(TagName.FJ_Level, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[4]));
						strut.AddTagKey(TagName.FJ_StartTime, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[5]));
						strut.AddTagKey(TagName.FJ_Interval, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[6]));
						strut.AddTagKey(TagName.FJ_EndTime, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						structList[i] = strut;
					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountReward_Query_Resp, 7);
				}
				else
				{
					return Message.COMMON_MES_RESP("û��Ӣ�ۿ���Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountReward_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP" + serverIP + ex.Message);
				return Message.COMMON_MES_RESP("û��Ӣ�ۿ���Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountReward_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		/// <summary>
		/// ��ѯ�����ʹ�����
		/// </summary>
		/// <returns></returns>
		public Message FJ_RewardInfo_Query()
		{
			string serverIP = null;
			string city = null;
			string account = null;
			string style = null;
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				style = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Style).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "�����ϸӢ�ۿ�������ȡ���!");

				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "�����ϸӢ�ۿ�������ȡ���!");
				ds = FJItemLogInfo.FJ_RewardInfo_Query(serverIP, city, account, style);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserID, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						string userNick = "";
						if (ds.Tables[0].Rows[i].IsNull(1) == false)
							userNick = ds.Tables[0].Rows[i][1].ToString();
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, userNick);
						strut.AddTagKey(TagName.FJ_UserNick, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						string gs_name = "";
						if (ds.Tables[0].Rows[i].IsNull(2) == false)
							gs_name = ds.Tables[0].Rows[i][2].ToString();
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, gs_name);
						strut.AddTagKey(TagName.FJ_GSName, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						int lvl = 0;
						if (ds.Tables[0].Rows[i].IsNull(3) == false)
							lvl = Convert.ToInt32(ds.Tables[0].Rows[i][3]);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, lvl);
						strut.AddTagKey(TagName.FJ_Level, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						DateTime getTime = new DateTime();
						if (ds.Tables[0].Rows[i].IsNull(4) == false)
							getTime = Convert.ToDateTime(ds.Tables[0].Rows[i][4]);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP, getTime);
						strut.AddTagKey(TagName.FJ_reward_get_time, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[5]);
						strut.AddTagKey(TagName.FJ_activity_name, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i][6].ToString()));
						strut.AddTagKey(TagName.FJ_select_num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i][7].ToString()));
						strut.AddTagKey(TagName.FJ_MinLevel, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[8]));
						strut.AddTagKey(TagName.FJ_Max_lvl, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						DataSet nameDS = FJItemShopInfo.FJ_ItemName_Query(Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[9]));
						string name = "";
						if (nameDS != null && nameDS.Tables[0].Rows.Count > 0)
						{
							name = nameDS.Tables[0].Rows[0][0].ToString();
						}
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, name);
						strut.AddTagKey(TagName.FJ_item_0_guid, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[10]));
						strut.AddTagKey(TagName.FJ_item_0_Num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						nameDS = FJItemShopInfo.FJ_ItemName_Query(Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[11]));
						name = "";
						if (nameDS != null && nameDS.Tables[0].Rows.Count > 0)
						{
							name = nameDS.Tables[0].Rows[0][0].ToString();
						}
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, name);
						strut.AddTagKey(TagName.FJ_item_1_guid, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[12]));
						strut.AddTagKey(TagName.FJ_item_1_Num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						nameDS = FJItemShopInfo.FJ_ItemName_Query(Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[13]));
						name = "";
						if (nameDS != null && nameDS.Tables[0].Rows.Count > 0)
						{
							name = nameDS.Tables[0].Rows[0][0].ToString();
						}
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, name);
						strut.AddTagKey(TagName.FJ_item_2_guid, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[14]));
						strut.AddTagKey(TagName.FJ_item_2_Num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						nameDS = FJItemShopInfo.FJ_ItemName_Query(Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[15]));
						name = "";
						if (nameDS != null && nameDS.Tables[0].Rows.Count > 0)
						{
							name = nameDS.Tables[0].Rows[0][0].ToString();
						}
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, name);
						strut.AddTagKey(TagName.FJ_item_3_guid, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[16]));
						strut.AddTagKey(TagName.FJ_item_3_Num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						nameDS = FJItemShopInfo.FJ_ItemName_Query(Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[17]));
						name = "";
						if (nameDS != null && nameDS.Tables[0].Rows.Count > 0)
						{
							name = nameDS.Tables[0].Rows[0][0].ToString();
						}
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, name);
						strut.AddTagKey(TagName.FJ_item_4_guid, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[18]));
						strut.AddTagKey(TagName.FJ_item_4_Num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						nameDS = FJItemShopInfo.FJ_ItemName_Query(Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[19]));
						name = "";
						if (nameDS != null && nameDS.Tables[0].Rows.Count > 0)
						{
							name = nameDS.Tables[0].Rows[0][0].ToString();
						}
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, name);
						strut.AddTagKey(TagName.FJ_item_5_guid, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[20]));
						strut.AddTagKey(TagName.FJ_item_5_Num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						nameDS = FJItemShopInfo.FJ_ItemName_Query(Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[21]));
						name = "";
						if (nameDS != null && nameDS.Tables[0].Rows.Count > 0)
						{
							name = nameDS.Tables[0].Rows[0][0].ToString();
						}
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, name);
						strut.AddTagKey(TagName.FJ_item_6_guid, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[22]));
						strut.AddTagKey(TagName.FJ_item_6_Num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						nameDS = FJItemShopInfo.FJ_ItemName_Query(Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[23]));
						name = "";
						if (nameDS != null && nameDS.Tables[0].Rows.Count > 0)
						{
							name = nameDS.Tables[0].Rows[0][0].ToString();
						}
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, name);
						strut.AddTagKey(TagName.FJ_item_7_guid, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[24]));
						strut.AddTagKey(TagName.FJ_item_7_Num, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						structList[i] = strut;
					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_RewardDetail_Query_Resp, 25);
				}
				else
				{
					return Message.COMMON_MES_RESP("û����ϸӢ�ۿ�������ȡ���", Msg_Category.FJ_ADMIN, ServiceKey.FJ_RewardDetail_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP" + serverIP + ex.Message);
				return Message.COMMON_MES_RESP("û����ϸӢ�ۿ�������ȡ���", Msg_Category.FJ_ADMIN, ServiceKey.FJ_RewardDetail_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		/// <summary>
		/// ��ѯ��ҳ�ֵ��Ϣ
		/// </summary>
		/// <returns></returns>
		public Message FJ_AccountCharage_Query(int index, int pageSize)
		{
			string serverIP = null;
			string city = null;
			string account = null;
			string tableName = null;
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "��ֵ��Ϣ!");
				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "��ֵ��Ϣ!");

				ds = FJItemLogInfo.FJ_AccountDeposit_Query(serverIP, account, city);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					//��ҳ��
					int pageCount = 0;
					pageCount = ds.Tables[0].Rows.Count % pageSize;
					if (pageCount > 0)
					{
						pageCount = ds.Tables[0].Rows.Count / pageSize + 1;
					}
					else
						pageCount = ds.Tables[0].Rows.Count / pageSize;

					if (index + pageSize > ds.Tables[0].Rows.Count)
					{
						pageSize = ds.Tables[0].Rows.Count - index;
					}
					Query_Structure[] structList = new Query_Structure[pageSize];
					for (int i = index; i < index + pageSize; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length + 1);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]));
						strut.AddTagKey(TagName.FJ_PaySN, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
						strut.AddTagKey(TagName.FJ_UserID, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP, Convert.ToDateTime(ds.Tables[0].Rows[i][2].ToString()));
						strut.AddTagKey(TagName.FJ_Option_Time, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[3]));
						strut.AddTagKey(TagName.FJ_Credit, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP, Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[4]));
						strut.AddTagKey(TagName.FJ_Add_Time, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[5]));
						strut.AddTagKey(TagName.FJ_Add_Ok, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						//��ҳ��
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));
						structList[i - index] = strut;

					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountDeposit_Query_Resp, 7);
				}
				else
				{
					return Message.COMMON_MES_RESP("�����û�г�ֵ��Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountDeposit_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP" + serverIP + ex.Message);
				return Message.COMMON_MES_RESP("�����û�г�ֵ��Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountDeposit_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		/// <summary>
		/// ������ҵĳ�ֵ��Ϣ
		/// </summary>
		/// <returns></returns>
		public Message FJ_AccountCharage_Update()
		{
			int result = -1;
			int operateUserID = 0;
			int paySN = 0;
			string serverIP = null;
			string city = null;
			string account = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				strut = new TLV_Structure(TagName.FJ_UserIndexID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserIndexID).m_bValueBuffer);
				paySN = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				result = FJItemLogInfo.FJ_AccountDeposit_Update(operateUserID, serverIP, city, account, paySN);
				if (result == 1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "������ҳ�ֵ��Ϣ�ɹ�!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "������ҳ�ֵ��Ϣ�ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountDeposit_Update_Resp);
				}
				else if (result == -1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "������ҳ�ֵ��Ϣʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "������ҳ�ֵ��Ϣʧ��!");
					return Message.COMMON_MES_RESP("�Բ�����û��Ȩ�������������", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountDeposit_Update_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "������ҳ�ֵ��Ϣʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + account + "������ҳ�ֵ��Ϣʧ��!");
					return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountDeposit_Update_Resp);
				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountDeposit_Update_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}

		/// <summary>
		/// ��ѯ��һ���Log��Ϣ
		/// </summary>
		/// <returns></returns>
		public Message FJ_MakeDream_Query(int index, int pageSize)
		{
			string serverIP = null;
			string city = null;
			string account = null;

			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				string querytime=Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.BeginTime).m_bValueBuffer);
				string logtype=Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Log_Type).m_bValueBuffer);
//				TLV_Structure strut1 = new TLV_Structure(TagName.FJ_ActionType, 4, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ActionType).m_bValueBuffer);
//				int searchtype= (int)strut1.toInteger();
//				string EndTime=Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.EndTime).m_bValueBuffer);

				//city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);

				SqlHelper.log.WriteLog("������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "��ѯ��һ���Log��Ϣ!");
				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "��ѯ��һ���Log��Ϣ!");

				ds = FJItemLogInfo.FJ_MakeDream_Query(serverIP, account, querytime,logtype,"",0);

				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					//��ҳ��
					int pageCount = 0;
					pageCount = ds.Tables[0].Rows.Count % pageSize;
					if (pageCount > 0)
					{
						pageCount = ds.Tables[0].Rows.Count / pageSize + 1;
					}
					else
						pageCount = ds.Tables[0].Rows.Count / pageSize;

					if (index + pageSize > ds.Tables[0].Rows.Count)
					{
						pageSize = ds.Tables[0].Rows.Count - index;
					}
					Query_Structure[] structList = new Query_Structure[pageSize];
					for (int i = index; i < index + pageSize; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length + 1);

						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[0].ToString());
						strut.AddTagKey(TagName.ServerInfo_City, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
						strut.AddTagKey(TagName.FJ_UserNick, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[2]));
						strut.AddTagKey(TagName.FJ_value1, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, Convert.ToString(ds.Tables[0].Rows[i].ItemArray[3]));
						strut.AddTagKey(TagName.FJ_Type, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP, Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[4]));
						strut.AddTagKey(TagName.MF_record_time, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
						//��ҳ��
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));
						structList[i - index] = strut;

					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_MakeDream_Query_RESP, 6);
				}
				else
				{
					return Message.COMMON_MES_RESP("�����û�г�ֵ��Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_MakeDream_Query_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP" + serverIP + ex.Message);
				return Message.COMMON_MES_RESP("�����û�г�ֵ��Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_MakeDream_Query_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}


		/// <summary>
		/// ��ѯ���Ԫ������ʱ����Ϣ
		/// </summary>
		/// <returns></returns>
		public Message FJ_CreditTime_Query(int index, int pageSize)
		{
			string serverIP = null;
			string city = null;
			string account = null;
			
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				string querytime=Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.BeginTime).m_bValueBuffer);
//				TLV_Structure strut1 = new TLV_Structure(TagName.FJ_ActionType, 4, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ActionType).m_bValueBuffer);
//				int searchtype= (int)strut1.toInteger();
//				string EndTime=Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.EndTime).m_bValueBuffer);
				//city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);

				SqlHelper.log.WriteLog("������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "Ԫ������ʱ����Ϣ!");
				Console.WriteLine(DateTime.Now + " - ������֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "Ԫ������ʱ����Ϣ!");

				ds = FJItemLogInfo.FJ_CreditTime_Query(serverIP, account, querytime,"",0);

				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					//��ҳ��
					int pageCount = 0;
					pageCount = ds.Tables[0].Rows.Count % pageSize;
					if (pageCount > 0)
					{
						pageCount = ds.Tables[0].Rows.Count / pageSize + 1;
					}
					else
						pageCount = ds.Tables[0].Rows.Count / pageSize;

					if (index + pageSize > ds.Tables[0].Rows.Count)
					{
						pageSize = ds.Tables[0].Rows.Count - index;
					}
					Query_Structure[] structList = new Query_Structure[pageSize];
					for (int i = index; i < index + pageSize; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length + 1);

						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[0].ToString());
						strut.AddTagKey(TagName.ServerInfo_City, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
						strut.AddTagKey(TagName.FJ_UserID, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[2]));
						strut.AddTagKey(TagName.FJ_pre_credit, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[3]));
						strut.AddTagKey(TagName.FJ_post_credit, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[4]));
						strut.AddTagKey(TagName.FJ_pre_time, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[5]));
						strut.AddTagKey(TagName.FJ_post_time, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP, Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[6]));
						strut.AddTagKey(TagName.FJ_Option_Time, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
						//��ҳ��
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));
						structList[i - index] = strut;

					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.FJ_ADMIN, ServiceKey.FJ_Credit_time_Query_RESP, 8);
				}
				else
				{
					return Message.COMMON_MES_RESP("�����û�г�ֵ��Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Credit_time_Query_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP" + serverIP + ex.Message);
				return Message.COMMON_MES_RESP("�����û�г�ֵ��Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Credit_time_Query_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
	}
}
