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
using System.Runtime.InteropServices;
using Common.Logic;
using Common.API;
using MySql.Data.MySqlClient;
using Common.DataInfo;
using lg = Common.API.LanguageAPI;

namespace GM_Server.JW2DataInfo
{
	/// <summary>
	/// JW2LoginDataInfo ��ժҪ˵����
	/// </summary>
	public class JW2LoginDataInfo
	{
		//����
		

		[DllImport("OperTool.dll", CharSet = CharSet.Ansi)]
		public static extern bool BulletinTool(StringBuilder IP, int port, StringBuilder content);
		[DllImport("OperTool.dll", CharSet = CharSet.Ansi)]
		public static extern bool KickTool(StringBuilder IP, int port, StringBuilder userID);

		#region DLL����
		/// <summary>
		///����DSNShell������ֵΪDSNShell�ľ����
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern int CreateDSNShell();
		/// <summary>
		///����DSNShell�������
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern void DestroyDSNShell(int handle);
		/// <summary>
		///����DSNShell��������յ���Ϣ�ı�־��
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern int DSNShellUpdate(int handle,byte[] result);
		/// <summary>
		///����GW�������IP���˿ڣ�---Send
		/// </summary>
		[DllImport("DSNShell.dll", CharSet = CharSet.Unicode)]
		public static extern int DSNShellConnectGW(int handle,StringBuilder Ip, int port);
		/// <summary>
		///����GW�������IP���˿ڣ�---Rev
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern int DSNShellIsGWConnected(int handle);
		/// <summary>
		///��½GW��������ʺţ����룬�汾�ţ�--Send
		/// </summary>
		[DllImport("DSNShell.dll", CharSet = CharSet.Unicode)]
		public static extern int DSNShellLoginGW(int handle,StringBuilder szAccount, StringBuilder szPassword, StringBuilder szVersion);
		/// <summary>
		///��½GW��������ʺţ����룬�汾�ţ�--Rev
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern int DSNShellLoginGWRet(int handle,byte[] result);
		/// <summary>
		///ȡ��GS�б������--Send
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern int DSNShellServerList(int handle);
		/// <summary>
		///ȡ��GS�б������--Rev
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern int DSNShellServerListRet(int handle, byte[] result);
		/// <summary>
		///���õ�ǰGS�ľ��鱶���������������������1��2��
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern int DSNShellSetGPointTimes(int handle,  byte times);
		/// <summary>
		///���õ�ǰGS��G�ұ����������������������1��2��
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern int DSNShellSetExpTimes(int handle,  byte times);
		/// <summary>
		///����GS�������IP���˿ڣ�--Send
		/// </summary>
		[DllImport("DSNShell.dll", CharSet = CharSet.Unicode)]
		public static extern int DSNShellConnectGS(int handle, StringBuilder Ip, int port);
		/// <summary>
		///����GS�������IP���˿ڣ�--Rev
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern int DSNShellIsGSConnected(int handle);
		/// <summary>
		///��½GS��������û���ţ�--Send
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern int DSNShellLoginGS(int handle, int ulSerialNo);
		/// <summary>
		///��½GS��������û���ţ�--Rev
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern int DSNShellLoginGSRet(int handle, byte result);
		/// <summary>
		/// ���ˣ�������ʺţ�
		/// </summary>		
		[DllImport("DSNShell.dll", CharSet = CharSet.Unicode)]
		public static extern int DSNShellKickUser(int handle, StringBuilder szAccount);
		#endregion

		public JW2LoginDataInfo()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PacketHeaderL
		{
			public byte[] sIP;
		}
		#region DLL�ӿڵ���
		#region ����DSNShell������ֵΪDSNShell�ľ����
		/// <summary>
		/// ����DSNShell������ֵΪDSNShell�ľ����
		/// </summary>
		/// <returns></returns>
		public static int JW2_CreateDLL()
		{
			return CreateDSNShell();
		}
		#endregion

		#region ����DSNShell�������
		/// <summary>
		/// ����DSNShell�������
		/// </summary>
		/// <returns></returns>
		public static void JW2_DestroyDLL(int C_Handle)
		{
			DestroyDSNShell(C_Handle);
		}
		#endregion

