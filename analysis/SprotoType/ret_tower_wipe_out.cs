using Sproto;

namespace SprotoType;

public class ret_tower_wipe_out
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private tower_info _tower_info;

		public tower_info tower_info
		{
			get
			{
				return _tower_info;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_tower_info = value;
			}
		}

		public bool HasTower_info => has_field.has_field(0);

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
					tower_info = deserialize.read_obj<tower_info>();
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
				serialize.write_obj(tower_info, 0);
			}
			return serialize.close();
		}
	}
}
