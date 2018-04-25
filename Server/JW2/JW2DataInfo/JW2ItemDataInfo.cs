using System;
using System.Xml;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Text;
using Common.Logic;
using Common.API;
using MySql.Data.MySqlClient;
using Common.DataInfo;
namespace GM_Server.JW2DataInfo
{
	/// <summary>
	/// JW2ItemDataInfo ��ժҪ˵����
	/// </summary>
	public class JW2ItemDataInfo
	{
		public JW2ItemDataInfo()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		#region ��ѯ�������
		/// <summary>
		/// ��ѯ�������
		/// </summary>
		public static DataSet RPG_QUERY(string serverIP,int usersn,string userName)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,2);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_RPG_QUERY' and sql_condition='JW2_RPG_QUERY'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2itemDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_RPG_QUERY_�鿴���"+usersn.ToString()+"-"+userName+"������ڷ�����IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴������ϵ�����Ϣ
		/// <summary>
		/// �鿴������ϵ�����Ϣ
		/// </summary>
		public static DataSet ITEMSHOP_BYOWNER_QUERY(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,2);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_ITEMSHOP_BYOWNER_QUERY' and sql_condition='JW2_ITEMSHOP_BYOWNER_QUERY'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2itemDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_ITEMSHOP_BYOWNER_QUERY_�鿴���"+usersn.ToString()+"���ϵ�����Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴������Ʒ�嵥������
		/// <summary>
		/// �鿴������Ʒ�嵥������
		/// </summary>
		public static DataSet HOME_ITEM_QUERY(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,2);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_HOME_ITEM_QUERY' and sql_condition='JW2_HOME_ITEM_QUERY'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2itemDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_HOME_ITEM_QUERY_�鿴���"+usersn.ToString()+"������Ʒ�嵥�����޷�����IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴�����Ե���
		/// <summary>
		/// �鿴�����Ե���
		/// </summary>
		public static DataSet WASTE_ITEM_QUERY(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,2);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_WASTE_ITEM_QUERY' and sql_condition='JW2_WASTE_ITEM_QUERY'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2itemDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_WASTE_ITEM_QUERY_�鿴���"+usersn.ToString()+"�����Ե��߷�����IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion
		
