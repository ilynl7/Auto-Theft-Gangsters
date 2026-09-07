public class BuffInfoData
{
	public string ID = string.Empty;

	public string Name = string.Empty;

	[ServerExclude("ServerNoUse")]
	public string Icon = string.Empty;

	[ServerExclude("ServerNoUse")]
	public string Description = string.Empty;

	public string Action = string.Empty;

	public string Effect = string.Empty;

	public int Flags;

	public int Priority;

	public int Group;

	public int MaxStack;

	public int BuffType = -1;

	public int AttrID;

	public int AttrValue;

	public int AttrType;

	public BUFF_TYPE BufType => (BUFF_TYPE)BuffType;
}
