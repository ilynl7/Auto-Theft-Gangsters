using Sproto;

namespace SprotoType;

public class attack_local_npc
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private long _damge;

		private string _effinfoId;

		public long damge
		{
			get
			{
				return _damge;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_damge = value;
			}
		}

		public bool HasDamge => has_field.has_field(0);

		public string effinfoId
		{
			get
			{
				return _effinfoId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_effinfoId = value;
			}
		}

		public bool HasEffinfoId => has_field.has_field(1);

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
					damge = deserialize.read_integer();
					break;
				case 1:
					effinfoId = deserialize.read_string();
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
				serialize.write_integer(damge, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(effinfoId, 1);
			}
			return serialize.close();
		}
	}
}
