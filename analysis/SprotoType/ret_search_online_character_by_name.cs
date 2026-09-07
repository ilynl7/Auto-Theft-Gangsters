using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_search_online_character_by_name
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private List<friend_info> _friend_list;

		public List<friend_info> friend_list
		{
			get
			{
				return _friend_list;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_friend_list = value;
			}
		}

		public bool HasFriend_list => has_field.has_field(0);

		public request()
			: base(max_field_count)
		{
		}

		public request(byte[] buffer)
			: base(max_field_count, buffer)
		{
			decode();
		}

		protected override void decode()
		{
			int num = -1;
			while ((num = deserialize.read_tag()) != -1)
			{
				if (num == 0)
				{
					friend_list = deserialize.read_obj_list<friend_info>();
				}
				else
				{
					deserialize.read_unknow_data();
				}
			}
		}

		public override int encode(SprotoStream stream)
		{
			serialize.open(stream);
			if (has_field.has_field(0))
			{
				serialize.write_obj(friend_list, 0);
			}
			return serialize.close();
		}
	}
}
