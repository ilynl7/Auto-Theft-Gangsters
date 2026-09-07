using Sproto;

namespace SprotoType;

public class update_team_setting
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private long _isVerfiy;

		public long isVerfiy
		{
			get
			{
				return _isVerfiy;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_isVerfiy = value;
			}
		}

		public bool HasIsVerfiy => has_field.has_field(0);

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
					isVerfiy = deserialize.read_integer();
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
				serialize.write_integer(isVerfiy, 0);
			}
			return serialize.close();
		}
	}
}
