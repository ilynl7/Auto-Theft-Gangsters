using Sproto;

namespace SprotoType;

public class syn_friend_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private friend_info _friend;

		public friend_info friend
		{
			get
			{
				return _friend;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_friend = value;
			}
		}

		public bool HasFriend => has_field.has_field(0);

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
					friend = deserialize.read_obj<friend_info>();
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
				serialize.write_obj(friend, 0);
			}
			return serialize.close();
		}
	}
}
