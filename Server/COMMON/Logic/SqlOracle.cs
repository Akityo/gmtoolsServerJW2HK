using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using STONE.HU.HELPER.UTIL;
using System.Configuration;
using Oracle.DataAccess.Client;
namespace Common.Logic
{
	/// <summary>
	/// ���ݿ�ͨ�ò�����
	/// </summary>
	public class SqlOracle
	{
		protected  OracleConnection con;//���Ӷ���

		#region AuLog
		private static string AuLogInOutUser_O = ConfigurationSettings.AppSettings["AuLogInOutUser_O"];
		private static string AuLogInOutPwd_O = ConfigurationSettings.AppSettings["AuLogInOutPwd_O"];

		private static string AuLogInOutIP43 = ConfigurationSettings.AppSettings["AuLogInOutIP43"];
		private static string AuLogInOutIP44 = ConfigurationSettings.AppSettings["AuLogInOutIP44"];
		private static string AuLogInOutIP45 = ConfigurationSettings.AppSettings["AuLogInOutIP45"];
		private static string AuLogInOutIP46 = ConfigurationSettings.AppSettings["AuLogInOutIP46"];
		private static string AuLogInOutIP47 = ConfigurationSettings.AppSettings["AuLogInOutIP47"];
		private static string AuLogInOutIP48 = ConfigurationSettings.AppSettings["AuLogInOutIP48"];
		private static string AuLogInOutIP49 = ConfigurationSettings.AppSettings["AuLogInOutIP49"];
		private static string AuLogInOutIP50 = ConfigurationSettings.AppSettings["AuLogInOutIP50"];
		#endregion

		#region SdLog
		private static string Sd_Oracle_User = ConfigurationSettings.AppSettings["SD_ORACLE_USER"];
		private static string Sd_Oracle_Pwd = ConfigurationSettings.AppSettings["SD_ORACLE_PWD"];
		#endregion


		#region tgLog �ƹ�齱�û��񽱼�¼��ѯ

		private static string Tg_Oracle_Data = ConfigurationSettings.AppSettings["TG_ORACLE_DATA"];
		private static string Tg_Oracle_User = ConfigurationSettings.AppSettings["TG_ORACLE_USER"];
		private static string Tg_Oracle_Pwd = ConfigurationSettings.AppSettings["TG_ORACLE_PWD"];
		#endregion
		public SqlOracle()
		{
			
		}
		public SqlOracle(string constr)
		{
			con=new OracleConnection(constr);
		}
		#region ����
		public static string auLogInOutIp43
		{
			get
			{
				return SqlOracle.AuLogInOutIP43;
			}
		}
		public static string auLogInOutIp44
		{
			get
			{
				return SqlOracle.AuLogInOutIP44;
			}
		}
		public static string auLogInOutIp45
		{
			get
			{
				return SqlOracle.AuLogInOutIP45;
			}
		}
		public static string auLogInOutIp46
		{
			get
			{
				return SqlOracle.AuLogInOutIP46;
			}
		}
		public static string auLogInOutIp47
		{
			get
			{
				return SqlOracle.AuLogInOutIP47;
			}
		}
		public static string auLogInOutIp48
		{
			get
			{
				return SqlOracle.AuLogInOutIP48;
			}
		}
		public static string auLogInOutIp49
		{
			get
			{
				return SqlOracle.AuLogInOutIP49;
			}
		}
		public static string auLogInOutIp50
		{
			get
			{
				return SqlOracle.AuLogInOutIP50;
			}
		}
		public static string auLogInOutPwd
		{
			get
			{
				return SqlOracle.AuLogInOutPwd_O;
			}
		}
		public static string auLogInOutUser
		{
			get
			{
				return SqlOracle.AuLogInOutUser_O;
			}
		}
		public static string SD_Oracle_User
		{
			get
			{
				return SqlOracle.Sd_Oracle_User;
			}
		}
		public static string SD_Oracle_Pwd
		{
			get
			{
				return SqlOracle.Sd_Oracle_Pwd;
			}
		}

