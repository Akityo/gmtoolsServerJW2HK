    using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.IO;
using Common.Logic;
using Common.API;
using Common.DataInfo;
//using GM_Server.FjAPI;
using lg = Common.API.LanguageAPI;
//using GM_Server.SDOnlineAPI;
//using GM_Server.SDOnlineDataInfo;
using GM_Server.JW2API;
using GM_Server.JW2DataInfo;
namespace GMSERVER.ServerSocket
{
	/// <summary>
	/// Handler ��ժҪ˵����
	/// </summary>
	public class Handler
	{
		private TcpClient svrSocket ;
		private NetworkStream networkStream ;
		private Mutex mut = new Mutex();
		bool ContinueProcess = false ;
		private byte[] bytes; 		// Data buffer for incoming data.
		private StringBuilder sb =  new StringBuilder(); // Received data string.
		private string data = null; // Incoming data from the client.
		Handler[] handler_ = new Handler[2];
		int handlerType = 0;

		public Handler (TcpClient acceptClientSocket) 
		{
			this.svrSocket = acceptClientSocket ;
			networkStream = acceptClientSocket.GetStream();
			bytes = new byte[acceptClientSocket.ReceiveBufferSize];
			handler_[handlerType]=this;
			//ContinueProcess = true ;
		}
		public Handler getHandler(int type)
		{
			return this.handler_[type];
		}
		/// <summary>
		/// �����ͻ������ӽ����Ժ󣬷������˽��ܺͷ�����Ϣ�Ĵ�������
		/// </summary>
		public void Process() 
		{
			int index = 0;
			int pageSize = 0;
			int userByID = 0;
			string name = null;
			string passwd = null;
			string mac = null;
			DateTime connTime;
			int status = 0;
			mut.WaitOne();
			try
			{
				while ( true ) 
				{

					UserValidate validate = null;
					int BytesRead =  networkStream.Read(bytes, 0, (int) bytes.Length) ;
					Message msg = new Message(bytes,(uint)bytes.Length);
					Packet packet = msg.m_packet;
					if( msg.IsValidMessage==true)
					{
						Packet_Body mesBody = new Packet_Body(packet.m_Body.m_bBodyBuffer,packet.m_Body.m_uiBodyLen);
						if (msg.GetMessageID() ==Message_Tag_ID.CONNECT 
							|| msg.GetMessageID() ==Message_Tag_ID.ACCOUNT_AUTHOR
							|| msg.GetMessageID() == Message_Tag_ID.DISCONNECT)
						{
							name = System.Text.Encoding.Unicode.GetString(mesBody.getTLVByTag(TagName.UserName).m_bValueBuffer);
							data = name;
							passwd = System.Text.Encoding.Unicode.GetString(mesBody.getTLVByTag(TagName.PassWord).m_bValueBuffer);
							mac = System.Text.Encoding.Unicode.GetString(mesBody.getTLVByTag(TagName.MAC).m_bValueBuffer);
							if(msg.GetMessageID()==Message_Tag_ID.CONNECT )
							{
								//TLV_Structure tvlLimit = new TLV_Structure(TagName.Conn_Time,mesBody.getTLVByTag(TagName.Conn_Time).m_uiValueLen,mesBody.getTLVByTag(TagName.Conn_Time).m_bValueBuffer);
								connTime = DateTime.Now; 
								//��Ӧ��������
								CommonAPI api = new CommonAPI(name,passwd,mac,connTime,"PASS");
								send(api.packConnectResp());
							}
							else if(msg.GetMessageID() ==Message_Tag_ID.ACCOUNT_AUTHOR)
							{
								//�û���֤����
								if (name.Equals("administrator")==true  && passwd.Equals("MengjianG!@#$%^")==true)
								{
									send(Message.Common_ACCOUNT_AUTHOR_RESP(0,"PASS"));
									userByID = 0;
									status = 1;
								}
								else
								{
									validate = new UserValidate(name,passwd,mac);
									send(validate.validateUser());
									userByID = validate.UserByID;
									status = validate.Status;
								}
							}
							else if(msg.GetMessageID() == Message_Tag_ID.DISCONNECT)
							{
								//��Ӧ�Ͽ�����
								TLV_Structure stuct = new TLV_Structure(TagName.UserByID,mesBody.getTLVByTag(TagName.UserByID).m_uiValueLen,mesBody.getTLVByTag(TagName.UserByID).m_bValueBuffer);
								userByID = (int)stuct.toInteger();
								CommonAPI api = new CommonAPI(userByID,"PASS",bytes);
								UserInfoAPI userInfo = new UserInfoAPI();
								userInfo.GM_UpdateActiveUser(userByID,0);
								send(api.packConnectResp());
							}
						}
						//��֤ͨ��
						if(status==1)
						{
							TLV_Structure stuct;
                            Common.DataInfo.UpdatePatch patch = new Common.DataInfo.UpdatePatch(bytes);

							
							DepartmentAPI departInfo = new DepartmentAPI(bytes);
							UserInfoAPI userInfo = new UserInfoAPI(bytes);
							ModuleInfoAPI moduleInfo = new ModuleInfoAPI(bytes);
							GameInfoAPI gameInfo = new GameInfoAPI(bytes);
							UserModuleAPI userModule = new UserModuleAPI(bytes);
							CommonAPI api = new CommonAPI(userByID,"PASS",bytes);
							NotesInfoAPI notesAPI = new NotesInfoAPI(bytes);
//							SDAccountInfoAPI SDAccountAPI = new SDAccountInfoAPI(bytes);
//							SDLogInfoAPI SDLogAPI = new SDLogInfoAPI(bytes);

							JW2AccountInfoAPI jw2accountinfo = new JW2AccountInfoAPI(bytes);
							JW2ItemInfoAPI jw2iteminfo = new JW2ItemInfoAPI(bytes);
							JW2LogInfoAPI jw2loginfo = new JW2LogInfoAPI(bytes);
							JW2LoginInfoAPI jw2logininfo = new JW2LoginInfoAPI(bytes);
							JW2MessengerInfoAPI jw2messengerinfo = new JW2MessengerInfoAPI(bytes);
						

							switch(msg.GetMessageID())
							{
                                //�ͻ��˱Ƚ�����
                                case Message_Tag_ID.CLIENT_PATCH_COMPARE:
                                 {
                                    send(patch.encodeMessage());
                                    break;
                                  }
                              //�ͻ��˸�������
                              case Message_Tag_ID.CLIENT_PATCH_UPDATE:
                                  {
                                      send(patch.transferPatchFile());
                                      break;
                                  }
									//�õ�������Ϣ
								case Message_Tag_ID.DEPART_QUERY:
									send(departInfo.GM_QueryDepartInfo());
									break;
								case Message_Tag_ID.DEPARTMENT_RELATE_QUERY:
									send(departInfo.GM_QueryDepartRelateInfo());
									break;
								case Message_Tag_ID.DEPART_RELATE_GAME_QUERY:
									send(departInfo.GM_QueryDepartRelateGameInfo());
									break;
									//��Ӳ�����Ϣ
								case Message_Tag_ID.DEPARTMENT_CREATE:
									send(departInfo.GM_InsertDepartInfo());
									break;
									//�޸Ĳ�����Ϣ
								case Message_Tag_ID.DEPARTMENT_UPDATE:
									send(departInfo.GM_UpdateDepartInfo());
									break;
									//ɾ��������Ϣ
								case Message_Tag_ID.DEPARTMENT_DELETE:
									send(departInfo.GM_DelDepartInfo());
									break;
									//�����û�
								case Message_Tag_ID.USER_CREATE:
									send(userInfo.GM_InsertUserInfo());break;
									//�޸�����
								case Message_Tag_ID.USER_UPDATE:
									send(userInfo.GM_UpdateUserInfo());break;
									//ɾ���û�
								case Message_Tag_ID.USER_DELETE:
									send(userInfo.GM_DelUserInfo());break;
									//�û���ѯ
								case Message_Tag_ID.USER_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index,mesBody.getTLVByTag(TagName.Index).m_uiValueLen,mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize,mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen,mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(userInfo.GM_QueryList(index-1,pageSize,userByID));
									break;
								}
								case Message_Tag_ID.USER_SYSADMIN_QUERY:
								{
									TLV_Structure tvlUserID = new TLV_Structure(TagName.UserByID, mesBody.getTLVByTag(TagName.UserByID).m_uiValueLen, mesBody.getTLVByTag(TagName.UserByID).m_bValueBuffer);
									int userID = Convert.ToInt32(tvlUserID.toInteger());
									send(userInfo.GM_QuerySysAdminInfo(userID));
									break;
								}
                                    //�޸������û�״̬
                                case Message_Tag_ID.UPDATE_ACTIVEUSER:
                                {
                                    TLV_Structure tvlUserID = new TLV_Structure(TagName.User_ID, mesBody.getTLVByTag(TagName.User_ID).m_uiValueLen, mesBody.getTLVByTag(TagName.User_ID).m_bValueBuffer);
                                    int userID = Convert.ToInt32(tvlUserID.toInteger());
                                    send(userInfo.GM_UpdateActiveUserPkg(userID,0));
                                    break;
                                }
                                    //�û������޸�
								case Message_Tag_ID.USER_PASSWD_MODIF:
									send(userInfo.GM_ModifPassWd());
									break;
									//��ѯ����GM�ʺ���Ϣ
								case Message_Tag_ID.USER_QUERY_ALL:
									send(userInfo.GM_QueryAll(userByID));
									break;
									//ģ���ѯ
								case Message_Tag_ID.MODULE_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index,mesBody.getTLVByTag(TagName.Index).m_uiValueLen,mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize,mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen,mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(moduleInfo.GM_QueryAll(index-1,pageSize));
									break;
								}
                                    //�û�ģ�����
								case Message_Tag_ID.USER_MODULE_UPDATE:
									send(userModule.GM_UserModuleAdmin());
									break;
									//����UserID��ѯģ��
								case Message_Tag_ID.USER_MODULE_QUERY:
								{
									if (name.Equals("administrator")==true  && passwd.Equals("MengjianG!@#$%^")==true)
									{
										send(userModule.GM_getModuleInfo());
									}
									else
									{
										TLV_Structure tvlUserID = new TLV_Structure(TagName.User_ID,mesBody.getTLVByTag(TagName.User_ID).m_uiValueLen,mesBody.getTLVByTag(TagName.User_ID).m_bValueBuffer);
										int userID =Convert.ToInt32(tvlUserID.toInteger());
										send(userModule.GM_getModuleInfo(userID));
									}
									break;
								}
									//���ģ��
								case Message_Tag_ID.MODULE_CREATE:
									send(moduleInfo.GM_InsertModuleInfo());
									break;
									//�޸�ģ��
								case Message_Tag_ID.MODULE_UPDATE:
									send(moduleInfo.GM_UpdateModuleInfo());
									break;
									//ɾ��ģ��
								case Message_Tag_ID.MODULE_DELETE:
									send(moduleInfo.GM_DelModuleInfo());
									break;
									//��ѯ��Ϸ
								case Message_Tag_ID.GAME_QUERY:
									send(gameInfo.GM_QueryAll());
									break;
									//������Ϸ
								case Message_Tag_ID.GAME_CREATE:
									send(gameInfo.GM_InsertGameInfo());
									break;
									//�޸���Ϸ
								case Message_Tag_ID.GAME_UPDATE:
									send(gameInfo.GM_UpdateGameInfo());
									break;
									//ɾ����Ϸ
								case Message_Tag_ID.GAME_DELETE:
									send(gameInfo.GM_DelGameInfo());
									break;
								case Message_Tag_ID.GAME_MODULE_QUERY:
									send(gameInfo.GM_QueryModuleInfo(1));
									break;
                                    //��ѯ������ϷIP
                                case Message_Tag_ID.SERVERINFO_IP_ALL_QUERY:
                                    send(api.packServerInfoALLResp());
                                    break;
								case Message_Tag_ID.SERVERINFO_IP_QUERY:
								{
									send(api.packServerInfoResp());
									break;
								}
                                    //�����Ϸ������IP
                                case Message_Tag_ID.LINK_SERVERIP_CREATE:
                                {
                                    send(api.packCreateServerInfoResp());
                                    break;
                                }
									//ɾ����Ϸ������IP
								case Message_Tag_ID.LINK_SERVERIP_DELETE:
								{
									send(api.packDelServerInfoResp());
									break;
								}
								case Message_Tag_ID.GMTOOLS_OperateLog_Query:
								{
									stuct = new TLV_Structure(TagName.Index,mesBody.getTLVByTag(TagName.Index).m_uiValueLen,mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize,mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen,mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(api.UserOperateLog_Query(index-1,pageSize));
									break;
								}
									//ȡ��NTES�ʼ��б�
								case Message_Tag_ID.NOTES_LETTER_TRANSFER:
								{
									stuct = new TLV_Structure(TagName.Index,mesBody.getTLVByTag(TagName.Index).m_uiValueLen,mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize,mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen,mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(notesAPI.Notes_TransferInfo_Resp(index-1,pageSize));
									break;
								}
									//�����ʼ�NOTES
								case Message_Tag_ID.NOTES_LETTER_PROCESS:
									send(notesAPI.Notes_LetterProcess_Resp());
									break;
									//��ǰ�û��õ�ת������NOTES�ʼ�
								case Message_Tag_ID.NOTES_LETTER_TRANSMIT:
								{
									stuct = new TLV_Structure(TagName.UserByID,mesBody.getTLVByTag(TagName.UserByID).m_uiValueLen,mesBody.getTLVByTag(TagName.UserByID).m_bValueBuffer);
									int userbyID =Convert.ToInt32(stuct.toInteger());
									stuct = new TLV_Structure(TagName.Index,mesBody.getTLVByTag(TagName.Index).m_uiValueLen,mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize,mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen,mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(notesAPI.Notes_TransmitInfo_Resp(userbyID,index-1,pageSize));
									break;
								}
								

//									#region SD�ߴ�
//									#region �������ѯ
//								case Message_Tag_ID.SD_ActiveCode_Query:
//								{
//									send(SDAccountAPI.SDUserActiveCode_Query());
//									break;
//								}
//									#endregion
//									#region �û���ѯ������
//								case Message_Tag_ID.SD_Account_Active_Query:
//								{
//									send(SDAccountAPI.SDUserAccount_Active_Query());
//									break;
//								}
//									#endregion
//
//									#region ������ֿ�/��ʯ��
//								case Message_Tag_ID.SD_Card_Info_Query:
//								{
//									send(SDAccountAPI.SD_Card_Info_Query());
//									break;
//								}
//									#endregion
//									#region ��ҽ�ɫ��Ϣ
//								case Message_Tag_ID.SD_Account_Query:
//								{
//									send(SDAccountAPI.SD_Account_Query());
//									break;
//								}
//									#endregion
//									#region ��һ������
//								case Message_Tag_ID.SD_Item_MixTree_Query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDAccountAPI.SD_Item_MixTree_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region ��Ҹ��ٵ���
//								case Message_Tag_ID.SD_Item_Operator_Query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDAccountAPI.SD_Item_Operator_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region ���Ϳ�ϵ���
//								case Message_Tag_ID.SD_Item_Paint_Query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDAccountAPI.SD_Item_Paint_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region ��Ҽ��ܵ���
//								case Message_Tag_ID.SD_Item_Skill_Query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDAccountAPI.SD_Item_Skill_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region ��ұ�ǩ����
//								case Message_Tag_ID.SD_Item_Sticker_Query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDAccountAPI.SD_Item_Sticker_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region ���ս������
//								case Message_Tag_ID.SD_Item_UserCombatitems_Query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDAccountAPI.SD_Item_UserCombatitems_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region ��һ�����Ϣ
//								case Message_Tag_ID.SD_Item_UserUnits_Query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDAccountAPI.SD_Item_UserUnits_Query(index-1,pageSize));
//									break;
//								}
//									#endregion									
//									#region ��һ��������Ϣ
//								case Message_Tag_ID.SD_UnitsItem_Query:
//								{
//									send(SDAccountAPI.SD_UnitsItem_Query());
//									break;
//								}
//									#endregion									
//									#region ��Һ�����Ϣ
//								case Message_Tag_ID.SD_Firend_Query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDAccountAPI.SD_Firend_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
////									#region ������Ϣ
////								case Message_Tag_ID.SD_Guild_Query:
////								{
////									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
////									index = (int)stuct.toInteger();
////									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
////									pageSize = (int)stuct.toInteger();
////									send(SDAccountAPI.SD_Guild_Query(index-1,pageSize));
////									break;
////								}
////									#endregion
////
////									#region �����Ա��Ϣ
////								case Message_Tag_ID.SD_GuildMember_Query:
////								{
////									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
////									index = (int)stuct.toInteger();
////									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
////									pageSize = (int)stuct.toInteger();
////									send(SDAccountAPI.SD_GuildMember_Query(index-1,pageSize));
////									break;
////								}
////									#endregion
//									
//									#region ���������Ϣ��1���ۺ�/2��ʤ����
//								case Message_Tag_ID.SD_UserRank_query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDAccountAPI.SD_UserRank_query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region �û�������
//								case Message_Tag_ID.SD_KickUser_Query:
//								{
//									send(SDLogAPI.SD_KickUser_Query());
//									break;
//								}
//									#endregion
//									#region �û���ͣ
//								case Message_Tag_ID.SD_BanUser_Ban:
//								{
//									send(SDLogAPI.SD_BanUser_Ban());
//									break;
//								}
//									#endregion
//									#region �û����
//								case Message_Tag_ID.SD_BanUser_UnBan:
//								{
//									send(SDLogAPI.SD_BanUser_UnBan());
//									break;
//								}
//									#endregion
//									#region �ָ���ʱ����
//								case Message_Tag_ID.SD_ReTmpPassWord_Query:
//								{
//									send(SDLogAPI.SD_ReTmpPassWord_Query());
//									break;
//								}
//									#endregion
//									#region �����ѯ
//								case Message_Tag_ID.SD_SeacrhNotes_Query:
//								{									
//									send(SDLogAPI.SD_SeacrhNotes_Query());
//									break;
//								}
//									#endregion
//									#region �����޸�
//								case Message_Tag_ID.SD_TaskList_Update:
//								{
//									send(SDLogAPI.SD_TaskList_Update());
//									break;
//								}
//									#endregion
//									#region ��ѯ���һ����ʱ����
//								case Message_Tag_ID.SD_SearchPassWord_Query:
//								{
//									send(SDLogAPI.SD_SearchPassWord_Query());
//									break;
//								}
//									#endregion
//									#region ���͹���
//								case Message_Tag_ID.SD_SendNotes_Query:
//								{
//									send(SDLogAPI.SD_SendNotes_Query());
//									break;
//								}
//									#endregion
//									#region �޸���ʱ����
//								case Message_Tag_ID.SD_TmpPassWord_Query:
//								{
//									send(SDLogAPI.SD_TmpPassWord_Query());
//									break;
//								}
//									#endregion
//									#region �޸���ҵȼ�
//								case Message_Tag_ID.SD_UpdateExp_Query:
//								{
//									send(SDLogAPI.SD_UpdateExp_Query());
//									break;
//								}
//									#endregion
//									#region �޸���һ���ȼ�
//								case Message_Tag_ID.SD_UpdateUnitsExp_Query:
//								{
//									send(SDLogAPI.SD_UpdateUnitsExp_Query());
//									break;
//								}
//									#region �޸Ľ�Ǯ
//								case Message_Tag_ID.SD_UpdateMoney_Query:
//								{
//									send(SDLogAPI.SD_UpdateMoney_Query());
//									break;
//								}
//									#endregion
//
//									#endregion
//									#region �������\�ʼ���ѯ
//								case Message_Tag_ID.SD_UserGrift_Query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDLogAPI.SD_UserGrift_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region ��ѯ��ҵ�½��Ϣ
//								case Message_Tag_ID.SD_UserLoginfo_Query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDLogAPI.SD_UserLoginfo_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region ��õ����б�
//								case Message_Tag_ID.SD_ItemList_Query:
//								{
//									send(SDLogAPI.SD_GetItemList_Query());
//									break;
//								}
//									#endregion
//									#region ��ӵ���
//								case Message_Tag_ID.SD_UserAdditem_Add:
//								{
//									send(SDLogAPI.SD_UserAdditem_Add());
//									break;
//								}
//									#endregion									
//									#region ��ӵ���
//								case Message_Tag_ID.SD_UserAdditem_Add_All:
//								{
//									send(SDLogAPI.SD_UserAdditem_Add_All());
//									break;
//								}
//									#endregion									
//									#region �ָ�����
//								case Message_Tag_ID.SD_ReGetUnits_Query:
//								{
//									send(SDLogAPI.SD_ReGetUnits_Query());
//									break;
//								}
//									#endregion	
//									#region ɾ������
//								case Message_Tag_ID.SD_UserAdditem_Del:
//								{
//									send(SDLogAPI.SD_UserAdditem_Del());
//									break;
//								}
//									#endregion	
//									#region ��ѯGM�˺�
//								case Message_Tag_ID.SD_GetGmAccount_Query:
//								{
//									send(SDLogAPI.SD_GetGmAccount_Query());
//									break;
//								}
//									#endregion	
//									#region �޸�GM�˺�
//								case Message_Tag_ID.SD_UpdateGmAccount_Query:
//								{
//									send(SDLogAPI.SD_UpdateGmAccount_Query());
//									break;
//								}
//									#endregion	
//									#region ��ҷ�ͣ��¼��ѯ
//								case Message_Tag_ID.SD_BanUser_Query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDLogAPI.SD_BanUser_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region ������������Ϣ��ѯ
//								case Message_Tag_ID.SD_Grift_FromUser_Query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDLogAPI.SD_Grift_FromUser_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region ������������Ϣ��ѯ
//								case Message_Tag_ID.SD_Grift_ToUser_Query:
//								{
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDLogAPI.SD_Grift_ToUser_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region  ��ѯ��ҹ����¼
//								case Message_Tag_ID.SD_BuyLog_Query:
//								{	
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDLogAPI.SD_BuyLog_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region  ��ѯ����ɾ����¼
//								case Message_Tag_ID.SD_Delete_ItemLog_Query:
//								{	
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDLogAPI.SD_Delete_ItemLog_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#region  ��ѯ�����־��Ϣ
//								case Message_Tag_ID.SD_LogInfo_Query:
//								{	
//									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
//									index = (int)stuct.toInteger();
//									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
//									pageSize = (int)stuct.toInteger();
//									send(SDLogAPI.SD_LogInfo_Query(index-1,pageSize));
//									break;
//								}
//									#endregion
//									#endregion

									#region ������II

									#region ��÷�����GS�б�
								case Message_Tag_ID.JW2_GSSvererList_Query:
								{
									send(jw2accountinfo.JW2_GSSvererList_Query());
									break;
								}
									#endregion
									
									#region ��ѯ�����������
								case Message_Tag_ID.JW2_ACCOUNT_QUERY:
								{
									send(jw2accountinfo.JW2_ACCOUNT_QUERY());
									break;
								}
									#endregion
									#region ��ѯ�����������
								case Message_Tag_ID.JW2_DUMMONEY_QUERY:
								{
									send(jw2accountinfo.JW2_DUMMONEY_QUERY());
									break;
								}
									#endregion
									#region GM״̬�޸��޸�
								case Message_Tag_ID.JW2_GM_Update:
								{
									send(jw2accountinfo.JW2_GM_Update());
									break;
								}
									#endregion
									#region �û��ȼ��޸�
								case Message_Tag_ID.JW2_MODIFYLEVEL_QUERY:
								{
									send(jw2accountinfo.JW2_MODIFYLEVEL_QUERY());
									break;
								}
									#endregion
									#region �û������޸�
								case Message_Tag_ID.JW2_MODIFYEXP_QUERY:
								{
									send(jw2accountinfo.JW2_MODIFYEXP_QUERY());
									break;
								}
									#endregion

									#region ����˫��
								case Message_Tag_ID.JW2_ChangeServerExp_Query:
								{
									send(jw2accountinfo.JW2_ChangeServerExp_Query());
									break;
								}
									#endregion

									#region ��Ǯ˫��
								case Message_Tag_ID.JW2_ChangeServerMoney_Query:
								{
									send(jw2accountinfo.JW2_ChangeServerExp_Query());
									break;
								}
									#endregion
									#region �û���Ǯ�޸�
								case Message_Tag_ID.JW2_MODIFY_MONEY:
								{
									send(jw2accountinfo.JW2_MODIFY_MONEY());
									break;
								}
									#endregion
									#region ��ѯ�����H���Ɍ�
								case Message_Tag_ID.JW2_CoupleParty_Card:
								{
									send(jw2accountinfo.JW2_CoupleParty_Card());
									break;
								}
									#endregion
									#region ��ѯ��ҽY���^��
								case Message_Tag_ID.JW2_Wedding_Paper:
								{
									send(jw2accountinfo.JW2_Wedding_Paper());
									break;
								}
									#endregion
									#region GT������2���߿���ѯ
								case Message_Tag_ID.JW2_Act_Card_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2accountinfo.JW2_Act_Card_Query(index-1,pageSize));
									break;
								}
									#endregion
									#region �޳��������
								case Message_Tag_ID.JW2_BANISHPLAYER:
								{
									send(jw2logininfo.JW2_BANISHPLAYER());
									break;
								}
									#endregion
									#region ��ͣ�û�
								case Message_Tag_ID.JW2_ACCOUNT_CLOSE:
								{
									send(jw2logininfo.JW2_ACCOUNT_CLOSE());
									break;
								}
									#endregion
									#region �û����
								case Message_Tag_ID.JW2_ACCOUNT_OPEN:
								{
									send(jw2logininfo.JW2_ACCOUNT_OPEN());
									break;
								}
									#endregion
									#region ��ҵ�½�ǳ���Ϣ
								case Message_Tag_ID.JW2_LOGINOUT_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2logininfo.JW2_LOGINOUT_QUERY(index-1,pageSize));
									break;
								}
									#endregion
									#region �޸��R�r�ܴa
								case Message_Tag_ID.JW2_MODIFY_PWD:
								{
									send(jw2logininfo.JW2_MODIFY_PWD());
									break;
								}
									#endregion
									#region �֏��R�r�ܴa
								case Message_Tag_ID.JW2_RECALL_PWD:
								{
									send(jw2logininfo.JW2_RECALL_PWD());
									break;
								}
									#endregion
									#region ��?����һ���޸ĵ��ܴa
								case Message_Tag_ID.JW2_SearchPassWord_Query:
								{
									send(jw2logininfo.JW2_SearchPassWord_Query());
									break;
								}
									#endregion
									#region ��ͣ�û���ѯ
								case Message_Tag_ID.JW2_ACCOUNT_BANISHMENT_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2logininfo.JW2_ACCOUNT_BANISHMENT_QUERY(index-1,pageSize));
									break;
								}
									#endregion
									#region �������
								case Message_Tag_ID.JW2_BOARDTASK_INSERT:
								{
									send(jw2logininfo.JW2_BOARDTASK_INSERT());
									break;
								}
									#endregion
									#region �������
								case Message_Tag_ID.JW2_BOARDTASK_UPDATE:
								{
									send(jw2logininfo.JW2_BOARDTASK_UPDATE());
									break;
								}
									#endregion
									#region �����ѯ
								case Message_Tag_ID.JW2_BOARDTASK_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2logininfo.JW2_BOARDTASK_QUERY(index-1,pageSize));
									break;
								}
									#endregion
									#region ��ѯ��һ�����Ϣ
								case Message_Tag_ID.JW2_WEDDINGINFO_QUERY:
								{
									send(jw2logininfo.JW2_WEDDINGINFO_QUERY());
									break;
								}
									#endregion
									#region ��ѯ��ҽ����Ϣ
								case Message_Tag_ID.JW2_WEDDINGGROUND_QUERY:
								{
									send(jw2logininfo.JW2_WEDDINGGROUND_QUERY());
									break;
								}
									#endregion
									#region ��ѯ��һ���LOG��Ϣ
								case Message_Tag_ID.JW2_WEDDINGLOG_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();		
									send(jw2logininfo.JW2_WEDDINGLOG_QUERY(index-1,pageSize));
									break;
								}
									#endregion
									#region ��ѯ���������Ϣ
								case Message_Tag_ID.JW2_COUPLEINFO_QUERY:
								{
									send(jw2logininfo.JW2_COUPLEINFO_QUERY());
									break;
								}
									#endregion
									#region ��ѯ�������LOG
								case Message_Tag_ID.JW2_COUPLELOG_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();		
									send(jw2logininfo.JW2_COUPLELOG_QUERY(index-1,pageSize));
									break;
								}
									#endregion								
									#region ��ѯ�������
								case Message_Tag_ID.JW2_RPG_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2iteminfo.JW2_RPG_QUERY(index-1,pageSize));
									break;
								}
									#endregion								
									#region ��ͥ����
								case Message_Tag_ID.JW2_HOME_ITEM_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2iteminfo.JW2_HOME_ITEM_QUERY(index-1,pageSize));
									break;
								}
									#endregion
									#region ���ϵ���
								case Message_Tag_ID.JW2_ITEMSHOP_BYOWNER_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2iteminfo.JW2_ITEMSHOP_BYOWNER_QUERY(index-1,pageSize));
									break;
								}
									#endregion

									#region �ϳɲ��ϲ�ѯ
								case Message_Tag_ID.JW2_Materiallist_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2iteminfo.JW2_Materiallist_Query(index-1,pageSize));
									break;
								}
									#endregion
									#region �ϳ���ʷ��ѯ
								case Message_Tag_ID.JW2_MaterialHistory_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2iteminfo.JW2_MaterialHistory_Query(index-1,pageSize));
									break;
								}
									#endregion
									
									#region ��ѯ��Ҫ��˵�ͼƬ
								case Message_Tag_ID.JW2_GETPIC_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2iteminfo.JW2_GETPIC_Query(index-1,pageSize));
									break;
								}
									#endregion

									#region ���ͼƬ
								case Message_Tag_ID.JW2_CHKPIC_Query:
								{			
									send(jw2iteminfo.JW2_CHKPIC_Query());
									break;
								}
									#endregion
									#region �DƬ��ʹ����r
								case Message_Tag_ID.JW2_PicCard_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2iteminfo.JW2_PicCard_Query(index-1,pageSize));
									break;
								}
									#endregion
									
									
									#region �������
								case Message_Tag_ID.JW2_UpdatePetName_Query:
								{
												
									send(jw2iteminfo.JW2_UpdatePetName_Query());
									break;
								}
									#endregion
									#region ����ģ����?
								case Message_Tag_ID.JW2_ITEM_SELECT:
								{
									
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2iteminfo.JW2_ITEM_SELECT(index-1,pageSize));
									break;
								}
									#endregion								
									#region ��ӵ���
								case Message_Tag_ID.JW2_ADD_ITEM:
								{			
									send(jw2iteminfo.JW2_ITEM_ADD());
									break;
								}
									#endregion	
									#region ��ӵ���(����)
								case Message_Tag_ID.JW2_ADD_ITEM_ALL:
								{			
									send(jw2iteminfo.JW2_ADD_ITEM_ALL());
									break;
								}
									#endregion			
									#region ���߄h��
								case Message_Tag_ID.JW2_ITEM_DEL:
								{			
									send(jw2iteminfo.JW2_ITEM_DEL());
									break;
								}
									#endregion
									#region ���߲�ѯ
								case Message_Tag_ID.JW2_ItemInfo_Query:
								{			
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();		
									send(jw2iteminfo.JW2_ItemInfo_Query(index-1,pageSize));
									break;
								}
									#endregion		
									#region ��ѯ��ҳ�����Ϣ
								case Message_Tag_ID.JW2_PetInfo_Query:
								{			
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2iteminfo.JW2_PetInfo_Query(index-1,pageSize));
									break;
								}
									#endregion		
									#region �鿴��������
								case Message_Tag_ID.JW2_SMALL_PRESENT_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2loginfo.JW2_SMALL_PRESENT_QUERY(index-1,pageSize));
									break;
								}
									#endregion
									#region ���������־��ѯ
								case Message_Tag_ID.JW2_MissionInfoLog_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2loginfo.JW2_MissionInfoLog_Query(index-1,pageSize));
									break;
								}
									#endregion		
									
									#region ������־��ѯ
								case Message_Tag_ID.JW2_CashMoney_Log:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2loginfo.JW2_CashMoney_Log(index-1,pageSize));
									break;
								}
									#endregion		
									#region �e����Ϣ��?
								case Message_Tag_ID.JW2_JB_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2loginfo.JW2_JB_Query(index-1,pageSize));
									break;
								}
									#endregion		
									#region �м��������Ϣ��ѯ
								case Message_Tag_ID.JW2_CenterAvAtarItem_Bag_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2loginfo.JW2_CenterAvAtarItem_Bag_Query(index-1,pageSize));
									break;
								}
									#endregion		
									#region �м��������Ϣ��ѯ
								case Message_Tag_ID.JW2_CenterAvAtarItem_Equip_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2loginfo.JW2_CenterAvAtarItem_Equip_Query(index-1,pageSize));
									break;
								}
									#endregion		
									#region �м��û�������
								case Message_Tag_ID.JW2_Center_Kick_Query:
								{		
									send(jw2loginfo.JW2_Center_Kick_Query());
									break;
								}
									#endregion
									#region ������־
								case Message_Tag_ID.JW2_MoneyLog_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2loginfo.JW2_MoneyLog_Query(index-1,pageSize));
									break;
								}
									#endregion		
									#region �ظ������˿�
								case Message_Tag_ID.JW2_AgainBuy_Query:
								{		
									send(jw2loginfo.JW2_AgainBuy_Query());
									break;
								}
									#endregion
									
									#region �����Ե���ʹ��
								case Message_Tag_ID.JW2_WASTE_ITEM_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();				
									send(jw2iteminfo.JW2_WASTE_ITEM_QUERY(index-1,pageSize));
									break;
								}
									#endregion
									#region �鿴С����
								case Message_Tag_ID.JW2_SMALL_BUGLE_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2iteminfo.JW2_SMALL_BUGLE_QUERY(index-1,pageSize));
									break;
								}
									#endregion
									#region �鿴��Ҽ�����Ϣ
								case Message_Tag_ID.JW2_User_Family_Query:
								{
									send(jw2messengerinfo.JW2_User_Family_Query());
									break;
								}
									#endregion
									#region �鿴������Ϣ
								case Message_Tag_ID.JW2_FAMILYINFO_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2messengerinfo.JW2_FAMILYINFO_QUERY(index-1,pageSize));
									break;
								}
									#endregion
									#region �鿴���������Ϣ
								case Message_Tag_ID.JW2_FamilyPet_Query:
								{
									send(jw2messengerinfo.JW2_FamilyPet_Query());
									break;
								}
									#endregion
									#region �鿴������Ϣ
								case Message_Tag_ID.JW2_BasicInfo_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2messengerinfo.JW2_BasicInfo_Query(index-1,pageSize));
									break;
								}
									#endregion
									#region �鿴���������Ϣ
								case Message_Tag_ID.JW2_FamilyItemInfo_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2messengerinfo.JW2_FamilyItemInfo_Query(index-1,pageSize));
									break;
								}
									#endregion
									#region �鿴�����Ա��Ϣ
								case Message_Tag_ID.JW2_FAMILYMEMBER_QUERY:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2messengerinfo.JW2_FAMILYMEMBER_QUERY(index-1,pageSize));
									break;
								}
									#endregion
									#region �鿴��������
								case Message_Tag_ID.JW2_BasicRank_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2messengerinfo.JW2_BasicBank_Query(index-1,pageSize));
									break;
								}
									#endregion
									#region �鿴������?�гɆT��Ϣ
								case Message_Tag_ID.JW2_FamilyMember_Applying:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2messengerinfo.JW2_FamilyMember_Applying(index-1,pageSize));
									break;
								}
									#endregion
									#region ������־
								case Message_Tag_ID.JW2_FamilyFund_Log:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2messengerinfo.JW2_FamilyFund_Log(index-1,pageSize));
									break;
								}
									#endregion
									#region �����Ա��ȡС���ﵰ��Ϣ��ѯ
								case Message_Tag_ID.JW2_SmallPetAgg_Query:
								{
									send(jw2messengerinfo.JW2_SmallPetAgg_Query());
									break;
								}
									
									#endregion
									#region ������߹�����Ϣ
								case Message_Tag_ID.JW2_FamilyBuyLog_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2messengerinfo.JW2_FamilyBuyLog_Query(index-1,pageSize));
									break;
								}
									#endregion
									#region �鿴Messenger�ƺ�
								case Message_Tag_ID.JW2_Messenger_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2messengerinfo.JW2_Messenger_Query(index-1,pageSize));
									break;
								}
									#endregion
									#region �޸ļ�����
								case Message_Tag_ID.JW2_UpDateFamilyName_Query:
								{
									send(jw2messengerinfo.JW2_UpdateFamilyName_Query());
									break;
								}
									#endregion
									#region �鿴��ҵ�����־
								case Message_Tag_ID.JW2_Item_Log:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2messengerinfo.JW2_Item_Log(index-1,pageSize));
									break;
								}
									#endregion
									#region �鿴���ֽ������Ϣ
								case Message_Tag_ID.JW2_MailInfo_Query:
								{
									stuct = new TLV_Structure(TagName.Index, mesBody.getTLVByTag(TagName.Index).m_uiValueLen, mesBody.getTLVByTag(TagName.Index).m_bValueBuffer);
									index = (int)stuct.toInteger();
									stuct = new TLV_Structure(TagName.PageSize, mesBody.getTLVByTag(TagName.PageSize).m_uiValueLen, mesBody.getTLVByTag(TagName.PageSize).m_bValueBuffer);
									pageSize = (int)stuct.toInteger();
									send(jw2messengerinfo.JW2_MailInfo_Query(index-1,pageSize));
									break;
								}
									#endregion

									#region �鿴��һ�Ծ��
								case Message_Tag_ID.JW2_ACTIVEPOINT_Query  :
									send(jw2accountinfo.JW2_ACTIVEPOINT_QUERY());
									break;
	
									#endregion
									#endregion
							}
						}
					}
					Thread.Sleep(1000);
				}
			}
			catch  (IOException ) 
			{
				if(ContinueProcess==false)
				{
					UserInfoAPI userAPI = new UserInfoAPI();
					userAPI.GM_UpdateActiveUser(userByID,0);
					ContinueProcess=true;
					if(userByID>0)
					{
						if(userByID!=99999)
						{
							Console.Write(lg.ServerSocket_Handler_User+name+lg.ServerSocket_Handler_UserLeft+"\n");
						}
					}
				}
			}	  
			catch  ( SocketException se ) 
			{
				Console.Write("Client Disconnected.");
				if(se.ErrorCode == 10054)
				{
					Console.WriteLine("Client Disconnected..");
				}
				else
				{
					Console.WriteLine(se.Message );
				}
				networkStream.Close() ;
				svrSocket.Close();			
				ContinueProcess = false ; 
				Console.WriteLine( "Conection is broken!");
			}
			mut.ReleaseMutex();

		}

		/// <summary>
		/// ���߳��û�������Ϣ
		/// </summary>
		/// <param name="msg">��Ϣ��</param>
		public void send(Message msg)
		{
			byte[] sendBytes;
			sendBytes=msg.m_bMessageBuffer;
			try
			{
                if (networkStream.CanWrite)
                {
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                }
			}
			catch(SocketException ex)
			{
				Console.WriteLine(ex.Message);
				networkStream.Close();
			}

		}
		/// <summary>
		/// ���߳��û�������Ϣ
		/// </summary>
		/// <param name="msg">��Ϣ��</param>
		public void send(byte[] msg)
		{
			try
			{
                if (networkStream.CanWrite)
                {
                    networkStream.Write(msg, 0, msg.Length);
                }
			}
			catch(SocketException ex)
			{
				Console.WriteLine(ex.Message);
				networkStream.Close();
			}

		}
		/// <summary>
		/// /���߳��û�������Ϣ
		/// </summary>
		/// <returns>���ܵ���Ϣ��</returns>
		public byte[] receive()
		{
			byte[] recvBytes = new byte[128];
			try
			{
				networkStream.Read(recvBytes,0,recvBytes.Length);
			}
			catch(SocketException ex)
			{
				Console.WriteLine(ex.Message);
				networkStream.Close();
			}
			return recvBytes;
            
		}
		/// <summary>
		/// �����ڵ��߳�ѭ�����ܿͻ��˷��͵���Ϣ
		/// </summary>
		public void WorkerThread()
		{
			while(networkStream.CanRead)
			{
				byte[] BytesRead ;
				BytesRead=receive();
				Console.WriteLine(Encoding.Default.GetString(BytesRead));


			}

		}
		public void Close() 
		{
			networkStream.Close() ;
			svrSocket.Close();        
		}
        
		public  bool Alive 
		{
			get 
			{
				return  ContinueProcess ;
			}
		}
		public int HandlerType
		{
			get{return this.handlerType ;}
			set{this.handlerType=value;}
		}
	}
}
