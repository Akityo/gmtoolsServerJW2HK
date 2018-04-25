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
	/// JW2MessengerDataInfo ��ժҪ˵����
	/// </summary>
	public class JW2MessengerDataInfo
	{
		public JW2MessengerDataInfo()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		#region �鿴��Ҽ�����Ϣ
		/// <summary>
		/// �鿴��Ҽ�����Ϣ
		/// </summary>
		public static DataSet User_Family_Query(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_User_Family_Query' and sql_condition='JW2_User_Family_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_User_Family_Query_�鿴���"+usersn.ToString()+"������Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴������Ϣ
		/// <summary>
		/// �鿴��Ҽ�����Ϣ
		/// </summary>
		public static DataSet FAMILYINFO_QUERY(string serverIP,string FamilyName)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_FAMILYINFO_QUERY' and sql_condition='JW2_FAMILYINFO_QUERY'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,FamilyName);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_FAMILYINFO_QUERY_�鿴����"+FamilyName+"������Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴���������Ϣ
		/// <summary>
		/// �鿴���������Ϣ
		/// </summary>
		public static DataSet FamilyPet_Query(string serverIP,int FamilyID)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_Family_Pet_Query' and sql_condition='JW2_Family_Pet_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,FamilyID);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_FamilyPet_Query_�鿴����"+FamilyID.ToString()+"������Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴�����Ա��Ϣ
		/// <summary>
		/// �鿴�����Ա��Ϣ
		/// </summary>
		public static DataSet FAMILYMEMBER_QUERY(string serverIP,int FamilyNameID)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_FAMILYMEMBER_QUERY' and sql_condition='JW2_FAMILYMEMBER_QUERY'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,FamilyNameID);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_FAMILYMEMBER_QUERY_�鿴����"+FamilyNameID.ToString()+"��Ա��Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ��ѯ���������Ϣ
		/// <summary>
		/// ��ѯ���������Ϣ
		/// </summary>
		public static DataSet FamilyItemInfo_Query(string serverIP,int FamilyNameID)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_FamilyItemInfo_Query' and sql_condition='JW2_FamilyItemInfo_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,FamilyNameID);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_FamilyItemInfo_Query_�鿴����"+FamilyNameID.ToString()+"������Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴���������г�Ա��Ϣ
		/// <summary>
		/// �鿴���������г�Ա��Ϣ
		/// </summary>
		public static DataSet FamilyMember_Applying(string serverIP,int FamilyNameID)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_FamilyMember_Applying' and sql_condition='JW2_FamilyMember_Applying'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,FamilyNameID);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_FamilyMember_Applying_�鿴����"+FamilyNameID.ToString()+"�����г�Ա��Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴����������Ϣ
		/// <summary>
		/// �鿴����������Ϣ
		/// </summary>
		public static DataSet BasicRank_Query(string serverIP,int FamilyNameID)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_BasicRank_Query' and sql_condition='JW2_BasicRank_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,FamilyNameID);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_BasicRank_Query_�鿴����"+FamilyNameID.ToString()+"������Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion
		
		#region �鿴Messenger�ƺ�
		/// <summary>
		/// �鿴Messenger�ƺ�
		/// </summary>
		public static DataSet Messenger_Query(string serverIP,int userSN)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_Messenger_Query' and sql_condition='JW2_Messenger_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userSN);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_Messenger_Query_�鿴����"+userSN.ToString()+"�ƺ���Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ��ѯ������߹�����־
		/// <summary>
		/// ��ѯ������߹�����־
		/// </summary>
		public static DataSet FamilyBuyLog_Query(string serverIP,int FamilyNameID,string BeginTime ,string EndTime)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_FamilyBuyLog_Query' and sql_condition='JW2_FamilyBuyLog_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,FamilyNameID,BeginTime,EndTime);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_FamilyBuyLog_Query_�鿴����"+FamilyNameID.ToString()+"���߹�����־������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ������־(0,����,1,����)
		/// <summary>
		/// ������־(0,����,1,����)
		/// </summary>
		public static DataSet FamilyFund_Log(string serverIP,int FamilyNameID,string BeginTime ,string EndTime,int type)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_FamilyFund_Log' and sql_condition='"+type+"'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,FamilyNameID,BeginTime,EndTime);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_FamilyFund_Log_�鿴����"+FamilyNameID.ToString()+"������־������IP"+serverIP+type+ex.Message);
			}
			return result;
		}
		#endregion

		#region �����Ա��ȡС������Ϣ��ѯ
		/// <summary>
		/// �����Ա��ȡС������Ϣ��ѯ
		/// </summary>
		public static DataSet SmallPetAgg_Query(string serverIP,int userSN,string BeginTime ,string EndTime)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_SmallPetAgg_Query' and sql_condition='JW2_SmallPetAgg_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userSN,BeginTime,EndTime);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_SmallPetAgg_Query_�鿴���"+userSN.ToString()+"��ȡС������Ϣ��ѯ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion
		
		#region ������־
		/// <summary>
		/// ������־
		/// </summary>
		public static DataSet Item_Log(string serverIP,int userSN,string BeginTime ,string EndTime)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_Item_Log' and sql_condition='JW2_Item_Log'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userSN,BeginTime,EndTime);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_Item_Log_�鿴���"+userSN.ToString()+"������־��ѯ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴������Ϣ
		/// <summary>
		/// �鿴��Ҽ�����Ϣ
		/// </summary>
		public static DataSet BasicInfo_Query(string serverIP,int FamilyNameID)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_BasicInfo_Query' and sql_condition='JW2_BasicInfo_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,FamilyNameID);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_BasicInfo_Query_�鿴����"+FamilyNameID.ToString()+"������Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ��ԃ���l��
		/// <summary>
		/// ��ԃ���l��
		/// </summary>
		public static DataSet MailInfo_Query(string serverIP,int userSN,string BeginTime,string EndTime)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_MailInfo_Query' and sql_condition='JW2_MailInfo_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userSN,BeginTime,EndTime);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_MailInfo_Query_�鿴���"+userSN.ToString()+"���l����Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion
	
		#region �޸ļ�����
		/// <summary>
		/// �޸ļ�����
		/// </summary>
		public static int UpdateFamilyName_Query(string serverIP,string OLD_familyName,string familyName,int userByID,int familyID)
		{
			int  result = -1;
			string sql = null;
			try
			{
				//�޸ĵȼ�1
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,9);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_UpdateFamilyName_Query' and sql_condition = 'JW2_UpdateFamilyName_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,familyID,familyName);
					result = MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
				if(result==1)
				{
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_UpdateFamilyName_Query","�޸ļ�������"+OLD_familyName.ToString()+"��Ϊ�¼�������"+familyName.ToString()+"���ɹ�(�޸ļ�����,jw2)");
				}
				else
				{
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_UpdateFamilyName_Query","�޸ļ�������"+OLD_familyName.ToString()+"��Ϊ�¼�������"+familyName.ToString()+"��ʧ��(�޸ļ�����,jw2)");
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion
		
		
	}
}
