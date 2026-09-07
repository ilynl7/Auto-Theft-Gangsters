using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_request_update_friend_useinfo
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private Dictionary<long, friend_info> _friend_list;

		private long _type;

		public Dictionary<long, friend_info> friend_list
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

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(1);

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
				switch (num)
				{
				case 0:
					friend_list = deserialize.read_map((friend_info v) => v.friendId);
					break;
				case 1:
					type = deserialize.read_integer();
					break;
				default:
					deserialize.read_unknow_data();
					break;
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
			if (has_field.has_field(1))
			{
				serialize.write_integer(type, 1);
			}
			return serialize.close();
		}
	}
}
