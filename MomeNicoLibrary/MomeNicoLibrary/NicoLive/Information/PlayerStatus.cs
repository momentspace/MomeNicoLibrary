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
		//<stream>
		//  <id>lv5237911</id>
		//  <watch_count>33</watch_count>
		//  <comment_count>41</comment_count>
		//  <danjo_comment_mode>0</danjo_comment_mode>
		//  <relay_comment>0</relay_comment>
		//  <bourbon_url/>
		//  <full_video/>
		//  <after_video/>
		//  <before_video/>
		//  <kickout_video/>
		//  <header_comment>0</header_comment>
		//  <footer_comment>0</footer_comment>
		//  <provider_type>community</provider_type>
		//  <default_community>co114382</default_community>
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
			requests.Add(new XmlRequest(ADDRESS));
			requests.Add(new XmlRequest(PORT));
			requests.Add(new XmlRequest(THREAD));
		}
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
