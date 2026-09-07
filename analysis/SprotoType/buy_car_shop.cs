using Sproto;

namespace SprotoType;

public class buy_car_shop
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private string _mountId;

		public string mountId
		{
			get
			{
				return _mountId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_mountId = value;
			}
		}

		public bool HasMountId => has_field.has_field(0);

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
					mountId = deserialize.read_string();
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
				serialize.write_string(mountId, 0);
			}
			return serialize.close();
		}
	}
}
