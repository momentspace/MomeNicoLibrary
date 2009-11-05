using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MomeNicoLibrary.Utility;

namespace MomeNicoLibrary.NicoLive.Information
{
	public class PlayerStatusItems : XPathItems
	{
		//<?xml version="1.0" encoding="utf-8"?>
		//<getplayerstatus status="ok" time="1255366238">
		//  <stream>
		//    <id>lv5237911</id>
		//    <watch_count>33</watch_count>
		//    <comment_count>41</comment_count>
		//    <danjo_comment_mode>0</danjo_comment_mode>
		//    <relay_comment>0</relay_comment>
		//    <bourbon_url/>
		//    <full_video/>
		//    <after_video/>
		//    <before_video/>
		//    <kickout_video/>
		//    <header_comment>0</header_comment>
		//    <footer_comment>0</footer_comment>
		//    <provider_type>community</provider_type>
		//    <default_community>co114382</default_community>
		//    <archive>0</archive>
		//    <is_dj_stream>0</is_dj_stream>
		//    <base_time>1255364708</base_time>
		//    <open_time>1255364708</open_time>
		//    <start_time>1255364708</start_time>
		//    <telop>
		//      <enable>0</enable>
		//    </telop>
		//    <ichiba_notice_enable>0</ichiba_notice_enable>
		//    <comment_lock>0</comment_lock>
		//    <background_comment>0</background_comment>
		//    <contents_list>
		//      <contents id="main" disableAudio="0" disableVideo="0" start_time="1255364711">rtmp:liverepeater:rtmp://nlpoca05.live.nicovideo.jp:1935/publicorigin/lv5237911</contents>
		//    </contents_list>
		//  </stream>
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
		//  <rtmp>
		//    <url>rtmp://nleta03.live.nicovideo.jp:1935/liveedge</url>
		//    <ticket>697078:lv5237911:4:1255366238:c0cbd3373cda4d8e</ticket>
		//  </rtmp>
		//  <ms>
		//    <addr>msg104.live.nicovideo.jp</addr>
		//    <port>2806</port>
		//    <thread>1006605973</thread>
		//  </ms>
		//</getplayerstatus>

		public PlayerStatusItems()
		{
			status = "getplayerstatus/@status";

			// 放送情報
			requests.Add(new XmlRequest(ID));
			requests.Add(new XmlRequest(WATCH_COUNT));
			requests.Add(new XmlRequest(COMMENT_COUNT));
			requests.Add(new XmlRequest(DANJO_COMMENT_MODE));
			requests.Add(new XmlRequest(RELAY_COMMENT));
			requests.Add(new XmlRequest(BASE_TIME));
			requests.Add(new XmlRequest(OPEN_TIME));
			requests.Add(new XmlRequest(START_TIME));
			requests.Add(new XmlRequest(DEFAULT_COMMUNITY));
			requests.Add(new XmlRequest(ARCHIVE));

			// ユーザ情報
			requests.Add(new XmlRequest(ROOM_LABEL));
			requests.Add(new XmlRequest(ROOM_SEETNO));
			requests.Add(new XmlRequest(USER_AGE));
			requests.Add(new XmlRequest(USER_SEX));
			requests.Add(new XmlRequest(USER_PREFECTURE));
			requests.Add(new XmlRequest(NICKNAME));
			requests.Add(new XmlRequest(IS_PREMIUM));
			requests.Add(new XmlRequest(USER_ID));
			requests.Add(new XmlRequest(IS_JOIN));

			// コメントサーバ
			requests.Add(new XmlRequest(ADDRESS));
			requests.Add(new XmlRequest(PORT));
			requests.Add(new XmlRequest(THREAD));
		}

		// 放送情報
		public const string ID = "getplayerstatus/stream/id";
		public const string WATCH_COUNT = "getplayerstatus/stream/watch_count";
		public const string COMMENT_COUNT = "getplayerstatus/stream/comment_count";
		public const string DANJO_COMMENT_MODE = "getplayerstatus/stream/danjo_comment_mode";
		public const string RELAY_COMMENT = "getplayerstatus/stream/relay_comment";
		public const string BASE_TIME = "getplayerstatus/stream/base_time";
		public const string OPEN_TIME = "getplayerstatus/stream/open_time";
		public const string START_TIME = "getplayerstatus/stream/start_time";
		public const string DEFAULT_COMMUNITY = "getplayerstatus/stream/default_community";
		public const string ARCHIVE = "getplayerstatus/stream/archive";
		//    <bourbon_url/>
		//    <full_video/>
		//    <after_video/>
		//    <before_video/>
		//    <kickout_video/>
		//    <is_dj_stream>0</is_dj_stream>
		//    <telop>
		//      <enable>0</enable>
		//    </telop>
		//    <ichiba_notice_enable>0</ichiba_notice_enable>
		//    <comment_lock>0</comment_lock>
		//    <background_comment>0</background_comment>
		//    <contents_list>
		//      <contents id="main" disableAudio="0" disableVideo="0" start_time="1255364711">rtmp:liverepeater:rtmp://nlpoca05.live.nicovideo.jp:1935/publicorigin/lv5237911</contents>
		//    </contents_list>

		// ユーザ情報
		public const string ROOM_LABEL = "getplayerstatus/user/room_label";
		public const string ROOM_SEETNO = "getplayerstatus/user/room_seetno";
		public const string USER_AGE = "getplayerstatus/user/userAge";
		public const string USER_SEX = "getplayerstatus/user/userSex";
		public const string USER_PREFECTURE = "getplayerstatus/user/userPrefecture";
		public const string NICKNAME = "getplayerstatus/user/nickname";
		public const string IS_PREMIUM = "getplayerstatus/user/is_premium";
		public const string USER_ID = "getplayerstatus/user/user_id";
		public const string IS_JOIN = "getplayerstatus/user/is_join";

		// コメントサーバ
		public const string ADDRESS = "getplayerstatus/ms/addr";
		public const string PORT = "getplayerstatus/ms/port";
		public const string THREAD = "getplayerstatus/ms/thread";
	}

	public class PlayerStatus : NicoInformation
	{
		private PlayerStatus(Dictionary<string, string> dict)
		{
			this.dict = dict;
		}

		public static PlayerStatus Parse(string message)
		{
			PlayerStatus info = new PlayerStatus(
				XmlParse(message, new PlayerStatusItems()));
			return info;
		}

		public string UserId
		{
			get
			{
				return dict[PlayerStatusItems.USER_ID];
			}
		}

		//時間情報だけ扱えるクラスを用意する
		//public BroadcastInformation BroadcastInfo
		//{
		//    get
		//    {
		//        int baseTime;
		//        int openTime;
		//        int startTime;
		//        int.TryParse(dict[PlayerStatusItems.BASE_TIME], out baseTime);
		//        int.TryParse(dict[PlayerStatusItems.OPEN_TIME], out openTime);
		//        int.TryParse(dict[PlayerStatusItems.START_TIME], out startTime);
		//        BroadcastInformation info =
		//            new BroadcastInformation(baseTime, openTime, startTime);
		//        return info;
		//    }
		//}

		public ServerInformation ServerInfo
		{
			get
			{
				string address = dict[PlayerStatusItems.ADDRESS];
				int port;
				int thread;
				int.TryParse(dict[PlayerStatusItems.THREAD], out port);
				int.TryParse(dict[PlayerStatusItems.PORT], out thread);
				ServerInformation info =
					new ServerInformation(address, port, thread);
				return info;
			}
		}
	}
}
