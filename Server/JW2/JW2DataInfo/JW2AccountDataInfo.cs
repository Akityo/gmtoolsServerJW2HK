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
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace GM_Server.JW2DataInfo
{
	/// <summary>
	/// JW2AccountDataInfo ��ժҪ˵����
	/// </summary>
	public class JW2AccountDataInfo
	{
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
		public static extern int DSNShellConnectGW(int handle,string Ip, int port);
		/// <summary>
		///����GW�������IP���˿ڣ�---Rev
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern int DSNShellIsGWConnected(int handle);
		/// <summary>
		///��½GW��������ʺţ����룬�汾�ţ�--Send
		/// </summary>
		[DllImport("DSNShell.dll", CharSet = CharSet.Unicode)]
		public static extern int DSNShellLoginGW(int handle, string szAccount, string szPassword, string szVersion);
		/// <summary>
		///��½GW��������ʺţ����룬�汾�ţ�--Rev
		/// </summary>
		[DllImport("DSNShell.dll")]
		public static extern int DSNShellLoginGWRet(int handle,byte[] result);
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
		public static extern int DSNShellConnectGS(int handle, string Ip, int port);
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
		public static extern int DSNShellLoginGSRet(int handle, byte[] result);
		#endregion
		#region ?��
		private static byte[] result = new byte[256];
		private static byte[] result_OK = new byte[1024];
		#endregion
		enum DSNSHELL_MSGIDX
		{
			MSGIDX_LOGINGW = 1,
			MSGIDX_SERVERLIST,

			MSGIDX_LOGINGS,
			MSGIDX_CHANNELLIST,
			MSGIDX_ENTERCHANNEL,

			MSGIDX_USERLIST,
			MSGIDX_ROOMLIST,
			MSGIDX_ROOMDETAIL,
			MSGIDX_CHAT,
			MSGIDX_WHISPER,

			MSGIDX_ROOMCREATED,
			MSGIDX_ROOMDESTROYED,
			MSGIDX_USERENTERCHANNEL,
			MSGIDX_USERLEAVECHANNEL,
			MSGIDX_LABA,

			MSGIDX_ROOMSLOTLIST,
		};
		public JW2AccountDataInfo()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		#region MD5����
		private static string MD5ToString(String argString) 
		{ 
			MD5 md5 = new MD5CryptoServiceProvider(); 
			byte[] data = System.Text.Encoding.Default.GetBytes(argString); 
			byte[] result = md5.ComputeHash(data); 
			String strReturn = String.Empty; 
			for (int i = 0; i < result.Length; i++) 
				strReturn += result[i].ToString("x").PadLeft(2, '0'); 
			return strReturn; 
		} 
		#endregion

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
				byte[] result_GW = new byte[256];
				//����LOGIN���ݿ�
				if (2 == DSNShellIsGWConnected(C_Handle))
				{
					if (0 == DSNShellConnectGW(C_Handle, Ip, Port))
					{
						//Console.WriteLine("Begin Connect->GW��" + Ip + "-->" + Port);
					}
				}
				int x = 0;
				//�ж�LOGIN ����״̬
				while (true)
				{
					System.Threading.Thread.Sleep(500);
					DSNShellUpdate(C_Handle, result_GW);
					System.Threading.Thread.Sleep(500);
					state = DSNShellIsGWConnected(C_Handle);
					Console.WriteLine("Now GW��" + Ip + "-->" + Port + "-->Connection Status��->" + state);
					if (0 == state)
					{
						//Console.WriteLine(Ip + "-->" + Port + "-->GW-->Connection Success");
						break;
					}
					else
					{
						x++;
						if (x > 10)
						{
							Console.WriteLine(Ip + "-->" + Port + "-->GW-->�B�ӳ��r OverTime");
							break;
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(Ip + "-->" + Port + "-->GW-->Connection Faile");
			}
			return state;
		}
		#endregion

		#region ��½GW��������ʺţ����룬�汾�ţ�--Send/Rev
		/// <summary>
		/// ��½GW��������ʺţ����룬�汾�ţ�--Send/Rev
		/// </summary>
		/// <param name="C_Handle"></param>
		/// <returns></returns>
		public static int JW2_LoginGW(int state, int C_Handle, string szAccount, string szPassword, string szVersion)
		{
			try
			{
				if (state == 0)
				{
					state = -1;
					DSNShellLoginGW(C_Handle, szAccount, szPassword, szVersion);
					int x = 0;
					while (true)
					{
						System.Threading.Thread.Sleep(500);
						DSNShellUpdate(C_Handle, result);
						System.Threading.Thread.Sleep(500);
						if (result[0] == (byte)1 && result[1] == (byte)1)
						{
							DSNShellLoginGWRet(C_Handle, result_OK);
							if (result_OK[0] == (byte)0)
							{
								state = 0;
								Console.WriteLine("��½GW�ɹ�");
								break;
							}
							else
							{
								state = -1;
								Console.WriteLine("��½GWʧ��");
								break;
							}
						}
						else
						{
							x++;
							if (x > 10)
							{
								Console.WriteLine("��½GW OverTime1");
								state = -1;
								break;
							}
						}
					}

				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine("��½GWʧ��->"+ex.Message);
			}
			return state;
		}
		#endregion

		#region ����GS�������IP���˿ڣ�---Send/Rev
		/// <summary>
		/// ����GS�������IP���˿ڣ�---Send/Rev
		/// </summary>
		/// <param name="C_Handle"></param>
		/// <returns></returns>
		public static int JW2_ConnectGS(int C_Handle, string Ip, int Port)
		{
			int state = -1;
			try
			{
				byte[] result_GS = new byte[256];
				//����LOGIN���ݿ�
				if (2 == DSNShellIsGSConnected(C_Handle))
				{
					if (0 == DSNShellConnectGS(C_Handle, Ip, Port))
					{
						//Console.WriteLine("Begin Connect->GS��" + Ip + "-->" + Port);
					}
				}
				int x = 0;
				//�ж�LOGIN ����״̬
				while (true)
				{
					DSNShellUpdate(C_Handle, result_GS);
					System.Threading.Thread.Sleep(500);
					state = DSNShellIsGSConnected(C_Handle);
					Console.WriteLine("Now GS��" + Ip + "-->" + Port + "-->Connection Status��->" + state);
					if (0 == state)
					{
						Console.WriteLine(Ip + "-->" + Port + "GS-->Connection Success");
						break;
					}
					else
					{
						x++;
						if (x > 10)
						{
							Console.WriteLine(Ip + "-->" + Port + "GS-->Connection OverTime");
							break;
						}
					}
				}
			}
			catch(System.Exception ex)
			{
				Console.WriteLine(Ip + "-->" + Port + "-->Connection Faile");
			}
			return state;
		}
		#endregion

		#region ��½GS��������ʺţ����룬�汾�ţ�--Send/Rev
		/// <summary>
		/// ��½GS��������ʺţ����룬�汾�ţ�--Send/Rev
		/// </summary>
		/// <param name="C_Handle"></param>
		/// <returns></returns>
		public static int JW2_LoginGS(int state, int C_Handle, int userSN)
		{
			try
			{
				byte[] result = new byte[256];
				byte[] b = new byte[1024];
				if (state == 0)
				{
					state = -1;
					DSNShellUpdate(C_Handle, result);
					DSNShellLoginGS(C_Handle, userSN);
					int x = 0;
					while (true)
					{                       
						DSNShellUpdate(C_Handle, result);
						System.Threading.Thread.Sleep(500);
						if (result[0] == (byte)1 && result[1] == (byte)3)
						{
							DSNShellLoginGSRet(C_Handle, b);
							if (b[0] == 0)
							{
								state = 0;
								Console.WriteLine("��½GS �ɹ�");
								break;
							}
							else
							{
								state = -1;
							}
						}
						else
						{
							x++;
							if (x > 10)
							{
								Console.WriteLine("��½GS OverTime1");
								state = -1;
								break;
							}
						}
					}
				}
			}
			catch (System.Exception)
			{
				Console.WriteLine("��½GS ʧ��");
			}
			return state;
		}
		#endregion

		#region ���õ�ǰGS�Ľ�Ǯ�����������������������1��2��
		/// <summary>
		/// ���õ�ǰGS�ľ��鱶���������������������1��2��
		/// </summary>
		/// <param name="C_Handle"></param>
		/// <returns></returns>
		public static string JW2_SetGPointTimes(int state, int C_Handle, byte times, string city, string name,int userByID)
		{
			string get_Result = "";
			try
			{				
				if (state == 0)
				{
					state = -1;
					state = DSNShellSetGPointTimes(C_Handle, times);
					if(state==0)
					{
						SqlHelper.insertGMtoolsLog(userByID,"������II",city,"JW2_ChangeServerExp_Query","��"+city.ToString()+"���������޸�GS����������"+name.ToString()+"������Ǯ����"+times.ToString()+"�������ɹ�");
						get_Result = "��"+city.ToString()+"���������޸�GS����������"+name.ToString()+"������Ǯ����"+times.ToString()+"�������ɹ�\n";
						Console.WriteLine(city + "-->" + name + "-->�޸��p�����X���ɹ�");
					}
					else
					{
						SqlHelper.insertGMtoolsLog(userByID,"������II",city,"JW2_ChangeServerExp_Query","��"+city.ToString()+"���������޸�GS����������"+name.ToString()+"������Ǯ����"+times.ToString()+"������ʧ��");
						get_Result = "��"+city.ToString()+"���������޸�GS����������"+name.ToString()+"������Ǯ����"+times.ToString()+"������ʧ��\n";
						Console.WriteLine(city + "-->" + name + "-->�޸��p�����X��ʧ��");
					}
				}
			}
			catch(System.Exception ex)
			{
				SqlHelper.insertGMtoolsLog(userByID,"������II",city,"JW2_ChangeServerExp_Query","��"+city.ToString()+"���������޸�GS����������"+name.ToString()+"������Ǯ����"+times.ToString()+"������ʧ��");
				get_Result = "��"+city.ToString()+"���������޸�GS����������"+name.ToString()+"������Ǯ����"+times.ToString()+"������ʧ��\n";
				Console.WriteLine(city + "-->" + name + "-->�޸��p�����X��ʧ��");
			}
			return get_Result;
		}
		#endregion

		#region ���õ�ǰGS�ľ��鱶���������������������1��2��
		/// <summary>
		/// ���õ�ǰGS�ľ��鱶���������������������1��2��
		/// </summary>
		/// <param name="C_Handle"></param>
		/// <returns></returns>
		public static string  JW2_SetExpTimes(int state, int C_Handle, byte times,string city,string name,int userByID)
		{
			string get_Result = "";
			try
			{
				if (state == 0)
				{
					state = -1;
					state = DSNShellSetExpTimes(C_Handle, times);
					if (state == 0)
					{
						SqlHelper.insertGMtoolsLog(userByID,"������II",city,"JW2_ChangeServerExp_Query","��"+city.ToString()+"���������޸�GS����������"+name.ToString()+"����??����"+times.ToString()+"�������ɹ�");
						get_Result = "��"+city.ToString()+"���������޸�GS����������"+name.ToString()+"����??����"+times.ToString()+"�������ɹ�\n";
						Console.WriteLine(city + "-->" + name + "-->�޸��p��??���ɹ�");
					}
					else
					{
						SqlHelper.insertGMtoolsLog(userByID,"������II",city,"JW2_ChangeServerExp_Query","��"+city.ToString()+"���������޸�GS����������"+name.ToString()+"����??����"+times.ToString()+"������ʧ��");
						get_Result = "��"+city.ToString()+"���������޸�GS����������"+name.ToString()+"����??����"+times.ToString()+"������ʧ��\n";
						Console.WriteLine(city + "-->" + name + "-->�޸��p��??��ʧ��");
					}
				}
			}
			catch (System.Exception ex)
			{
				SqlHelper.insertGMtoolsLog(userByID,"������II",city,"JW2_ChangeServerExp_Query","��"+city.ToString()+"���������޸�GS����������"+name.ToString()+"����??����"+times.ToString()+"������ʧ��");
				get_Result = "��"+city.ToString()+"���������޸�GS����������"+name.ToString()+"����??����"+times.ToString()+"������ʧ��\n";
				Console.WriteLine(city + "-->" + name + "-->�޸��p��??��ʧ��");
			}
			return get_Result;
		}
		#endregion
		#endregion

		#region �޸��p����B
		/// <summary>
		/// GM�޸��p����B
		/// </summary>
		public static string  ChangeServerExp_Query(string serverIP,int iExp,int iMoney,int userByID,string serverName,int type)
		{

			int handle = -1;
			int state = -1;
			string get_Result = "";
			serverIP = CommonInfo.JW2_FindDBIP(serverIP,5);
			string userPwd = MD5EncryptAPI.MDString(SqlHelper.jw2gwUserPwd);
			try
			{
				string[] serverList = serverName.Split('|');
				for(int i = 0;i<serverList.Length-1;i++)
				{
					string[] GSserverList = serverList[i].Split(',');
					string GSserverName = GSserverList[0].ToString();
					string GSserverIP = GSserverList[1].ToString();
					int GSserverNo = int.Parse(GSserverList[2].ToString());
					int port = int.Parse(GSserverList[3].ToString());
					string city = CommonInfo.serverIP_Query(serverIP);
					handle = JW2_CreateDLL();
					state = JW2_ConnectGW(handle, serverIP, 58118);
					if (state == 0)
					{
						state = JW2_LoginGW(state, handle, SqlHelper.jw2gwUser, userPwd, SqlHelper.jw2gwVersion);
						if (state == 0)
						{

							state = JW2_ConnectGS(handle, GSserverIP, port);
							if (state == 0)
							{
								state = JW2_LoginGS(state, handle, 7044699);
								if (state == 0)
								{
									if(type==0)
										get_Result += JW2_SetGPointTimes(state, handle, (byte)iMoney,city ,GSserverName,userByID);
									else if(type==1)
										get_Result = JW2_SetExpTimes(state, handle, (byte)iExp,city,GSserverName,userByID);
									else if(type==2)
									{
										get_Result += JW2_SetGPointTimes(state, handle, (byte)iMoney,city ,GSserverName,userByID);
										get_Result += JW2_SetExpTimes(state, handle, (byte)iExp,  city,GSserverName,userByID);
									}        
								}
								else
								{
									get_Result += "��"+city.ToString()+"����������?GS����������"+GSserverName.ToString()+"����ʧ��\n";
								}
							}
							else
							{
								get_Result += "��"+city.ToString()+"���������B��GS����������"+GSserverName.ToString()+"����ʧ��\n";
							}
						}
						else
						{
							get_Result += "��"+city.ToString()+"����������?��ʧ��\n";
						}
					}
					else
					{
						get_Result += "��"+city.ToString()+"���������B�ӣ�ʧ��\n";
					}
				}
			}
			catch (System.Exception ex)
			{
				get_Result +=  "���ݿ�����ʧ�ܣ������³��ԣ�";
				Console.WriteLine("789"+ex.Message);
			}
			JW2_DestroyDLL(handle);
			return get_Result;
		}
		#endregion

		#region �鿴�������
		/// <summary>
		/// �鿴�������
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet ACCOUNT_QUERY(string serverIP,string strname,ref string strDesc)
		{
			DataSet result = null;
			string sql = "";
			int zone = 0;
			string serverName = "";
			try
			{
				serverName = CommonInfo.JW2_FindDBName(serverIP);
//				zone = CommonInfo.JW2_GetZone_Query(13,serverName);
//				if(serverName=="����һ��"||serverName=="����һ��"||serverName=="����һ��"||serverName=="����һ��")
//				{
					serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);

					sql = "select sql_statement from sqlexpress where sql_type='JW2_ACCOUNT_QUERYBYACCOUNT_bak' and sql_condition='JW2_ACCOUNT_QUERYBYACCOUNT_bak'";
					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{
						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,strname);
						result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
					}
//				}
//				else
//				{
//					sql = "select sql_statement from sqlexpress where sql_type='JW2_ACCOUNT_QUERYBYACCOUNT_ORACLE' and sql_condition='JW2_ACCOUNT_QUERYBYACCOUNT_ORACLE'";
//					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
//					if(ds!=null && ds.Tables[0].Rows.Count>0)
//					{
//						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
//						sql = string.Format(sql,strname,zone);
//						result = CommonInfo.RunOracle(sql,SqlHelper.oracleData,SqlHelper.oracleUser,SqlHelper.oraclePwd);
//					}
//				}
			}
			catch(MySqlException ex)
			{
				strDesc = "���ݿ�����ʧ��";
				SqlHelper.errLog.WriteLog("���JW2_ACCOUNT_QUERY_���"+strname+"��Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		/// <summary>
		/// �鿴�������
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ǳ�</param>
		/// <returns></returns>
		public static DataSet USERNICK_QUERY(string serverIP,string strname)
		{
			DataSet result = null;
			string sql = "";
			int zone = 0;
			string serverName = "";
			try
			{
				serverName = CommonInfo.JW2_FindDBName(serverIP);
				zone = CommonInfo.JW2_GetZone_Query(13,serverName);
				if(serverName=="����һ��"||serverName=="����һ��"||serverName=="����һ��"||serverName=="����һ��")
				{
					serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
					sql = "select sql_statement from sqlexpress where sql_type='JW2_ACCOUNT_QUERYBYNICKNAME_Bak' and sql_condition='JW2_ACCOUNT_QUERYBYNICKNAME_Bak'";
					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{
						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,strname);
						result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
					}
				}
				else
				{
					sql = "select sql_statement from sqlexpress where sql_type='JW2_ACCOUNT_QUERYBYNICKNAME_ORACLE' and sql_condition='JW2_ACCOUNT_QUERYBYNICKNAME_ORACLE'";
					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{
						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,strname,zone);
						result = CommonInfo.RunOracle(sql,SqlHelper.oracleData,SqlHelper.oracleUser,SqlHelper.oraclePwd);
					}
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_USERNICK_QUERY_����ǳ�"+strname+"��Ϣ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ��ѯ�����������
		/// <summary>
		/// ��ѯ�����������
		/// </summary>
		public static DataSet DUMMONEY_QUERY(string serverIP,int usersn)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_DUMMONEY_QUERY' and sql_condition='JW2_DUMMONEY_QUERY'";
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
				SqlHelper.errLog.WriteLog("���JW2_DUMMONEY_QUERY_���"+usersn.ToString()+"����������ҷ�����IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �޸ĵȼ�
		/// <summary>
		/// �޸ĵȼ�
		/// </summary>
		public static int MODIFYLEVEL_QUERY(string serverIP,int usersn,int iLevel,int userByID,string UserName,ref string strDesc)
		{
			int  result = -1;
			string sql = null;
			try
			{
				//�޸ĵȼ�1
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,7);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_MODIFYLEVEL_QUERY1' and sql_condition = 'JW2_MODIFYLEVEL_QUERY1'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,iLevel,usersn);
					result = MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
				//�޸ĵȼ�2
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,9);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_MODIFYLEVEL_QUERY2' and sql_condition = 'JW2_MODIFYLEVEL_QUERY2'";
				System.Data.DataSet ds1 = SqlHelper.ExecuteDataset(sql);
				if(ds1!=null && ds1.Tables[0].Rows.Count>0)
				{	
					sql = ds1.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,iLevel,usersn);
					result += MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
				//��ѯ�ȼ���Ӧ�ľ���ֵ
				float iExp = CommonInfo.JW2_LevelToExp(iLevel);
				//�޸ľ���
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,7);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_MODIFYEXP_QUERY' and sql_condition = 'JW2_MODIFYEXP_QUERY'";
				System.Data.DataSet ds2 = SqlHelper.ExecuteDataset(sql);
				if(ds2!=null && ds2.Tables[0].Rows.Count>0)
				{	
					sql = ds2.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,iExp,usersn);
					result += MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
				if(result==3)
				{
					strDesc ="�޸���ң�"+UserName.ToString()+"���ȼ���"+iLevel.ToString()+"�����飺"+iExp.ToString()+"���ɹ������Եȣ�ϵͳ�����У�";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_MODIFYLEVEL_QUERY","�޸���ң�"+UserName.ToString()+"���ȼ���"+iLevel.ToString()+"�����飺"+iExp.ToString()+"���ɹ�(�޸ĵȼ�,jw2)");
				}
				else
				{
					strDesc ="�޸���ң�"+UserName.ToString()+"���ȼ���"+iLevel.ToString()+"�����飺"+iExp.ToString()+"��ʧ�ܣ���ȷ�ϸ��û��Ƿ����";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_MODIFYLEVEL_QUERY","�޸���ң�"+UserName.ToString()+"���ȼ���"+iLevel.ToString()+"�����飺"+iExp.ToString()+"��ʧ��(�޸ĵȼ�,jw2)");
				}
			}
			catch (System.Exception ex)
			{
				strDesc = "���ݿ�����ʧ�ܣ������³��ԣ�";
				SqlHelper.errLog.WriteLog("���JW2_MODIFYLEVEL_QUERY_���"+usersn.ToString()+"-"+UserName+"�޸ĵȼ�������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �޸ľ���
		/// <summary>
		/// �޸ľ���
		/// </summary>
		public static int MODIFYEXP_QUERY(string serverIP,int usersn,float iExp,int userByID,string UserName,ref string strDesc)
		{
			int  result = -1;
			string sql = null;
			try
			{
				//��ѯ�ȼ���Ӧ�ľ���ֵ
				long iLevel = CommonInfo.JW2_ExpToLevel(iExp);
				//�޸ĵȼ�1
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,7);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_MODIFYLEVEL_QUERY1' and sql_condition = 'JW2_MODIFYLEVEL_QUERY1'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,iLevel,usersn);
					result = MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
				//�޸ĵȼ�2
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,9);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_MODIFYLEVEL_QUERY2' and sql_condition = 'JW2_MODIFYLEVEL_QUERY2'";
				System.Data.DataSet ds1 = SqlHelper.ExecuteDataset(sql);
				if(ds1!=null && ds1.Tables[0].Rows.Count>0)
				{	
					sql = ds1.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,iLevel,usersn);
					result += MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2messengerDB),sql);
				}
				
				//�޸ľ���
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,7);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_MODIFYEXP_QUERY' and sql_condition = 'JW2_MODIFYEXP_QUERY'";
				System.Data.DataSet ds2 = SqlHelper.ExecuteDataset(sql);
				if(ds2!=null && ds2.Tables[0].Rows.Count>0)
				{	
					sql = ds2.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,iExp,usersn);
					result += MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
				if(result==3)
				{
					strDesc ="�޸���ң�"+UserName.ToString()+"���ȼ���"+iLevel.ToString()+"�����飺"+iExp.ToString()+"���ɹ������Եȣ�ϵͳ�����У�";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_MODIFYEXP_QUERY","�޸���ң�"+UserName.ToString()+"���ȼ���"+iLevel.ToString()+"�����飺"+iExp.ToString()+"���ɹ�(�޸ľ���,jw2)");
				}
				else
				{
					strDesc ="�޸���ң�"+UserName.ToString()+"���ȼ���"+iLevel.ToString()+"�����飺"+iExp.ToString()+"��ʧ�ܣ���ȷ�ϸý�ɫ�Ƿ���ڣ�";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_MODIFYEXP_QUERY","�޸���ң�"+UserName.ToString()+"���ȼ���"+iLevel.ToString()+"�����飺"+iExp.ToString()+"��ʧ��(�޸ľ���,jw2)");
				}
			}
			catch (System.Exception  ex)
			{
				strDesc = "���ݿ�����ʧ�ܣ������³��ԣ�";
				SqlHelper.errLog.WriteLog("���JW2_MODIFYEXP_QUERY_���"+usersn.ToString()+"-"+UserName+"�޸ľ��������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �޸Ľ�Ǯ
		/// <summary>
		/// �޸Ľ�Ǯ
		/// </summary>
		public static int MODIFY_MONEY(string serverIP,int usersn,int iMoney,int userByID,string UserName,ref string strDesc)
		{
			int  result = -1;
			string sql = null;
			try
			{
				//�޸Ľ�Ǯ
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,7);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_MODIFY_MONEY' and sql_condition = 'JW2_MODIFY_MONEY'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,iMoney,usersn);
					result = MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
				if(result==1)
				{
					strDesc ="�޸���ң�"+UserName.ToString()+"����Ǯ��"+iMoney.ToString()+"���ɹ������Եȣ�ϵͳ�����У�";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_MODIFY_MONEY","�޸���ң�"+UserName.ToString()+"����Ǯ��"+iMoney.ToString()+"���ɹ�(�޸Ľ�Ǯ,jw2)");
				}
				else
				{
					strDesc ="�޸���ң�"+UserName.ToString()+"����Ǯ��"+iMoney.ToString()+"��ʧ�ܣ���ȷ�ϸý�ɫ�Ƿ���ڣ�";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_MODIFY_MONEY","�޸���ң�"+UserName.ToString()+"����Ǯ��"+iMoney.ToString()+"��ʧ��(�޸Ľ�Ǯ,jw2)");
				}
			}
			catch (System.Exception  ex)
			{
				strDesc = "���ݿ�����ʧ�ܣ������³��ԣ�";
				SqlHelper.errLog.WriteLog("���JW2_MODIFY_MONEY_���"+usersn.ToString()+"-"+UserName+"�޸Ľ�Ǯ������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴��ҽ��֤��
		/// <summary>
		/// �鿴��ҽ��֤��
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="userSN">�û�ID</param>
		/// <returns></returns>
		public static DataSet Wedding_Paper(string serverIP,int userSN)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_Wedding_Paper' and sql_condition='JW2_Wedding_Paper'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userSN);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_Wedding_Paper_�鿴���"+userSN.ToString()+"���֤�������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion
		
		#region �鿴��������ɶԿ�
		/// <summary>
		/// �鿴��������ɶԿ�
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="userSN">�û�ID</param>
		/// <returns></returns>
		public static DataSet CoupleParty_Card(string serverIP,int userSN)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_CoupleParty_Card' and sql_condition='JW2_CoupleParty_Card'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userSN);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_CoupleParty_Card_�鿴���"+userSN.ToString()+"�����ɶԿ�������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region GM��B�޸�
		/// <summary>
		/// GM��B�޸�
		/// </summary>
		public static int GM_Update(string serverIP,int usersn,int type,int userByID,string userName,ref string strDesc)
		{
			int  result = -1;
			string sql = null;
			string typeName = "";
			try
			{
				switch(type)
				{
					case 0:
						typeName="����Ա";
						break;
					case 1:
						typeName="��ͨԱ";
						break;
					case 2:
						typeName="�۲�Ա";
						break;

				}
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_GM_Update' and sql_condition = 'JW2_GM_Update'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,usersn,type);
					result = MySqlHelper.ExecuteNonQuery(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
				if(result==1)
				{
					strDesc ="�޸�GM��ң�"+userName.ToString()+"����B��"+typeName.ToString()+"���ɹ������Եȣ�ϵͳ�����У�";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_GM_Update","�޸�GM��ң�"+userName.ToString()+"����B��"+typeName.ToString()+"���ɹ�(GM״̬�޸�,jw2)");
				}
				else
				{
					strDesc ="�޸�GM��ң�"+userName.ToString()+"����B��"+typeName.ToString()+"��ʧ�ܣ�ȷ�ϸ��û��Ƿ���ڣ�";
					SqlHelper.insertGMtoolsLog(userByID,"������II",serverIP,"JW2_GM_Update","�޸�GM��ң�"+userName.ToString()+"����B��"+typeName.ToString()+"��ʧ��(GM״̬�޸�,jw2)");
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

		#region �����ѯ
		/// <summary>
		/// �����ѯ
		/// </summary>
		/// <param name="serverIP"></param>
		/// <param name="account"></param>
		/// <returns></returns>
		public static ArrayList Act_Card_Query(string userName,string card,ref string strDesc)
		{
			string getUser = null;
			string sign = null;
			string parameter ="";
			XmlDocument xmlfile = new XmlDocument();
			getUser =userName;
			parameter = getUser+card;
			MD5Encrypt md5 = new MD5Encrypt();
			sign = md5.getMD5ofStr(parameter+"|T4pa3A.jw2").ToLower();
			try   
			{
				System.Data.DataSet ds = SqlHelper.ExecuteDataset("select ServerIP from gmtools_serverInfo where gameid=10");
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					string serverIP = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					string url = null;
					url= "http://"+serverIP+"/PayCenter/jw2actcard.php";
					HttpWebRequest  request  = (HttpWebRequest)
						WebRequest.Create(url);
					request.ContentType="application/x-www-form-urlencoded";
					request.KeepAlive=false; 
					request.Method="POST";
					//����POST���̳ǵĽӿ�
					Stream writer = request.GetRequestStream(); 
					string postData="getuser="+getUser+"&card="+card+"&sign="+sign+"&encoding=UTF-8";  
					ASCIIEncoding encoder = new ASCIIEncoding();
					byte[] ByteArray = encoder.GetBytes(postData);
					writer.Write(ByteArray,0,postData.Length);
					writer.Close();
					//�õ��̳ǽӿڵĻ�Ӧ
					WebResponse
						resp = request.GetResponse();
					StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
					//Console.WriteLine(sr.ReadToEnd().Trim());
					xmlfile.Load(sr);
					XmlNode descNodes = xmlfile.SelectSingleNode("you9/status");
					strDesc = descNodes.InnerText;
					int i=0;
					if(strDesc!=null && strDesc.Equals("RESULT_0"))
					{
						strDesc = "��ѯ�ɹ�";                        
					}
					else if(strDesc!=null && strDesc.Equals("RESULT_1"))
					{
						strDesc = "��������";
					}
					else if(strDesc!=null && strDesc.Equals("RESULT_2"))
					{
						strDesc = "��ѯ��Կ����";
					}
					else if(strDesc!=null && strDesc.Equals("RESULT_3"))
					{
						strDesc = "���������ѯ�޽��";
					}
					else
					{
						strDesc = "�쳣";
					}
					XmlNode nodes ;
					System.Collections.ArrayList rowList = new System.Collections.ArrayList();
					while( (nodes=xmlfile.SelectSingleNode("you9/row"+i))!=null)
					{
						System.Collections.ArrayList colList = new System.Collections.ArrayList();
						foreach(XmlNode xmlnodes in nodes.ChildNodes)
						{
							colList.Add(xmlnodes.InnerText);
						}
						i++;
						rowList.Add(colList);
					}
					sr.Close();
					return rowList;
				}
			}
			catch (SqlException ex)
			{
				strDesc = "�쳣";
				SqlHelper.errLog.WriteLog("���JW2_Act_Card_Query_�鿴���"+userName+"�����ѯ"+ex.Message);
			}
			return null;
		}
		#endregion

		#region ��÷�����GS�б�
		/// <summary>
		/// ��÷�����GS�б�
		/// </summary>
		public static DataSet GSSvererList_Query(string serverIP)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_GSServerList_Query' and sql_condition='JW2_GSServerList_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
			}
			catch(System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return result;
		}
		#endregion

		#region �鿴��һ�Ծ��
		/// <summary>
		/// �鿴��һ�Ծ��
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet ACTIVEPOINT_ACCOUNT_QUERY(string serverIP,int intusersn,string BeginTime,string EndTime)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_ACTIVEPOINT_QUERYBYACCOUNT' and sql_condition='JW2_ACTIVEPOINT_QUERYBYACCOUNT'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,intusersn,BeginTime,EndTime);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_ACTIVEPOINT_ACCOUNT_QUERY_�鿴���"+intusersn.ToString()+"��Ծ�ȷ�����IP"+serverIP+ex.Message);
			}
			return result;
		}
		/// <summary>
		/// �鿴��һ�Ծ��
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ǳ�</param>
		/// <returns></returns>
		public static DataSet ACTIVEPOINT_USERNICK_QUERY(string serverIP,string strname,string BeginTime,string EndTime)
		{
			DataSet result = null;
			string sql = "";
			try
			{
				serverIP = CommonInfo.JW2_FindDBIP(serverIP,1);
				sql = "select sql_statement from sqlexpress where sql_type='JW2_ACTIVEPOINT_QUERYBYNICKNAME' and sql_condition='JW2_ACTIVEPOINT_QUERYBYNICKNAME'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,strname,BeginTime,EndTime);
					result = MySqlHelper.ExecuteDataset(SqlHelper.JW2GetConnectionString(serverIP,SqlHelper.jw2User,SqlHelper.jw2UserPwd,SqlHelper.jw2gameDB),sql);
				}
			}
			catch(MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("���JW2_ACTIVEPOINT_USERNICK_QUERY_�鿴���"+strname+"��Ծ�ȷ�����IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion
		

	}
}
