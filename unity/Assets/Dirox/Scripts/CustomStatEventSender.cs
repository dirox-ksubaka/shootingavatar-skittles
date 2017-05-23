using UnityEngine;
using System.Collections;
using ksubaka.activity;
using ksubaka.core.events;
using ksubaka.core.services;

public class CustomStatEventSender
{
	public static string STATUS_FAIL = "fail";
	public static string STATUS_PASS = "pass";
	public static string AVATAR_CREATED_ACTION_NAME = "action_avatar_created";
	public static string AVATARS_FED_ACTION_NAME = "action_avatar_feed";


	/// <summary>
	/// avatarsFed number value based on amount of avatars successfully fed a skittle (aka ‘Score’)
	/// </summary>
	/// <param name="action">Action.</param>
	/// <param name="value">Value.</param>
	public static void SendAvatarsFed( int value){
		if(value > -1){
			CustomAvatarsFed eventSent = new CustomAvatarsFed(AVATARS_FED_ACTION_NAME, value);
			ServiceRepository.GetService<EventBus>().Publish(eventSent);
		}	
	}
	/// <summary>
	/// avatarCreated fail or pass based on whether player created an avatar during the session.
	/// </summary>
	/// <param name="action">Action.</param>
	/// <param name="status">Status. CustomStatEventSender.STATUS_FAIL or CustomStatEventSender.STATUS_FAIL</param>
	public static void SendAvatarCreated( string status){
		if(!string.IsNullOrEmpty(status)){
			CustomEventAvatarCreated eventSent = new CustomEventAvatarCreated(AVATAR_CREATED_ACTION_NAME, status);
			ServiceRepository.GetService<EventBus>().Publish(eventSent);
		}
	}


	public class CustomAvatarsFed : GameExperienceEvent{
		public int avatarsFed;
		public CustomAvatarsFed(string action, int avatarsFed): base(action){
			this.avatarsFed = avatarsFed;
		}
	}

	public class CustomEventAvatarCreated : GameExperienceEvent{
		public string avatarCreated;
		public CustomEventAvatarCreated(string action, string avatarCreated) : base(action) {
			this.avatarCreated = avatarCreated;
		}
	}
}