		public static string TG_Oracle_User
		{
			get
			{
				return SqlOracle.Tg_Oracle_User;
			}
		}
		public static string TG_Oracle_Pwd
		{
			get
			{
				return SqlOracle.Tg_Oracle_Pwd;
			}
		}
		
		public static string  TG_Oracle_Data
		{
			get
			{
				return SqlOracle.Tg_Oracle_Data;
			}
		}
		#endregion

		#region �����ݿ�����
		/// <summary>
		/// �����ݿ�����
		/// </summary>
		private  void Open()
		{
			//�����ݿ�����
			if(con.State==ConnectionState.Closed)
			{
				try
				{
					//�����ݿ�����
					con.Open();
				}
				catch(Exception e)
				{
					throw e;
				}
			}
		}
		#endregion
		#region �ر����ݿ�����
		/// <summary>
		/// �ر����ݿ�����
		/// </summary>
		private  void Close()
		{
			//�ж����ӵ�״̬�Ƿ��Ѿ���
			if(con.State==ConnectionState.Open)
			{
				con.Close();
			}
		}
		#endregion
		#region ִ�в�ѯ��䣬����OracleDataReader ( ע�⣺���ø÷�����һ��Ҫ��OracleDataReader����Close )
		/// <summary>
		/// ִ�в�ѯ��䣬����OracleDataReader ( ע�⣺���ø÷�����һ��Ҫ��OracleDataReader����Close )
		/// </summary>
		/// <param name="sql">��ѯ���</param>
		/// <returns>OracleDataReader</returns>   
		public  OracleDataReader ExecuteReader(string sql)
		{
			OracleDataReader myReader;
			Open();
			OracleCommand cmd = new OracleCommand(sql, con);
			myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			return myReader;
		}
		#endregion

		#region ִ��SQL��䣬�������ݵ�DataSet��
		/// <summary>
		/// ִ��SQL��䣬�������ݵ�DataSet��
		/// </summary>
		/// <param name="sql">sql���</param>
		/// <returns>����DataSet</returns>
		public DataSet GetDataSet(string sql)
		{
			DataSet ds=new DataSet();
			try
			{
				Open();//����������
				OracleDataAdapter adapter=new OracleDataAdapter(sql,con);
				adapter.Fill(ds);
			}
			catch(Oracle.DataAccess.Client.OracleException ex)
			{
				ds = null;
				Console.WriteLine(ex.Message);
			}
			catch(System.Exception exs)
			{
				ds = null;
				Console.WriteLine(exs.Message);
			}
			finally
			{
				Close();//�ر����ݿ�����
			}
			return ds;
		}
		#endregion
		#region ִ��SQL��䣬�������ݵ��Զ���DataSet��
		/// <summary>
		/// ִ��SQL��䣬�������ݵ�DataSet��
		/// </summary>
		/// <param name="sql">sql���</param>
		/// <param name="DataSetName">�Զ��巵�ص�DataSet����</param>
		/// <returns>����DataSet</returns>
		public  DataSet GetDataSet(string sql,string DataSetName)
		{
			DataSet ds=new DataSet();
			Open();//����������
			OracleDataAdapter adapter=new OracleDataAdapter(sql,con);
			adapter.Fill(ds,DataSetName);
			Close();//�ر����ݿ�����
			return ds;
		}
		#endregion
		#region ִ��Sql���,���ش���ҳ���ܵ��Զ���dataset
		/// <summary>
		/// ִ��Sql���,���ش���ҳ���ܵ��Զ���dataset
		/// </summary>
		/// <param name="sql">Sql���</param>
		/// <param name="PageSize">ÿҳ��ʾ��¼��</param>
		/// <param name="CurrPageIndex">��ǰҳ</param>
		/// <param name="DataSetName">����dataset����</param>
		/// <returns>����DataSet</returns>
		public  DataSet GetDataSet(string sql,int PageSize,int CurrPageIndex,string DataSetName)
		{
			DataSet ds=new DataSet();
			Open();//����������
			OracleDataAdapter adapter=new OracleDataAdapter(sql,con);
			adapter.Fill(ds,PageSize * (CurrPageIndex - 1), PageSize,DataSetName);
			Close();//�ر����ݿ�����
			return ds;
		}
		#endregion
		#region ִ��SQL��䣬���ؼ�¼����
		/// <summary>
		/// ִ��SQL��䣬���ؼ�¼����
		/// </summary>
		/// <param name="sql">sql���</param>
		/// <returns>���ؼ�¼������</returns>
		public  int GetRecordCount(string sql)
		{
			int recordCount = 0;
			Open();//����������
			OracleCommand command = new OracleCommand(sql,con);
			OracleDataReader dataReader = command.ExecuteReader();
			while(dataReader.Read())
			{
				recordCount++;
			}
			dataReader.Close();
			Close();//�ر����ݿ�����
			return recordCount;
		}
		#endregion

