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
using Common.DataInfo;
using Common.API;
using MySql.Data.MySqlClient;
namespace GM_Server.SDOnlineDataInfo
{
	/// <summary>
	/// SDLogDataInfo ��ժҪ˵����
	/// </summary>
	public class SDLogDataInfo
	{
		public SDLogDataInfo()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		#region ��ҵ�½��Ϣ
		/// <summary>
		/// ��ҵ�½��Ϣ
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet UserLoginfo_Query(string serverIP,int userid,DateTime startTime,DateTime endTime)
		{
			DataSet result = null;
			string sql = null;
			string eTime = null;
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				int i=0;
				if(DateTime.Compare(endTime,DateTime.Now.AddDays(1))>0)
					eTime=DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
				else
					eTime=endTime.AddDays(1).ToString("yyyy-MM-dd");
				while(!startTime.AddDays(i).ToString("yyyy-MM-dd").Equals(eTime))
				{
					if(i<45)
					{
						int iTime = Convert.ToInt32(startTime.AddDays(i).ToString("yyyyMMdd"));
						sql = "select sql_statement from sqlexpress where sql_type='SD_UserLoginfo_Query' and sql_condition = 'SD_UserLoginfo_Query'";
						System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
						if(ds!=null && ds.Tables[0].Rows.Count>0)
						{
							sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
							sql = string.Format(sql,userid,startTime,endTime,iTime);
							if(i==0)
							{
								result= SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
							}
							else
							{
								result.Merge(SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql));
							}
						}
					}
					else
					{
						break;
					}
					i++;
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �������\�ʼ���ѯ
		/// <summary>
		/// �������\�ʼ���ѯ
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet UserGrift_Query(string serverIP,int userid,DateTime BeginTime,DateTime EndTime,int type)
		{
			DataSet result = null;
			string sql = null;
			try
			{
				if(type==1)
					sql = "select sql_statement from sqlexpress where sql_type='SD_FromUserGrift_Query' and sql_condition = 'SD_FromUserGrift_Query'";
				else if(type==2)
					sql = "select sql_statement from sqlexpress where sql_type='SD_ToUserGrift_Query' and sql_condition = 'SD_ToUserGrift_Query'";
				else if(type==3)
					sql = "select sql_statement from sqlexpress where sql_type='SD_FromUserMail_Query' and sql_condition = 'SD_FromUserMail_Query'";
				else 
					sql = "select sql_statement from sqlexpress where sql_type='SD_ToUserMail_Query' and sql_condition = 'SD_ToUserMail_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userid,BeginTime,EndTime);
					string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
					result = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ������������Ϣ��ѯ
		/// <summary>
		/// ������������Ϣ��ѯ
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet Grift_FromUser_Query(string serverIP,int toidx,int fromidx,DateTime Time)
		{
			DataSet result = null;
			string sql = null;
			try
			{
				int iTime = Convert.ToInt32(Time.ToString("yyyyMMdd"));
				string sTime = Time.ToString("H:mm");
				sql = "select sql_statement from sqlexpress where sql_type='SD_Grift_FromUser_Query' and sql_condition = 'SD_Grift_FromUser_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,iTime,fromidx,toidx,sTime);
					string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
					result = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ������������Ϣ��ѯ
		/// <summary>
		/// ������������Ϣ��ѯ
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet Grift_ToUser_Query(string serverIP,int toidx,string item,DateTime Time,int s_id)
		{
			DataSet result = null;
			string sql = null;
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				int i=0;
				while(!Time.AddDays(i).ToString("yyyy-MM-dd").Equals(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")))
				{
					if(i<45)
					{
						string[] iItem = item.Split('|');
						for(int j = 0;j<iItem.Length-1;j++)
						{
							int itemID = Convert.ToInt32(iItem[j].ToString());
							if(itemID!=0)
							{
								int iTime = Convert.ToInt32(Time.AddDays(i).ToString("yyyyMMdd"));
								sql = "select sql_statement from sqlexpress where sql_type='SD_Grift_ToUser_Query' and sql_condition = 'SD_Grift_ToUser_Query'";
								System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
								if(ds!=null && ds.Tables[0].Rows.Count>0)
								{
									sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
									sql = string.Format(sql,iTime,toidx,itemID,s_id,Time);
									if(i==0)
									{
										result= SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
									}
									else
									{
										result.Merge(SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql));
									}
								}
							}
						}
					}
					else
					{
						break;
					}
					i++;
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion
		
		#region ��ͣ�û�
		/// <summary>
		/// ��ͣ�û�
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static int BanUser_Ban(string serverIP,string serverName,int UserByID,int userid,string username,string content,DateTime banTime)
		{
			int result = -1;
			string sql = null;
			try
			{
				sql = "select sql_statement from sqlexpress where sql_type='SD_BanUser_Ban' and sql_condition = 'SD_BanUser_Ban'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,banTime,userid);
					string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
					result = SqlHelper.ExecCommand(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql,true);
				}
				if(result==1)
				{
					sql = "select sql_statement from sqlexpress where sql_type='SD_InsertBan_Query' and sql_condition = 'SD_InsertBan_Query'";
					System.Data.DataSet ds1 = SqlHelper.ExecuteDataset(sql);
					if(ds1!=null && ds1.Tables[0].Rows.Count>0)
					{	
						sql = ds1.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,userid,username,content,banTime,serverName);
						result = SqlHelper.ExecCommand(sql);
					}
				}
				if(result==1)
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_BanUser_Ban","��ͣ�û�"+username.ToString()+"����"+banTime.ToString()+"���ɹ�");
				}
				else
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_BanUser_Ban","��ͣ�û�"+username.ToString()+"����"+banTime.ToString()+"��ʧ��");
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ����û�
		/// <summary>
		/// ����û�
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static int BanUser_UnBan(string serverIP,string serverName,int UserByID,int userid,string username)
		{
			int result = -1;
			string sql = null;
			try
			{
				sql = "select sql_statement from sqlexpress where sql_type='SD_BanUser_UnBan' and sql_condition = 'SD_BanUser_UnBan'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userid);
					string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
					result = SqlHelper.ExecCommand(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql,true);
				}
				if(result==1)
				{
					sql = "select sql_statement from sqlexpress where sql_type='SD_UpdateBanUser_Query' and sql_condition = 'SD_UpdateBanUser_Query'";
					System.Data.DataSet ds1 = SqlHelper.ExecuteDataset(sql);
					if(ds1!=null && ds1.Tables[0].Rows.Count>0)
					{	
						sql = ds1.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,userid,serverName);
						SqlHelper.ExecCommand(sql);
					}
					

					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_BanUser_UnBan","����û�"+username.ToString()+"���ɹ�");
				}
				else
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_BanUser_UnBan","����û�"+username.ToString()+"��ʧ��");
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ��ͣ�û���ѯ
		/// <summary>
		/// ��ͣ�û���ѯ
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet BanUser_Query(string serverIP,string serverName,int type,string userid)
		{
			DataSet result = null;
			string sql = null;
			try
			{
				sql = "select sql_statement from sqlexpress where sql_type='SD_BanUser_Query' and sql_condition = '"+type+"'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					if(type==0)
						sql = string.Format(sql,serverName);
					else
						sql = string.Format(sql,serverName,userid);
					result = SqlHelper.ExecuteDataset(sql);
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
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
		public static int TmpPassWord_Query(string serverIP,string serverName,int UserByID,int userid,string username,string TmpPwd)
		{
			DataSet ds = null;
			int result = -1;
			string RelPwd = null;
			string sql = null;
			MD5Encrypt st = new MD5Encrypt();
			string sign = st.getMD5ofStr(TmpPwd).ToLower();
			try
			{

				//��ѯ�Ƿ��޸Ĺ�
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				sql = "select sql_statement from sqlexpress where sql_type='SD_SearchTmpPWD_Staute_Query' and sql_condition = 'SD_SearchTmpPWD_Staute_Query'";
				System.Data.DataSet ds4 = SqlHelper.ExecuteDataset(sql);
				if(ds4!=null && ds4.Tables[0].Rows.Count>0)
				{	
					sql = ds4.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,serverName,userid);
					System.Data.DataSet ds5 = SqlHelper.ExecuteDataset(sql);
					if(ds5.Tables[0].Rows.Count==0)
					{
						//��ȡ��ʵ����
						sql = "select sql_statement from sqlexpress where sql_type='SD_GetPassWord_Query' and sql_condition = 'SD_GetPassWord_Query'";
						System.Data.DataSet ds1 = SqlHelper.ExecuteDataset(sql);
						if(ds1!=null && ds1.Tables[0].Rows.Count>0)
						{	
							sql = ds1.Tables[0].Rows[0].ItemArray[0].ToString();
							sql = string.Format(sql,userid);
							ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
							if(ds!=null && ds.Tables[0].Rows.Count>0)
							{
								RelPwd = ds.Tables[0].Rows[0].ItemArray[0].ToString();
								//�޸�����
								sql = "select sql_statement from sqlexpress where sql_type='SD_TmpPassWord_Query' and sql_condition = 'SD_TmpPassWord_Query'";
								System.Data.DataSet ds2 = SqlHelper.ExecuteDataset(sql);
								if(ds2!=null && ds2.Tables[0].Rows.Count>0)
								{	
									sql = ds2.Tables[0].Rows[0].ItemArray[0].ToString();
									sql = string.Format(sql,userid,sign);
									result = SqlHelper.ExecCommand(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql,true);
								}
								if(result==1)
								{
									//��¼��ʵ����ʱ������125��
									sql = "select sql_statement from sqlexpress where sql_type='SD_InsertTmpPassWord_Query' and sql_condition = 'SD_InsertTmpPassWord_Query'";
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
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"TmpPassWord_Query","�޸��û�"+username.ToString()+"��ʱ���룬�ɹ�");
				}
				else
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"TmpPassWord_Query","�޸��û�"+username.ToString()+"��ʱ���룬ʧ��");
				}
			}
			catch (MySqlException ex)
			{
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
		public static int ReTmpPassWord_Query(string serverIP,string serverName,int UserByID,int userid,string username)
		{
			DataSet ds = null;
			int result = -1;
			string RelPwd = null;
			string sql = null;
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				//��ȡ������ʵ����
				sql = "select sql_statement from sqlexpress where sql_type='SD_GetRelPassWord_Query' and sql_condition = 'SD_GetRelPassWord_Query'";
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
						sql = "select sql_statement from sqlexpress where sql_type='SD_TmpPassWord_Query' and sql_condition = 'SD_TmpPassWord_Query'";
						System.Data.DataSet ds2 = SqlHelper.ExecuteDataset(sql);
						if(ds2!=null && ds2.Tables[0].Rows.Count>0)
						{	
							sql = ds2.Tables[0].Rows[0].ItemArray[0].ToString();
							sql = string.Format(sql,userid,RelPwd);
							result = SqlHelper.ExecCommand(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql,true);
						}
						if(result==1)
						{
							//���±�����ʱ����״̬
							sql = "select sql_statement from sqlexpress where sql_type='SD_UpdateTmpPassWord_Query' and sql_condition = 'SD_UpdateTmpPassWord_Query'";
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
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"TmpPassWord_Query","�ָ��û�"+username.ToString()+"��ʱ���룬�ɹ�");
				}
				else
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"TmpPassWord_Query","�ָ��û�"+username.ToString()+"��ʱ���룬ʧ��");
				}
			}
			catch (MySqlException ex)
			{
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
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				sql = "select sql_statement from sqlexpress where sql_type='SD_SearchPassWord_Query' and sql_condition = 'SD_SearchPassWord_Query'";
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
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �޸���ҵȼ�
		/// <summary>
		/// �޸���ҵȼ�
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static int UpdateExp_Query(string serverIP,int UserByID,int userid,string username,int level)
		{
			int result = -1;
			string sql = null;
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				sql = "select sql_statement from sqlexpress where sql_type='SD_UpdateExp_Query' and sql_condition = 'SD_UpdateExp_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userid,level);
					result = SqlHelper.ExecCommand(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql,true);
				}
				if(result==1)
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UpdateExp_Query","�޸����"+username.ToString()+"���ȼ�"+level.ToString()+"���ɹ�");
				}
				else
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UpdateExp_Query","�޸��û�"+username.ToString()+"���ȼ�"+level.ToString()+"��ʧ��");
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �޸���һ���ȼ�
		/// <summary>
		/// �޸���һ���ȼ�
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static string UpdateUnitsExp_Query(string serverIP,int UserByID,int userid,string username,int level,int CustomLvMax,int UnitNumber)
		{
			DataSet result = null;
			string str = null;
			string sql = null;
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				sql = "select sql_statement from sqlexpress where sql_type='SD_UpdateUnit_Query' and sql_condition = 'SD_UpdateUnit_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userid,UnitNumber,level,CustomLvMax);
					sql = sql.Replace("\t","");
					result = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
					str = result.Tables[0].Rows[0].ItemArray[0].ToString();
					
				}
				SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UpdateUnitsExp_Query",str);
				
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return str;
		}
		#endregion

		#region ��ӵ���
		/// <summary>
		/// ��ӵ���
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static int UserAdditem_Add(string serverIP,int UserByID,int userid,string username,string item,string sendUser,string content)
		{
			string[] itemList = item.Split('|');
			int itemID = 0;
			int itemNum = 0;
			int strItemID = 0;
			string itemName = null;
			int nCategoryNumber1 = 0;
			int	nCategoryNumber2 = 0;
			int nCategoryNumber3 = 0;
			int nCategoryNumber4 = 0;
			int nValidity1 = 0;
			int nValidity2 = 0;
			int nValidity3 = 0;
			int nValidity4 = 0;
			int nItemtblidx1 = 0;
			int nItemtblidx2 = 0;
			int nItemtblidx3 = 0;
			int nItemtblidx4 = 0;
			int result = -1;
			string sql = null;
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				for(int i = 0;i<itemList.Length-1;i++)
				{
					result = -1;
					itemID = int.Parse(itemList[i].Split(' ')[0].ToString());
					itemNum = int.Parse(itemList[i].Split(' ')[1].ToString());
					itemName = itemList[i].Split(' ')[2].ToString();

					sql = "select sql_statement from sqlexpress where sql_type='SD_GetItemBillingCode' and sql_condition = 'SD_GetItemBillingCode'";
					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{	
						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,itemID);
						DataSet ds1= SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
						switch(ds1.Tables[0].Rows.Count)
						{
							case 1:
								nItemtblidx1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[0].ToString());
								strItemID = int.Parse(ds1.Tables[0].Rows[0].ItemArray[1].ToString());
								nValidity1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[3].ToString());
								nCategoryNumber1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[5].ToString());
								break;
							case 2:
								nItemtblidx1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[0].ToString());
								nItemtblidx2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[0].ToString());
								strItemID = int.Parse(ds1.Tables[0].Rows[0].ItemArray[1].ToString());
								nValidity1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[3].ToString());
								nValidity2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[3].ToString());
								nCategoryNumber1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[5].ToString());
								nCategoryNumber2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[5].ToString());
								break;
							case 3:
								nItemtblidx1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[0].ToString());
								nItemtblidx2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[0].ToString());
								nItemtblidx3 = int.Parse(ds1.Tables[0].Rows[2].ItemArray[0].ToString());
								strItemID = int.Parse(ds1.Tables[0].Rows[0].ItemArray[1].ToString());
								nValidity1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[3].ToString());
								nValidity2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[3].ToString());
								nValidity3 = int.Parse(ds1.Tables[0].Rows[2].ItemArray[3].ToString());
								nCategoryNumber1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[5].ToString());
								nCategoryNumber2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[5].ToString());
								nCategoryNumber3 = int.Parse(ds1.Tables[0].Rows[2].ItemArray[5].ToString());
								break;
							case 4:
								nItemtblidx1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[0].ToString());
								nItemtblidx2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[0].ToString());
								nItemtblidx3 = int.Parse(ds1.Tables[0].Rows[2].ItemArray[0].ToString());
								nItemtblidx4 = int.Parse(ds1.Tables[0].Rows[3].ItemArray[0].ToString());
								strItemID = int.Parse(ds1.Tables[0].Rows[0].ItemArray[1].ToString());
								nValidity1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[3].ToString());
								nValidity2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[3].ToString());
								nValidity3 = int.Parse(ds1.Tables[0].Rows[2].ItemArray[3].ToString());
								nValidity4 = int.Parse(ds1.Tables[0].Rows[3].ItemArray[3].ToString());
								nCategoryNumber1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[5].ToString());
								nCategoryNumber2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[5].ToString());
								nCategoryNumber3 = int.Parse(ds1.Tables[0].Rows[2].ItemArray[5].ToString());
								nCategoryNumber4 = int.Parse(ds1.Tables[0].Rows[3].ItemArray[5].ToString());
								break;
						}
						nValidity1 = nValidity1*itemNum;
						nValidity2 = nValidity2*itemNum;
						nValidity3 = nValidity3*itemNum;
						nValidity4 = nValidity4*itemNum;

						sql = "select sql_statement from sqlexpress where sql_type='SD_UserAdditem_Add' and sql_condition = 'SD_UserAdditem_Add'";
						ds = SqlHelper.ExecuteDataset(sql);
						if(ds!=null && ds.Tables[0].Rows.Count>0)
						{	
							sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
							sql = string.Format(sql,userid,
								nCategoryNumber1,nCategoryNumber2,nCategoryNumber3,nCategoryNumber4,
								nValidity1,nValidity2,nValidity3,nValidity4,
								nItemtblidx1,nItemtblidx2,nItemtblidx3,nItemtblidx4,
								strItemID,sendUser,userid,username,content,itemID);
							ds1= SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
							result = int.Parse(ds1.Tables[0].Rows[0].ItemArray[0].ToString());
						}
					}
					if(result!=-1)
					{
						SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UserAdditem_Add","��ӵ���"+itemName.ToString()+"������ID"+itemID.ToString()+"����������"+itemNum.ToString()+"�������"+username.ToString()+"���ɹ�");
					}
					else
					{
						SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UserAdditem_Add","��ӵ���"+itemName.ToString()+"������ID"+itemID.ToString()+"����������"+itemNum.ToString()+"�������"+username.ToString()+"��ʧ��");
					}
				} 
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ��ӵ��ߣ�������
		/// <summary>
		/// ��ӵ���
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static string  UserAdditem_Add_All(string serverIP,int UserByID,string item)
		{
			string[] itemList = item.Split(',');
			int userID = 0;
			string ItemName = null;
			int itemNum = 0;
			string userName = null;
			string itemName = null;
			string get_Result = null;
			int result = -1;
			string sql = null;
			int itemID = 0;
			int strItemID = 0;
			int nCategoryNumber1 = 0;
			int	nCategoryNumber2 = 0;
			int nCategoryNumber3 = 0;
			int nCategoryNumber4 = 0;
			int nValidity1 = 0;
			int nValidity2 = 0;
			int nValidity3 = 0;
			int nValidity4 = 0;
			int nItemtblidx1 = 0;
			int nItemtblidx2 = 0;
			int nItemtblidx3 = 0;
			int nItemtblidx4 = 0;
			
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				for(int i = 0;i<itemList.Length-1;i++)
				{
					result = -1;
					userName = itemList[i].Split('|')[0].ToString();
					userID = CommonInfo.SD_GetUserId_Query(serverIP,userName);
					itemID = int.Parse(itemList[i].Split('|')[1].ToString());
					itemNum = int.Parse(itemList[i].Split('|')[2].ToString());
					itemName = CommonInfo.SD_GetShopItemName_Query(serverIP,itemID);

					sql = "select sql_statement from sqlexpress where sql_type='SD_GetItemBillingCode' and sql_condition = 'SD_GetItemBillingCode'";
					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{	
						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,itemID);
						DataSet ds1= SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
						switch(ds1.Tables[0].Rows.Count)
						{
							case 1:
								nItemtblidx1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[0].ToString());
								strItemID = int.Parse(ds1.Tables[0].Rows[0].ItemArray[1].ToString());
								nValidity1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[3].ToString());
								nCategoryNumber1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[5].ToString());
								break;
							case 2:
								nItemtblidx1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[0].ToString());
								nItemtblidx2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[0].ToString());
								strItemID = int.Parse(ds1.Tables[0].Rows[0].ItemArray[1].ToString());
								nValidity1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[3].ToString());
								nValidity2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[3].ToString());
								nCategoryNumber1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[5].ToString());
								nCategoryNumber2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[5].ToString());
								break;
							case 3:
								nItemtblidx1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[0].ToString());
								nItemtblidx2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[0].ToString());
								nItemtblidx3 = int.Parse(ds1.Tables[0].Rows[2].ItemArray[0].ToString());
								strItemID = int.Parse(ds1.Tables[0].Rows[0].ItemArray[1].ToString());
								nValidity1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[3].ToString());
								nValidity2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[3].ToString());
								nValidity3 = int.Parse(ds1.Tables[0].Rows[2].ItemArray[3].ToString());
								nCategoryNumber1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[5].ToString());
								nCategoryNumber2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[5].ToString());
								nCategoryNumber3 = int.Parse(ds1.Tables[0].Rows[2].ItemArray[5].ToString());
								break;
							case 4:
								nItemtblidx1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[0].ToString());
								nItemtblidx2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[0].ToString());
								nItemtblidx3 = int.Parse(ds1.Tables[0].Rows[2].ItemArray[0].ToString());
								nItemtblidx4 = int.Parse(ds1.Tables[0].Rows[3].ItemArray[0].ToString());
								strItemID = int.Parse(ds1.Tables[0].Rows[0].ItemArray[1].ToString());
								nValidity1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[3].ToString());
								nValidity2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[3].ToString());
								nValidity3 = int.Parse(ds1.Tables[0].Rows[2].ItemArray[3].ToString());
								nValidity4 = int.Parse(ds1.Tables[0].Rows[3].ItemArray[3].ToString());
								nCategoryNumber1 = int.Parse(ds1.Tables[0].Rows[0].ItemArray[5].ToString());
								nCategoryNumber2 = int.Parse(ds1.Tables[0].Rows[1].ItemArray[5].ToString());
								nCategoryNumber3 = int.Parse(ds1.Tables[0].Rows[2].ItemArray[5].ToString());
								nCategoryNumber4 = int.Parse(ds1.Tables[0].Rows[3].ItemArray[5].ToString());
								break;
						}
						nValidity1 = nValidity1*itemNum;
						nValidity2 = nValidity2*itemNum;
						nValidity3 = nValidity3*itemNum;
						nValidity4 = nValidity4*itemNum;
						sql = "select sql_statement from sqlexpress where sql_type='SD_UserAdditem_Add' and sql_condition = 'SD_UserAdditem_Add'";
						ds = SqlHelper.ExecuteDataset(sql);
						if(ds!=null && ds.Tables[0].Rows.Count>0)
						{
							sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
							sql = string.Format(sql,userID,
								nCategoryNumber1,nCategoryNumber2,nCategoryNumber3,nCategoryNumber4,
								nValidity1,nValidity2,nValidity3,nValidity4,
								nItemtblidx1,nItemtblidx2,nItemtblidx3,nItemtblidx4,
								strItemID,"SD�Ҵ�OnLine",userID,userName,"SD�Ҵ�OnLine��������",itemID);
							ds1= SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
							result = int.Parse(ds1.Tables[0].Rows[0].ItemArray[0].ToString());
						}
					}
					if(result!=-1)
					{
						get_Result +="��ӵ���"+itemName.ToString()+"������ID"+itemID.ToString()+"����������"+itemNum.ToString()+"�������"+userName.ToString()+"���ɹ�\n";
						SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UserAdditem_Add_All","������ӵ���"+itemName.ToString()+"������ID"+itemID.ToString()+"����������"+itemNum.ToString()+"�������"+userName.ToString()+"���ɹ�");
					}
					else
					{
						SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UserAdditem_Add_All","������ӵ���"+itemName.ToString()+"������ID"+itemID.ToString()+"����������"+itemNum.ToString()+"�������"+userName.ToString()+"��ʧ��");
					}
				} 
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return get_Result;
		}
		#endregion

		#region ��õ����б�
		/// <summary>
		/// ��õ����б�
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet GetItemList_Query(string serverIP,int type,string itemName)
		{
			DataSet result = null;
			string sql = null;
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				sql = "select sql_statement from sqlexpress where sql_type='SD_GetItemList_Query' and sql_condition = '"+type+"'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					if(type==2)
						sql = string.Format(sql,itemName);
					result = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ���͹�������
		public static int BoardTask_Insert(int userByID,string serverIP,string boardMessage,DateTime begintime,DateTime endTime,int interval,int noticeType,int Type)
		{
			int result = -1;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[9]{
												   new SqlParameter("@Gm_UserID",SqlDbType.Int),
												   new SqlParameter("@SD_ServerIP",SqlDbType.VarChar,2000),
												   new SqlParameter("@SD_BoardMessage",SqlDbType.VarChar,500),
												   new SqlParameter("@SD_begintime",SqlDbType.DateTime),
												   new SqlParameter("@SD_endTime",SqlDbType.DateTime),
												   new SqlParameter("@SD_NoticeType",SqlDbType.Int),
												   new SqlParameter("@SD_interval",SqlDbType.Int),
												   new SqlParameter("@Type",SqlDbType.Int),
												   new SqlParameter("@result",SqlDbType.Int)};
				paramCode[0].Value = userByID;
				paramCode[1].Value = serverIP;
				paramCode[2].Value = boardMessage;
				paramCode[3].Value = begintime;
				paramCode[4].Value = endTime;
				paramCode[5].Value = noticeType;
				paramCode[6].Value = interval;
				paramCode[7].Value = Type;
				paramCode[8].Direction = ParameterDirection.ReturnValue;
				result = SqlHelper.ExecSPCommand("SD_TaskList_Insert", paramCode);
			}
			catch (SqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region	��ѯ�����б�
		public static DataSet BoardTask_Query()
		{
			DataSet result = null;
			try
			{
				result = SqlHelper.ExecuteDataset("SD_BoardTask_Query");
			}
			catch (SqlException ex)
			{
				SqlHelper.errLog.WriteLog(ex.Message);
			}
			return result;
		}
		#endregion

		#region �޸Ĺ���״̬
		public static int BoardTask_Update(string serverIP,int userByID,int taskID,DateTime beginTime,DateTime endTime,int interval,int NoticeType,int status,string boardMessage)
		{
			int result = -1;
			SqlParameter[] paramCode;
			try
			{
				paramCode = new SqlParameter[10]{
												   new SqlParameter("@Gm_UserID",SqlDbType.Int),
												   new SqlParameter("@SD_serverip",SqlDbType.VarChar,300),
												   new SqlParameter("@SD_TaskID",SqlDbType.Int),
												   new SqlParameter("@SD_BoardMessage",SqlDbType.VarChar,500),
												   new SqlParameter("@SD_begintime",SqlDbType.DateTime),
												   new SqlParameter("@SD_endTime",SqlDbType.DateTime),
												   new SqlParameter("@SD_interval",SqlDbType.Int),
												   new SqlParameter("@SD_NoticeType",SqlDbType.Int),
												   new SqlParameter("@SD_status",SqlDbType.Int),
												   new SqlParameter("@result",SqlDbType.Int)};
				paramCode[0].Value = userByID;
				paramCode[1].Value = serverIP;
				paramCode[2].Value = taskID;
				paramCode[3].Value = boardMessage;
				paramCode[4].Value = beginTime;
				paramCode[5].Value = endTime;
				paramCode[6].Value = interval;
				paramCode[7].Value = NoticeType;
				paramCode[8].Value = status;
				paramCode[9].Direction = ParameterDirection.ReturnValue;
				result = SqlHelper.ExecSPCommand("SD_TaskList_Update", paramCode);
			}
			catch (SqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ɾ������
		/// <summary>
		/// ɾ������
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static int UserAdditem_Del(string serverIP,int UserByID,int userid,string username,int ItemID,string ItemName,int type)
		{
			int result = -1;
			string sql = null;
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				sql = "select sql_statement from sqlexpress where sql_type='SD_UserAdditem_Del' and sql_condition = '"+type.ToString()+"'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userid,ItemID,result);
					DataSet ds1= SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
					result = int.Parse(ds1.Tables[0].Rows[0].ItemArray[0].ToString());
				}
				if(result==0)
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UserAdditem_Del","ɾ�����"+username.ToString()+"����������"+type.ToString()+"������id"+type.ToString()+"��������"+ItemName.ToString()+"���ɹ�");
				}
				else
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UserAdditem_Del","ɾ�����"+username.ToString()+"����������"+type.ToString()+"������id"+type.ToString()+"��������"+ItemName.ToString()+"��ʧ��");
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �����¼��ѯ
		/// <summary>
		/// �����¼��ѯ
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet BuyLog_Query(string serverIP,int userid,DateTime startTime,DateTime endTime)
		{

			DataSet result = null;
			string sql = null;
			string eTime = null;
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				int i=0;
				if(DateTime.Compare(endTime,DateTime.Now.AddDays(1))>0)
					eTime=DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
				else
					eTime=endTime.AddDays(1).ToString("yyyy-MM-dd");
				while(!startTime.AddDays(i).ToString("yyyy-MM-dd").Equals(eTime))
				{
					if(i<45)
					{
						int iTime = Convert.ToInt32(startTime.AddDays(i).ToString("yyyyMMdd"));
						sql = "select sql_statement from sqlexpress where sql_type='SD_BuyLog_Query' and sql_condition = 'SD_BuyLog_Query'";
						System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
						if(ds!=null && ds.Tables[0].Rows.Count>0)
						{
							sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
							sql = string.Format(sql,userid,startTime,endTime,iTime);
							if(i==0)
							{
								result= SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
							}
							else
							{
								result.Merge(SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql));
							}
						}
					}
					else
					{
						break;
					}
					i++;
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
			
		}
		#endregion

		#region ��Ʒɾ����¼
		/// <summary>
		/// ��Ʒɾ����¼
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet Delete_ItemLog_Query(string serverIP,int userid,int type,DateTime startTime,DateTime endTime)
		{

			DataSet result = null;
			string sql = null;
			string eTime = null;
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				int i=0;
				if(DateTime.Compare(endTime,DateTime.Now.AddDays(1))>0)
					eTime=DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
				else
					eTime=endTime.AddDays(1).ToString("yyyy-MM-dd");
				while(!startTime.AddDays(i).ToString("yyyy-MM-dd").Equals(eTime))
				{
					if(i<45)
					{
						int iTime = Convert.ToInt32(startTime.AddDays(i).ToString("yyyyMMdd"));
						sql = "select sql_statement from sqlexpress where sql_type='SD_Delete_ItemLog_Query' and sql_condition = '"+type+"'";
						System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
						if(ds!=null && ds.Tables[0].Rows.Count>0)
						{
							sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
							sql = string.Format(sql,userid,startTime,endTime,iTime);
							if(i==0)
							{
								result= SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
							}
							else
							{
								result.Merge(SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql));
							}
						}
					}
					else
					{
						break;
					}
					i++;
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;

		}
		#endregion

		#region �����־��ѯ
		/// <summary>
		/// �����־��ѯ
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet LogInfo_Query(string serverIP,int userid,int type,DateTime startTime,DateTime endTime)
		{

			DataSet result = null;
			string sql = null;
			string eTime = null;
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				if(type==2||type==3||type==8||type==7)
				{
					int i=0;
					if(DateTime.Compare(endTime,DateTime.Now.AddDays(1))>0)
						eTime=DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
					else
						eTime=endTime.AddDays(1).ToString("yyyy-MM-dd");
					while(!startTime.AddDays(i).ToString("yyyy-MM-dd").Equals(eTime))
					{
						if(i<45)
						{
							int iTime = Convert.ToInt32(startTime.AddDays(i).ToString("yyyyMMdd"));
							sql = "select sql_statement from sqlexpress where sql_type='SD_LogInfo_Query' and sql_condition = '"+type+"'";
							System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
							if(ds!=null && ds.Tables[0].Rows.Count>0)
							{
								sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
								sql = string.Format(sql,iTime,userid,startTime,endTime);
								if(i==0)
								{
									result= SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
								}
								else
								{
									result.Merge(SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql));
								}
							}
						}
						else
						{
							break;
						}
						i++;
					}
				}
				else if(type==9)
				{
					int iTime = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
					sql = "select sql_statement from sqlexpress where sql_type='SD_LogInfo_Query' and sql_condition = '"+type+"'";
					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{
						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,iTime,userid,startTime,endTime);
						result= SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
					}					
				}
				else
				{
					sql = "select sql_statement from sqlexpress where sql_type='SD_LogInfo_Query' and sql_condition = '"+type+"'";
					System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
					if(ds!=null && ds.Tables[0].Rows.Count>0)
					{
						sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,userid,startTime,endTime);
						result= SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
					}
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;

		}
		#endregion

		#region ��ȡGM�˺��б�
		/// <summary>
		/// ��ȡGM�˺��б�
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static DataSet GetGmAccount_Query(string serverIP,int type,string UserName)
		{
			DataSet result = null;
			string sql = null;
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				sql = "select sql_statement from sqlexpress where sql_type='SD_GetGmAccount_Query' and sql_condition = '"+type+"'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					if(type==2)
						sql = string.Format(sql,UserName);
					result = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �޸�GM�˺�
		/// <summary>
		/// �޸�GM�˺�
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static int UpdateGmAccount_Query(string serverIP,int UserByID,int userid,string username,string PWD,string oldUserName,string pilotName)
		{
			int result = -1;
			string sql = null;
			try
			{
				MD5Encrypt st = new MD5Encrypt();
				string sign = st.getMD5ofStr(PWD).ToLower();

				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				sql = "select sql_statement from sqlexpress where sql_type='SD_UpdateGmAccount_Query' and sql_condition = 'SD_UpdateGmAccount_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userid,username,sign,pilotName);
					result = SqlHelper.ExecCommand(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql,true);
				}
				if(result==2)
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UpdateGmAccount_Query","�޸�GM�˺�Ϊ"+username.ToString()+"/����/�س�"+pilotName.ToString()+"���ɹ�");
					//���뱾��
					sql = "select sql_statement from sqlexpress where sql_type='SD_InGmUser_Query' and sql_condition = 'SD_InGmUser_Query'";
					System.Data.DataSet ds2 = SqlHelper.ExecuteDataset(sql);
					if(ds2!=null && ds2.Tables[0].Rows.Count>0)
					{	
						sql = ds2.Tables[0].Rows[0].ItemArray[0].ToString();
						sql = string.Format(sql,serverIP,oldUserName,username,sign,PWD,pilotName);
						result = SqlHelper.ExecCommand(sql);
					}
				}
				else
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UpdateGmAccount_Query","�޸�GM�˺�Ϊ"+username.ToString()+"/����/�س�"+pilotName.ToString()+"��ʧ��");
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region ��ӽ�Ǯ
		/// <summary>
		/// ��ӽ�Ǯ
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static int UpdateMoney_Query(string serverIP,int UserByID,int userid,string username,int money,int old_money)
		{
			int result = -1;
			string sql = null;
			try
			{
				string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
				sql = "select sql_statement from sqlexpress where sql_type='SD_UpdateMoney_Query' and sql_condition = 'SD_UpdateMoney_Query'";
				System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
				if(ds!=null && ds.Tables[0].Rows.Count>0)
				{	
					sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
					sql = string.Format(sql,userid,money);
					result = SqlHelper.ExecCommand(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql,true);
				}
				if(result==1)
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UpdateGmAccount_Query","�û�"+username.ToString()+"����Ǯ��"+old_money.ToString()+"�ĳ�"+money.ToString()+"���ɹ�");
				}
				else
				{
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UpdateGmAccount_Query","�û�"+username.ToString()+"����Ǯ��"+old_money.ToString()+"�ĳ�"+money.ToString()+"��ʧ��");
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return result;
		}
		#endregion

		#region �ָ�����
		/// <summary>
		/// �ָ�����
		/// </summary>
		/// <param name="serverIP">������Ip</param>
		/// <param name="account">�ʺ���</param>
		/// <returns></returns>
		public static string  ReGetUnits_Query(string serverIP,int UserByID,int userid,string username,int SDID,int itemid,string itemName,string getDate)
		{
			string __Result = null;
			System.Data.DataSet get_Result = null;
			int result = -1;
			string[] iList = null;
			int SlotIndex = -1;
			int UserUnitNum = -1;
			long ItemSerialNum = -1;
			int nCustomLvMax = -1;
			int customlvMax = -1;
			string sql = null;
			try
			{
				if(CommonInfo.SD_IsFullBox_Query(serverIP,userid)>0)
				{
					
					string serverPwd = CommonInfo.SD_GameDBInfo_Query(serverIP)[1].ToString();
					int iTime = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
					iList = CommonInfo.SD_GetReData_Query(serverIP,iTime,userid,SDID,itemid,getDate);
					if(iList!=null && iList.Length>0)
					{
						int number = int.Parse(iList[0].ToString());
						int itemID = int.Parse(iList[1].ToString());
						string shopitemid = iList[2].ToString();
						int itemtblidx = int.Parse(iList[3].ToString());
						int CategoryNumber = int.Parse(iList[4].ToString());
						string DateEnd = iList[5].ToString();
						int ShopUnit = int.Parse(iList[6].ToString());
						int TimeLimit = int.Parse(iList[7].ToString());
					
						sql = "select sql_statement from sqlexpress where sql_type='SD_ReGetUnits_Query' and sql_condition = '1'";
						System.Data.DataSet ds = SqlHelper.ExecuteDataset(sql);
						if(ds!=null && ds.Tables[0].Rows.Count>0)
						{	
							sql = ds.Tables[0].Rows[0].ItemArray[0].ToString();
							sql = string.Format(sql,number,itemID,shopitemid,itemtblidx,CategoryNumber,DateEnd,ShopUnit,TimeLimit);
							get_Result = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql);
						}
						if (get_Result != null && get_Result.Tables[0].Rows.Count > 0)
						{
							SlotIndex = int.Parse(get_Result.Tables[0].Rows[0].ItemArray[0].ToString());
							UserUnitNum = int.Parse(get_Result.Tables[0].Rows[0].ItemArray[1].ToString());
							ItemSerialNum = long.Parse(get_Result.Tables[0].Rows[0].ItemArray[2].ToString());
							nCustomLvMax = int.Parse(get_Result.Tables[0].Rows[0].ItemArray[3].ToString());
							customlvMax = CommonInfo.SD_isItemName_Query(itemName);
							if(customlvMax!=-1)
							{
								sql = "select sql_statement from sqlexpress where sql_type='SD_ReGetUnits_Query' and sql_condition = '2'";
								System.Data.DataSet ds1 = SqlHelper.ExecuteDataset(sql);
								if(ds1!=null && ds1.Tables[0].Rows.Count>0)
								{	
									sql = ds1.Tables[0].Rows[0].ItemArray[0].ToString();
									sql = string.Format(sql,customlvMax,userid,SlotIndex,ItemSerialNum);
									result = SqlHelper.ExecCommand(SqlHelper.GetConnectionString(serverIP,SqlHelper.SdUser,serverPwd,SqlHelper.SdMember),sql,true);
								}
							}
							else
							{
								result = 1;
							}
						}
					}
				}
				else
				{
					result = 2;
				}
				if(result==1)
				{
					__Result = "�û�"+username.ToString()+"���ָ�����"+itemName.ToString()+"����"+SlotIndex+"�����ӣ��������к�Ϊ"+ItemSerialNum.ToString()+"ǿ���ȼ�Ϊ"+nCustomLvMax.ToString()+"���ɹ�";
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UpdateGmAccount_Query","�û�"+username.ToString()+"���ָ�����"+itemName.ToString()+"����"+SlotIndex+"�����ӣ��������к�Ϊ"+ItemSerialNum.ToString()+"ǿ���ȼ�Ϊ"+nCustomLvMax.ToString()+"���ɹ�");
				}
				else if(result==2)
				{
					__Result = "�û�"+username.ToString()+"���ָ�����"+itemName.ToString()+"����"+SlotIndex+"�����ӣ��������к�Ϊ"+ItemSerialNum.ToString()+"ǿ���ȼ�Ϊ"+nCustomLvMax.ToString()+"���û������������";
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UpdateGmAccount_Query","�û�"+username.ToString()+"���ָ�����"+itemName.ToString()+"����"+SlotIndex+"�����ӣ��������к�Ϊ"+ItemSerialNum.ToString()+"ǿ���ȼ�Ϊ"+nCustomLvMax.ToString()+"���û������������");
				}
				else
				{
					__Result = "�û�"+username.ToString()+"���ָ�����"+itemName.ToString()+"����"+SlotIndex+"�����ӣ��������к�Ϊ"+ItemSerialNum.ToString()+"ǿ���ȼ�Ϊ"+nCustomLvMax.ToString()+"��ʧ��";
					SqlHelper.insertGMtoolsLog(UserByID,"SD�ߴ�",serverIP,"SD_UpdateGmAccount_Query","�û�"+username.ToString()+"���ָ�����"+itemName.ToString()+"����"+SlotIndex+"�����ӣ��������к�Ϊ"+ItemSerialNum.ToString()+"ǿ���ȼ�Ϊ"+nCustomLvMax.ToString()+"��ʧ��");
				}
			}
			catch (MySqlException ex)
			{
				SqlHelper.errLog.WriteLog("������IP"+serverIP+ex.Message);
			}
			return __Result;
		}
		#endregion
	}
}
