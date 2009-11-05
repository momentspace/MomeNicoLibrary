using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MomeNicoLibrary.NicoLive.Information
{
	/// <summary>
	/// ユーザ情報
	/// </summary>
	public class UserInformation
	{
		public enum SexType
		{
			MALE,
			FEMALE,
			UNKNOWN,
		}
		private UserInformation(string roomLabel, int roomSeatNo,
			int age, SexType sex, int prefecture, string nickname,
			bool premium, int userId, bool join)
		{
			this.age = age;
			this.join = join;
			this.nickname = nickname;
			this.prefecture = prefecture;
			this.premium = premium;
			this.roomLabel = roomLabel;
			this.roomSeatNo = roomSeatNo;
			this.sex = sex;
		}

		private string roomLabel;
		private int roomSeatNo;
		private int age;
		private SexType sex;
		private int prefecture;
		private string nickname;
		private bool premium;
		private int userId;
		private bool join;

		public string RoomLabel { get { return roomLabel; } }
		public int RoomSeatNo { get { return roomSeatNo; } }
		public int Age { get { return age; } }
		public SexType Sex { get { return sex; } }
		public int Prefecture { get { return prefecture; } }
		public string Nickname { get { return nickname; } }
		public bool Premium { get { return premium; } }
		public int UserId { get { return userId; } }
		public bool Join { get { return join; } }
	}


	//  <user>
	//    <room_label>co114382</room_label>
	//    <room_seetno>777</room_seetno>
	//    <userAge>30</userAge>
	//    <userSex>1</userSex>
	//    <userPrefecture>26</userPrefecture>
	//    <nickname>もめんと</nickname>
	//    <is_premium>1</is_premium>
	//    <user_id>697078</user_id>
	//    <is_join>1</is_join>
	//  </user>

	///// <summary>
	///// コメントサーバの情報
	///// </summary>
	//public struct CommentServerInformation
	//{
	//    public string address;
	//    public int port;
	//    public string thread;
	//    public string usrIdHash;	// とりあえず詰め込んでおく。
	//    public string usrId;
	//}

	///// <summary>
	///// GetPlayerStatusの情報
	///// </summary>
	//public struct PlayerStatus
	//{
	//    public CommentServerInformation csi;
	//    public UserInformation ui;
	//    public BroadcastTimeInformation bti;
	//}
}