		#region ͳ��ĳ���¼���� 
		/// <summary>
		/// ͳ��ĳ���¼����
		/// </summary>
		/// <param name="KeyField">����/������</param>
		/// <param name="TableName">���ݿ�.�û���.����</param>
		/// <param name="Condition">��ѯ����</param>
		/// <returns>���ؼ�¼����</returns> 
		public  int GetRecordCount(string keyField, string tableName, string condition)
		{
			int RecordCount = 0;
			string sql = "select count(" + keyField + ") as count from " + tableName + " " + condition;
			DataSet ds = GetDataSet(sql);
			if (ds.Tables[0].Rows.Count > 0)
			{
				RecordCount =Convert.ToInt32(ds.Tables[0].Rows[0][0]);
			}
			ds.Clear();
			ds.Dispose();
			return RecordCount;
		}
		/// <summary>
		/// ͳ��ĳ���¼����
		/// </summary>
		/// <param name="Field">���ظ����ֶ�</param>
		/// <param name="tableName">���ݿ�.�û���.����</param>
		/// <param name="condition">��ѯ����</param>
		/// <param name="flag">�ֶ��Ƿ�����</param>
		/// <returns>���ؼ�¼����</returns> 
		public  int GetRecordCount(string Field, string tableName, string condition, bool flag)
		{
			int RecordCount = 0;
			if (flag)
			{
				RecordCount = GetRecordCount(Field, tableName, condition);
			}
			else
			{
				string sql = "select count(distinct(" + Field + ")) as count from " + tableName + " " + condition;
				DataSet ds = GetDataSet(sql);
				if (ds.Tables[0].Rows.Count > 0)
				{
					RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
				}
				ds.Clear();
				ds.Dispose();
			}
			return RecordCount;
		}
		#endregion
		#region ͳ��ĳ���ҳ���� 
		/// <summary>
		/// ͳ��ĳ���ҳ����
		/// </summary>
		/// <param name="keyField">����/������</param>
		/// <param name="tableName">����</param>
		/// <param name="condition">��ѯ����</param>
		/// <param name="pageSize">ҳ��</param>
		/// <param name="RecordCount">��¼����</param>
		/// <returns>���ط�ҳ����</returns> 
		public  int GetPageCount(string keyField, string tableName, string condition, int pageSize, int RecordCount)
		{
			int PageCount = 0;
			PageCount = (RecordCount % pageSize) > 0 ? (RecordCount / pageSize) + 1 : RecordCount / pageSize;
			if (PageCount < 1) PageCount = 1;
			return PageCount;
		}
		/// <summary>
		/// ͳ��ĳ���ҳ����
		/// </summary>
		/// <param name="keyField">����/������</param>
		/// <param name="tableName">����</param>
		/// <param name="condition">��ѯ����</param>
		/// <param name="pageSize">ҳ��</param>
		/// <returns>����ҳ������</returns> 
		public  int GetPageCount(string keyField, string tableName, string condition, int pageSize, ref int RecordCount)
		{
			RecordCount = GetRecordCount(keyField, tableName, condition);
			return GetPageCount(keyField, tableName, condition, pageSize, RecordCount);
		}
		/// <summary>
		/// ͳ��ĳ���ҳ����
		/// </summary>
		/// <param name="Field">���ظ����ֶ�</param>
		/// <param name="tableName">����</param>
		/// <param name="condition">��ѯ����</param>
		/// <param name="pageSize">ҳ��</param>
		/// <param name="flag">�Ƿ�����</param>
		/// <returns>����ҳҳ����</returns> 
		public  int GetPageCount(string Field, string tableName, string condition, ref int RecordCount, int pageSize, bool flag)
		{
			RecordCount = GetRecordCount(Field, tableName, condition, flag);
			return GetPageCount(Field, tableName, condition, pageSize, ref RecordCount);
		}
		#endregion 