		#region �鿴С����
		/// <summary>
		/// �鿴С����
		/// </summary>
		public static DataSet SMALL_BUGLE_QUERY(string serverIP,int usersn,string startTime,string endTime)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,2);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_SMALL_BUGLE_QUERY' and sql_condition='JW2_SMALL_BUGLE_QUERY'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn,startTime,endTime);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2itemDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_SMALL_BUGLE_QUERY_�鿴���"+usersn.ToString()+"С���ȷ�����IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ���߲�ѯ
		/// <summary>
		/// ���߲�ѯ
		/// </summary>
		public static DataSet ItemInfo_Query(string serverIP,int usersn,int type)
		{
			DataSet result = null;
			string sql = "";
			string db = "";
			try
			{
				switch(type)
				{
					case 0://����
					{
						serverIP = CommonInfo.JW2_FindDBIP(serverIP,2);
						db = SqlHelper.jw2itemDB;
						break;
					}
					case 1://��Ʒ��
					{
						serverIP = CommonInfo.JW2_FindDBIP(serverIP,2);
						db = SqlHelper.jw2itemDB;
						break;
					}
					case 2://������
					{
						serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
						db = SqlHelper.jw2messengerDB;
						break;
					}	
				}
				sql = "select sql_statement from sqlexpress where sql_type='JW2_ItemInfo_Query"+type+"' and sql_condition='JW2_ItemInfo_Query"+type+"'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,db),sql);
				}
				
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_ItemInfo_Query_�鿴���"+usersn.ToString()+"���߲�ѯ������IP"+serverIP+type.ToString()+ex.Message);
			}
			return result;
		}
		#endregion

		#region ɾ������
		/// <summary>
		/// ɾ������
		/// </summary>
		public static int ITEM_DEL(string serverIP,int usersn,int userByID,string UserName,int itemID,string itemName,int type,int itemNo,ref string strDesc)
		{
			int  result = -1;
			string sql = null;
			string db = "";
			try
			{

				switch(type)
				{
					case 0://����
					{
						serverIP = CommonInfo.JW2_FindDBIP(serverIP,10);
						db = SqlHelper.jw2itemDB;
						break;
					}
					case 1://��Ʒ��
					{
						serverIP = CommonInfo.JW2_FindDBIP(serverIP,10);
						db = SqlHelper.jw2itemDB;
						break;
					}
					case 2://������
					{
						serverIP = CommonInfo.JW2_FindDBIP(serverIP,9);
						db = SqlHelper.jw2messengerDB;
						break;
					}	
				}

				sql = "select sql_statement from sqlexpress where sql_type='JW2_ITEM_DEL"+type+"' and sql_condition='JW2_ITEM_DEL"+type+"'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					if(type==2)
					{
						sql = string.Format(sql,itemNo,itemID,usersn);
					}
					else
					{
						sql = string.Format(sql,itemID,usersn);
					}
					result = MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,db),sql);
				}
				if(result==1)
				{
					strDesc = "ɾ�����"+UserName.ToString()+"����"+itemName.ToString()+"���ɹ������Եȣ�ϵͳ�����У�";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_ITEM_DEL","ɾ����ң�"+UserName.ToString()+"�����ߣ�"+itemName.ToString()+"���ɹ�");
				}
				else
				{
					strDesc = "ɾ�����"+UserName.ToString()+"����"+itemName.ToString()+"��ʧ�ܣ�����Ϸ��ȷ�ϴ˵����Ƿ���ڣ�";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_ITEM_DEL","ɾ����ң�"+UserName.ToString()+"�����ߣ�"+itemName.ToString()+"��ʧ��");
				}
			}
			catch (MySqlException ex)
			{
				strDesc = "���ݿ�����ʧ�ܣ������³��ԣ�";
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ��ӵ���
		/// <summary>
		/// ��ӵ���
		/// </summary>
		public static string ITEM_ADD(string serverIP,int usersn,int userByID,string UserName,string itemName,string strMailTitle,string strMailContent)
		{
			int  result = 0;
			string get_result = "";
			string sql = null;
			int itemID = 0;
			int itemNum = 0;
			string itemLimit = "";
			string itemN = "";
			string ItemSex = "";
			int count = 0;

			try
			{
				string[] item=itemName.Split('|');
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,9);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_ADD_ITEM' and sql_condition='JW2_ADD_ITEM'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<item.Length-1;i++)
					{
						result = 0;
						itemID = int.Parse(item[i].Split(',')[0].ToString());
						itemNum = int.Parse(item[i].Split(',')[1].ToString());
						itemN = CommonInfo.JW2_ProductIDToName(itemID);
						ItemSex = CommonInfo.JW2_ItemID_Sex(itemID);
						switch(CommonInfo.JW2_ItemCodeToLimitDay(itemID))
						{
							case 0:
								itemLimit = "����";
								break;
							case 7:
								itemLimit = "7��";
								break;
							case 30:
								itemLimit = "30��";
								break;
						}
						for(int j = 0;j<itemNum;j++)
						{
							sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
							sql = string.Format(sql,usersn,strMailTitle,strMailContent,itemID);
							result += MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
						}

						if(result >0)
						{
							get_result += "�����ң���"+UserName.ToString()+"��������ID����"+itemID.ToString()+"���������Ա𣺡�"+ItemSex.ToString()+"�������ߣ���"+itemN.ToString()+"������������"+result+"�����ɹ�\n";
							SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_ADD_ITEM","�����ң�"+UserName.ToString()+"������ID��"+itemID.ToString()+"�������Ա�"+ItemSex.ToString()+"�����ߣ�"+itemN.ToString()+"������������"+result+"���������ޣ�"+itemLimit+"���ɹ�");
						}
						else
						{
							get_result += "�����ң���"+UserName.ToString()+"��������ID����"+itemID.ToString()+"���������Ա𣺡�"+ItemSex.ToString()+"�������ߣ���"+itemN.ToString()+"������������"+result+"����ʧ��\n";
							SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_ADD_ITEM","�����ң�"+UserName.ToString()+"������ID��"+itemID.ToString()+"�������Ա�"+ItemSex.ToString()+"�����ߣ�"+itemN.ToString()+"������������"+result+"���������ޣ�"+itemLimit+"��ʧ��");
						}
					}
				}
			}
			catch (MySqlException ex)
			{
				get_result +="���ݿ�����ʧ�ܣ����Ժ���";
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return get_result;
		}
		#endregion

		#region ��ӵ���(����)
		/// <summary>
		/// ��ӵ���(����)
		/// </summary>
		public static string ITEM_ADD_ALL(string serverIP,int userByID,string itemName,string strMailTitle,string strMailContent)
		{
			int  result = 0;
			int userSN = 0;
			string UserName = "";
			string get_result = "";
			string sql = null;
			int itemID = 0;
			int itemNum = 0;
			string itemLimit = "";
			string itemN = "";
			int count = 0;
			string ItemSex = "";
			try
			{
				string[] item=itemName.Split('|');
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,9);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_ADD_ITEM' and sql_condition='JW2_ADD_ITEM'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<item.Length-1;i++)
					{
						result = 0;
						UserName = item[i].Split(',')[0].ToString();
						userSN = CommonInfo.JW2_Account_UserSn(serverIP,UserName);
						if(userSN!=0)
						{
							itemID = int.Parse(item[i].Split(',')[1].ToString());
							itemNum = int.Parse(item[i].Split(',')[2].ToString());
							itemN = CommonInfo.JW2_ProductIDToName(itemID);
							ItemSex = CommonInfo.JW2_ItemID_Sex(itemID);
							switch(CommonInfo.JW2_ItemCodeToLimitDay(itemID))
							{
								case 0:
									itemLimit = "����";
									break;
								case 7:
									itemLimit = "7��";
									break;
								case 30:
									itemLimit = "30��";
									break;
							}
							for(int j = 0;j<itemNum;j++)
							{
								sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
								sql = string.Format(sql,userSN,strMailTitle,strMailContent,itemID);
								result += MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
							}
							if(result >0)
							{
								get_result += "�����ң���"+UserName.ToString()+"��������ID����"+itemID.ToString()+"���������Ա𣺡�"+ItemSex.ToString()+"�������ߣ���"+itemN.ToString()+"������������"+result+"�����ɹ�\n";
								SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_ITEM_ADD_ALL","�����ң�"+UserName.ToString()+"������ID��"+itemID.ToString()+"�������Ա�"+ItemSex.ToString()+"�����ߣ�"+itemN.ToString()+"������������"+result+"���������ޣ�"+itemLimit+"���ɹ�");
							}
							else
							{
								get_result += "�����ң���"+UserName.ToString()+"��������ID����"+itemID.ToString()+"���������Ա𣺡�"+ItemSex.ToString()+"�������ߣ���"+itemN.ToString()+"������������"+result+"�����ɹ�\n";
								SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_ITEM_ADD_ALL","�����ң�"+UserName.ToString()+"������ID��"+itemID.ToString()+"�������Ա�"+ItemSex.ToString()+"�����ߣ�"+itemN.ToString()+"������������"+result+"���������ޣ�"+itemLimit+"���ɹ�");
							}
						}
						else
						{
							get_result += "��ң���"+UserName.ToString()+"����������\n";
							SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_ITEM_ADD_ALL","��ң���"+UserName.ToString()+"����������");
						}
					}
				}
			}
			catch (MySqlException ex)
			{
				get_result +="���ݿ�����ʧ�ܣ����Ժ���";
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return get_result;
		}
		#endregion

		#region ���߲�ѯ(ģ����ѯ)
		/// <summary>
		/// ���߲�ѯ(ģ����ѯ)
		/// </summary>
		public static DataSet ITEM_SELECT(string itenName,string typeflag,int type)
		{

			DataSet result = null;
			string sql = "select sql_statement from sqlexpress where sql_type='JW2_GetItemList' and sql_condition='"+type+"'";
			System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
			if(ds!=null && ds.Tables[0].Rows.Count>0)
			{
				sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
				sql = string.Format(sql,itenName,typeflag);
				result = SqlHelper.ExecuteDataset(sql);
			}
			return result;
		}
		#endregion

		#region ������Ϣ��ѯ
		/// <summary>
		/// ������Ϣ��ѯ
		/// </summary>
		public static DataSet JW2_FriendList_Query(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			string db = "";
			try
			{
				sql = "select sql_statement from sqlexpress where sql_type='JW2_FriendList_Query' and sql_condition='JW2_FriendList_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,db),sql);
				}
				
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_JW2_FriendList_Query_�鿴���"+usersn.ToString()+"������Ϣ��ѯ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ������Ϣ��ѯ
		/// <summary>
		/// ������Ϣ��ѯ
		/// </summary>
		public static DataSet PetInfo_Query(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			string db = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,2);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_PetInfo_Query' and sql_condition='JW2_PetInfo_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2itemDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_PetInfo_Query_�鿴���"+usersn.ToString()+"������Ϣ��ѯ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �޸ĳ�����
		/// <summary>
		/// �޸ļ�����
		/// </summary>
		public static int UpdatePetName_Query(string serverIP,string OLD_petName,string petName,int userByID,int petID,ref string strDesc)
		{
			int  result = -1;
			string sql = null;
			try
			{
				//�޸ĵȼ�1
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,10);//9);//maple
				sql = "select sql_statement from sqlexpress where sql_type='JW2_UpdatePETName_Query' and sql_condition = 'JW2_UpdatePETName_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,petID,petName);
					result = MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2itemDB),sql);
				}
				if(result==1)
				{
					strDesc = "�޸ĳ�������"+OLD_petName+"��Ϊ�³�������"+petName+"���ɹ������Եȣ�ϵͳ�����У�";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_UpdatePETName_Query","�޸ĳ�������"+OLD_petName.ToString()+"��Ϊ�³�������"+petName.ToString()+"���ɹ�");
				}
				else
				{
					strDesc = "�޸ĳ�������"+OLD_petName+"��Ϊ�³�������"+petName+"��ʧ�ܣ���ȷ����Ϸ�г����Ƿ���ڣ�";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_UpdatePETName_Query","�޸ĳ�������"+OLD_petName.ToString()+"��Ϊ�³�������"+OLD_petName.ToString()+"��ʧ��");
				}
			}
			catch (MySqlException ex)
			{
				strDesc = "���ݿ�����ʧ�ܣ����Ժ��ԣ�";
				SqlHelper.errLog.WriteLog("������2_UpdatePetName_Query�޸ĳ�������"+OLD_petName+"��Ϊ�³�������"+petName+"��"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �ϳɲ��ϲ�ԃ
		/// <summary>
		/// �ϳɲ��ϲ�ԃ
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet Materiallist_Query(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,2);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_Materiallist_Query' and sql_condition='JW2_Materiallist_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2itemDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_Materiallist_Query_�鿴���"+usersn.ToString()+"�ϳɲ��ϲ�ԃ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �ϳ���ʷ��¼��ѯ
		/// <summary>
		/// �ϳ���ʷ��¼��ѯ
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet MaterialHistory_Query(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,2);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_MaterialHistory_Query' and sql_condition='JW2_MaterialHistory_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2itemDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_MaterialHistory_Query_�鿴���"+usersn.ToString()+"�ϳ���ʷ��¼��ѯ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �����Ҫ��˵�ͼƬ�б�
		/// <summary>
		/// �����Ҫ��˵�ͼƬ�б�
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet GETPIC_Query(string serverIP,string account)
		{
			DataSet result = null;
			string sql = "";
			string str = "";
			try
			{
				if(account!="")
				{
					int UserSn = CommonInfo.JW2_Account_UserSn(serverIP,account);
					str = "and usersn="+UserSn;
				}
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,2);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_GETPIC_Query_Query' and sql_condition='JW2_GETPIC_Query_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,str);
					if(serverIP!="114.80.167.192,3306")
						result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2itemDB),sql);
					else
						result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,"gmtools","E#.92lG^$kd)205K",SqlHelper.jw2itemDB),sql);

				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_GETPIC_Query_�鿴���"+account+"�����Ҫ��˵�ͼƬ�б������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ���ͼƬ
		/// <summary>
		/// ���ͼƬ
		/// </summary>
		public static int CHKPIC_Query(string serverIP,int usersn,int userByID,string UserName,string Url,int type,ref string strDesc)
		{
			int  result = -1;
			string sql = null;
			string db = "";
			string Pic_Name = "";
			string str = "";
			try
			{
				if(type==2)
					str = "���ͨ��";
				else
					str = "��˲�ͨ��";

				serverIP = CommonInfo.JW2_FindDBIP(serverIP,10);
				sql = "select sql_statement from sqlexpress where sql_type='jw2_CHKPIC_Query' and sql_condition='jw2_CHKPIC_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn,Url,type);
					
					result = MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2itemDB),sql);
				}
				if(result==1)
				{
					strDesc = "������"+UserName.ToString()+"ͼƬ"+Url.ToString()+"��"+str+"���ɹ������Եȣ�ϵͳ�����У�";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_MODIFYLEVEL_QUERY","ɾ����ң�"+UserName.ToString()+"�����ߣ�"+Url.ToString()+"��"+str+"���ɹ�");
				}
				else
				{
					strDesc = "������"+UserName.ToString()+"ͼƬ"+Url.ToString()+"��"+str+"��ʧ�ܣ�����Ϸ��ȷ�ϴ˵����Ƿ���ڣ�";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_MODIFYLEVEL_QUERY","���ͼƬ��"+UserName.ToString()+"�����ߣ�"+Url.ToString()+"��"+str+"��ʧ��");
				}
			}
			catch (MySqlException ex)
			{
				strDesc = "���ݿ�����ʧ�ܣ������³��ԣ�";
				SqlHelper.errLog.WriteLog("���ͼƬ->JW2_CHKPIC_Query->������IP->"+serverIP+"->�ʺ�->"+UserName+"-"+usersn.ToString()+"->ͼƬ->"+Url+"->"+ex.Message);
			}
			return result;
		}
		#endregion

		#region �DƬ��ʹ����r
		/// <summary>
		/// �DƬ��ʹ����r
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet PicCard_Query(string serverIP,int BType,int SType, int usersn,string BeginTime,string EndTime)
		{
			DataSet result = null;
			string sql = "";
			string str = "";
			try
			{
				if(BType==1)
				{
					serverIP = CommonInfo.JW2_FindDBIP(serverIP,2);
					sql = "select sql_statement from sqlexpress where sql_type='jw2_PicCard_Query' and sql_condition='jw2_PicCard_Query'";
					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{
						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,usersn,BeginTime,EndTime);
						result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2itemDB),sql);
					}
				}
				else if(BType==2)
				{
					serverIP = CommonInfo.JW2_FindDBIP(serverIP,4);
					sql = "select sql_statement from sqlexpress where sql_type='jw2_Garden_Log_Query' and sql_condition='"+SType+"'";
					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{
						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,usersn,BeginTime,EndTime);
						result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2logDB),sql);
					}
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("��ÈDƬ��ʹ����r->JW2_PicCard_Query->������IP->"+serverIP+"->�ʺ�->"+usersn.ToString()+"->"+ex.Message);
			}
			return result;
		}
		#endregion
		
		

	}
}