		#region ����GW�������IP���˿ڣ�---Send/Rev
		/// <summary>
		/// ����GW�������IP���˿ڣ�---Send/Rev
		/// </summary>
		/// <param name="C_Handle"></param>
		/// <returns></returns>
		public static int JW2_ConnectGW(int C_Handle,string Ip,int Port)
		{
			int state = -1;
			try
			{
				byte[] result_GW = new byte[255];
				StringBuilder sb_IP = new StringBuilder(Ip);
				//����LOGIN���ݿ�
				if (2 == DSNShellIsGWConnected(C_Handle))
				{
					if (0 == DSNShellConnectGW(C_Handle, sb_IP, Port))
					{
						Console.WriteLine("Begin Connect->GS��" + Ip + "-->" + Port);
					}
				}
				int x = 0;
				//�ж�LOGIN ����״̬
				while (true)
				{
					System.Threading.Thread.Sleep(500);
					DSNShellUpdate(C_Handle, result_GW);
					state = DSNShellIsGWConnected(C_Handle);
					Console.WriteLine("Now GS��" + Ip + "-->" + Port + "-->Connection Status��->" + state);
					if (0 == state)
					{
						Console.WriteLine(Ip + "-->" + Port + "-->Connection Success");
						break;
					}
					else
					{
						x++;
						if (x > 30)
						{
							Console.WriteLine(Ip + "-->" + Port + "-->Connection OverTime");
							break;
						}
					}
				}
			}
			catch(System.Exception ex)
			{
				
			}
			return state;
		}
		#endregion
		#endregion
		public static int BanishPlayer  (string serverIP,string userName)
		{
			byte[] result = new byte[255];
			int handle = -1;
			int int_Result = -1;
			try
			{
				
				handle = JW2_CreateDLL();
				string serverIPStard = CommonInfo.JW2_FindDBIP(serverIP,5);
				int state = JW2_ConnectGW(handle,serverIPStard,58118);
				if (state == 0)
				{
					DSNShellUpdate(handle,result);
					StringBuilder S_UserName = new StringBuilder(userName);
					int_Result = DSNShellKickUser(handle,S_UserName);
				}
			}
			catch(System.Exception ex)
			{
				string exc = ex.Message;
			}
			finally
			{
				JW2_DestroyDLL(handle);
			}
			return int_Result;
		}

