using Sproto;

namespace SprotoType;

public class ret_battle_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private battle_info _battle_info;

		private long _type;

		public battle_info battle_info
		{
			get
			{
				return _battle_info;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_battle_info = value;
			}
		}

		public bool HasBattle_info => has_field.has_field(0);

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
					battle_info = deserialize.read_obj<battle_info>();
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
				serialize.write_obj(battle_info, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(type, 1);
			}
			return serialize.close();
		}
	}
}
