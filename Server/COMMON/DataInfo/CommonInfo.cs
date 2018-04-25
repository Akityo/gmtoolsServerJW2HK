 using System;
using System.Data;
using System.Data.SqlClient;
using Common.Logic;
using MySql.Data.MySqlClient;
using System.Web;
using System.Net;
using System.Collections;
using System.IO;
using System.Xml;
using System.Text;
using Oracle.DataAccess.Client;
namespace Common.DataInfo
{
	/// <summary>
	/// CommonInfo ��ժҪ˵����
	/// </summary>
	public class CommonInfo
	{
		public CommonInfo()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
        public static int LinkServerIP_Create(int userByID,string gameIP,string usr,string pwd,string city,int gameID,int gamedbID,int gameFlag)
        {
            int result = -1;
            SqlParameter[] paramCode;
            try
            {
                paramCode = new SqlParameter[9]{
                                                   new SqlParameter("@GM_UserID",SqlDbType.Int),
												   new SqlParameter("@Game_IP",SqlDbType.VarChar,30),
												   new SqlParameter("@Game_Usr",SqlDbType.VarChar,50),
												   new SqlParameter("@Game_PWD",SqlDbType.VarChar,50),
                                                   new SqlParameter("@Game_City",SqlDbType.VarChar,50),
                                                   new SqlParameter("@Game_ID",SqlDbType.Int),
                                                   new SqlParameter("@Game_DBID",SqlDbType.Int),
                                                   new SqlParameter("@Game_Flag",SqlDbType.Int),
												   new SqlParameter("@result",SqlDbType.Int)
											   };
                paramCode[0].Value = userByID;
                paramCode[1].Value = gameIP.Trim();
                paramCode[2].Value = usr.Trim();
                paramCode[3].Value = pwd.Trim();
                paramCode[4].Value = city.Trim();
                paramCode[5].Value = gameID;
                paramCode[6].Value = gamedbID;
                paramCode[7].Value = gameFlag;
                paramCode[8].Direction = ParameterDirection.ReturnValue;
                result = SqlHelper.ExecSPCommand("sp_linkGameIP", paramCode);
				if(userByID == 0)
				{
					CommonInfo.SDO_OperatorLogDel(userByID);
				}
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);

            }
            return result;
        }
		public static int FJdbid_Query(string serverIP)
		{
			int serverName = 0;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@ServerIP",SqlDbType.VarChar,30)};
				paramCode[0].Value = serverIP;
				DataSet ds = SqlHelper.ExecSPDataSet("FJdbid_Query",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					serverName = int.Parse( ds.Tables[0].Rows[0].ItemArray[0].ToString());
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return serverName;
		}
		public static int LinkServerIP_Delete(int userByID,int idx,string gameIP)
		{
			int result = -1;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[4]{
												   new SqlParameter("@GM_UserID",SqlDbType.Int),
												   new SqlParameter("@idx",SqlDbType.Int),
												   new SqlParameter("@ServerIP",SqlDbType.VarChar,30),
												   new SqlParameter("@Result",SqlDbType.Int)
											   };
				paramCode[0].Value = userByID;
				paramCode[1].Value = idx;
				paramCode[2].Value = gameIP.Trim();
				paramCode[3].Direction = ParameterDirection.ReturnValue;
				result = SqlHelper.ExecSPCommand("sp_deleteLinkDown", paramCode);
				if(userByID == 0)
				{
					CommonInfo.SDO_OperatorLogDel(userByID);
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);

			}
			return result;
		}
        public static DataSet serverIP_QueryAll()
        {
            try
            {
                return SqlHelper.ExecuteDataset("ServerInfo_Query_All");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public static string serverIP_Query(string serverIP)
        {
            string serverName = null;
			SqlParameter[] paramCode;
            try
            {
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@ServerIP",SqlDbType.VarChar,30)};
			    paramCode[0].Value = serverIP;
                DataSet ds = SqlHelper.ExecSPDataSet("ServerName_Query",paramCode);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    serverName = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return serverName;
        }
		public static DataSet serverIP_Query(int gameID,int gameDBID)
		{
            DataSet ds = null;
            SqlParameter[] paramCode;
            try
            {
                paramCode = new SqlParameter[2]{
												   new SqlParameter("@GM_gameID",SqlDbType.Int),
												   new SqlParameter("@GM_gameDBID",SqlDbType.Int)};
                paramCode[0].Value = gameID;
                paramCode[1].Value = gameDBID;
                ds = SqlHelper.ExecSPDataSet("ServerInfo_Query", paramCode);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return ds;
		}
		#region �鿴���߲�����¼
		/// <summary>
		/// �鿴���߲�����¼
		/// </summary>
		/// <param name="userID">�û�ID</param>
		/// <param name="beginDate">��ʼ����</param>
		/// <param name="endDate">��������</param>
		/// <returns></returns>
		public static DataSet OperateLog_Query(int userID,DateTime beginDate,DateTime endDate)
		{
			DataSet result = null;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[3]{
												   new SqlParameter("@Gm_UserID",SqlDbType.Int),
												   new SqlParameter("@BeginDate",SqlDbType.DateTime),
												   new SqlParameter("@EndDate",SqlDbType.DateTime)};
				paramCode[0].Value=userID;
				paramCode[1].Value=beginDate;
				paramCode[2].Value=endDate;
				result = SqlHelper.ExecSPDataSet("GMTools_Log_Query",paramCode);
			}
			catch(SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return result;
		}
		#endregion
		#region ɾ������Ա������־
		/// <summary>
		/// ɾ������Ա������־
		/// </summary>
		/// <param name="userByID">����ԱID</param>
		/// <returns></returns>
		public static int SDO_OperatorLogDel(int userByID)
		{
			int result = -1;
			try
			{
				result = SqlHelper.ExecCommand("delete from  GMTools_Log where UserID = "+userByID);
				result = SqlHelper.ExecCommand("delete from  GMTools_Log_UpdateAgo where UserID = "+userByID);
				result = SqlHelper.ExecCommand("delete from  GMTools_LogTime where OperateUserID = "+userByID);
			}
			catch(System.Data.SqlClient.SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return result;

		}
		#endregion

		#region ���Բ�ѯ
		public static DataSet Mf_ItemName_Query(int guID)
		{
			DataSet result = null;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@GeneID",SqlDbType.VarChar,20)};
				paramCode[0].Value=guID;
				result = SqlHelper.ExecSPDataSet("Mf_GeneName_Query",paramCode);
			}
			catch(SqlException ex)
			{
				SqlHelper.errLog.WriteLog(ex.Message);
			}
			return result;
		}
		public static string MfPetPart_Query(string PetId)
		{
			string serverName ="";
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@PetId",SqlDbType.VarChar,30)};
				paramCode[0].Value = PetId;
				DataSet ds = SqlHelper.ExecSPDataSet("MfPetPartnum_Query",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					serverName = ds.Tables[0].Rows[0].ItemArray[0].ToString();
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return serverName;
		}

		#endregion

		#region �Ҵ�IP,pwd��ѯ
		public static string[] SD_GameDBInfo_Query(string serverip)
		{
			string[] itemName = new string[2];
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@ServerIP",SqlDbType.VarChar,50)};
				paramCode[0].Value = serverip;
				DataSet ds = SqlHelper.ExecSPDataSet("SD_GameDBInfo_Query",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					itemName[0] = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					itemName[1] = ds.Tables[0].Rows[0].ItemArray[1].ToString();
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return itemName;
		}
		#endregion
		#region �̳ǵ�������ѯ
		/// <summary>
		/// �̳ǵ�������ѯ
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="itemID"></param>
		/// <returns></returns>
		public static string SD_GetShopItemName_Query(string serverIP,int itemID)
		{
			string result = null;	
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				string sql = "select sql_statement from sqlexpress where sql_type='SD_GetItemBillingName' and sql_condition = 'SD_GetItemBillingName'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,itemID);
					DataSet Ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
					if (Ds != null && Ds.Tables[0].Rows.Count > 0)
					{
						result = Ds.Tables[0].Rows[0].ItemArray[0].ToString();
					}
				}
				
			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return result;
		}
		#endregion

		#region ��ûָ���������Ҫ������
		public static string[] SD_GetReData_Query(string serverIP,int iTime,int accountID,int SDID,int itemID,string getTime)
		{
			string[] itemName = new string[8];
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				string sql = "select sql_statement from sqlexpress where sql_type='SD_GetReData_Query' and sql_condition = 'SD_GetReData_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,iTime,accountID,itemID,SDID);
					DataSet Ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
					if (Ds != null && Ds.Tables[0].Rows.Count > 0)
					{
						itemName[0] = Ds.Tables[0].Rows[0].ItemArray[0].ToString();
						itemName[1] = Ds.Tables[0].Rows[0].ItemArray[1].ToString();
						itemName[2] = Ds.Tables[0].Rows[0].ItemArray[2].ToString();
						itemName[3] = Ds.Tables[0].Rows[0].ItemArray[3].ToString();
						itemName[4] = Ds.Tables[0].Rows[0].ItemArray[4].ToString();
						itemName[5] = Ds.Tables[0].Rows[0].ItemArray[5].ToString();
						itemName[6] = Ds.Tables[0].Rows[0].ItemArray[6].ToString();
						itemName[7] = Ds.Tables[0].Rows[0].ItemArray[7].ToString();

					}
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return itemName;
		}
		#endregion

		#region �û��������Ƿ�����
		public static int SD_IsFullBox_Query(string serverIP,int userid)
		{
			int result = -1;	
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				string sql = "select sql_statement from sqlexpress where sql_type='SD_IsFullBox_Query' and sql_condition = 'SD_IsFullBox_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userid);
					DataSet Ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
					if (Ds != null && Ds.Tables[0].Rows.Count > 0)
					{
						result = int.Parse(Ds.Tables[0].Rows[0].ItemArray[0].ToString());
					}
				}
				
			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return result;
		}
		#endregion
		#region ��ѯ�û�ID
		public static int SD_GetUserId_Query(string serverIP,string username)
		{
			int result = -1;	
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				string sql = "select sql_statement from sqlexpress where sql_type='SD_GetUserId_Query' and sql_condition = 'SD_GetUserId_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,username);
					DataSet Ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
					if (Ds != null && Ds.Tables[0].Rows.Count > 0)
					{
						result = int.Parse(Ds.Tables[0].Rows[0].ItemArray[0].ToString());
					}
				}
				
			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return result;
		}
		#endregion
		#region ��õ�������
		public static string SD_GetItemName_Query(string serverIP,int itemID)
		{
			string result = null;	
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				string sql = "select sql_statement from sqlexpress where sql_type='SD_GetItemName_Query' and sql_condition = 'SD_GetItemName_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,itemID);
					DataSet Ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
					if (Ds != null && Ds.Tables[0].Rows.Count > 0)
					{
						result = Ds.Tables[0].Rows[0].ItemArray[0].ToString();
					}
				}
				
			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return result;
		}
		#endregion
		#region ��ѯ�ָ���������ǿ��ֵ
		/// <summary>
		/// ��ѯ�ָ���������ǿ��ֵ
		/// </summary>
		public static int  SD_isItemName_Query(string itemName)
		{
			int  result = -1;
			SqlParameter[] paramCode;
			try
			{
				string sql = "select sql_statement from sqlexpress where sql_type='SD_getUnitsCMax_Query' and sql_condition = 'SD_getUnitsCMax_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,itemName);
					DataSet Ds = SqlHelper.ExecuteDataset(sql);
					if (Ds != null && Ds.Tables[0].Rows.Count > 0)
					{
						result = int.Parse(Ds.Tables[0].Rows[0].ItemArray[0].ToString());
					}
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return result;
		}
		#endregion

		#region ������II
		#region ��ѯserverIP
		/// <summary>
		/// ��ѯserverIP
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_FindDBIP(string serverIP,int dbid)
		{
			string serverName = "";
			try
			{
				SqlParameter[] paramCode;
				paramCode = new SqlParameter[2]{
												   new SqlParameter("@serverIp",SqlDbType.VarChar,50),
												   new SqlParameter("@dbid",SqlDbType.Int) };
				paramCode[0].Value = serverIP;
				paramCode[1].Value = dbid;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_FindDBIP",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					serverName = ds.Tables[0].Rows[0].ItemArray[0].ToString();
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message+"123");
			}
			return serverName;
		}
		#endregion

		#region ��ѯserverIP
		/// <summary>
		/// ��ѯserverIP
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_FindDBIP(string serverIP,string city ,int dbid)
		{
			string serverName = "";
			try
			{
				SqlParameter[] paramCode;
				paramCode = new SqlParameter[3]{
												   new SqlParameter("@serverIp",SqlDbType.VarChar,50),
												   new SqlParameter("@city",SqlDbType.VarChar,50),
												   new SqlParameter("@dbid",SqlDbType.Int) };
				paramCode[0].Value = serverIP;
				paramCode[1].Value = city;
				paramCode[2].Value = dbid;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_FindDBIP_City",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					serverName = ds.Tables[0].Rows[0].ItemArray[0].ToString();
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message+"123");
			}
			return serverName;
		}
		#endregion

		
		#region ��ѯIP���ڵĴ���
		/// <summary>
		/// ��ѯIP���ڵĴ���
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_FindDBName(string serverIP)
		{
			string serverName = "";
			try
			{
				SqlParameter[] paramCode;
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@serverIp",SqlDbType.VarChar,50)
											   };
				paramCode[0].Value = serverIP;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_FindDBName",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					serverName = ds.Tables[0].Rows[0].ItemArray[0].ToString();
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message+"123");
			}
			return serverName;
		}
		#endregion

		#region ��ô���ZONE
		/// <summary>
		/// ��ô���
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static int  JW2_GetZone_Query (int gameID,string serverName)
		{
			DataSet result = null;
			int zone = 0;
			string sql = null;
			try
			{
				sql = "select sql_statement from sqlexpress where sql_type='JW2_GetZone_Query' and sql_condition = 'JW2_GetZone_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,gameID,serverName);
					result = CommonInfo.RunOracle(sql,SqlHelper.oracleData,SqlHelper.oracleUser,SqlHelper.oraclePwd);
					if(result!=null && result.Tables[0].Rows.Count>0)
					{
						zone = int.Parse(result.Tables[0].Rows[0].ItemArray[0].ToString());
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return zone;
		}
		#endregion			

		#region ���·�ͣ�û�--����
		/// <summary>
		/// ���·�ͣ�û�--����
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int JW2_BanUser(string serverIP,int UserByID,int userSN,string userName,string userNick,string Reason)
		{
			int result = -1;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[7]{
												   new SqlParameter("@Gm_UserByID",SqlDbType.Int),
												   new SqlParameter("@JW2_UserSN",SqlDbType.Int),
												   new SqlParameter("@JW2_UserID",SqlDbType.VarChar,256),
												   new SqlParameter("@JW2_UserNick",SqlDbType.VarChar,256),
												   new SqlParameter("@JW2_ServerIP",SqlDbType.VarChar,256),
												   new SqlParameter("@JW2_Reason",SqlDbType.VarChar,2000),
												   new SqlParameter("@result",SqlDbType.Int)};
				paramCode[0].Value = UserByID;
				paramCode[1].Value = userSN;
				paramCode[2].Value = userName;
				paramCode[3].Value = userNick;
				paramCode[4].Value = serverIP;
				paramCode[5].Value = Reason;
				paramCode[6].Direction = ParameterDirection.ReturnValue;
				result = SqlHelper.ExecSPCommand("JW2_BanUser",paramCode);
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return  result;
		}
		#endregion
		#region ���½���û�--����
		/// <summary>
		/// ���½���û�--����
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int JW2_UnBanUser(string serverIP,int UserByID,int userSN,string userName,string Reason)
		{
			int result = -1;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[6]{
												   new SqlParameter("@Gm_UserByID",SqlDbType.Int),
												   new SqlParameter("@JW2_UserSN",SqlDbType.Int),
												   new SqlParameter("@JW2_UserID",SqlDbType.VarChar,256),
												   new SqlParameter("@JW2_ServerIP",SqlDbType.VarChar,256),
												   new SqlParameter("@JW2_Reason",SqlDbType.VarChar,2000),
												   new SqlParameter("@result",SqlDbType.Int)};
				paramCode[0].Value = UserByID;
				paramCode[1].Value = userSN;
				paramCode[2].Value = userName;
				paramCode[3].Value = serverIP;
				paramCode[4].Value = Reason;
				paramCode[5].Direction = ParameterDirection.ReturnValue;
				result= SqlHelper.ExecSPCommand("JW2_UnBanUser",paramCode);
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return  result;
		}
		#endregion
		#region �ȼ���ѯ����
		/// <summary>
		/// �ȼ���ѯ����
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static float JW2_LevelToExp(int iLevel)
		{
			float iExp = 0;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@level",SqlDbType.Int) };
				paramCode[0].Value = iLevel;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_LevelToExp",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					iExp = float.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return iExp;
		}
		#endregion
		#region �����ѯ�ȼ�
		/// <summary>
		/// �����ѯ�ȼ�
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int JW2_ExpToLevel(float iExp)
		{
			int iLevel = 0;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@Exp",SqlDbType.Int) };
				paramCode[0].Value = iExp;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_ExpToLevel",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					iLevel = int.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return iLevel;
		}
		#endregion

		#region �����ѯ�ȼ�--��԰
		/// <summary>
		/// �����ѯ�ȼ�--��԰
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string  JW2_Garden_ExpToLevel(float iExp)
		{
			int iLevel = 0;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@Exp",SqlDbType.Int) };
				paramCode[0].Value = iExp;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_Garden_ExpToLevel",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					iLevel = int.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return "��԰"+iLevel.ToString()+"��";
		}
		#endregion
		
		#region �����ѯ�ȼ�--PET
		/// <summary>
		/// �����ѯ�ȼ�
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int JW2_Pet_ExpToLevel(float iExp)
		{
			int iLevel = 0;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@Exp",SqlDbType.Int) };
				paramCode[0].Value = iExp;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_Pet_ExpToLevel",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					iLevel = int.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return iLevel;
		}
		#endregion
		#region �����c��
		/// <summary>
		/// �����c��
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int JW2_ItemCodeToFamilyPoint(int itemID)
		{
			int FamilyPoin = 0;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@itemcode",SqlDbType.Int) };
				paramCode[0].Value = itemID;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_ItemCodeToFamilyPoint",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					FamilyPoin = int.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return FamilyPoin;
		}
		#endregion
		#region ������X
		/// <summary>
		/// �����c��
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int JW2_ItemCodeToMoney(int itemID)
		{
			int MoneyCost = 0;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@itemcode",SqlDbType.Int) };
				paramCode[0].Value = itemID;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_ItemCodeToMoney",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					MoneyCost = int.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return MoneyCost;
		}
		#endregion
		#region ����ID�D�Q
		/// <summary>
		/// ����ID�D�Q
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int JW2_ItemCodeToProductID(int itemID)
		{
			int ProductID = 0;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@itemcode",SqlDbType.Int) };
				paramCode[0].Value = itemID;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_ItemCodeToProductID",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					ProductID = int.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
				}
				else
				{
					ProductID = itemID;
				}

			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return ProductID;
		}
		#endregion
		
		#region �������ڵļ۸�
		/// <summary>
		/// �������ڵļ۸�
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int JW2_ItemCodeToMoneyCost(int itemID)
		{
			int MoneyCost = 0;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@itemcode",SqlDbType.Int) };
				paramCode[0].Value = itemID;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_ItemCodeToMoneyCost",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					MoneyCost = int.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return MoneyCost;
		}
		#endregion
		#region ����ԭ��
		/// <summary>
		/// ����ԭ��
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int JW2_ItemCodeToOrgMoney(int itemID)
		{
			int MoneyCost = 0;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@itemcode",SqlDbType.Int) };
				paramCode[0].Value = itemID;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_ItemCodeToOrgMoney",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					MoneyCost = int.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return MoneyCost;
		}
		#endregion
		#region ��������
		/// <summary>
		/// ��������
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int JW2_ItemCodeToLimitDay(int itemID)
		{
			int limitDay = 0;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@itemcode",SqlDbType.Int) };
				paramCode[0].Value = itemID;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_ItemCodeToLimitDay",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					limitDay = int.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return limitDay;
		}
		#endregion
		#region �ϳɲ��ϲ�ѯ
		/// <summary>
		/// �ϳɲ��ϲ�ѯ
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_MaterialCodeToName(int itemID)
		{
			string itemName = "";
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@itemcode",SqlDbType.Int) };
				paramCode[0].Value = itemID;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_MaterialCodeToName",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					itemName = ds.Tables[0].Rows[0].ItemArray[0].ToString();
				}
				else
				{
					itemName = itemID.ToString();
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return itemName;
		}
		#endregion
		
			
		#region ��������ѯ
		/// <summary>
		/// ��������ѯ
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_ItemCodeToName(int itemID)
		{
			string itemName = "";
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@itemcode",SqlDbType.Int) };
				paramCode[0].Value = itemID;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_ItemCodeToName",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					itemName = ds.Tables[0].Rows[0].ItemArray[0].ToString();
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return itemName;
		}
		#endregion
		#region ��������ѯ
		/// <summary>
		/// ��������ѯ
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_ProductIDToName(int ProductID)
		{
			string itemName = "";
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@ProductID",SqlDbType.Int) };
				paramCode[0].Value = ProductID;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_ProductIDToName",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					itemName = ds.Tables[0].Rows[0].ItemArray[0].ToString();
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return itemName;
		}
		#endregion
		#region �������D����ID
		/// <summary>
		/// ��������ѯ
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static System.Data.DataSet JW2_ProductNameToID(string ProductName)
		{
			int itemID = 0;
			DataSet ds = new DataSet();
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@ProductIName",SqlDbType.VarChar,50) };
				paramCode[0].Value = ProductName;
				ds = SqlHelper.ExecSPDataSet("JW2_ProductNameToID",paramCode);
				
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return ds;
		}
		#endregion
		#region �ʺ�IDת���ܶ�
		/// <summary>
		/// �ʺ�IDת���ܶ�
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int JW2_UserSn_FriendDegree(string serverIP,int usersn)
		{
			DataSet result = null;
			int UserName = 0;
			string sql = null;
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,6);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_UserSn_FriendDegree' and sql_condition = 'JW2_UserSn_FriendDegree'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
					if(result!=null && result.Tables[0].Rows.Count>0)
					{
						UserName = int.Parse(result.Tables[0].Rows[0].ItemArray[0].ToString());
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return UserName;
		}
		#endregion
		#region �ʺ�IDת�ʺ���
		/// <summary>
		/// �ʺ�IDת�ʺ���
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_UserSn_Account(string serverIP,int usersn)
		{
			DataSet result = null;
			string UserName = null;
			string sql = null;
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_UserSn_Account' and sql_condition = 'JW2_UserSn_Account'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
					if(result!=null && result.Tables[0].Rows.Count>0)
					{
						UserName = result.Tables[0].Rows[0].ItemArray[0].ToString();
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return UserName;
		}
		#endregion
		#region �ʺ�IDת�ʺ���_ORACLE
		/// <summary>
		/// �ʺ�IDת�ʺ���_ORACLE
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_UserSn_Account_Oracle(string serverIP,int usersn)
		{
			DataSet result = null;
			string UserName = null;
			string sql = null;
			int zone = 0;
			string serverName = "";
			try
			{
				serverName = CommonInfo.JW2_FindDBName(serverIP);
				zone = CommonInfo.JW2_GetZone_Query(13,serverName);
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_UserSn_Account_Oracle' and sql_condition = 'JW2_UserSn_Account_Oracle'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn,zone);
					result = CommonInfo.RunOracle(sql,SqlHelper.oracleData,SqlHelper.oracleUser,SqlHelper.oraclePwd);
					if(result!=null && result.Tables[0].Rows.Count>0)
					{
						UserName = result.Tables[0].Rows[0].ItemArray[0].ToString();
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return UserName;
		}
		#endregion

		#region �ʺ�IDת�ǳ�
		/// <summary>
		/// �ʺ�IDת�ǳ�
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_UserSn_UserNick(string serverIP,int usersn)
		{
			DataSet result = null;
			string UserNick = "";
			string sql = null;
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_UserSn_UserNick' and sql_condition = 'JW2_UserSn_UserNick'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
					if(result!=null && result.Tables[0].Rows.Count>0)
					{
						UserNick = result.Tables[0].Rows[0].ItemArray[0].ToString();
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return UserNick;
		}
		#endregion
		#region �ʺ���ת�ʺ�ID
		/// <summary>
		/// �ʺ���ת�ʺ�ID
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int JW2_Account_UserSn(string serverIP,string userName)
		{
			DataSet result = null;
			int UserSn = 0;
			string sql = null;
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_Account_UserSn' and sql_condition = 'JW2_Account_UserSn'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userName);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
					if(result!=null && result.Tables[0].Rows.Count>0)
					{
						UserSn = int.Parse(result.Tables[0].Rows[0].ItemArray[0].ToString());
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return UserSn;
		}
		#endregion
		#region ͨ��ITEMID�鿴���߷����Ա�
		/// <summary>
		/// �ʺ���ת�ʺ�ID
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_ItemID_Sex(int itemID)
		{
			DataSet result = null;
			string ItemSex = "";
			string sql = null;
			try
			{
				sql = "select sql_statement from sqlexpress where sql_type='jw2_getItemSex' and sql_condition = 'jw2_getItemSex'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,itemID);
					result = SqlHelper.ExecuteDataset(sql);
					if(result!=null && result.Tables[0].Rows.Count>0)
					{
						ItemSex = result.Tables[0].Rows[0].ItemArray[0].ToString();
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return ItemSex;
		}
		#endregion
		#region ����½ʱ��
		/// <summary>
		/// ����½ʱ��
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_LastLogDate_Query(string serverIP,int usersn)
		{
			DataSet result = null;
			string LastLogDate = "";
			string sql = null;
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,3);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_LastLogDate_Query' and sql_condition = 'JW2_LastLogDate_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2loginDB),sql);
					if(result!=null && result.Tables[0].Rows.Count>0)
					{
						LastLogDate = result.Tables[0].Rows[0].ItemArray[0].ToString();
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return LastLogDate;
		}
		#endregion
		#region ����ʱ��
		/// <summary>
		/// ����ʱ��
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_RegistDate_Query(string serverIP,int usersn)
		{
			DataSet result = null;
			string RegistDate = "";
			string sql = null;
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,3);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_RegistDate_Query' and sql_condition = 'JW2_RegistDate_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2loginDB),sql);
					if(result!=null && result.Tables[0].Rows.Count>0)
					{
						RegistDate = result.Tables[0].Rows[0].ItemArray[0].ToString();
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return RegistDate;
		}
		#endregion
		#region ������״̬��ѯ
		/// <summary>
		/// ������״̬��ѯ
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_Fcm_Query(string serverIP,int usersn)
		{
			DataSet result = null;
			string Fcm_Result ="δ����";
			string sql = null;
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_Fcm_Query' and sql_condition = 'JW2_Fcm_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
					if(result!=null && result.Tables[0].Rows.Count>0)
					{
						if(int.Parse(result.Tables[0].Rows[0].ItemArray[0].ToString())>0)
						{
							Fcm_Result ="�ѳ���";
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return Fcm_Result;
		}
		#endregion
		#region �ƺŲ�ѯ
		/// <summary>
		/// �ƺŲ�ѯ
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static string JW2_FindTitleName(int titleID)
		{
			string titleName = "";
			long iTitleType = (titleID & 0xff000000)>>24;
			long iTitleNo= (titleID & 0x00ffffff) ;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[2]{
												   new SqlParameter("@iTitleType",SqlDbType.Int),
												   new SqlParameter("@iTitleNo",SqlDbType.Int)
											   };
				paramCode[0].Value = iTitleType;
				paramCode[1].Value = iTitleNo;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_FindTitleName_New",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					titleName = ds.Tables[0].Rows[0].ItemArray[0].ToString();
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return titleName;
		}
		#endregion		
		#region ZoneID��?
		/// <summary>
		/// ZoneID��?
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int JW2_ServerIPToZoneID(string serverip)
		{
			int zoneid = 0;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[1]{
												   new SqlParameter("@serverip",SqlDbType.VarChar,50) };
				paramCode[0].Value = serverip;
				DataSet ds = SqlHelper.ExecSPDataSet("JW2_ServerIPToZoneID",paramCode);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					zoneid = int.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return zoneid;
		}
		#endregion
		#region ���g������
		public static int JW2_KickUser(int userid,string type,int userByID,string serverip,string UserName,ref string strDesc)
		{
			int result =-1;
			string parameter ="";
			try
			{
				System.Data.DataSet ds = SqlHelper.ExecuteDataset("select ServerIP from gmtools_serverInfo where gameid=10000");
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					string url = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					HttpWebRequest  request  = (HttpWebRequest)
						WebRequest.Create(url);
					request.ContentType="application/x-www-form-urlencoded";
					request.KeepAlive=true; 
					request.Method="post";
					//����POST���Ľӿ�
					Stream writer = request.GetRequestStream();
					string postData="type="+type+"&id="+userid;
					//postData="";
					ASCIIEncoding encoder = new ASCIIEncoding();
					byte[] ByteArray = encoder.GetBytes(postData);
					writer.Write(ByteArray,0,postData.Length);
					writer.Close();
					//�õ��ӿڵĻ�Ӧ
					WebResponse
						resp = request.GetResponse();
					StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
					string tmpResult = sr.ReadToEnd();
					resp.Close();
					if(type.Equals("get_userid"))
					{
						tmpResult = tmpResult.Substring(tmpResult.IndexOf("Result<<")+8,tmpResult.IndexOf(">>")-tmpResult.IndexOf("result<<")-11);
						if(tmpResult.Equals("null"))
						{
							result=0;
						}
						else
						{
							result =1;
						}
					}
					else
					{
						if(tmpResult.Equals("null"))
						{
							strDesc="����ң�"+UserName.ToString()+"���м��ʧ�ܣ���ȷ�ϴ��û��Ƿ���ڣ�";
							SqlHelper.insertGMtoolsLog(userByID,"������II",serverip,"JW2_Center_KickUser","����ң�"+UserName.ToString()+"���м��ʧ��");
							result=0;
						}
						else
						{
							strDesc="����ң�"+UserName.ToString()+"���м���ɹ������Եȣ�ϵͳ�����У�";
							SqlHelper.insertGMtoolsLog(userByID,"������II",serverip,"JW2_Center_KickUser","����ң�"+UserName.ToString()+"���м���ɹ�");
							result =1;
						}
					}
				}	
			}
			catch (System.Exception ex)
			{
				strDesc = "���ݿ�����ʧ�ܣ������³��ԣ�";
				//SqlHelper.errLog.WriteLog("������UserShowUserVipLevel_Query"+ex.Message);
				result = -1;
			}
			return result;
		}
		#endregion 

		#region �ʺ���ת�ʺ�ID
		/// <summary>
		/// �ʺ���ת�ʺ�ID
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="dbid"></param>
		/// <returns></returns>
		public static int AU_Account_UserSn(string serverIP,string userName)
		{
			DataSet result = null;
			int UserSn = 0;
			string sql = null;
			try
			{
				sql = "select sql_statement from sqlexpress where sql_type='AU_Account_UserSn' and sql_condition = 'AU_Account_UserSn'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userName);
					DataSet Ds = MySqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(true,serverIP,SqlHelper.AuUser,SqlHelper.AuPwd,"AuditionLogin"),sql);
					if(Ds!=null && Ds.Tables[0].Rows.Count>0)
					{
						UserSn = int.Parse(Ds.Tables[0].Rows[0].ItemArray[0].ToString());
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return UserSn;
		}
		#endregion

		#region AU�û������ѯ
		/// <summary>
		/// AU�û������ѯ
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="itemID"></param>
		/// <returns></returns>
		public static string AU_Get_Pwd(string serverIP,int id)
		{
			string result = "";	
			try
			{
				string sql = "select sql_statement from sqlexpress where sql_type='AU_Get_Pwd' and sql_condition = 'AU_Get_Pwd'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,id);
					DataSet Ds = MySqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(true,serverIP,SqlHelper.AuUser,SqlHelper.AuPwd,"AuditionLogin"),sql);
					if (Ds != null && Ds.Tables[0].Rows.Count > 0)
					{
						result = Ds.Tables[0].Rows[0].ItemArray[0].ToString();
					}
				}
				
			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return result;
		}
		#endregion
		
		#region AU�û�ԭʼ������뱾��
		/// <summary>
		/// AU�û�ԭʼ������뱾��
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="itemID"></param>
		/// <returns></returns>
		public static int AU_insert_LocalPwd(string serverIP,int userSN,string username, string pwd)
		{
			int result = -1;
			try
			{
				string sql = "select sql_statement from sqlexpress where sql_type='AU_insert_LocalPwd' and sql_condition = 'AU_insert_LocalPwd'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,serverIP,userSN,username,pwd);
					result  = SqlHelper.ExecCommand(sql);
				}
				
			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return result;
		}
		#endregion

		#region AU�޸��û�����/��ԭ�û�����
		/// <summary>
		/// AU�޸��û�����/��ԭ�û�����
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="itemID"></param>
		/// <returns></returns>
		public static int AU_Update_Pwd(string serverIP,int userSN,string pwd)
		{
			int result = -1;
			try
			{
				string sql = "select sql_statement from sqlexpress where sql_type='AU_Update_Pwd' and sql_condition = 'AU_Update_Pwd'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userSN,pwd);
					result  = MySqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(true,serverIP,SqlHelper.AuUser,SqlHelper.AuPwd,"AuditionLogin"),sql);
				}
				
			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴�����Ƿ��д��ʺ�����
		/// <summary>
		/// �鿴�����Ƿ��д��ʺ�����
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="itemID"></param>
		/// <returns></returns>
		public static  string AU_Check_UserPwd(string serverIP,int userSN)
		{
			System.Data.DataSet result = new DataSet();
			string pwd = "";
			try
			{
				string sql = "select sql_statement from sqlexpress where sql_type='AU_Check_UserPwd' and sql_condition = 'AU_Check_UserPwd'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userSN,serverIP);
					result  = SqlHelper.ExecuteDataset(sql);
					if(result !=null &&result.Tables[0].Rows.Count>0)
					{
						pwd = result.Tables[0].Rows[0].ItemArray[0].ToString();
					}
				}
				
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return pwd;
		}
		#endregion
		#endregion


		#region ͨ��Oracle
		/// <summary>
		/// ͨ��Oracle
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="zonie"></param>
		/// <returns></returns>
		public static DataSet RunOracle(string sql,string DataSource,string userID,string Password)
		{
			try
			{
				DataSet result = new DataSet();
				SqlOracle sqlOrl = null;
				sqlOrl = new SqlOracle("Data Source="+DataSource+";User Id="+userID+";PERSIST SECURITY INFO=True;Password="+Password+";");
				result = sqlOrl.GetDataSet(sql);
				return result;
			}
			catch(System.Exception ex)
			{
				Console.WriteLine("ORACLE���ݿ��쳣"+ex.Message);
				return null;
			}
		}
		#endregion

		#region ִ��Oracle_jw2
		/// <summary>
		/// ִ��Oracle_jw2
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="zonie"></param>
		/// <returns></returns>
		public static DataSet RunOracle(string sql,int zonie)
		{
			try
			{
				DataSet result = new DataSet();
				SqlOracle sqlOrl = null;
				sqlOrl = new SqlOracle("Data Source=GTAVATAR;User Id=gmtools;Password=E#92lGkd205K;");
				result = sqlOrl.GetDataSet(sql);
				return result;
			}
			catch(System.Exception)
			{
				return null;
			}
		}
		#endregion
	}
}
