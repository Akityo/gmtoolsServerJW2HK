using System;
using System.Data;
using System.Text;
using GM_Server.FJDataInfo;
using Common.Logic;
using Common.DataInfo;
using Domino;
using Common.NotesDataInfo;
namespace GM_Server.FjAPI
{
	/// <summary>
	/// SDOCharacterInfoAPI ��ժҪ˵����
	/// </summary>
	public class FJCharacterInfoAPI
	{
		Message msg = null;
		public FJCharacterInfoAPI()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		#region ���캯��
		public FJCharacterInfoAPI(byte[] packet)
		{
			msg = new Message(packet,(uint)packet.Length);
			
		}
		#endregion
		#region ��Ҽ������ѯ
		public Message FJUserActiveCode_Query()
		{
			string activeCode = null;
			string strDesc = null;
			System.Collections.ArrayList ds = new System.Collections.ArrayList();
			try
			{
				//serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				activeCode = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UseActiveCode).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>���"+activeCode+"������!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>���"+activeCode+"������!");
				ds=CharacterInfo.ActiveCode_Query(activeCode,1,ref strDesc);
				if(ds!=null && ds.Count>0)
				{
					Query_Structure[] structList = new Query_Structure[1];

					Query_Structure strut = new Query_Structure((uint)ds.Count);
					byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds[0]);
					strut.AddTagKey(TagName.FJ_UseActiveCode,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
					bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds[1]);
					strut.AddTagKey(TagName.FJ_GoldAccount,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
					bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds[2]);
					strut.AddTagKey(TagName.FJ_isUse, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
					bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds[3]);
					strut.AddTagKey(TagName.FJ_UseAccount,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
					bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP,Convert.ToDateTime(ds[4]));
					strut.AddTagKey(TagName.FJ_UseTime, TagFormat.TLV_TIMESTAMP, (uint)bytes.Length, bytes);
					structList[0] = strut;
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_ActiveCode_Query_Resp,5);
				}
				else
				{
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.FJ_ADMIN,ServiceKey.FJ_ActiveCode_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception)
			{
				return Message.COMMON_MES_RESP(strDesc,Msg_Category.FJ_ADMIN,ServiceKey.FJ_ActiveCode_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);                
			}
            
		}
		#endregion
		#region �����ֶһ����߼�¼
		public Message FJJP_Query()
		{
			string account = null;
			int actiontype = 0;
			string strDesc = null;
			System.Collections.ArrayList ds = new System.Collections.ArrayList();
			try
			{
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				TLV_Structure tlv = new TLV_Structure(TagName.FJ_ActionType,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ActionType).m_bValueBuffer);
				actiontype  =(int)tlv.toInteger();
				SqlHelper.log.WriteLog("������֮��+>���"+account+"���ֶһ����߼�¼!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>���"+account+"���ֶһ����߼�¼!");
				ds=CharacterInfo.FJ_JPQuery(account,actiontype,ref strDesc);
				if(ds!=null && ds.Count>0)
				{
					if(actiontype ==1)
					{
						Query_Structure[] structList = new Query_Structure[1];
						Query_Structure strut = new Query_Structure((uint)ds.Count);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds[0]);
						strut.AddTagKey(TagName.FJ_UserID,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds[1]);
						strut.AddTagKey(TagName.FJ_pointused,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds[2]));
						strut.AddTagKey(TagName.FJ_pointamt, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						structList[0] = strut;
						return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_JP_Query_RESP,3);
					}
					else
					{
						Query_Structure[] structList = new Query_Structure[ds.Count];
						for(int i=0;i<ds.Count;i++)
						{
							System.Collections.ArrayList colList  = (System.Collections.ArrayList)ds[i];
							Query_Structure strut = new Query_Structure((uint)colList.Count);
							byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,colList[0]);
							strut.AddTagKey(TagName.FJ_UserID,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP,Convert.ToDateTime(colList[1]));
							strut.AddTagKey(TagName.FJ_CreateTime,TagFormat.TLV_TIMESTAMP,(uint)bytes.Length,bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,colList[2]);
							strut.AddTagKey(TagName.FJ_propname, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,colList[3]);
							strut.AddTagKey(TagName.ServerInfo_City,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(colList[4]));
							strut.AddTagKey(TagName.FJ_integral, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
							structList[i] = strut;
						}
						return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_JP_Query_RESP,5);

					}
				}
				else
				{
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.FJ_ADMIN,ServiceKey.FJ_JP_Query_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception)
			{
				return Message.COMMON_MES_RESP(strDesc,Msg_Category.FJ_ADMIN,ServiceKey.FJ_JP_Query_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);                
			}
            
		}
		#endregion
		#region ����ʺŲ�ѯ
		public Message Account_Query()
		{
			string account = null;
			string errDesc ="";
			System.Collections.ArrayList ds = new System.Collections.ArrayList();
			try
			{
				//serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ���"+account+"������Ϣ!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>���"+account+"������Ϣ!");

				ds=CharacterInfo.Account_Query(account,1,ref errDesc);
				if(ds!=null && ds.Count>0)
				{
					Query_Structure[] structList = new Query_Structure[ds.Count];
					for(int i=0;i<ds.Count;i++)
					{
						System.Collections.ArrayList colList = (System.Collections.ArrayList)ds[i];
						Query_Structure strut = new Query_Structure((uint)colList.Count);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,colList[0]);
						strut.AddTagKey(TagName.FJ_UseAccount,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP,Convert.ToDateTime(colList[1]));
						strut.AddTagKey(TagName.FJ_ActiveTime,TagFormat.TLV_TIMESTAMP,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,colList[2]);
						strut.AddTagKey(TagName.FJ_ServerIP, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,colList[3]);
						strut.AddTagKey(TagName.FJ_GoldAccount, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						structList[i] = strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_AccountActive_Query_Resp,4);

				}
				else
				{
					return Message.COMMON_MES_RESP(errDesc,Msg_Category.FJ_ADMIN,ServiceKey.FJ_AccountActive_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception)
			{
				return Message.COMMON_MES_RESP(errDesc,Msg_Category.FJ_ADMIN,ServiceKey.FJ_AccountActive_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
               
			}
		}
		#endregion
		#region ��ѯ����޸���ʱ����
		/// <summary>
		/// 		/// ��ѯ����޸���ʱ����
		/// </summary>
		/// <returns></returns>
		public Message FJ_ModifPwd_Query()
		{
			DataSet ds = null;
			string serverIP = null;
			string city = null;
			string account = null;
			try
			{
			    serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UseAccount).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"��ʱ�����ѯ!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"��ʱ�����ѯ!");
				ds = CharacterInfo.FJ_ModifPwd_Query(account,serverIP,city);
				Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[1]);
						strut.AddTagKey(TagName.FJ_ServerIP,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[2]);
						strut.AddTagKey(TagName.FJ_UseAccount,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						string pwd = "";
						if(ds.Tables[0].Rows[i].IsNull(3)==false)
						{
                            pwd = ds.Tables[0].Rows[i].ItemArray[3].ToString();                    
						}
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,pwd);
						strut.AddTagKey(TagName.FJ_Pwd, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						structList[i]=strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_TempPassword_Query_Resp,3);

				}
				else
				{
					return Message.COMMON_MES_RESP("û�и������ʱ�޸�������Ϣ!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_TempPassword_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("û�и������ʱ�޸�������Ϣ!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_TempPassword_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region �޸��������
		/// <summary>
		/// �޸��������
		/// </summary>
		/// <returns></returns>
		public Message AccountPwd_Update()
		{
			int result = -1;
			int userByID = 0;
			string serverIP = null;
			string city = null;
			string username = null;
			string password = null;
			try
			{
				TLV_Structure tlv = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				userByID  =(int)tlv.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);

				username = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UseAccount).m_bValueBuffer);
				password = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Pwd).m_bValueBuffer);
				result = CharacterInfo.FJ_ModifPwd(userByID,username,password,serverIP,city);
				if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"�������"+username+"��Ϣ�޸ĳɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>�������"+username+"��Ϣ�޸ĳɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerPwd_Update_Resp);
				}
				else if(result==-1)
				{
					return Message.COMMON_MES_RESP("������ʺŲ�����!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerPwd_Update_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��>��������ַ"+serverIP+"�������"+username+"��Ϣ�޸�ʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+serverIP+"�������"+username+"��Ϣ�޸�ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerPwd_Update_Resp);
				}
			}
			catch(System.Exception ex)
			{
				return Message.COMMON_MES_RESP(ex.Message,Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerPwd_Update_Resp);
			}

		}
		#endregion
		#region �ָ��������
		/// <summary>
		/// �ָ��������
		/// </summary>
		/// <returns></returns>
		public Message AccountPwd_Recover()
		{
			int result = -1;
			int userByID = 0;
			string serverIP = null;
			string city  = null;
			string username = null;
			try
			{
				TLV_Structure tlv = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				userByID  =(int)tlv.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				username = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				result = CharacterInfo.FJ_RecoverPwd(userByID,username,serverIP,city);
				if(result==-1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"���"+username+"���벻����!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>�������"+username+"���벻����!");
					return Message.COMMON_MES_RESP("�����û�б��޸Ĺ�����",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PWD_Recover_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				else if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"�������"+username+"��Ϣ�ָ��ɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>�������"+username+"��Ϣ�ָ��ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PWD_Recover_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��>��������ַ"+serverIP+"�������"+username+"��Ϣ�ָ�ʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+serverIP+"�������"+username+"��Ϣ�ָ�ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PWD_Recover_Resp);
				}
			}
			catch(System.Exception ex)
			{
				return Message.COMMON_MES_RESP(ex.Message,Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerPwd_Update_Resp);
			}

		}
		#endregion
		#region ��ѯ���гƺ�
		/// <summary>
		/// ��ѯ���гƺ�
		/// </summary>
		/// <returns></returns>
		public Message FJ_CurRank_Query()
		{
			DataSet ds = null;
			string serverIP = null;
			string userNick = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				userNick = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+userNick+"�ƺ��б�!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+userNick+"�ƺ��б�!");
				ds = CharacterInfo.FJ_curRank_Query(serverIP,userNick);
				Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]));
						strut.AddTagKey(TagName.FJ_ItemCode,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[1]);
						strut.AddTagKey(TagName.FJ_ItemName,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[2]);
						strut.AddTagKey(TagName.FJ_Message, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						structList[i]=strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_Currank_Query_Resp,3);

				}
				else
				{
					return Message.COMMON_MES_RESP("û����ҳƺ��б�!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Currank_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("û����ҳƺ��б�!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Currank_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region ��ѯ����ʺ���Ϣ
		/// <summary>
		/// ��ѯ����ʺ���Ϣ
		/// </summary>
		/// <returns></returns>
		public Message FJAccount_Query()
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

				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"������Ϣ!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"������Ϣ!");
				ds = CharacterInfo.FJAccount_Query(serverIP,city,account);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];

					if(ds.Tables[0].Columns.Count>11)
					{
						for(int i=0;i<ds.Tables[0].Rows.Count;i++)
						{	
							Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
							byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
							strut.AddTagKey(TagName.FJ_UserID,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[1]);
							strut.AddTagKey(TagName.FJ_UserNick, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[2]);
							strut.AddTagKey(TagName.FJ_MSex,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);	
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[3]);
							strut.AddTagKey(TagName.FJ_Level,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);	
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,Convert.ToString(ds.Tables[0].Rows[i].ItemArray[4]));
							strut.AddTagKey(TagName.FJ_RemainCredit,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);	
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[5]);
							strut.AddTagKey(TagName.FJ_card_time,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[6]);
							strut.AddTagKey(TagName.FJ_isAdult,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[7]);
							strut.AddTagKey(TagName.FJ_9youID,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);	
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[8]);
							strut.AddTagKey(TagName.FJ_LinPai,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);	
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[9]);
							strut.AddTagKey(TagName.FJ_BlockFlag,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);	
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP,Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[10]));
							strut.AddTagKey(TagName.FJ_CreateTime,TagFormat.TLV_TIMESTAMP,(uint)bytes.Length,bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP,Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[11]));
							strut.AddTagKey(TagName.FJ_OfflineTime,TagFormat.TLV_TIMESTAMP,(uint)bytes.Length,bytes);
							structList[i] = strut;

						}
						return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_Account_Query_Resp,12);

					}
					else
					{
						for(int i=0;i<ds.Tables[0].Rows.Count;i++)
						{	
							Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
							byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
							strut.AddTagKey(TagName.FJ_UserID,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[1]);
							strut.AddTagKey(TagName.FJ_UserNick, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[2]);
							strut.AddTagKey(TagName.FJ_MSex,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);	
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[3]);
							strut.AddTagKey(TagName.FJ_Level,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);	
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,Convert.ToString(ds.Tables[0].Rows[i].ItemArray[4]));
							strut.AddTagKey(TagName.FJ_RemainCredit,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);	
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[5]);
							strut.AddTagKey(TagName.FJ_isAdult,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[6]);
							strut.AddTagKey(TagName.FJ_9youID,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);	
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[7]);
							strut.AddTagKey(TagName.FJ_LinPai,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);	
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[8]);
							strut.AddTagKey(TagName.FJ_BlockFlag,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);	
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP,Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[9]));
							strut.AddTagKey(TagName.FJ_CreateTime,TagFormat.TLV_TIMESTAMP,(uint)bytes.Length,bytes);
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP,Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[10]));
							strut.AddTagKey(TagName.FJ_OfflineTime,TagFormat.TLV_TIMESTAMP,(uint)bytes.Length,bytes);
							structList[i] = strut;

						}
						return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_Account_Query_Resp,11);
					}

				}
				else
				{
					return Message.COMMON_MES_RESP("û�и�����ʺ���Ϣ",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Account_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("û�и�����ʺ���Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Account_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion
		#region �������������Ϣ
		/// <summary>
		/// �������������Ϣ
		/// </summary>
		public Message FJCharInfo_Query()
		{
			string serverIP = null;
			string city = null;
			string account = null;
			string userNick = null;
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				userNick = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				if(account.Length<=0 && userNick.Length>0)
				{
					SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+userNick+"����������Ϣ!");
					Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+userNick+"����������Ϣ!");
				}
				else
				{
					SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"����������Ϣ!");
					Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"����������Ϣ!");
				}
				ds = CharacterInfo.characterInfo_Query(serverIP,city,account,userNick);	
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList  = new Query_Structure[ds.Tables[0].Rows.Count];
					int  cnt = 20;
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						Query_Structure strut  = null;
						if(userNick.Equals("ALL"))
						{
							strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length+1);
							cnt = 21;
						}
						else
						{
							strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						}
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserID,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[1]);
						strut.AddTagKey(TagName.FJ_UserNick, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[2]));
						strut.AddTagKey(TagName.FJ_isDel, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[3]);
						strut.AddTagKey(TagName.FJ_Sex,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);	
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[4]);
						strut.AddTagKey(TagName.FJ_Level,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);	
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[5]);
						strut.AddTagKey(TagName.FJ_TotalExp,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_MONEY,ds.Tables[0].Rows[i].ItemArray[6]);
						strut.AddTagKey(TagName.FJ_CurExp,TagFormat.TLV_MONEY,(uint)bytes.Length,bytes);	
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,ds.Tables[0].Rows[i].ItemArray[7]);
						strut.AddTagKey(TagName.FJ_TotalSP,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);	
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_MONEY,ds.Tables[0].Rows[i].ItemArray[8]);
						strut.AddTagKey(TagName.FJ_CurSP,TagFormat.TLV_MONEY,(uint)bytes.Length,bytes);	
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[9]);
						strut.AddTagKey(TagName.FJ_Occupation,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[10]));
						strut.AddTagKey(TagName.FJ_MCash,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP,Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[11]));
						strut.AddTagKey(TagName.FJ_CreateTime,TagFormat.TLV_TIMESTAMP,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP,Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[12]));
						strut.AddTagKey(TagName.FJ_LastOfflineTime,TagFormat.TLV_TIMESTAMP,(uint)bytes.Length,bytes);
						string guildName ="�ް���";
						if(!userNick.Equals("ALL"))
						{
							DataSet guildDS	= CharacterInfo.GuildName_Query(serverIP,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[13]));
							if(guildDS!=null && guildDS.Tables[0].Rows.Count>0)
							{
								guildName = guildDS.Tables[0].Rows[0].ItemArray[0].ToString();                            
							}
						}
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,guildName);
						strut.AddTagKey(TagName.FJ_GuildName,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[14]));
						strut.AddTagKey(TagName.FJ_ihonor,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i][15].ToString());
						strut.AddTagKey(TagName.FJ_icurrank,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,Convert.ToString(ds.Tables[0].Rows[i][16]));
						strut.AddTagKey(TagName.FJ_RemainCredit,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i][17].ToString());
						strut.AddTagKey(TagName.FJ_LoginIP,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[18]));
						strut.AddTagKey(TagName.FJ_Occupation_id,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[19]));
						strut.AddTagKey(TagName.FJ_IsOnline,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						if(userNick.Equals("ALL"))
						{
							bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[20]);
							strut.AddTagKey(TagName.ServerInfo_City,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						}
						structList[i] = strut;

					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_CharacterInfo_Query_Resp,cnt);
				}
				else
				{
					return Message.COMMON_MES_RESP("�����û�����������Ϣ",Msg_Category.FJ_ADMIN,ServiceKey.FJ_CharacterInfo_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("�����û�����������Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_CharacterInfo_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion
		#region ���������Ϣ�޸�
		/// <summary>
		/// ���������Ϣ�޸�
		/// </summary>
		/// <returns></returns>
		public Message FJCharacterInfo_Update()
		{
			int result = -1;
			string serverIP = null;
			string account = null;
			string charName = null;
			int level = 0;
			int operateUserID = 0;
			int imoney = 0;
			int fexp = 0;
			int ihonor = 0;
			int currank = 0;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UseAccount).m_bValueBuffer);
				charName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				strut = new TLV_Structure(TagName.FJ_Type,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Type).m_bValueBuffer);
				currank  =(int)strut.toInteger();
				strut = new TLV_Structure(TagName.FJ_Level,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Level).m_bValueBuffer);
				level  =(int)strut.toInteger();
				strut = new TLV_Structure(TagName.FJ_CurExp,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_CurExp).m_bValueBuffer);
				fexp  =(int)strut.toInteger();
				strut = new TLV_Structure(TagName.FJ_MCash,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_MCash).m_bValueBuffer);
				imoney  =(int)strut.toInteger();
				strut = new TLV_Structure(TagName.FJ_ihonor,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ihonor).m_bValueBuffer);
				ihonor  =(int)strut.toInteger();
				result = CharacterInfo.characterInfo_Update(operateUserID,serverIP,account,charName,level,imoney,fexp,ihonor,currank);
				if (result == -1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "���ʺŲ�����");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "���ʺŲ�����!");
					return Message.COMMON_MES_RESP("����ʧ�ܣ�!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_CharacterInfo_Update_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				else if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"���"+account+"����������Ϣ�޸ĳɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"����������Ϣ�޸ĳɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_CharacterInfo_Update_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"���"+account+"����������Ϣ�޸�ʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"����������Ϣ�޸�ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_CharacterInfo_Update_Resp);
				}
			}
			catch(System.Exception ex)
			{
				return Message.COMMON_MES_RESP(ex.Message,Msg_Category.FJ_ADMIN,ServiceKey.FJ_CharacterInfo_Update_Resp);
			}
		}
		#endregion
		#region �޳�������Ϸ���
		/// <summary>
		/// �޳�������Ϸ���
		/// </summary>
		/// <returns></returns>
		public Message FJUserOnline_out()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string city = null;
			string account = null;
			//string passWD = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				result = CharacterInfo.userOnline_out(operateUserID,serverIP,city,account);
				if (result == -1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "���ʺŲ�����");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "���ʺŲ�����!");
					return Message.COMMON_MES_RESP("����ҵĲ�����!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Login_Out_Update_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				else if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"���"+account+"�޳��ɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"�޳��ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Login_Out_Update_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"���"+account+"�޳�ʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"�޳�ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Login_Out_Update_Resp);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ҵĲ�����!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Login_Out_Update_Resp);
			}

		}
		#endregion
		#region ����λ�ò�ѯ����Ϸ���ڵ�ͼ
		/// <summary>
		/// ����λ�ò�ѯ����Ϸ���ڵ�ͼ
		/// </summary>
		/// <returns></returns>
		public Message FJ_Map_Query()
		{
			DataSet ds = null;
			//string posx = null;
			//string posy = null;
			try
			{
				//posx = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_XPosition).m_bValueBuffer);
				//posy = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_YPosition).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ��ͼ����λ��!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ��ͼ����λ��!");
				ds = CharacterInfo.FJMapQuery();
				Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_Map, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[1]);
						strut.AddTagKey(TagName.FJ_XPosition, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[2]);
						strut.AddTagKey(TagName.FJ_YPosition, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[3]);
						strut.AddTagKey(TagName.FJ_ZPosition, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						structList[i]=strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_MAP_Query_Resp,4);

				}
				else
				{
					return Message.COMMON_MES_RESP("û�и�λ�õĵ�ͼ",Msg_Category.FJ_ADMIN,ServiceKey.FJ_MAP_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				return Message.COMMON_MES_RESP("û�и�λ�õĵ�ͼ!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_MAP_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region ��ѯ�������Ϸ����λ��
		/// <summary>
		/// ��ѯ�������Ϸ����λ��
		/// </summary>
		/// <returns></returns>
		public Message FJ_PlayerPosition()
		{
			DataSet ds = null;
			string serverIP = null;
			string account = null;
			string char_name = null;
			try
			{
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				char_name = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"��ͼ����λ��!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"��ͼ����λ��!");
				ds = CharacterInfo.FJPlayerPosition(serverIP,account,char_name);
				Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserID,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[1]);
						strut.AddTagKey(TagName.FJ_UserNick,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[2]);
						strut.AddTagKey(TagName.FJ_Map, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[3].ToString());
						strut.AddTagKey(TagName.FJ_XPosition, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[4].ToString());
						strut.AddTagKey(TagName.FJ_YPosition, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[5].ToString());
						strut.AddTagKey(TagName.FJ_ZPosition, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						structList[i]=strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayPosition_Query_Resp,6);

				}
				else
				{
					return Message.COMMON_MES_RESP("�����û�����������Ϣ",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayPosition_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("û�и���ҵ������Ϣ!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayPosition_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region �����������Ϸ����λ��
		/// <summary>
		/// �����������Ϸ����λ��
		/// </summary>
		/// <returns></returns>
		public Message FJ_PlayerPosition_Update()
		{
			int result = -1;
			int GmUserID = 0;
			string serverIP = null;
			string account = null;
			string char_name = null;
			string posx =null;
			string posy = null;
			string posz = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				GmUserID =(int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				char_name = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				posx = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_XPosition).m_bValueBuffer);
				posy = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_YPosition).m_bValueBuffer);
				posz = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ZPosition).m_bValueBuffer);
				
				result = CharacterInfo.FJPlayerPosition_Update(GmUserID,serverIP,account,char_name,float.Parse(posx),float.Parse(posy),float.Parse(posz));
				if(result ==1)
				{
					return Message.COMMON_MES_RESP("SUCCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayPosition_Update_Resp);

				}
				else
				{
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayPosition_Update_Resp);
				}

			}
			catch(System.Exception ex)
			{
				return Message.COMMON_MES_RESP("����ʧ��",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayPosition_Update_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region ��ѯ�������Ϸ����ǳ�
		/// <summary>
		/// ��ѯ�������Ϸ����ǳ�
		/// </summary>
		/// <returns></returns>
		public Message FJ_PlayerLoginOut(int index,int pageSize)
		{
			DataSet ds = null;
			string serverIP = null;
			string city = null;
			string charName = "";
			int actionType = 0;
			string loginIP = "";
			DateTime startDate;
			DateTime endDate;
			try
			{
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				loginIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_LoginIP).m_bValueBuffer);
				charName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				TLV_Structure tlvstrut = new TLV_Structure(TagName.FJ_ActionType, 4, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ActionType).m_bValueBuffer);
				actionType = (int)tlvstrut.toInteger();
				tlvstrut = new TLV_Structure(TagName.FJ_StartTime,3,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_StartTime).m_bValueBuffer);
				startDate =tlvstrut.toDate();
				tlvstrut = new TLV_Structure(TagName.FJ_EndTime,3,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_EndTime).m_bValueBuffer);
				endDate =tlvstrut.toDate();
				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+charName+"��¼��¼!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+charName+"��¼��¼!");
				ds = CharacterInfo.FJPlayLoginOut(serverIP,city,charName,actionType,loginIP,startDate,endDate);
				if(ds!=null && ds.Tables[0].Rows.Count>1)
				{
					//��ҳ��
					int pageCount = 0;
					pageCount = (ds.Tables[0].Rows.Count) % pageSize;
					if (pageCount > 0)
					{
						pageCount = (ds.Tables[0].Rows.Count) / pageSize + 1;
					}
					else
						pageCount = (ds.Tables[0].Rows.Count) / pageSize;
					if (index + pageSize > (ds.Tables[0].Rows.Count))
					{
						pageSize = (ds.Tables[0].Rows.Count) - index;
					}
					Query_Structure[] structList = new Query_Structure[pageSize];
					//int rows = 0;
					for (int i = index; i < index + pageSize; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length+2);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserNick,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP,ds.Tables[0].Rows[i].ItemArray[1]);
						strut.AddTagKey(TagName.FJ_LoginTime,TagFormat.TLV_TIMESTAMP,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[2]);
						strut.AddTagKey(TagName.FJ_Log_Type,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[3]);
						strut.AddTagKey(TagName.FJ_LoginIP,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));
						structList[i-index]=strut;
						//rows+=1;

					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_Login_Query,5);

				}
				else
				{
					return Message.COMMON_MES_RESP("�����û����ؼ�¼",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Login_Query,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("û�и���ҵ���ؼ�¼!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Login_Query,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region ��ѯ��ҵȼ���Ǯ����
		/// <summary>
		/// ��ѯ��ҵȼ���Ǯ����
		/// </summary>
		/// <returns></returns>
		public Message FJ_PlayerLevelMoney(int index,int pageSize)
		{
			DataSet ds = null;
			string serverIP = null;
			int max = 0;
			try
			{
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				TLV_Structure TLVstrut = new TLV_Structure(TagName.FJ_Max,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Max).m_bValueBuffer);
				max =(int)TLVstrut.toInteger();				
				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ҵȼ���Ǯ����!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ҵȼ���Ǯ����!");
				ds = CharacterInfo.LevelMoneySort_Query(serverIP,max);
				if(ds!=null && ds.Tables[0].Rows.Count>1)
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
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length+1);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserID,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[1]);
						strut.AddTagKey(TagName.FJ_UserNick,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[2]));
						strut.AddTagKey(TagName.FJ_Level,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[3]));
						strut.AddTagKey(TagName.FJ_GCash,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 5, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));
						structList[i-index]=strut;

					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerLevelMoney_Query_Resp,5);

				}
				else
				{
					return Message.COMMON_MES_RESP("û����ؼ�¼",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerLevelMoney_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("û�е���ؼ�¼!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerLevelMoney_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region ��Ҽ������
		/// <summary>
		/// ��Ҽ������
		/// </summary>
		/// <returns></returns>
		public Message FJ_JointGuild()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			//string account = null;
			string charName = null;
			int guidID = 0;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				//account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				charName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				strut = new TLV_Structure(TagName.FJ_GuidID,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_GuidID).m_bValueBuffer);
				guidID =(int)strut.toInteger();
				result = CharacterInfo.FJ_JointGuild(operateUserID,serverIP,charName,guidID);
				if (result == -1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + charName + "���ʺŲ�����");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + charName + "���ʺŲ�����!");
					return Message.COMMON_MES_RESP("����ҵĲ�����!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Guild_Create_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				else if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"���"+charName+"������ɳɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+charName+"������ɳɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Guild_Create_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"���"+charName+"�������ʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+charName+"�������ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Guild_Create_Resp);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ҵĲ�����",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Guild_Create_Resp);
			}

		}
		#endregion
		#region ����˳�����
		/// <summary>
		/// ����˳�����
		/// </summary>
		/// <returns></returns>
		public Message FJ_DeleteGuild()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			//string account = null;
			string charName = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				//account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				charName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				result = CharacterInfo.FJ_DeleteGuild(operateUserID,serverIP,charName);
				if (result == -1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + charName + "���ʺŲ�����");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + charName + "���ʺŲ�����!");
					return Message.COMMON_MES_RESP("����ҵĲ�����!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Guild_Delete_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				else if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"���"+charName+"�˳����ɳɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+charName+"�˳����ɳɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Guild_Delete_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"���"+charName+"�˳�����ʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+charName+"�˳�����ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Guild_Delete_Resp);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ҵĲ�����!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Guild_Delete_Resp);
			}

		}
		#endregion
		#region �����Ҽ���
		/// <summary>
		/// �����Ҽ���
		/// </summary>
		/// <returns></returns>
		public Message FJ_PlayerSkill_Insert()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string charName = null;
			int skillID = 0;
			int skill = 0;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				charName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				strut = new TLV_Structure(TagName.FJ_SkillID,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_SkillID).m_bValueBuffer);
				skillID =(int)strut.toInteger();
				strut = new TLV_Structure(TagName.FJ_Level,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Level).m_bValueBuffer);
				skill =(int)strut.toInteger();
				result = CharacterInfo.FJ_PlayerSkill_Insert(operateUserID,serverIP,charName,skillID,skill);
				if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"������"+charName+"���ܳɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"������"+charName+"���ܳɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerSkill_Insert_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"������"+charName+"����ʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"������"+charName+"����ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerSkill_Insert_Resp);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ҵĲ�����!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerSkill_Insert_Resp);
			}
		}
		#endregion
		#region �޸���Ҽ���
		/// <summary>
		/// �޸���Ҽ���
		/// </summary>
		/// <returns></returns>
		public Message FJ_PlayerSkill_Update()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string charName = null;
			int skillID = 0;
			int skill_lvl = 0;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				charName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				strut = new TLV_Structure(TagName.FJ_SkillID,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_SkillID).m_bValueBuffer);
				skillID =(int)strut.toInteger();
				strut = new TLV_Structure(TagName.FJ_Level,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Level).m_bValueBuffer);
				skill_lvl =(int)strut.toInteger();
				result = CharacterInfo.FJ_Skill_Update(operateUserID,serverIP,skill_lvl,charName,skillID);
				if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"�޸����"+charName+"���ܳɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�޸����"+charName+"���ܳɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerSkill_Update_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"�޸����"+charName+"����ʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�޸����"+charName+"����ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerSkill_Update_Resp);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ҵĲ�����!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerSkill_Update_Resp);
			}

		}
		#endregion
		#region ɾ����Ҽ���
		/// <summary>
		/// ɾ����Ҽ���
		/// </summary>
		/// <returns></returns>
		public Message FJ_PlayerSkill_Delete()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string charName = null;
			int skillID = 0;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				charName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				strut = new TLV_Structure(TagName.FJ_SkillID,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_SkillID).m_bValueBuffer);
				skillID =(int)strut.toInteger();
				result = CharacterInfo.FJ_Skill_Delete(operateUserID,serverIP,charName,skillID);
				if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"ɾ�����"+charName+"���ܳɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"ɾ�����"+charName+"���ܳɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerSkill_Delete_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"ɾ�����"+charName+"����ʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"ɾ�����"+charName+"����ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerSkill_Delete_Resp);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ҵĲ�����!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerSkill_Delete_Resp);
			}

		}
		#endregion
		#region ��ѯ���ʦͽ��ϵ
		/// <summary>
		/// ��ѯ���ʦͽ��ϵ
		/// </summary>
		/// <returns></returns>
		public Message FJ_SocialRelate_Query()
		{
			DataSet ds = null;
			string serverIP = null;
			//string account = null;
			string char_name = null;
			int type = 0 ;
			try
			{
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				char_name = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				TLV_Structure tlvstrut = new TLV_Structure(TagName.FJ_Type,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Type).m_bValueBuffer);
				type =(int)tlvstrut.toInteger();
				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+char_name+"����ϵ!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+char_name+"����ϵ!");
				ds = CharacterInfo.FJ_SocialRelate_Query(serverIP,char_name,type);
				Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserNick,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[1]);
						strut.AddTagKey(TagName.FJ_RelateCHarName,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[2]));
						strut.AddTagKey(TagName.FJ_Relate, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						structList[i]=strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_SocialRelate_Query_Resp,3);

				}
				else
				{
					return Message.COMMON_MES_RESP("û�и���ҵ������Ϣ",Msg_Category.FJ_ADMIN,ServiceKey.FJ_SocialRelate_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("û�и���ҵ������Ϣ!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_SocialRelate_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region �������ʦͽ��ϵ
		/// <summary>
		/// �������ʦͽ��ϵ
		/// </summary>
		/// <returns></returns>
		public Message FJ_SocialRelate_Insert()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			//string account = null;
			string charName = null;
			string relateCharName = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				//account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				charName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				relateCharName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_RelateCHarName).m_bValueBuffer);
				TLV_Structure tlvstrut = new TLV_Structure(TagName.FJ_Type,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Type).m_bValueBuffer);
				int type =(int)tlvstrut.toInteger();
				result = CharacterInfo.FJ_SocialRelate_Insert(operateUserID,serverIP,charName,relateCharName,type);
				if (result == -1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + charName + "���ʺŲ�����");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + charName + "���ʺŲ�����!");
					return Message.COMMON_MES_RESP("���ʺŲ�����!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_SocialRelate_Create_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				else if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"�������"+charName+"����ϵ�ɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�������"+charName+"����ϵ�ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_SocialRelate_Create_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"�������"+charName+"����ϵʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�������"+charName+"����ϵʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_SocialRelate_Create_Resp);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ҵĲ�����!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_SocialRelate_Create_Resp);
			}

		}
		#endregion
		#region ������ʦͽ��ϵ
		/// <summary>
		/// ������ʦͽ��ϵ
		/// </summary>
		/// <returns></returns>
		public Message FJ_SocialRelate_Delete()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			//string account = null;
			string charName = null;
			string relateCharName = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				charName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				relateCharName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_RelateCHarName).m_bValueBuffer);
				TLV_Structure tlvstrut = new TLV_Structure(TagName.FJ_Type,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Type).m_bValueBuffer);
				int type =(int)tlvstrut.toInteger();
				result = CharacterInfo.FJ_SocialRelate_Delete(operateUserID,serverIP,charName,relateCharName,type);
				if (result == -1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + charName + "���ʺŲ�����");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + charName + "���ʺŲ�����!");
					return Message.COMMON_MES_RESP("����ҵ��ʺŲ�����!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_SocialRelate_Delete_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				else if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"������"+charName+"����ϵ�ɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"������"+charName+"����ϵ�ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_SocialRelate_Delete_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"������"+charName+"����ϵʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"������"+charName+"����ϵʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_SocialRelate_Delete_Resp);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ҵĲ�����!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_SocialRelate_Delete_Resp);
			}

		}
		#endregion
		#region ��ѯ��һ�����Ϣ
		/// <summary>
		/// ��ѯ��һ�����Ϣ
		/// </summary>
		/// <returns></returns>
		public Message FJ_Wedding_Query()
		{
			DataSet ds = null;
			string serverIP = null;
			string account = null;
			string char_name = null;
			try
			{
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				char_name = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"��ͼ����λ��!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"��ͼ����λ��!");
				ds = CharacterInfo.FJ_Wedding_Query(serverIP,char_name);
				Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserNick,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[1]);
						strut.AddTagKey(TagName.FJ_RelateCHarName,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						structList[i]=strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_Wedding_Query_Resp,3);

				}
				else
				{
					return Message.COMMON_MES_RESP("�����û�����������Ϣ",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Wedding_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("û�и���ҵ������Ϣ!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Wedding_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region ������һ�����ϵ
		/// <summary>
		/// ������һ�����ϵ
		/// </summary>
		/// <returns></returns>
		public Message FJ_CreateWedding()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string account = null;
			string charName = null;
			string relateCharName = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				charName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				relateCharName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_RelateCHarName).m_bValueBuffer);
				result = CharacterInfo.FJ_CreateWedding(operateUserID,serverIP,charName,relateCharName);
				if (result == -1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "���ʺŲ�����");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "���ʺŲ�����!");
					return Message.COMMON_MES_RESP("����ҵĲ�����!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Wedding_Create_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				else if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"�������"+account+"������ϵ�ɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�������"+account+"������ϵ�ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Wedding_Create_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"�������"+account+"������ϵʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�������"+account+"������ϵʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Wedding_Create_Resp);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ҵĲ�����!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Wedding_Create_Resp);
			}

		}
		#endregion
		#region �����һ�����ϵ
		/// <summary>
		/// �����һ�����ϵ
		/// </summary>
		/// <returns></returns>
		public Message FJ_DeleteWedding()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string account = null;
			string charName = null;
			string relateCharName = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				charName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				relateCharName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_RelateCHarName).m_bValueBuffer);
				result = CharacterInfo.FJ_DeleteWedding(operateUserID,serverIP,charName,relateCharName);
				if (result == -1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + account + "���ʺŲ�����");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + CommonInfo.serverIP_Query(serverIP) + "���" + account + "���ʺŲ�����!");
					return Message.COMMON_MES_RESP("����ҵĲ�����!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Wedding_Delete_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				else if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"������"+account+"������ϵ�ɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"������"+account+"������ϵ�ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Wedding_Delete_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+serverIP+"������"+account+"������ϵʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"������"+account+"������ϵʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Wedding_Delete_Resp);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ҵĲ�����!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Wedding_Delete_Resp);
			}

		}
		#endregion
		#region ��ҽ�ɫɾ��
		/// <summary>
		/// ��ҽ�ɫɾ��
		/// </summary>
		/// <returns></returns>
		public Message FJ_Role_Delete()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string nickName = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				nickName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				result = CharacterInfo.FJ_Role_Delete(operateUserID, serverIP,nickName);
				if (result == 1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + nickName + "��ɫɾ���ɹ�!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + nickName + "��ɫɾ���ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Delete_Character_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + nickName + "��ɫɾ��ʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + nickName + "��ɫɾ��ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Delete_Character_Resp);
				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("��ɫɾ��ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Delete_Character_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion
		#region ��ҽ�ɫ�ָ�
		/// <summary>
		/// ��ҽ�ɫ�ָ�
		/// </summary>
		/// <returns></returns>
		public Message FJ_Role_Recovery()
		{
			int result = -1;
			int operateUserID = 0;
			string serverIP = null;
			string nickName = null;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID = (int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				nickName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				result = CharacterInfo.FJ_RoleRecover(operateUserID, serverIP,nickName);
				if (result == 1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + nickName + "��ɫ�ָ��ɹ�!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + nickName + "��ɫ�ָ��ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Recovery_Character_Resp);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ" + serverIP + "���" + nickName + "��ɫ�ָ�ʧ��!");
					Console.WriteLine(DateTime.Now + " - ���֮��+>��������ַ" + serverIP + "���" + nickName + "��ɫ�ָ�ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Recovery_Character_Resp);
				}
			}
			catch (System.Exception e)
			{
				return Message.COMMON_MES_RESP("��ɫ�ָ�ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_Recovery_Character_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion
		#region ����ѯ
		/// <summary>
		/// ����ѯ
		/// </summary>
		/// <returns></returns>
		public Message FJ_Guild_Query()
		{
			DataSet ds = null;
			string serverIP = null;
			string guildName = null;
			try
			{
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				guildName = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_GuildName).m_bValueBuffer);

				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����б�!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"����б�!");
				ds = CharacterInfo.Guild_Query(serverIP,guildName);
				Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]));
						strut.AddTagKey(TagName.FJ_GuidID,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[1]);
						strut.AddTagKey(TagName.FJ_GuildName, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						structList[i]=strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_Guild_QueryAll_RESP,2);

				}
				else
				{
					return Message.COMMON_MES_RESP("û�а���б�!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Guild_QueryAll_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("û�а���б�!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Guild_QueryAll_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region ��ѯ������г�Ա
		/// <summary>
		/// ��ѯ������г�Ա
		/// </summary>
		/// <returns></returns>
		public Message FJ_Guild_Member_Query(int index,int pageSize)
		{
			string serverIP = null;
			int guidID = 0;
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				TLV_Structure tlvstrut = new TLV_Structure(TagName.FJ_GuidID, 4, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_GuidID).m_bValueBuffer);
				guidID = (int)tlvstrut.toInteger();
				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"������г�Ա!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"������г�Ա!");
				ds = CharacterInfo.GuildMember_Query(serverIP,guidID);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					//��ҳ��
					int pageCount = 0;
					pageCount = (ds.Tables[0].Rows.Count) % pageSize;
					if (pageCount > 0)
					{
						pageCount = (ds.Tables[0].Rows.Count) / pageSize + 1;
					}
					else
						pageCount = (ds.Tables[0].Rows.Count) / pageSize;
					if (index + pageSize > (ds.Tables[0].Rows.Count))
					{
						pageSize = (ds.Tables[0].Rows.Count) - index;
					}
					Query_Structure[] structList = new Query_Structure[pageSize];
					//int rows = 0;
					for (int i = index; i < index + pageSize; i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length+1);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserID,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i][1].ToString());
						strut.AddTagKey(TagName.FJ_UserNick, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i][2].ToString()));
						strut.AddTagKey(TagName.FJ_GuildLvl, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));
						structList[i-index] = strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_GuildMember_Query_RESP,4);
				}
				else
				{
					return Message.COMMON_MES_RESP("�ð��û�г�Ա�б�",Msg_Category.FJ_ADMIN,ServiceKey.FJ_GuildMember_Query_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("�ð��û�г�Ա�б�", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GuildMember_Query_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}	
		#endregion
		#region ��ѯ��Ҽ�����Ϣ
		/// <summary>
		/// ��ѯ��Ҽ�����Ϣ
		/// </summary>
		/// <returns></returns>
		public Message FJ_SkillLvl_Query()
		{
			DataSet ds = null;
			string serverIP = null;
			string char_name = null;
			try
			{
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				char_name = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+char_name+"��ؼ�����Ϣ!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+char_name+"��ؼ�����Ϣ!");
				ds = CharacterInfo.FJ_SkillLvl_Query(serverIP,char_name);
				Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserNick,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[1]));
						strut.AddTagKey(TagName.FJ_SkillID,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[2]);
						strut.AddTagKey(TagName.FJ_Skill,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[3]));
						strut.AddTagKey(TagName.FJ_Level,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[4]));
						strut.AddTagKey(TagName.FJ_isDel,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						structList[i]=strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_Wedding_Query_Resp,5);

				}
				else
				{
					return Message.COMMON_MES_RESP("�����û����ؼ�����Ϣ",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerSkill_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("�����û����ؼ�����Ϣ!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_PlayerSkill_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region ��ѯ������Ϣ�б�
		/// <summary>
		/// ��ѯ������Ϣ�б�
		/// </summary>
		/// <returns></returns>
		public Message FJ_SkillList_Query()
		{
			DataSet ds = null;
			int professionid = 0;
			try
			{
				TLV_Structure struts = new TLV_Structure(TagName.FJ_Occupation_id, 4, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Occupation_id).m_bValueBuffer);
				professionid = (int)struts.toInteger();
				SqlHelper.log.WriteLog("������֮��+>������Ϣ�б�");
				Console.WriteLine(DateTime.Now+" - ������֮��+>������Ϣ�б�");
				ds = CharacterInfo.FJ_SkillList_Query(professionid);
				Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]));
						strut.AddTagKey(TagName.FJ_SkillID,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[1]);
						strut.AddTagKey(TagName.FJ_Skill,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[2]));
						strut.AddTagKey(TagName.FJ_Level,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						structList[i]=strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_TempPassword_Query_Resp,3);

				}
				else
				{
					return Message.COMMON_MES_RESP("û�м�����Ϣ�б�",Msg_Category.FJ_ADMIN,ServiceKey.FJ_TempPassword_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				//SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("û�м�����Ϣ�б�!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_TempPassword_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region �޳���ҵ�½��������
		/// <summary>
		/// �޳���ҵ�½��������
		/// </summary>
		/// <returns></returns>
		public Message FJ_ExceptLogin_delete()
		{
			int result = -1;
			string serverIP = null;
			string city = null;
			string account = null;
			int operateUserID = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				result = CharacterInfo.FJExceptLogin_Del(operateUserID,serverIP,city,account);
				if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�޳����"+account+"��½��������ɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"��½��������ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_ExceptLogin_Delete_Resp);
				}
				else if(result==-1)
				{
					return Message.COMMON_MES_RESP("�����û�п���!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_ExceptLogin_Delete_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);

				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"��½��������ʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"��½��������ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_ExceptLogin_Delete_Resp); 
				}
			}
			catch(System.Exception ex)
			{
				return Message.COMMON_MES_RESP(ex.Message,Msg_Category.FJ_ADMIN,ServiceKey.FJ_ExceptLogin_Delete_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region ��ѯ���п�ͨ��ɫͨ�������
		/// <summary>
		///  ��ѯ���п�ͨ��ɫͨ�������
		/// </summary>
		/// <returns></returns>
		public Message FJGreenGate_Query(int index,int pageSize)
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
				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"���п�ͨ��ɫͨ��״̬!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"���п�ͨ��ɫͨ��״̬!");
				ds = CharacterInfo.FJGreenGate_Query(serverIP,city,account);	
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,index,pageSize,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_GreenGate_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�п�ͨ��ɫͨ��",Msg_Category.FJ_ADMIN,ServiceKey.FJ_GreenGate_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("û�п�ͨ��ɫͨ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GreenGate_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region ��ͨ��ɫͨ��
		/// <summary>
		/// ��ͨ��ɫͨ��
		/// </summary>
		/// <returns></returns>
		public Message FJ_GreenGate_Open()
		{
			int result = -1;
			string serverIP = null;
			string city = null;
			string account = null;
			int operateUserID = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				result = CharacterInfo.FJGreenGate_Open(operateUserID,serverIP,city,account);
				if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ͨ���"+account+"��ɫͨ���ɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ͨ���"+account+"��ɫͨ���ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_GreenGate_Open_Resp);
				}
				else if(result==-1)
				{
					return Message.COMMON_MES_RESP("�����û�п�ͨ��ɫͨ��!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GreenGate_Open_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ͨ���"+account+"��ɫͨ��ʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��ͨ���"+account+"��ɫͨ��ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_GreenGate_Open_Resp); 
				}
			}
			catch(System.Exception ex)
			{
				return Message.COMMON_MES_RESP(ex.Message,Msg_Category.FJ_ADMIN,ServiceKey.FJ_ExceptLogin_Delete_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region �ر���ɫͨ��
		/// <summary>
		/// �ر���ɫͨ��
		/// </summary>
		/// <returns></returns>
		public Message FJ_GreenGate_Close()
		{
			int result = -1;
			string serverIP = null;
			string city = null;
			string account = null;
			int operateUserID = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				result = CharacterInfo.FJGreenGate_Close(operateUserID,serverIP,city,account);
				if(result==1)
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�ر����"+account+"��ɫͨ���ɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�ر����"+account+"��ɫͨ���ɹ�!");
					return Message.COMMON_MES_RESP("SUCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_GreenGate_Close_Resp);
				}
				else if(result==-1)
				{
					return Message.COMMON_MES_RESP("�����û�п�ͨ��ɫͨ��!", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GreenGate_Close_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);

				}
				else
				{
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�ر����"+account+"��ɫͨ��ʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"�ر����"+account+"��ɫͨ��ʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_GreenGate_Close_Resp); 
				}
			}
			catch(System.Exception ex)
			{
				return Message.COMMON_MES_RESP(ex.Message,Msg_Category.FJ_ADMIN,ServiceKey.FJ_ExceptLogin_Delete_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region GM�ʺŵļ�¼��ѯ
		/// <summary>
		/// GM�ʺŵļ�¼��ѯ
		/// </summary>
		/// <returns></returns>
		public Message FJ_GMAccounts_Query()
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
				city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				charName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+charName+"GM�ʺŲ�������־!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+charName+"GM�ʺŲ�������־!");
				//�������з�������б�
				ds = CharacterInfo.GMAccounts_Query(serverIP,city,charName);
				Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserID,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[1]));
						strut.AddTagKey(TagName.FJ_Level, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						structList[i]=strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_GMUser_Qyery_Resp,2);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�и�GM�ʺ�",Msg_Category.FJ_ADMIN,ServiceKey.FJ_GMUser_Qyery_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				return Message.COMMON_MES_RESP("û�и�GM�ʺ�", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GMUser_Qyery_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion
		#region GM�ʺ�ͳ����Ϣ
		public Message FJ_GMAccountsDesc_Query(int index,int pageSize)
		{
			string serverIP = null;
	
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);

				SqlHelper.log.WriteLog("������֮��+>GM�ʺ�ͳ����Ϣ!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>GM�ʺ�ͳ����Ϣ!");
	
				ds = CharacterInfo.GMAccountDesc_QueryAll(serverIP);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
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
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length+1);

						string strCity = "";
						strCity = CommonInfo.serverIP_Query(ds.Tables[0].Rows[i].ItemArray[0].ToString());
						if(strCity == null)
						{
							strCity = "";
						}
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,strCity);
						strut.AddTagKey(TagName.FJ_ServerIP,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[1]));
						strut.AddTagKey(TagName.FJ_Gmlevel,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[2]));
						strut.AddTagKey(TagName.FJ_Gmcount, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,pageCount);
					    strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						structList[i-index]=strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_GMAccountDesc_Query_Resp,4);
				}
				else
				{
					return Message.COMMON_MES_RESP("û���ʺ���Ϣ",Msg_Category.FJ_ADMIN,ServiceKey.FJ_GMAccountDesc_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				return Message.COMMON_MES_RESP("û���ʺ���Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_GMAccountDesc_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion
		#region �����ʺ�
		public Message PlayerAccount_Create()
		{
			string serverIP = null;
			string gamename = null;
			int operateUserID = 0;
			string NyUserID = null;
			string NyPassWD = null;
			string City = null;
			string strGuid = null;
			string strTitle = null;
			string sender = null;
			int startNum  = 0;
			int endNum  = 0;
			string StrConet ="�ʺ�         ����          ����ʱ��          ����\r\n";
			DateTime endDate = new DateTime();
			int result = -1;
			string[] arrSupervisors;
			string[] arrSong = "CN=��¶/OU=���ݹ���/OU=���ݹ�������/O=runstar;CN=����/OU=���ݹ���/OU=���ݹ�������/O=runstar;CN=�»���/OU=���ݹ���/OU=���ݹ�������/O=runstar".Split(";".ToCharArray());
			string[] arrSong1 = "CN=��¶/OU=���ݹ���/OU=���ݹ�������/O=runstar;CN=����/OU=���ݹ���/OU=���ݹ�������/O=runstar;CN=�»���/OU=���ݹ���/OU=���ݹ�������/O=runstar;CN=��η/OU=���ܲ�/OU=��Ʒ��Ӫ����/O=runstar".Split(";".ToCharArray());
			string[] arrSong2 = "CN=��¶/OU=���ݹ���/OU=���ݹ�������/O=runstar;CN=����/OU=���ݹ���/OU=���ݹ�������/O=runstar;CN=�»���/OU=���ݹ���/OU=���ݹ�������/O=runstar;CN=���/OU=���ܲ�/OU=��Ʒ��Ӫ����/O=runstar".Split(";".ToCharArray());
		
			//NOTES_SUPERVISORS
			DataSet ds;
			try
			{
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				operateUserID =(int)strut.toInteger();
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				NyUserID = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				NyPassWD = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Pwd).m_bValueBuffer);
				City = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				strTitle = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Content).m_bValueBuffer);
				arrSupervisors = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.NOTES_SUPERVISORS).m_bValueBuffer).ToString().Split(";".ToCharArray());
				sender = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.NOTES_SUPERVISORS).m_bValueBuffer).ToString();
				strut = new TLV_Structure(TagName.FJ_StartNum,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_StartNum).m_bValueBuffer);
				startNum =(int)strut.toInteger();
				strut = new TLV_Structure(TagName.FJ_EndNum,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_EndNum).m_bValueBuffer);
				endNum =(int)strut.toInteger();
				strut = new TLV_Structure(TagName.FJ_EndTime, 3, msg.m_packet.m_Body.getTLVByTag(TagName.FJ_EndTime).m_bValueBuffer);
				endDate = strut.toDate();
				strGuid = Guid.NewGuid().ToString();
				gamename = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_GameName).m_bValueBuffer);

				if(gamename == "���֮��")
				{
					result = CharacterInfo.CreatePlayerAccount(operateUserID,serverIP,NyUserID,NyPassWD,City,startNum,endNum,endDate,strGuid,sender,strTitle);
				}
				else if(gamename == "��������")
				{
					result = CharacterInfo.SdoCreatePlayerAccount(operateUserID,serverIP,NyUserID,NyPassWD,City,startNum,endNum,endDate,strGuid,sender,strTitle);
				}
				else if(gamename == "���߷ɳ�")
				{
					result = CharacterInfo.RcCreatePlayerAccount(operateUserID,serverIP,NyUserID,NyPassWD,City,startNum,endNum,endDate,strGuid,sender,strTitle,NyPassWD);
				}


				if(result==1)
				{
//					ds = CharacterInfo.CreateFJAccount_Query(strGuid);
//					if(ds!=null && ds.Tables[0].Rows.Count>0)
//					{
//						for(int i=0;i<ds.Tables[0].Rows.Count;i++)
//
//						{
//							
//							StrConet += ds.Tables[0].Rows[i].ItemArray[0]+"     ";
//							StrConet += ds.Tables[0].Rows[i].ItemArray[1]+"     ";
//							StrConet += ds.Tables[0].Rows[i].ItemArray[2]+"     ";
//							StrConet += ds.Tables[0].Rows[i].ItemArray[3]+"     ";
//							StrConet += "\r\n";
//						}
//					}
//					
//					if(gamename == "���֮��")
//					{
//						arrSong = arrSong2;
//					}
//					else if (gamename == "��������")
//					{
//						arrSong = arrSong1;
//					}
//
//					NotesSessionClass pNotesSessionClass = new NotesSessionClass();
//					NotesUtils pNotesUtils = new NotesUtils(pNotesSessionClass,"mail9you/runstar","mail\\��¶.nsf");
//					if (pNotesUtils.OpenDataBase("��¶","12341234"))
//					{
//						pNotesUtils.SendMailInfo(arrSupervisors,arrSong,"",strTitle,StrConet);
//						StrConet = "";
//
//					}


					SqlHelper.log.WriteLog("���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��������ʺ�"+NyUserID+"��"+startNum+"��"+endNum+"��Ϣ�ɹ�!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��������ʺ�"+NyUserID+"��"+startNum+"��"+endNum+"��Ϣ�ɹ�!");
					return Message.COMMON_MES_RESP("SUCCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_CreateAccount_Insert_Resp);

				}
				else
				{	
					SqlHelper.ExecCommand("delete from FjAccounts where StrGuid = '"+ strGuid +"'");
					SqlHelper.log.WriteLog("���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��������ʺ�"+NyUserID+"��"+startNum+"��"+endNum+"��Ϣʧ��!");
					Console.WriteLine(DateTime.Now+" - ���֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"��������ʺ�"+NyUserID+"��"+startNum+"��"+endNum+"��Ϣʧ��!");
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_CreateAccount_Insert_Resp);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("����ʧ��", Msg_Category.FJ_ADMIN, ServiceKey.FJ_CreateAccount_Insert_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region �����ʺ�
		public Message IsCreateFJAccount_Query()
		{
			string serverIP = null;
			string city = null;
			string account = null;
			string gamename = null;

			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				city = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				gamename = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_GameName).m_bValueBuffer);

				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"������Ϣ!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+account+"������Ϣ!");
				if(gamename == "���֮��")
				{
					ds = CharacterInfo.IsFJAccount_Query(serverIP,city,account);	
				}
				else if(gamename == "��������")
				{
					ds = CharacterInfo.IsSdoAccount_Query(serverIP,account);
				}
				else if(gamename == "���߷ɳ�")
				{
					ds = CharacterInfo.IsRcAccount_Query(serverIP,account);
				}
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserID,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						structList[i] = strut;
					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_IsCreateAccount_Query_Resp,1);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�и�����ʺ���Ϣ",Msg_Category.FJ_ADMIN,ServiceKey.FJ_IsCreateAccount_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("�쳣", Msg_Category.FJ_ADMIN, ServiceKey.FJ_IsCreateAccount_Query_Resp, TagName.Status, TagFormat.TLV_STRING);
			}
		}
		#endregion
		#region ������ʱ�ʺ�
		public Message TempCreateAccount_Query(int index,int pageSize)
		{
			string strtitle = null;
			string strgamename = null;
	
			DataSet ds = null;
			try
			{
				strtitle = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Content).m_bValueBuffer);
				strgamename = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_Command_Content).m_bValueBuffer);
				SqlHelper.log.WriteLog("������������������ʺ���Ϣ!");
				Console.WriteLine(DateTime.Now+" - ������������������ʺ���Ϣ!");
				ds = CharacterInfo.TempAccount_Query(strgamename,strtitle);	
				if(ds!=null && ds.Tables[0].Rows.Count>0)
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
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length+1);

						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.FJ_UserID,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, ds.Tables[0].Rows[i].ItemArray[1]);
						strut.AddTagKey(TagName.FJ_Pwd, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[2].ToString());
						strut.AddTagKey(TagName.ServerInfo_City,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);	

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[3].ToString());
						strut.AddTagKey(TagName.FJ_EndTime,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);
						
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[4]);
						strut.AddTagKey(TagName.FJ_NotesSender,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[5]);
						strut.AddTagKey(TagName.FJ_Gamename,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[6]);
						strut.AddTagKey(TagName.FJ_titlename,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[7]));
						strut.AddTagKey(TagName.FJ_Isuse, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[8]));
						strut.AddTagKey(TagName.FJ_Accontnum, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);


						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,pageCount);
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, (uint)bytes.Length, bytes);
						structList[i-index]=strut;

					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_CreateTempAccont_Query,10);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�и�����ʺ���Ϣ",Msg_Category.FJ_ADMIN,ServiceKey.FJ_CreateTempAccont_Query,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Message.COMMON_MES_RESP("û�и�����ʺ���Ϣ", Msg_Category.FJ_ADMIN, ServiceKey.FJ_CreateTempAccont_Query, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}

		#endregion
		#region ��ѯ��ҽ��۳����ּ�¼
		/// <summary>
		///  ��ѯ��ҽ��۳����ּ�¼
		/// </summary>
		/// <returns></returns>
		public Message FJUserConsume_Query(int index,int pageSize)
		{
			string account = null;
			DataSet ds = null;
			try
			{

				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>���"+account+"�۳����ּ�¼!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>���"+account+"�۳����ּ�¼!");
				ds = CharacterInfo.FJUserConsume_Query(account);	
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.buildTLV(ds,index,pageSize,false);
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_AccountGold_Query_RESP,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("û�и���ҿ۳����ּ�¼",Msg_Category.FJ_ADMIN,ServiceKey.FJ_AccountGold_Query_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+"192.168.0.125"+ex.Message);
				return Message.COMMON_MES_RESP("û�и���ҿ۳����ּ�¼", Msg_Category.FJ_ADMIN, ServiceKey.FJ_AccountGold_Query_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion
		#region ��ѯ����������ɾ���Ľ�ɫ��Ϣ
		/// <summary>
		/// ��ѯ����������ɾ���Ľ�ɫ��Ϣ
		/// </summary>
		/// <returns></returns>
		public Message FJ_DeletedCharacters_Query()
		{
			DataSet ds = null;
			string serverIP = null;
			string char_name = null;
			string userid=null;
			try
			{
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				char_name = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserNick).m_bValueBuffer);
				userid= System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				SqlHelper.log.WriteLog("������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+char_name+"����������ɾ���Ľ�ɫ��Ϣ!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>��������ַ"+CommonInfo.serverIP_Query(serverIP)+"���"+char_name+"����������ɾ���Ľ�ɫ��Ϣ!");
				ds = CharacterInfo.FJ_DeletedChar_Query(serverIP,char_name,userid);
				Query_Structure[] structList = new Query_Structure[ds.Tables[0].Rows.Count];
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{	
						Query_Structure strut = new Query_Structure((uint)ds.Tables[0].Rows[i].ItemArray.Length);

						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[0]);
						strut.AddTagKey(TagName.ServerInfo_City,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[1]);
						strut.AddTagKey(TagName.FJ_UserID,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[2]);
						strut.AddTagKey(TagName.FJ_UserNick,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,ds.Tables[0].Rows[i].ItemArray[3].ToString());
						strut.AddTagKey(TagName.SDO_DelTimes,TagFormat.TLV_STRING,(uint)bytes.Length,bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_TIMESTAMP,Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[4]));
						strut.AddTagKey(TagName.FJ_LastOfflineTime,TagFormat.TLV_TIMESTAMP,(uint)bytes.Length,bytes);

						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER,Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[5]));
						strut.AddTagKey(TagName.RC_DeleteFlag,TagFormat.TLV_INTEGER,(uint)bytes.Length,bytes);
						
						structList[i] = strut;

					}
					return Message.COMMON_MES_RESP(structList,Msg_Category.FJ_ADMIN,ServiceKey.FJ_DelCharinfo_Query_RESP,6);

				}
				else
				{
					return Message.COMMON_MES_RESP("�����û����ر�ɾ���Ľ�ɫ��Ϣ",Msg_Category.FJ_ADMIN,ServiceKey.FJ_DelCharinfo_Query_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}

			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
				return Message.COMMON_MES_RESP("�����û����ر�ɾ���Ľ�ɫ��Ϣ!",Msg_Category.FJ_ADMIN,ServiceKey.FJ_DelCharinfo_Query_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region �޸�ʣ�����ʱ��
		/// <summary>
		/// �޸�ʣ�����ʱ��
		/// </summary>
		/// <returns></returns>
		public Message FJ_CardTime_Update()
		{
			int result = -1;
			int GmUserID = 0;
			string serverIP = null;
			string account = null;
			int FJ_card_time = -1;

			try
			{	

				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				GmUserID =(int)strut.toInteger();
				serverIP = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_ServerIP).m_bValueBuffer);
				account = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.FJ_UserID).m_bValueBuffer);
				string city = System.Text.Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.ServerInfo_City).m_bValueBuffer);
				strut = new TLV_Structure(TagName.FJ_card_time,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_card_time).m_bValueBuffer);
				FJ_card_time =(int)strut.toInteger();
				strut = new TLV_Structure(TagName.FJ_FInt,4,msg.m_packet.m_Body.getTLVByTag(TagName.FJ_FInt).m_bValueBuffer);
				int oldtime =(int)strut.toInteger();
				
				SqlHelper.log.WriteLog("������֮��+>���"+account+"�޸�ʣ�����ʱ��!");
				Console.WriteLine(DateTime.Now+" - ������֮��+>���"+account+"�޸�ʣ�����ʱ��!");
				
				result = CharacterInfo.FJPlayerCardtime_Update(GmUserID,serverIP,city,account,FJ_card_time.ToString(),oldtime.ToString());
				if(result ==1)
				{
					return Message.COMMON_MES_RESP("SUCCESS",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Credit_time_Update_RESP);

				}
				else
				{
					return Message.COMMON_MES_RESP("FAILURE",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Credit_time_Update_RESP);
				}

			}
			catch(System.Exception ex)
			{
				return Message.COMMON_MES_RESP("����ʧ��",Msg_Category.FJ_ADMIN,ServiceKey.FJ_Credit_time_Update_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
			}

		}
		#endregion
	}
}
