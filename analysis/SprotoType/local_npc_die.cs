using Sproto;

namespace SprotoType;

public class local_npc_die
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private string _npcid;

		private long _x;

		private long _z;

		private long _type;

		public string npcid
		{
			get
			{
				return _npcid;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_npcid = value;
			}
		}

		public bool HasNpcid => has_field.has_field(0);

		public long x
		{
			get
			{
				return _x;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_x = value;
			}
		}

		public bool HasX => has_field.has_field(1);

		public long z
		{
			get
			{
				return _z;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_z = value;
			}
		}

		public bool HasZ => has_field.has_field(2);

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(3);

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
					npcid = deserialize.read_string();
					break;
				case 1:
					x = deserialize.read_integer();
					break;
				case 2:
					z = deserialize.read_integer();
					break;
				case 3:
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
				serialize.write_string(npcid, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(x, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(z, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(type, 3);
			}
			return serialize.close();
		}
	}
}
