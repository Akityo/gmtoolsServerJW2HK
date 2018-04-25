using System;
using System.Data;
using System.Text;
using GM_Server.JW2DataInfo;
using Common.Logic;
using Common.DataInfo;
using Domino;
using Common.NotesDataInfo;
using lg = Common.API.LanguageAPI;
namespace GM_Server.JW2API
{
	/// <summary>
	/// JW2ItemInfoAPI ��ժҪ˵����
	/// </summary>
	public class JW2ItemInfoAPI
	{
		Message msg = null;
		public JW2ItemInfoAPI()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		#region ���캯��
		public JW2ItemInfoAPI(byte[] packet)
		{
			msg = new Message(packet,(uint)packet.Length);
		}
		#endregion

		#region ��ѯ�������
		/// <summary>
		/// ��ѯ�������
		/// </summary>
		/// <returns></returns>
		public Message JW2_RPG_QUERY(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			string userName = "";
			DataSet ds = null;
			int userSN = -1;
			try
			{
//				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
//				userName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserID).m_bValueBuffer);
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				userName = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserID).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();

				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userName+lg.JW2API_Story);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userName+lg.JW2API_Story);
				ds = JW2DataInfo.JW2ItemDataInfo.RPG_QUERY(serverIP,userSN,userName);	
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_RPG_QUERY_RESP,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerStory,Msg_Category.JW2_ADMIN,ServiceKey.JW2_RPG_QUERY_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryStory+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerStory, Msg_Category.JW2_ADMIN, ServiceKey.JW2_RPG_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region �鿴������ϵ�����Ϣ
		/// <summary>
		/// �鿴������ϵ�����Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_ITEMSHOP_BYOWNER_QUERY(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int userSN = -1;
			try
			{
//				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_BodyItem);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_BodyItem);
				ds = JW2DataInfo.JW2ItemDataInfo.ITEMSHOP_BYOWNER_QUERY(serverIP,userSN);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ITEMSHOP_BYOWNER_QUERY_RESP,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerBodyItem,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ITEMSHOP_BYOWNER_QUERY_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryPlayerBodyItem+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerBodyItem, Msg_Category.JW2_ADMIN, ServiceKey.JW2_ITEMSHOP_BYOWNER_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region �鿴������Ʒ�嵥������
		/// <summary>
		/// �鿴������Ʒ�嵥������
		/// </summary>
		/// <returns></returns>
		public Message JW2_HOME_ITEM_QUERY(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			int userSN = -1;
			try
			{
//				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_RoomItemList);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_RoomItemList);
				ds = JW2DataInfo.JW2ItemDataInfo.HOME_ITEM_QUERY(serverIP,userSN);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_HOME_ITEM_QUERY_RESP,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerRoomItemList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_HOME_ITEM_QUERY_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryPlayerRoomItemList+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerRoomItemList, Msg_Category.JW2_ADMIN, ServiceKey.JW2_HOME_ITEM_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region �鿴�����Ե���
		/// <summary>
		/// �鿴�����Ե���
		/// </summary>
		/// <returns></returns>
		public Message JW2_WASTE_ITEM_QUERY(int index,int pageSize)
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
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_ConsumeItem);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_ConsumeItem);
				ds = JW2DataInfo.JW2ItemDataInfo.WASTE_ITEM_QUERY(serverIP,userSN);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_WASTE_ITEM_QUERY_RESP,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerRoomItemList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_WASTE_ITEM_QUERY_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryPlayerRoomItemList+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerRoomItemList, Msg_Category.JW2_ADMIN, ServiceKey.JW2_WASTE_ITEM_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region �鿴С����
		/// <summary>
		/// �鿴С����
		/// </summary>
		/// <returns></returns>
		public Message JW2_SMALL_BUGLE_QUERY(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			string startTime = "";
			string endTime = "";
			int userSN = -1;
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				startTime = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.BeginTime).m_bValueBuffer);
				endTime = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.EndTime).m_bValueBuffer);
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_Trumpet);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_Trumpet);
				ds = JW2DataInfo.JW2ItemDataInfo.SMALL_BUGLE_QUERY(serverIP,userSN,startTime,endTime);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_WASTE_ITEM_QUERY_RESP,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerTrumpet,Msg_Category.JW2_ADMIN,ServiceKey.JW2_WASTE_ITEM_QUERY_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryTrumpet+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerTrumpet, Msg_Category.JW2_ADMIN, ServiceKey.JW2_WASTE_ITEM_QUERY_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion

		#region ���߲�ѯ
		/// <summary>
		/// ���߲�ѯ
		/// </summary>
		/// <returns></returns>
		public Message JW2_ItemInfo_Query(int index ,int pageSize)
		{
			string serverIP = "";
			int type = -1;
			
			DataSet ds = null;
			int userSN = -1;
			try
			{
				//ip��ַ
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				//�û�SN
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				//��ѯ����
				strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ItemPos).m_bValueBuffer);
				type =(int)strut.toInteger();
				
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_ItemInfo);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_ItemInfo);
				ds = JW2DataInfo.JW2ItemDataInfo.ItemInfo_Query(serverIP,userSN,type);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ItemInfo_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerItemInfo,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ItemInfo_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryItemInfo+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerItemInfo, Msg_Category.JW2_ADMIN, ServiceKey.JW2_ItemInfo_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region ����ɾ��
		/// <summary>
		/// ����ɾ��
		/// </summary>
		/// <returns></returns>
		public Message JW2_ITEM_DEL()
		{
			string strDesc = "";
			string serverIP = "";
			int result = -1;
			int itemID = 0;
			int itemNo = 0;
			int type = -1;
			string itemName = "";
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
				//����ID
				strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_AvatarItem).m_bValueBuffer);
				itemID =(int)strut.toInteger();

				//������
				itemName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_AvatarItemName).m_bValueBuffer);

				//����
				strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ItemPos).m_bValueBuffer);
				type =(int)strut.toInteger();

				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_DelPlayer+UserName+lg.JW2API_Item+itemName);
				result = JW2DataInfo.JW2ItemDataInfo.ITEM_DEL(serverIP,userSN,userbyid,UserName,itemID,itemName,type,itemNo,ref strDesc);
				if(result ==1)
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_DelPlayer+UserName+lg.JW2API_Item+itemName+lg.JW2API_Success);
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ITEM_DEL_RESP);
				}
				else
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_DelPlayer+UserName+lg.JW2API_Item+itemName+lg.JW2API_Failure);
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ITEM_DEL_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_DelPlayer+UserName+lg.JW2API_DelPlayer+itemName+lg.JW2API_Failure, Msg_Category.JW2_ADMIN, ServiceKey.JW2_ITEM_DEL_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion		

		#region �������
		/// <summary>
		/// �������
		/// </summary>
		/// <returns></returns>
		public Message JW2_ITEM_ADD()
		{
			string serverIP = "";
			string mailTitle = "";
			string mailContent = "";
			string result = "";
			int itemNo = 0;
			int type = -1;
			string itemName = "";
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
				//�����ַ���
				itemName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_AvatarItemName).m_bValueBuffer);

				//�ʼ�����
				mailTitle = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_MailTitle).m_bValueBuffer);

				//�ʼ�����
				mailContent = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_MailContent).m_bValueBuffer);

				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_Item + itemName + lg.JW2API_PlayerAccount+UserName);
				result = JW2DataInfo.JW2ItemDataInfo.ITEM_ADD(serverIP,userSN,userbyid,UserName,itemName,mailTitle,mailContent);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_AddItem + itemName + lg.JW2API_PlayerAccount+UserName);
				return Message.COMMON_MES_RESP(result,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ADD_ITEM_RESP);
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryAddItem+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_Item+ itemName +lg.JW2API_Failure, Msg_Category.JW2_ADMIN, ServiceKey.JW2_ADD_ITEM_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion		

		#region �������(����)
		/// <summary>
		/// �������(����)
		/// </summary>
		/// <returns></returns>
		public Message JW2_ADD_ITEM_ALL()
		{
			string serverIP = "";
			string mailTitle = "";
			string mailContent = "";
			string result = "";
			int itemNo = 0;
			int type = -1;
			string itemName = "";
			string UserName = "";
			int userSN = 0;
			int userbyid = 0;
			try
			{
				//ip
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				
				//����Աid
				TLV_Structure strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				userbyid =(int)strut.toInteger();

				//�����ַ���
				itemName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_AvatarItemName).m_bValueBuffer);

				//�ʼ�����
				mailTitle = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_MailTitle).m_bValueBuffer);

				//�ʼ�����
				mailContent = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_MailContent).m_bValueBuffer);

				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_AddItemBatch);
				result = JW2DataInfo.JW2ItemDataInfo.ITEM_ADD_ALL(serverIP,userbyid,itemName,mailTitle,mailContent);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_AddItemBatch);
				return Message.COMMON_MES_RESP(result,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ADD_ITEM_ALL_RESP);
				
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryAddItemBatch+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_Failure, Msg_Category.JW2_ADMIN, ServiceKey.JW2_ADD_ITEM_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion		

		#region ���߲�ѯ(ģ����ѯ)
		/// <summary>
		/// ���߲�ѯ(ģ����ѯ)
		/// </summary>
		/// <returns></returns>
		public Message JW2_ITEM_SELECT(int index ,int pageSize)
		{
			DataSet ds = null;
			string ItemName = "";
			string type = "";
			int type_item =-1;
			try
			{
				//ip��ַ
				ItemName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ItemName).m_bValueBuffer);
				type = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_GOODSTYPE).m_bValueBuffer);
				//������/����ID
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ItemPos).m_bValueBuffer);
				type_item =(int)strut.toInteger();
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerItem);
				ds = JW2DataInfo.JW2ItemDataInfo.ITEM_SELECT(ItemName,type,type_item);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,0,ds.Tables[0].Rows.Count,false,"");
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ITEM_SELECT_RESP,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoItemInfo,Msg_Category.JW2_ADMIN,ServiceKey.JW2_ITEM_SELECT_RESP,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_ItemQuery+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoItemInfo, Msg_Category.JW2_ADMIN, ServiceKey.JW2_ITEM_SELECT_RESP, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region �鿴��ҳ�����Ϣ��ѯ
		/// <summary>
		/// �鿴��ҳ�����Ϣ��ѯ
		/// </summary>
		/// <returns></returns>
		public Message JW2_PetInfo_Query(int index,int pageSize)
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
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_PetInfo);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_QueryPetInfo);
				ds = JW2DataInfo.JW2ItemDataInfo.PetInfo_Query(serverIP,userSN);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_PetInfo_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP("NoPetInfo",Msg_Category.JW2_ADMIN,ServiceKey.JW2_PetInfo_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryPetInfo+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP("NoPetInfo", Msg_Category.JW2_ADMIN, ServiceKey.JW2_PetInfo_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region �޸ĳ�����
		/// <summary>
		/// �޸ĳ�����
		/// </summary>
		/// <returns></returns>
		public Message JW2_UpdatePetName_Query()
		{
			string strDesc = "";
			string serverIP = "";
			int result = -1;
			int itemID = 0;
			int itemNo = 0;
			int type = -1;
			string itemName = "";
			string UserName = "";
			string OLD_PetName = "";
			string PetName = "";
			int petID = 0;
			int userbyid = 0;
			try
			{
				//ip
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				//�ϳ�����
				OLD_PetName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_OLD_PETNAME).m_bValueBuffer);
				//������
				PetName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_PetName).m_bValueBuffer);
				//����Աid
				TLV_Structure strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				userbyid =(int)strut.toInteger();
				//����id
				strut = new TLV_Structure(TagName.Magic_PetID,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_PetSn).m_bValueBuffer);
				petID =(int)strut.toInteger();

				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_ModiPetName+OLD_PetName+"-"+PetName);
				result = JW2DataInfo.JW2ItemDataInfo.UpdatePetName_Query(serverIP,OLD_PetName,PetName,userbyid,petID,ref strDesc);
				if(result ==1)
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_ModiPetName+OLD_PetName+"-"+PetName+lg.JW2API_Failure);
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_UpdatePetName_Query_Resp);
				}
				else
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_OldPetName+OLD_PetName+"-"+PetName+lg.JW2API_Failure);
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_UpdatePetName_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryModiPetName+serverIP+lg.JW2API_PlayerAccount+UserName+lg.JW2API_OldPetName+OLD_PetName+lg.JW2API_PetName+PetName+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_ModiPetName+OLD_PetName+",Ϊ"+PetName+lg.JW2API_ErrorPetName, Msg_Category.JW2_ADMIN, ServiceKey.JW2_UpdatePetName_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion		

		#region �鿴��Һϳ���Ϣ
		/// <summary>
		/// �鿴��ҳ�����Ϣ��ѯ
		/// </summary>
		/// <returns></returns>
		public Message JW2_Materiallist_Query(int index,int pageSize)
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
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_MaterialList);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_MaterialList);
				ds = JW2DataInfo.JW2ItemDataInfo.Materiallist_Query(serverIP,userSN);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_PetInfo_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerMaterialList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_Materiallist_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryPlayerMaterialList+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerMaterialHistory, Msg_Category.JW2_ADMIN, ServiceKey.JW2_Materiallist_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region �鿴��Һϳ���ʷ��¼��Ϣ��Ϣ
		/// <summary>
		/// �鿴��Һϳ���ʷ��¼��Ϣ��Ϣ
		/// </summary>
		/// <returns></returns>
		public Message JW2_MaterialHistory_Query(int index,int pageSize)
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
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_MaterialHistory);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_MaterialHistory);
				ds = JW2DataInfo.JW2ItemDataInfo.MaterialHistory_Query(serverIP,userSN);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_MaterialHistory_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerMaterialHistory,Msg_Category.JW2_ADMIN,ServiceKey.JW2_MaterialHistory_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryMaterialHistory+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoPlayerMaterialHistory, Msg_Category.JW2_ADMIN, ServiceKey.JW2_MaterialHistory_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region �����Ҫ��˵�ͼƬ�б�
		/// <summary>
		/// �����Ҫ��˵�ͼƬ�б�
		/// </summary>
		/// <returns></returns>
		public Message JW2_GETPIC_Query(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			string  strusername = "";
			try
			{
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				strusername = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ACCOUNT).m_bValueBuffer);
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+strusername+lg.JW2API_GETPIC);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+strusername+lg.JW2API_GETPIC);
				ds = JW2DataInfo.JW2ItemDataInfo.GETPIC_Query(serverIP,strusername);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_GETPIC_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoGETPIC,Msg_Category.JW2_ADMIN,ServiceKey.JW2_GETPIC_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryGETPIC+serverIP+lg.JW2API_PlayerAccount+strusername+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoGETPIC, Msg_Category.JW2_ADMIN, ServiceKey.JW2_GETPIC_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion

		#region ���ͼƬ
		/// <summary>
		/// ���ͼƬ
		/// </summary>
		/// <returns></returns>
		public Message JW2_CHKPIC_Query()
		{
			string strDesc = "";
			string serverIP = "";
			int result = -1;
			int itemID = 0;
			int itemNo = 0;
			int type = -1;
			string itemName = "";
			string UserName = "";
			int userSN = 0;
			int userbyid = 0;
			string Url = "";
			try
			{
				//ip
				serverIP = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				//�û���
				UserName = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserID).m_bValueBuffer);
				//����Աid
				TLV_Structure strut = new TLV_Structure(TagName.UserByID,4,msg.m_packet.m_Body.getTLVByTag(TagName.UserByID).m_bValueBuffer);
				userbyid =(int)strut.toInteger();
				//��ɫid
				strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();

				Url = Encoding.Default.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.jw2_pic_Name).m_bValueBuffer);

				//����
				strut = new TLV_Structure(TagName.JW2_ItemPos,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ItemPos).m_bValueBuffer);
				type =(int)strut.toInteger();

				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_CHKPICPlayer+UserName+lg.JW2API_CHKPIC+Url);
				result = JW2DataInfo.JW2ItemDataInfo.CHKPIC_Query(serverIP,userSN,userbyid,UserName,Url,type,ref strDesc);
				if(result ==1)
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_CHKPICPlayer+UserName+lg.JW2API_CHKPIC+Url);
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_CHKPIC_Query_Resp);
				}
				else
				{
					Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_CHKPICPlayer+UserName+lg.JW2API_CHKPIC+Url);
					return Message.COMMON_MES_RESP(strDesc,Msg_Category.JW2_ADMIN,ServiceKey.JW2_CHKPIC_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryCHKPIC+serverIP+lg.JW2API_PlayerAccount+userSN+lg.JW2API_CHKPIC+Url+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_CHKPICPlayer+UserName+lg.JW2API_CHKPIC+itemName+lg.JW2API_Failure, Msg_Category.JW2_ADMIN, ServiceKey.JW2_CHKPIC_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}
		}
		#endregion		
		
		#region �DƬ��ʹ����r
		/// <summary>
		/// �DƬ��ʹ����r
		/// </summary>
		/// <returns></returns>
		public Message JW2_PicCard_Query(int index,int pageSize)
		{
			string serverIP = "";
			//int uid = 0;
			DataSet ds = null;
			string beginTime = "";
			string endTime = "";
			int userSN = 0;
			int BType = 0;
			int SType = 0;

			string  strusername = "";
			try
			{
				//�û�SN
				TLV_Structure strut = new TLV_Structure(TagName.JW2_UserSN,4,msg.m_packet.m_Body.getTLVByTag(TagName.JW2_UserSN).m_bValueBuffer);
				userSN =(int)strut.toInteger();
				//����
				strut = new TLV_Structure(TagName.Magic_category,4,msg.m_packet.m_Body.getTLVByTag(TagName.Magic_category).m_bValueBuffer);
				BType =(int)strut.toInteger();
				//С��
				strut = new TLV_Structure(TagName.Magic_action,4,msg.m_packet.m_Body.getTLVByTag(TagName.Magic_action).m_bValueBuffer);
				SType =(int)strut.toInteger();
				serverIP = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.JW2_ServerIP).m_bValueBuffer);
				beginTime = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.BeginTime).m_bValueBuffer);
				endTime = Encoding.Unicode.GetString(msg.m_packet.m_Body.getTLVByTag(TagName.EndTime).m_bValueBuffer);
				SqlHelper.log.WriteLog(lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+strusername+lg.JW2API_LogInfo);
				Console.WriteLine(DateTime.Now+lg.JW2API_BrowseServerAddress+CommonInfo.serverIP_Query(serverIP)+lg.JW2API_PlayerAccount+userSN+lg.JW2API_LogInfo);
				ds = JW2DataInfo.JW2ItemDataInfo.PicCard_Query(serverIP,BType,SType,userSN,beginTime,endTime);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					Query_Structure[] structList = Message.JW2_buildTLV(ds,index,pageSize,false,serverIP);
					return Message.COMMON_MES_RESP(structList,Msg_Category.JW2_ADMIN,ServiceKey.JW2_PicCard_Query_Resp,(int)structList[0].structLen);
				}
				else
				{
					return Message.COMMON_MES_RESP(lg.JW2API_NoLogInfo,Msg_Category.JW2_ADMIN,ServiceKey.JW2_PicCard_Query_Resp,TagName.ERROR_Msg,TagFormat.TLV_STRING);
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.errLog.WriteLog(lg.JW2API_QueryLogInfo+serverIP+lg.JW2API_PlayerAccount+userSN+"->"+ex.Message);
				return Message.COMMON_MES_RESP(lg.JW2API_NoLogInfo, Msg_Category.JW2_ADMIN, ServiceKey.JW2_PicCard_Query_Resp, TagName.ERROR_Msg, TagFormat.TLV_STRING);
			}

		}
		#endregion
		
		
	}
}
