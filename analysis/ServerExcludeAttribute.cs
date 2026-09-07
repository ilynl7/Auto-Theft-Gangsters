using System;

public class ServerExcludeAttribute : Attribute
{
	public string Msg { get; set; }

	public ServerExcludeAttribute(string msg)
	{
		Msg = msg;
	}
}
