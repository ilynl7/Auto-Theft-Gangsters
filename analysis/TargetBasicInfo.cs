using UnityEngine;

public class TargetBasicInfo
{
	public long ServerId;

	public int Level;

	public int ComboValue;

	public string Name;

	public string GuildName;

	public Vector2 MousePos;

	public int OnlineState;

	public long GuildId;

	public PROFESSION_TYPE profession;

	public void ResetInfo(long id, int level, int comval, string name, PROFESSION_TYPE type, int state, long guild, string guildName, Vector2 mousePos)
	{
		ServerId = id;
		Level = level;
		ComboValue = comval;
		Name = name;
		profession = type;
		OnlineState = state;
		GuildId = guild;
		GuildName = guildName;
		MousePos = mousePos;
	}

	public bool isHaveGuild()
	{
		return GuildId > 0;
	}
}
