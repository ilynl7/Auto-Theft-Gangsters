using Sproto;

namespace SprotoType;

public class aoi_social_dance
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _id;

		private string _danceId;

		public long id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(0);

		public string danceId
		{
			get
			{
				return _danceId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_danceId = value;
			}
		}

		public bool HasDanceId => has_field.has_field(1);

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
					id = deserialize.read_integer();
					break;
				case 1:
					danceId = deserialize.read_string();
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
				serialize.write_integer(id, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(danceId, 1);
			}
			return serialize.close();
		}
	}
}