		#region Sql��ҳ���� 
		/// <summary>
		/// �����ҳ��ѯSQL���
		/// </summary>
		/// <param name="KeyField">����</param>
		/// <param name="FieldStr">������Ҫ��ѯ���ֶ�(field1,field2...)</param>
		/// <param name="TableName">����.ӵ����.����</param>
		/// <param name="where">��ѯ����1(where ...)</param>
		/// <param name="order">��������2(order by ...)</param>
		/// <param name="CurrentPage">��ǰҳ��</param>
		/// <param name="PageSize">ҳ��</param>
		/// <returns>SQL���</returns> 
		public  string JoinPageSQL(string KeyField, string FieldStr, string TableName, string Where, string Order, int CurrentPage, int PageSize)
		{
			string sql = null;
			if (CurrentPage == 1)
			{
				sql = "select  " + CurrentPage * PageSize + " " + FieldStr + " from " + TableName + " " + Where + " " + Order + " ";
			}
			else
			{
				sql = "select * from (";
				sql += "select  " + CurrentPage * PageSize + " " + FieldStr + " from " + TableName + " " + Where + " " + Order + ") a ";
				sql += "where " + KeyField + " not in (";
				sql += "select  " + (CurrentPage - 1) * PageSize + " " + KeyField + " from " + TableName + " " + Where + " " + Order + ")";
			}
			return sql;
		}
		/// <summary>
		/// �����ҳ��ѯSQL���
		/// </summary>
		/// <param name="Field">�ֶ���(������)</param>
		/// <param name="TableName">����.ӵ����.����</param>
		/// <param name="where">��ѯ����1(where ...)</param>
		/// <param name="order">��������2(order by ...)</param>
		/// <param name="CurrentPage">��ǰҳ��</param>
		/// <param name="PageSize">ҳ��</param>
		/// <returns>SQL���</returns> 
		public  string JoinPageSQL(string Field, string TableName,string Where, string Order, int CurrentPage, int PageSize)
		{
			string sql = null;
			if (CurrentPage == 1)
			{
				sql = "select rownum " + CurrentPage * PageSize + " " + Field + " from " + TableName + " " + Where + " " + Order + " group by " + Field;
			}
			else
			{
				sql = "select * from (";
				sql += "select rownum " + CurrentPage * PageSize + " " + Field + " from " + TableName + " " + Where + " " + Order + " group by " + Field + " ) a ";
				sql += "where " + Field + " not in (";
				sql += "select rownum " + (CurrentPage - 1) * PageSize + " " + Field + " from " + TableName + " " + Where + " " + Order + " group by " + Field + ")";
			}
			return sql;
		}
		#endregion
		#region ����ϵͳʱ�䶯̬���ɸ��ֲ�ѯ���(���Ѿ�ע�͵����Ա��Ժ�ʹ��)
		//  #region ���ݲ�ѯʱ�����������̬���ɲ�ѯ���
		//  /// <summary>
		//  /// ���ݲ�ѯʱ�����������̬���ɲ�ѯ���
		//  /// </summary>
		//  /// <param name="starttime">��ʼʱ��</param>
		//  /// <param name="endtime">����ʱ��</param>
		//  /// <param name="dw">��λ</param>
		//  /// <param name="startxsl">��ʼ������</param>
		//  /// <param name="endxsl">����������</param>
		//  /// <param name="danwei">��λ�ֶ�</param>
		//  /// <param name="xiansunlv">�������ֶ�</param>
		//  /// <param name="tablehz">���׺</param>
		//  /// <returns>SQL���</returns>
		//  public  string SQL(DateTime starttime,DateTime endtime,string dw,float startxsl,float endxsl,string danwei,string xiansunlv,string tablehz)
		//  {
		//
		//   string sql=null;
		//   //�������ʱ���ʽת���ɹ̶��ĸ�ʽ"yyyy-mm-dd"
		//   string zstarttime=starttime.GetDateTimeFormats('D')[1].ToString();
		//   string zendtime=endtime.GetDateTimeFormats('D')[1].ToString();
		//   string nTime=DateTime.Now.GetDateTimeFormats('D')[1].ToString();
		//
		//
		//   //ȡ����ֵ��ǰ��λ��������ֵ
		//   string sTime=zstarttime.Substring(0,4)+zstarttime.Substring(5,2);
		//   string eTime=zendtime.Substring(0,4)+zendtime.Substring(5,2);
		//   string nowTime=nTime.Substring(0,4)+nTime.Substring(5,2);
		//   //�ֱ�ȡ���ڵ������
		//   int sy=Convert.ToInt32(zstarttime.Substring(0,4));
		//   int ey=Convert.ToInt32(zendtime.Substring(0,4));
		//   int sm=Convert.ToInt32(zstarttime.Substring(5,2));
		//   int em=Convert.ToInt32(zendtime.Substring(5,2));
		//   //��ر�������
		//   int s;
		//   int e;
		//   int i;
		//   int j;
		//   int js;
		//   int nz;
		//   string x;
		//   //һ��ȡ��ǰ������SQL���
		//   if(sTime==nowTime&&eTime==nowTime)
		//   {
		//    sql="select  * from "+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" ";
		//   }
		//    //����ȡ��ǰ�������������SQL���
		//   else if(sTime==nowTime&&eTime!=nowTime)
		//   {
		//    sql="select  * from "+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
		//    //���������
		//    if(sy==ey)
		//    {
		//     s=Convert.ToInt32(sTime);
		//     e=Convert.ToInt32(eTime);
		//     for(i=s+1;i<e;i++)
		//     {
		//      i=i++;
		//      sql+="select  * from "+i.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
		//     }
		//     sql+="select  * from "+e.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" ";
		//    }
		//     //������ݴ��ڿ�ʼ���
		//    else
		//    {
		//     //1����ѭ������ʼʱ�����ʼʱ���12��
		//     s=Convert.ToInt32(sTime);
		//     x=zstarttime.Substring(0,4)+"12";
		//     nz=Convert.ToInt32(x);
		//     for(i=s+1;i<=nz;i++)
		//     {
		//      i=i++;
		//      sql+="select  * from "+i.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
		//     }
		//     //2��ѭ������������
		//     for(i=sy+1;i<ey;i++)
		//     {
		//
		//      for(j=1;j<=12;j++)
		//      {
		//       if(j<10)
		//       {
		//        sql+="select  * from "+i.ToString()+"0"+j.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
		//       }
		//       else
		//       {
		//        sql+="select  * from "+i.ToString()+j.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
		//       }
		//      }
		//     }
		//     //3��ѭ�����������·�
		//     js=Convert.ToInt32(zendtime.Substring(0,4)+"00");
		//     for(i=js;i<Convert.ToInt32(eTime);i++)
		//     {
		//      i++;
		//      sql+="select  * from "+i.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
		//
		//     }
		//     sql+="select  * from "+eTime.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" ";
		//
		//    }
		//   }
		//    //����ȡ��������������SQL���
		//   else
		//   {
		//    //1����ѭ������ʼʱ�����ʼʱ���12��
		//    s=Convert.ToInt32(sTime);
		//    x=zstarttime.Substring(0,4)+"12";
		//    nz=Convert.ToInt32(x);
		//    for(i=s;i<=nz;i++)
		//    {
		//     i=i++;
		//     sql+="select  * from "+i.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
		//    }
		//    //2��ѭ������������
		//    for(i=sy+1;i<ey;i++)
		//    {
		//
		//     for(j=1;j<=12;j++)
		//     {
		//      if(j<10)
		//      {
		//       sql+="select  * from "+i.ToString()+"0"+j.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
		//      }
		//      else
		//      {
		//       sql+="select  * from "+i.ToString()+j.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
		//      }
		//     }
		//    }
		//    //3��ѭ�����������·�
		//    js=Convert.ToInt32(zendtime.Substring(0,4)+"00");
		//    for(i=js;i<Convert.ToInt32(eTime);i++)
		//    {
		//     i++;
		//     sql+="select  * from "+i.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" "+"union"+" ";
		//
		//    }
		//    sql+="select  * from "+eTime.ToString()+'_'+tablehz+" "+"where"+" "+danwei+"="+dw+" "+" "+"and"+" "+xiansunlv+">="+startxsl+" "+"and"+" "+xiansunlv+"<="+endxsl+" ";
		//
		//   }
		//   return sql;
		//  }
		//  #endregion

