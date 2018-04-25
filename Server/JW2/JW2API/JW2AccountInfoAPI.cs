      using System;
using System.Data;
using System.Text;
using GM_Server.JW2DataInfo;
using Common.Logic;
using Common.DataInfo;
using Domino;
using System.Collections;
using Common.NotesDataInfo;
using System.Runtime.InteropServices;
using lg = Common.API.LanguageAPI;
using MySql.Data.MySqlClient;
namespace GM_Server.JW2API
{
	/// <summary>
	/// JW2AccountInfo ��ժҪ˵����
	/// </summary>
	public class JW2AccountInfoAPI
	{
		Message msg = null;
		public JW2AccountInfoAPI()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		#region ���캯��
		public JW2AccountInfoAPI(byte[] packet)
		{
			msg = new Message(packet,(uint)packet.Length);
		}
		#endregion

		#region ��ѯ��ҽ�ɫ��Ϣ
		/// <summary>
		/// ��ѯ��ҽ�ɫ��Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_ACCOUNT_QUERY()
		{
			string strDesc = "";
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			string strnick = "";
			string strusername = "";
			try
			{
//				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
//				strnick = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserNick).m_bValueBuffer);
//				strusername = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ACCOUNT).m_bValueBuffer);

								serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
								strnick = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserNick).m_bValueBuffer);
								strusername = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ACCOUNT).m_bValueBuffer);

				//���ݿͻ��˷����ʺź��ǳƲ�ѯ��ҽ�ɫ��Ϣ
				//�ǳ�Ϊ��������ʺŲ�ѯ
				if(strnick=="")
				{	
					SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+strusername+lg.JW2API_RoleInfomation);
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+strusername+strusername+lg.JW2API_RoleInfomation);
					ds = JW2DataInfo.JW2AccountDataInfo.ACCOUNT_QUERY(serverIP,strusername,ref strDesc);	
				}
					//��Ϊ��������ǳ�ģ����ѯ
				else
				{	
					SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+strnick+lg.JW2API_RoleInfomation);
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+strnick+lg.JW2API_RoleInfomation);
					ds =  JW2DataInfo.JW2AccountDataInfo.USERNICK_QUERY(serverIP,strnick);	
				}	
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					
					Query_Structure[] structList = Message.JW2_buildTLV(ds,0,ds.Tables[0].Rows.Count,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ACCOUNT_QUERY_RESP,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerInformation,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ACCOUNT_QUERY_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryPlayerServerIP+serverIP+lg.JW2API_PlayerAccount+strusername+lg.JW2API_PlayerSN+strnick+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_InputContentNotCorrent, Msg_Category.JW2_ADMIN, ServiceKey.JW2_ACCOUNT_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region ��ѯ�����������
		/// <summary>
		/// ��ѯ�����������
		/// </summary>
		/// <returns></returns>
		public Message JW2_DUMMONEY_QUERY()
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int userSN = -1;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_PointAndVirtualCurrency);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_PointAndVirtualCurrency);
				ds = JW2DataInfo.JW2AccountDataInfo.DUMMONEY_QUERY(serverIP,userSN);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,0,ds.Tables[0].Rows.Count,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_DUMMONEY_QUERY_RESP,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoPointAndVirtualCurrency,Msg_Category.JW2_ADMIN,ServiceKey.JW2_DUMMONEY_QUERY_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryPointAndVirtualCurrency+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_InputContentNotCorrent, Msg_Category.JW2_ADMIN, ServiceKey.JW2_DUMMONEY_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion
		
		#region �û��ȼ��޸�
		/// <summary>
		/// �û��ȼ��޸�
		/// </summary>
		/// <returns></returns>
		public Message JW2_MODIFYLEVEL_QUERY()
		{
			string strDesc = "";
			string serverIP = "";
			int result = -1;
			string UserName = "";
			int iLevel = 0;
			int userSN = 0;
			int userbyid = 0;
			try
			{
				//ip
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				//�û���
				UserName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserID).m_bValueBuffer);
				//����Աid
				TLV_Structure strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				userbyid =(int)strut.toInteger();
				//��ɫid
				strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				//��Ҫ�޸ĵĵȼ�
				strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_Level).m_bValueBuffer);
				iLevel =(int)strut.toInteger();

				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_ModiPlayer+UserName+lg.JW2API_Level+iLevel);
				result = JW2DataInfo.JW2AccountDataInfo.MODIFYLEVEL_QUERY(serverIP,userSN,iLevel,userbyid,UserName,ref strDesc);
				if(result ==3)
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_ModiPlayer+UserName+lg.JW2API_Level+iLevel+lg.JW2API_Success);
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_MODIFYLEVEL_QUERY_RESP);
				}
				else
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_ModiPlayer+UserName+lg.JW2API_Level+iLevel+lg.JW2API_Failure);
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_MODIFYLEVEL_QUERY_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_UserLevelModi+serverIP+lg.JW2API_PlayerAccount+UserName+lg.JW2API_Level+iLevel+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_ModiPlayer+UserName+lg.JW2API_Level+iLevel+lg.JW2API_Failure, Msg_Category.JW2_ADMIN, ServiceKey.JW2_MODIFYLEVEL_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion		
		
		#region �û������޸�
		/// <summary>
		/// �û������޸�
		/// </summary>
		/// <returns></returns>
		public Message JW2_MODIFYEXP_QUERY()
		{
			string strDesc = "";
			string serverIP = "";
			int result = -1;
			int iExp = 0;
			string UserName = "";
			int userSN = 0;
			int userbyid = 0;
			try
			{
				//ip
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				//�û���
				UserName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserID).m_bValueBuffer);
				//����Աid
				TLV_Structure strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				userbyid =(int)strut.toInteger();
				//��ɫid
				strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				//��Ҫ�޸ĵľ���
				strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_Exp).m_bValueBuffer);
				iExp =(int)strut.toInteger();

				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_ModiPlayer+UserName+lg.JW2API_Experience+iExp);
				result = JW2DataInfo.JW2AccountDataInfo.MODIFYEXP_QUERY(serverIP,userSN,iExp,userbyid,UserName,ref strDesc);
				if(result ==3)
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_ModiPlayer+UserName+lg.JW2API_Experience+iExp+lg.JW2API_Success);
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_MODIFYEXP_QUERY_RESP);
				}
				else
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_ModiPlayer+UserName+lg.JW2API_Experience+iExp+lg.JW2API_Failure);
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_MODIFYEXP_QUERY_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_UserExperienceModi+serverIP+lg.JW2API_PlayerAccount+UserName+lg.JW2API_UserLevelModi+iExp+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_ModiPlayer+UserName+lg.JW2API_Experience+iExp+lg.JW2API_FailureConformAccount, Msg_Category.JW2_ADMIN, ServiceKey.JW2_MODIFYEXP_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion		

		#region �û���Ǯ�޸�
		/// <summary>
		/// �û���Ǯ�޸�
		/// </summary>
		/// <returns></returns>
		public Message JW2_MODIFY_MONEY()
		{
			string strDesc = "";
			string serverIP = "";
			int result = -1;
			int iMoney = 0;
			string UserName = "";
			int userSN = 0;
			int userbyid = 0;
			try
			{
				//ip
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				//�û���
				UserName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserID).m_bValueBuffer);
				//����Աid
				TLV_Structure strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				userbyid =(int)strut.toInteger();
				//��ɫid
				strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				//��Ҫ�޸ĵľ���
				strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_Money).m_bValueBuffer);
				iMoney =(int)strut.toInteger();

				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_ModiPlayer+UserName+lg.JW2API_Money+iMoney);
				result = JW2DataInfo.JW2AccountDataInfo.MODIFY_MONEY(serverIP,userSN,iMoney,userbyid,UserName,ref strDesc);
				if(result ==1)
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_ModiPlayer+UserName+lg.JW2API_Money+iMoney+lg.JW2API_Success);
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_MODIFYEXP_QUERY_RESP);
				}
				else
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_ModiPlayer+UserName+lg.JW2API_Money+iMoney+lg.JW2API_Failure);
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_MODIFYEXP_QUERY_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_UserMoneyModi+serverIP+lg.JW2API_PlayerAccount+UserName+lg.JW2API_Money+iMoney+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_ModiPlayer+UserName+lg.JW2API_Money+iMoney+lg.JW2API_FailureConformAccount, Msg_Category.JW2_ADMIN, ServiceKey.JW2_MODIFYEXP_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion		
		
		#region ��ѯ��ҽ��֤��
		/// <summary>
		/// ��ѯ��ҽ��֤��
		/// </summary>
		/// <returns></returns>
		public Message JW2_Wedding_Paper()
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int userSN = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				//��ɫid
				TLV_Structure strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();

				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN.ToString()+lg.JW2API_MarriageCertificate);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN.ToString()+lg.JW2API_MarriageCertificate);
				ds =  JW2DataInfo.JW2AccountDataInfo.Wedding_Paper(serverIP,userSN);	
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,0,ds.Tables[0].Rows.Count,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_Wedding_Paper_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerMarriageCertificate,Msg_Category.JW2_ADMIN,ServiceKey.JW2_Wedding_Paper_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryPlayerMarriageCertificate+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerMarriageCertificate, Msg_Category.JW2_ADMIN, ServiceKey.JW2_Wedding_Paper_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region ��ѯ��������ɶԿ�
		/// <summary>
		/// ��ѯ��������ɶԿ�
		/// </summary>
		/// <returns></returns>
		public Message JW2_CoupleParty_Card()
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int userSN = 0;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				//��ɫid
				TLV_Structure strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();

				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_BrowseServerAddress+userSN.ToString()+lg.JW2API_CouplesPartyCard);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerSN+userSN.ToString()+lg.JW2API_CouplesPartyCard);
				ds =  JW2DataInfo.JW2AccountDataInfo.CoupleParty_Card(serverIP,userSN);	
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,0,ds.Tables[0].Rows.Count,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_CoupleParty_Card_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerCouplesPartyCard,Msg_Category.JW2_ADMIN,ServiceKey.JW2_CoupleParty_Card_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryPlayerCouplesPartyCard+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerCouplesPartyCard, Msg_Category.JW2_ADMIN, ServiceKey.JW2_CoupleParty_Card_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region GM��B�޸�
		/// <summary>
		/// GM��B�޸�
		/// </summary>
		/// <returns></returns>
		public Message JW2_GM_Update()
		{
			string strDesc = "";
			string serverIP = "";
			//int uid = 0;
			int result =-1;
			int type =0;
			int userSN = 0;
			int UserByID = 0;
			string userName = "";
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				userName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserID).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				//GM ����Ա
				strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				UserByID =(int)strut.toInteger();

				strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_Type).m_bValueBuffer);
				type =(int)strut.toInteger();
				
				SqlHelper.log.WriteLog(lg.JW2API_ModiServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_GMAccount+userName+lg.JW2API_StatusSuccess);
				Console.WriteLine(DateTime.Now+lg.JW2API_ModiServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_GMAccount+userName+lg.JW2API_StatusSuccess);
				result = JW2DataInfo.JW2AccountDataInfo.GM_Update(serverIP,userSN,type,UserByID,userName,ref strDesc);
				if(result ==1)
				{
					SqlHelper.log.WriteLog(lg.JW2API_ModiServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_GMAccount+userName+lg.JW2API_StatusSuccess);
					Console.WriteLine(DateTime.Now+lg.JW2API_ModiServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_GMAccount+userName+lg.JW2API_StatusSuccess);
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_GM_Update_Resp);
				}
				else
				{
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_GM_Update_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_GMStatusModi+serverIP+lg.JW2API_PlayerAccount+userSN+lg.JW2API_Type+type+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_GMAccount+userName+lg.JW2API_StatusModiFailure, Msg_Category.JW2_ADMIN, ServiceKey.JW2_GM_Update_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region GT������2���߿��콱��Ϣ
		/// <summary>
		/// GT������2���߿�
		/// </summary>
		/// <returns></returns>
		public Message JW2_Act_Card_Query(int index, int pageSize)
		{
			string account = "";
			string card = "";
			ArrayList ds = null;
			string strDesc = "";
			try
			{
				account = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.CARD_username).m_bValueBuffer);
				card = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.CARD_id).m_bValueBuffer);
				
				if(account=="")
				{
					SqlHelper.log.WriteLog(lg.JW2API_BrowseItemCardNumber + card + lg.JW2API_PrizeInformation);
					Console.WriteLine(DateTime.Now + lg.JW2API_BrowseItemCardNumber + card + lg.JW2API_PrizeInformation);
					ds = JW2DataInfo.JW2AccountDataInfo.Act_Card_Query("1",card,ref strDesc);
				}
				else
				{
					SqlHelper.log.WriteLog(lg.JW2API_BrowseItemCardNumber + account + lg.JW2API_PrizeInformation);
					Console.WriteLine(DateTime.Now +lg.JW2API_BrowseItemCardNumber + account + lg.JW2API_PrizeInformation);
					ds = JW2DataInfo.JW2AccountDataInfo.Act_Card_Query(account,"1",ref strDesc);
				}

				if (ds != null && ds.Count > 0)
				{
					//��ҳ��
					int pageCount = 0;
					pageCount = ds.Count % pageSize;
					if (pageCount > 0)
					{
						pageCount = ds.Count / pageSize + 1;
					}
					else
						pageCount = ds.Count / pageSize;
					if (index + pageSize > ds.Count)
					{
						pageSize = ds.Count - index;
					}
					Query_Structure[] structList = new Query_Structure[pageSize];
					for (int i = index; i < index + pageSize; i++)
					{
						ArrayList colList = (ArrayList)ds[i];
						Query_Structure strut = new Query_Structure((uint)colList.Count+1);
						byte[] bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, colList[0].ToString());
						strut.AddTagKey(TagName.CARD_username, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, colList[1].ToString());
						strut.AddTagKey(TagName.AU_CARDNUM, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING,  colList[2].ToString());
						strut.AddTagKey(TagName.CARD_GetTime, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, colList[3].ToString());
						strut.AddTagKey(TagName.CARD_Area, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						bytes = TLV_Structure.ValueToByteArray(TagFormat.TLV_STRING, colList[4].ToString());
						strut.AddTagKey(TagName.SD_TypeName, TagFormat.TLV_STRING, (uint)bytes.Length, bytes);
						
						//��ҳ��
						strut.AddTagKey(TagName.PageCount, TagFormat.TLV_INTEGER, 4, TLV_Structure.ValueToByteArray(TagFormat.TLV_INTEGER, pageCount));

						structList[i - index] = strut;
					}
					return Message.COMMON_MES_RESP(structList, Msg_Category.JW2_ADMIN, ServiceKey.JW2_Act_Card_Query_Resp, 6);
				}
				else
				{
					return Message.COMMON_MES_RESP(strDesc, Msg_Category.JW2_ADMIN, ServiceKey.JW2_Act_Card_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}

			}
			catch (System.Exception ex)
			{ 
				SqlHelper.errLog.WriteLog(lg.JW2API_ItemCardPrizeInformation+account+lg.JW2API_CardNumber+card+"->"+ex.Message);
				return Message.COMMON_MES_RESP(strDesc, Msg_Category.JW2_ADMIN, ServiceKey.JW2_Act_Card_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region ��÷�����GS�б�
		/// <summary>
		/// ��÷�����GS�б�
		/// </summary>
		/// <returns></returns>
		public Message JW2_GSSvererList_Query()
		{
			string serverIP = "";
			DataSet ds = null;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_GSListQuery);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_GSListQuery);
				ds = JW2DataInfo.JW2AccountDataInfo.GSSvererList_Query(serverIP);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,0,ds.Tables[0].Rows.Count,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_GSSvererList_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoServerGSList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_GSSvererList_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				Console.WriteLine(ex.Message+"->11111111");
				return Message.COMMON_MES_RESP(lg.JW2API_NoServerGSList, Msg_Category.JW2_ADMIN, ServiceKey.JW2_GSSvererList_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region ������˫���޸�
		/// <summary>
		/// ������˫���޸�
		/// </summary>
		/// <returns></returns>
		public Message JW2_ChangeServerExp_Query()
		{
			string serverIP = "";
			string serverName = "";
			string GSserverIP = "";
			int serverNo = 0;
			string result = "";
			int port = 0;
			int iExp = 1;
			int iMoney = 1;
			int userbyid = 0;
			int type =-1;
			try
			{
				//ip				
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				//serverName
				serverName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerName).m_bValueBuffer);
				//����Աid
				TLV_Structure strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				userbyid =(int)strut.toInteger();
				//��Ҫ�޸ĵľ���
				strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_Exp).m_bValueBuffer);
				iExp =(int)strut.toInteger();
				//��Ҫ�޸ĵ�Ǯ
				strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_Money).m_bValueBuffer);
				iMoney =(int)strut.toInteger();
				//����
				strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_Type).m_bValueBuffer);
				type =(int)strut.toInteger();
				result = JW2DataInfo.JW2AccountDataInfo.ChangeServerExp_Query(serverIP,iExp,iMoney,userbyid,serverName,type);
				if(result!="")
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+serverName+lg.JW2API_ExperienceMoneyComplete);
					return Message.COMMON_MES_RESP(result,Msg_Category.JW2_ADMIN,ServiceKey.JW2_MODIFYEXP_QUERY_RESP);
				}
				else
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+serverName+lg.JW2API_ExperienceMoneyFailure);
					return Message.COMMON_MES_RESP(lg.JW2API_ServerDoubleModiError, Msg_Category.JW2_ADMIN, ServiceKey.JW2_MODIFYEXP_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
				}
				
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_ServerIP+serverIP+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_ServerIP+serverName+lg.JW2API_Failure, Msg_Category.JW2_ADMIN, ServiceKey.JW2_MODIFYEXP_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion		

		#region ��ѯ��һ�Ծ����Ϣ
		/// <summary>
		/// ��ѯ��һ�Ծ����Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_ACTIVEPOINT_QUERY()
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int userSN = 0;
			string dtBegin ="";
			string dtEnd = "";
			try
			
			{

				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();

				dtBegin = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.BeginTime ).m_bValueBuffer);
				dtEnd = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.EndTime).m_bValueBuffer);
 
				//���ݿͻ��˷����ʺź��ǳƲ�ѯ��ҽ�ɫ��Ϣ					
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerSN+userSN+lg.JW2API_LivingnessInfo);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerSN+userSN+lg.JW2API_LivingnessInfo);
				ds = JW2DataInfo.JW2AccountDataInfo.ACTIVEPOINT_ACCOUNT_QUERY(serverIP,userSN,dtBegin,dtEnd);	
			
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,0,ds.Tables[0].Rows.Count,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ACTIVEPOINT_QUERY_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoLivingnessInfo,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ACTIVEPOINT_QUERY_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryPlayerLivingnessInfo+serverIP+lg.JW2API_PlayerAccount+userSN+lg.JW2API_BeginTime+dtBegin+lg.JW2API_EndTime+dtEnd+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerStory, Msg_Category.JW2_ADMIN, ServiceKey.JW2_ACTIVEPOINT_QUERY_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		
	}
}