		#region ���û�����
		/// <summary>
		/// ���û�����
		/// </summary>
		public static int BANISHPLAYER(string serverIP,string userName,int userbyid,ref string strDesc)
		{
			int  result = -1;
			string sql = null;
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,5);
				string usby = "0";
				result = BanishPlayer(serverIP,userName);
				if(result==0)
				{
					strDesc = lg.JW2API_BANISHPLAYER+userName.ToString()+lg.JW2API_SuccessPleaseWait;
					SqlHelper.insertGMtoolsLog(userbyid,"jw2",serverIP,"JW2_BanishPlayer",lg.JW2API_BANISHPLAYER+userName.ToString()+lg.JW2API_Success);
				}
				else
				{
					strDesc = lg.JW2API_BANISHPLAYER+userName.ToString()+lg.JW2API_Failure;
					SqlHelper.insertGMtoolsLog(userbyid,"jw2",serverIP,"JW2_BanishPlayer",lg.JW2API_BANISHPLAYER+userName.ToString()+lg.JW2API_Failure);
				}
			}
			catch (System.Exception ex)
			{
				strDesc = lg.JW2API_DatebaseConnectError;
				SqlHelper.errLog.WriteLog("ServerIP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ����û�
		/// <summary>
		/// ����û�
		/// </summary>
		public static int ACCOUNT_OPEN(string serverIP,int usersn,string userNick,string userName,int userbyid,string Reason,ref string strDesc)
		{
			int  result = -1;
			string sql = null;
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,8);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_ACCOUNT_OPEN' and sql_condition = 'JW2_ACCOUNT_OPEN'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2loginDB),sql);
				}
				if(result==1)
				{
					strDesc =lg.JW2API_ACCOUNTOPEN+userName.ToString()+lg.JW2API_PlayerSN+userNick.ToString()+lg.JW2API_Success;
					result = CommonInfo.JW2_UnBanUser(serverIP,userbyid,usersn,userName,Reason);
					SqlHelper.insertGMtoolsLog(userbyid,"JW2",serverIP,"JW2_ACCOUNT_OPEN",lg.JW2API_ACCOUNTOPEN+userName.ToString()+lg.JW2API_Success);
				}
				else
				{
					strDesc =lg.JW2API_ACCOUNTOPEN+userName.ToString()+lg.JW2API_PlayerSN+userNick.ToString()+lg.JW2API_Failure;
					result = CommonInfo.JW2_UnBanUser(serverIP,userbyid,usersn,userName,Reason);
					SqlHelper.insertGMtoolsLog(userbyid,"JW2",serverIP,"JW2_ACCOUNT_OPEN",lg.JW2API_ACCOUNTOPEN+userName.ToString()+lg.JW2API_Failure);
				}
			}
			catch (System.Exception  ex)
			{
				strDesc = lg.JW2API_DatebaseConnectError;
				SqlHelper.errLog.WriteLog("ServerIP"+serverIP+ex.Message);
			}
//			catch (MySqlException  ex)
//			{
//				strDesc = "���ݿ�����ʧ�ܣ������³��ԣ�";
//				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
//			}
			return result;
		}
		#endregion

		#region ��ͣ�û�
		/// <summary>
		/// ��ͣ�û�
		/// </summary>
		public static int ACCOUNT_CLOSE(string serverIP,int usersn,string userNick,string userName,int userbyid,string Reason,ref string strDesc)
		{
			int  result = -1;
			string sql = null;
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,8);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_ACCOUNT_CLOSE' and sql_condition = 'JW2_ACCOUNT_CLOSE'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2loginDB),sql);
				}
				if(result>0)
				{
					strDesc = lg.JW2API_ACCOUNTCLOSE+userName.ToString()+lg.JW2API_PlayerSN+userNick.ToString()+lg.JW2API_Success;
					result = CommonInfo.JW2_BanUser(serverIP,userbyid,usersn,userName,userNick,Reason);
					SqlHelper.insertGMtoolsLog(userbyid,"JW2",serverIP,"JW2_ACCOUNT_CLOSE",lg.JW2API_ACCOUNTCLOSE+userName.ToString()+lg.JW2API_Success);
				}
				else
				{
					strDesc =  lg.JW2API_ACCOUNTCLOSE+userName.ToString()+lg.JW2API_PlayerSN+userNick.ToString()+lg.JW2API_Failure;
					result = CommonInfo.JW2_BanUser(serverIP,userbyid,usersn,userName,userNick,Reason);
					SqlHelper.insertGMtoolsLog(userbyid,"JW2",serverIP,"JW2_ACCOUNT_CLOSE",lg.JW2API_ACCOUNTCLOSE+userName.ToString()+lg.JW2API_Failure);
				}
			}
			catch (MySqlException ex)
			{
				strDesc =  lg.JW2API_DatebaseConnectError;
				SqlHelper.errLog.WriteLog("ServerIP"+serverIP+ex.Message);
				if(ex.Message=="Duplicate entry '"+usersn+"' for key 1")
				{
					result=2;
				}
			}
			return result;
		}
		#endregion

		#region ��ҷ�ͣ�ʺ���Ϣ��ѯ
		/// <summary>
		/// ��ҷ�ͣ�ʺ���Ϣ��ѯ
		/// </summary>
		public static DataSet ACCOUNT_BANISHMENT_QUERY(string serverIP,string userid,int type)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,8);
				if(type==0)
				{
					sql = "select sql_statement from sqlexpress where sql_type='JW2_ACCOUNT_BANISHMENT_QUERY_ALL' and sql_condition='JW2_ACCOUNT_BANISHMENT_QUERY_ALL'";
					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{
						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,serverIP);
						result = SqlHelper.ExecuteDataset(sql);
					}
				}
				else
				{
					sql = "select sql_statement from sqlexpress where sql_type='JW2_ACCOUNT_BANISHMENT_QUERY' and sql_condition='JW2_ACCOUNT_BANISHMENT_QUERY'";
					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{
						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,userid,serverIP);
						result = SqlHelper.ExecuteDataset(sql);
					}
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("ServerIP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �������
		/// <summary>
		/// �������
		/// </summary>
		public static int BOARDTASK_UPDATE(string serverIP,int Taskid,string BoardMessage,string BeginTime,string EndTime,int userbyid,int Interval,int Status)
		{
			int result = -1;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[8]{
												   new SqlParameter("@Gm_UserID",SqlDbType.Int),
												   new SqlParameter("@JW2_TaskID",SqlDbType.Int),
												   new SqlParameter("@JW2_BoardMessage",SqlDbType.VarChar,256),
												   new SqlParameter("@JW2_begintime",SqlDbType.DateTime),
												   new SqlParameter("@JW2_endTime",SqlDbType.DateTime),
												   new SqlParameter("@JW2_interval",SqlDbType.Int),
												   new SqlParameter("@JW2_Status",SqlDbType.Int),
												   new SqlParameter("@result",SqlDbType.Int)};
				paramCode[0].Value = userbyid;
				paramCode[1].Value = Taskid;
				paramCode[2].Value = BoardMessage;
				paramCode[3].Value = BeginTime; 
				paramCode[4].Value = EndTime;
				paramCode[5].Value = Interval;
				paramCode[6].Value = Status;
				paramCode[7].Direction = ParameterDirection.ReturnValue;
				result = SqlHelper.ExecSPCommand("JW2_BOARDTASK_UPDATE",paramCode);
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return  result;
		}
		#endregion

		#region �����ѯ
		/// <summary>
		/// �����ѯ
		/// </summary>
		public static DataSet BOARDTASK_QUERY()
		{
			DataSet ds = null;
			try
			{
				ds = SqlHelper.ExecSPDataSet("JW2_BOARDTASK_QUERY");
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return ds;
		}
		#endregion

		#region �������
		/// <summary>
		/// �������
		/// </summary>
		public static int BOARDTASK_INSERT(string serverIP,string GSserverIp,string BoardMessage,string BeginTime,string EndTime,int userbyid,int Interval)
		{
			int result = -1;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[8]{
												   new SqlParameter("@Gm_UserID",SqlDbType.Int),
												   new SqlParameter("@JW2_ServerIP",SqlDbType.VarChar,500),
												   new SqlParameter("@JW2_GSServerIP",SqlDbType.VarChar,500),
												   new SqlParameter("@JW2_BoardMessage",SqlDbType.VarChar,256),
												   new SqlParameter("@JW2_begintime",SqlDbType.DateTime),
												   new SqlParameter("@JW2_endTime",SqlDbType.DateTime),
												   new SqlParameter("@JW2_interval",SqlDbType.Int),
												   new SqlParameter("@result",SqlDbType.Int)};
				paramCode[0].Value = userbyid;
				paramCode[1].Value = serverIP;
				paramCode[2].Value = GSserverIp;
				paramCode[3].Value = BoardMessage;
				paramCode[4].Value = BeginTime;
				paramCode[5].Value = EndTime;
				paramCode[6].Value = Interval;
				paramCode[7].Direction = ParameterDirection.ReturnValue;
				result = SqlHelper.ExecSPCommand("JW2_BOARDTASK_INSERT",paramCode);


			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return  result;
		}
		#endregion

		#region �鿴��ҵ�½�ǳ���Ϣ
		/// <summary>
		/// �鿴��ҵ�½�ǳ���Ϣ
		/// </summary>
		public static DataSet LOGINOUT_QUERY(string serverIP,int usersn,string login_IP,string BeginTime,string EndTime)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,3);
				if(login_IP=="")
				{
					sql = "select sql_statement from sqlexpress where sql_type='JW2_LOGINOUT_QUERY' and sql_condition='1'";
					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{
						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,usersn,BeginTime,EndTime);
						result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2loginDB),sql);
					}
				}
				else
				{
					sql = "select sql_statement from sqlexpress where sql_type='JW2_LOGINOUT_QUERY' and sql_condition='2'";
					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{
						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,login_IP,BeginTime,EndTime);
						result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2loginDB),sql);
					}
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_LOGINOUT_QUERY_�鿴���"+usersn.ToString()+"��½�ǳ���Ϣ������IP"+serverIP+"��½ip"+login_IP+"��ʼʱ��"+BeginTime+"����ʱ��"+EndTime+ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴��һ�����Ϣ
		/// <summary>
		/// �鿴��һ�����Ϣ
		/// </summary>
		public static DataSet WEDDINGINFO_QUERY(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_WEDDINGINFO_QUERY' and sql_condition='JW2_WEDDINGINFO_QUERY'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_WEDDINGINFO_QUERY_�鿴���"+usersn.ToString()+"������Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴��һ�����ʷ
		/// <summary>
		/// �鿴��һ�����ʷ
		/// </summary>
		public static DataSet WEDDINGLOG_QUERY(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_WEDDINGLOG_QUERY' and sql_condition='JW2_WEDDINGLOG_QUERY'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_WEDDINGLOG_QUERY_�鿴���"+usersn.ToString()+"������ʷ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion
		
		#region �鿴��ҽ����Ϣ
		/// <summary>
		/// �鿴��ҽ����Ϣ
		/// </summary>
		public static DataSet WEDDINGGROUND_QUERY(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_WEDDINGGROUND_QUERY' and sql_condition='JW2_WEDDINGGROUND_QUERY'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_WEDDINGGROUND_QUERY_�鿴���"+usersn.ToString()+"�����Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion
		
		#region �鿴���������Ϣ
		/// <summary>
		/// �鿴���������Ϣ
		/// </summary>
		public static DataSet COUPLEINFO_QUERY(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_COUPLEINFO_QUERY' and sql_condition='JW2_COUPLEINFO_QUERY'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_COUPLEINFO_QUERY_�鿴���"+usersn.ToString()+"������Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴���������ʷ
		/// <summary>
		/// �鿴���������ʷ
		/// </summary>
		public static DataSet COUPLELOG_QUERY(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_COUPLELOG_QUERY' and sql_condition='JW2_COUPLELOG_QUERY'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_COUPLELOG_QUERY_�鿴���"+usersn.ToString()+"������ʷ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �޸���ʱ����
		/// <summary>
		/// �޸���ʱ����
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static int TmpPassWord_Query(string serverIP,string serverName,int UserByID,int userid,string username,string TmpPwd,ref string strDesc)
		{
			DataSet ds = null;
			int result = -1;
			string RelPwd = null;
			string sql = null;
			MD5Encrypt st = new MD5Encrypt();
			//string sign = st.getMD5ofStr(TmpPwd).ToLower();
			string sign = TmpPwd;
			try
			{
				//��ѯ�Ƿ��޸Ĺ�
				sql = "select sql_statement from sqlexpress where sql_type='JW2_SearchTmpPWD_Staute_Query' and sql_condition = 'JW2_SearchTmpPWD_Staute_Query'";
				System.Data.DataSet ds4 = SqlHelper.ExecuteDataset(sql);
				if(ds4!=null && ds4.Tables[0].Rows.Count>0)
				{	
					sql = ds4.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,serverName,userid);
					System.Data.DataSet ds5 = SqlHelper.ExecuteDataset(sql);
					if(ds5.Tables[0].Rows.Count==0)
					{
						//��ȡ��ʵ����
						serverIP = CommonInfo.JW2_FindDBIP(serverIP,3);
						sql = "select sql_statement from sqlexpress where sql_type='JW2_GetPassWord_Query' and sql_condition = 'JW2_GetPassWord_Query'";
						System.Data.DataSet ds1 = SqlHelper.ExecuteDataset(sql);
						if(ds1!=null && ds1.Tables[0].Rows.Count>0)
						{	
							sql = ds1.Tables[0].Rows[0].ItemArray[0].ToString();
							sql = string.Format(sql,userid);
							ds = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2loginDB),sql);
							if(ds!=null && ds.Tables[0].Rows.Count>0)
							{
								serverIP = CommonInfo.JW2_FindDBIP(serverIP,8);
								RelPwd = ds.Tables[0].Rows[0].ItemArray[0].ToString();
								//�޸�����
								sql = "select sql_statement from sqlexpress where sql_type='JW2_TmpPassWord_Query' and sql_condition = 'JW2_TmpPassWord_Query'";
								System.Data.DataSet ds2 = SqlHelper.ExecuteDataset(sql);
								if(ds2!=null && ds2.Tables[0].Rows.Count>0)
								{	
									sql = ds2.Tables[0].Rows[0].ItemArray[0].ToString();
									sql = string.Format(sql,sign,userid);
									result = MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2loginDB),sql);
								}
								if(result==1)
								{
									//��¼��ʵ����ʱ������125��
									sql = "select sql_statement from sqlexpress where sql_type='JW2_InsertTmpPassWord_Query' and sql_condition = 'JW2_InsertTmpPassWord_Query'";
									System.Data.DataSet ds3 = SqlHelper.ExecuteDataset(sql);
									if(ds3!=null && ds3.Tables[0].Rows.Count>0)
									{	
										sql = ds3.Tables[0].Rows[0].ItemArray[0].ToString();
										sql = string.Format(sql,serverName,userid,username,RelPwd,TmpPwd,sign,1);
										result = SqlHelper.ExecCommand(sql);
									}
								}
							}
						}
					}
					else
					{
						result = 2;
					}
				}
				if(result==1)
				{
					strDesc = "�޸��û�"+username.ToString()+"��ʱ����ɹ������Եȣ�ϵͳ�����У�";
					SqlHelper.insertGMtoolsLog(UserByID,"������2",serverIP,"JW2_TmpPassWord_Query","�޸��û�"+username.ToString()+"��ʱ���룬�ɹ�(�޸���ʱ����,jw2)");
				}
				else
				{
					strDesc = "�޸��û�"+username.ToString()+"��ʱ����ʧ�ܣ�ȷ�ϸ�����Ƿ�������ʱ���룡";
					SqlHelper.insertGMtoolsLog(UserByID,"������2",serverIP,"JW2_TmpPassWord_Query","�޸��û�"+username.ToString()+"��ʱ���룬ʧ��(�޸���ʱ����,jw2)");
				}
			}
			catch (System.Exception ex)
			{
				strDesc = "���ݿ�����ʧ�ܣ������³��ԣ�";
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �ָ���ʱ����
		/// <summary>
		/// �ָ���ʱ����
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static int ReTmpPassWord_Query(string serverIP,string serverName,int UserByID,int userid,string username,ref string strDesc)
		{
			DataSet ds = null;
			int result = -1;
			string RelPwd = null;
			string sql = null;
			try
			{
				//��ȡ������ʵ����
				sql = "select sql_statement from sqlexpress where sql_type='JW2_GetRelPassWord_Query' and sql_condition = 'JW2_GetRelPassWord_Query'";
				System.Data.DataSet ds1 = SqlHelper.ExecuteDataset(sql);
				if(ds1!=null && ds1.Tables[0].Rows.Count>0)
				{	
					sql = ds1.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,serverName,userid);
					ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{
						RelPwd = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						//�ָ���ʱ����
						serverIP = CommonInfo.JW2_FindDBIP(serverIP,8);
						sql = "select sql_statement from sqlexpress where sql_type='JW2_TmpPassWord_Query' and sql_condition = 'JW2_TmpPassWord_Query'";
						System.Data.DataSet ds2 = SqlHelper.ExecuteDataset(sql);
						if(ds2!=null && ds2.Tables[0].Rows.Count>0)
						{	
							sql = ds2.Tables[0].Rows[0].ItemArray[0].ToString();
							sql = string.Format(sql,RelPwd,userid);
							result = MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2loginDB),sql);
						}
						if(result==1)
						{
							//���±�����ʱ����״̬
							sql = "select sql_statement from sqlexpress where sql_type='JW2_UpdateTmpPassWord_Query' and sql_condition = 'JW2_UpdateTmpPassWord_Query'";
							System.Data.DataSet ds3 = SqlHelper.ExecuteDataset(sql);
							if(ds3!=null && ds3.Tables[0].Rows.Count>0)
							{	
								sql = ds3.Tables[0].Rows[0].ItemArray[0].ToString();
								sql = string.Format(sql,serverName,userid,0);
								SqlHelper.ExecCommand(sql);
							}
						}
					}
					else
					{
						result = 2;
					}
				}
				if(result==1)
				{
					strDesc = "�ָ��û�"+username.ToString()+"��ʱ����ɹ������Եȣ�ϵͳ�����У�";
					SqlHelper.insertGMtoolsLog(UserByID,"������2",serverIP,"JW2_TmpPassWord_Query","�ָ��û�"+username.ToString()+"��ʱ���룬�ɹ�(�ָ���ʱ����,jw2)");
				}
				else
				{
					strDesc = "�ָ��û�"+username.ToString()+"��ʱ����ʧ�ܣ�ȷ�ϸ�����Ƿ�������ʱ���룡";
					SqlHelper.insertGMtoolsLog(UserByID,"������2",serverIP,"JW2_TmpPassWord_Query","�ָ��û�"+username.ToString()+"��ʱ���룬ʧ��(�ָ���ʱ����,jw2)");
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

		#region ��ѯ���һ����ʱ����
		/// <summary>
		/// ��ѯ���һ����ʱ����
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet SearchPassWord_Query(string serverIP,string serverName,int userid,string username)
		{
			DataSet result = null;
			string sql = null;
			try
			{
				sql = "select sql_statement from sqlexpress where sql_type='JW2_SearchPassWord_Query' and sql_condition = 'JW2_SearchPassWord_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,serverName,userid);
					result = SqlHelper.ExecuteDataset(sql);
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_SearchPassWord_Query_�鿴���"+userid.ToString()+"-"+username+"���һ����ʱ���������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		

		



	}

}