		//
		//  #region ���ݲ�ѯʱ�����������̬���ɲ�ѯ���
		//  /// <summary>
		//  /// ���ݲ�ѯʱ�����������̬���ɲ�ѯ���
		//  /// </summary>
		//  /// <param name="starttime">��ʼʱ��</param>
		//  /// <param name="endtime">����ʱ��</param>
		//  /// <param name="zhiduan">��ѯ�ֶ�</param>
		//  /// <param name="tiaojiao">��ѯ����</param>
		//  /// <param name="tablehz">���׺</param>
		//  /// <returns>SQL���</returns>
		//  public  string SQL(DateTime starttime,DateTime endtime,string zhiduan,string tiaojiao,string tablehz)
		//  {
		//
		//   string sql=null;
		//   //�������ʱ���ʽת���ɹ̶��ĸ�ʽ"yyyy-mm-dd"
		//   string zstarttime=starttime.GetDateTimeFormats('D')[1].ToString();
		//   string zendtime=endtime.GetDateTimeFormats('D')[1].ToString();
		//   string nTime=DateTime.Now.GetDateTimeFormats('D')[1].ToString();
		//
		//
		//   //ȡ����ֵ��ǰ��λ��������ֵ
		//   string sTime=zstarttime.Substring(0,4)+zstarttime.Substring(5,2);
		//   string eTime=zendtime.Substring(0,4)+zendtime.Substring(5,2);
		//   string nowTime=nTime.Substring(0,4)+nTime.Substring(5,2);
		//   //�ֱ�ȡ���ڵ������
		//   int sy=Convert.ToInt32(zstarttime.Substring(0,4));
		//   int ey=Convert.ToInt32(zendtime.Substring(0,4));
		//   int sm=Convert.ToInt32(zstarttime.Substring(5,2));
		//   int em=Convert.ToInt32(zendtime.Substring(5,2));
		//   //��ر�������
		//   int s;
		//   int e;
		//   int i;
		//   int j;
		//   int js;
		//   int nz;
		//   string x;
		//   //һ��ȡ��ǰ������SQL���
		//   if(sTime==nowTime&&eTime==nowTime)
		//   {
		//    sql="select"+" "+zhiduan+" "+"from"+" "+tablehz+" "+"where"+" "+tiaojiao+" ";
		//
		//   }
		//    //����ȡ��ǰ�������������SQL���
		//   else if(sTime==nowTime&&eTime!=nowTime)
		//   {
		//    sql="select"+" "+zhiduan+" "+"from"+" "+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
		//
		//    //���������
		//    if(sy==ey)
		//    {
		//     s=Convert.ToInt32(sTime);
		//     e=Convert.ToInt32(eTime);
		//     for(i=s+1;i<e;i++)
		//     {
		//      i=i++;
		//      sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
		//
		//     }
		//     sql+="select"+" "+zhiduan+" "+"from"+" "+e.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" ";
		//
		//    }
		//     //������ݴ��ڿ�ʼ���
		//    else
		//    {
		//     //1����ѭ������ʼʱ�����ʼʱ���12��
		//     s=Convert.ToInt32(sTime);
		//     x=zstarttime.Substring(0,4)+"12";
		//     nz=Convert.ToInt32(x);
		//     for(i=s+1;i<=nz;i++)
		//     {
		//      i=i++;
		//      sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
		//
		//     }
		//     //2��ѭ������������
		//     for(i=sy+1;i<ey;i++)
		//     {
		//
		//      for(j=1;j<=12;j++)
		//      {
		//       if(j<10)
		//       {
		//        sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+"0"+j.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
		//
		//       }
		//       else
		//       {
		//        sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+j.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
		//       }
		//      }
		//     }
		//     //3��ѭ�����������·�
		//     js=Convert.ToInt32(zendtime.Substring(0,4)+"00");
		//     for(i=js;i<Convert.ToInt32(eTime);i++)
		//     {
		//      i++;
		//      sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
		//
		//     }
		//     sql+="select"+" "+zhiduan+" "+"from"+" "+eTime.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" ";
		//
		//    }
		//   }
		//    //����ȡ��������������SQL���
		//   else
		//   {
		//    //1����ѭ������ʼʱ�����ʼʱ���12��
		//    s=Convert.ToInt32(sTime);
		//    x=zstarttime.Substring(0,4)+"12";
		//    nz=Convert.ToInt32(x);
		//    for(i=s;i<=nz;i++)
		//    {
		//     i=i++;
		//     sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
		//
		//    }
		//    //2��ѭ������������
		//    for(i=sy+1;i<ey;i++)
		//    {
		//
		//     for(j=1;j<=12;j++)
		//     {
		//      if(j<10)
		//      {
		//       sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+"0"+j.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
		//
		//      }
		//      else
		//      {
		//       sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+j.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
		//      }
		//     }
		//    }
		//    //3��ѭ�����������·�
		//    js=Convert.ToInt32(zendtime.Substring(0,4)+"00");
		//    for(i=js;i<Convert.ToInt32(eTime);i++)
		//    {
		//     i++;
		//     sql+="select"+" "+zhiduan+" "+"from"+" "+i.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" "+"union"+" ";
		//
		//    }
		//    sql+="select"+" "+zhiduan+" "+"from"+" "+eTime.ToString()+'_'+tablehz+" "+"where"+" "+tiaojiao+" ";
		//
		//   }
		//   return sql;
		//  }
		#endregion

		
		#region ExecSPDataSet
		/// <summary>
		/// �õ��洢���̷������ݼ�
		/// </summary>
		/// <param name="strProc">�洢������</param>
		/// <param name="paramers">���������</param>
		/// <returns>�������ݼ�</returns>
		public DataSet ExecSPDataSet(string strProc,System.Data.IDataParameter[] paramers)
		{
			
			try
			{
				Open();//����������
				OracleCommand sqlcom=new OracleCommand(strProc,con);
		
				sqlcom.CommandTimeout = 6000;
				sqlcom.CommandType= CommandType.StoredProcedure;

				foreach(System.Data.IDataParameter paramer in paramers)
				{
					sqlcom.Parameters.Add(paramer);
				}            
				
				OracleDataAdapter da=new OracleDataAdapter();
				da.SelectCommand=sqlcom;
				DataSet ds=new DataSet();
				da.Fill(ds);
				return ds;
				
			}
			catch(System.Exception ex)
			{
               
                Console.WriteLine(ex.Message);
				return null;
				
			}
			finally
			{
				Close();//�ر����ݿ�����
			}
		}
		#endregion
	}
}
