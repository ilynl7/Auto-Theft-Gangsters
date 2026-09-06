using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class update_game_server
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count;

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
				int num2 = num;
				deserialize.read_unknow_data();
			}
		}

		public override int encode(SprotoStream stream)
		{
			serialize.open(stream);
			return serialize.close();
		}
	}

	public class response : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private List<game_server> _game_server;

		public List<game_server> game_server
		{
			get
			{
				return _game_server;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_game_server = value;
			}
		}

		public bool HasGame_server => has_field.has_field(0);

		public response()
			: base(max_field_count)
		{
		}

		public response(byte[] buffer)
			: base(max_field_count, buffer)
		{
			decode();
		}

		protected override void decode()
		{
			int num = -1;
			while ((num = deserialize.read_tag()) != -1)
			{
				int num2 = num;
				if (num2 == 2)
				{
					game_server = deserialize.read_obj_list<game_server>();
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
				serialize.write_obj(game_server, 2);
			}
			return serialize.close();
		}
	}
}
