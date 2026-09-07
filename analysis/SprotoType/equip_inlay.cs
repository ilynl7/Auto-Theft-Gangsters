using Sproto;

namespace SprotoType;

public class equip_inlay
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private long _index1;

		private long _index2;

		private long _diamond_index1;

		private long _diamond_index2;

		public long index1
		{
			get
			{
				return _index1;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_index1 = value;
			}
		}

		public bool HasIndex1 => has_field.has_field(0);

		public long index2
		{
			get
			{
				return _index2;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_index2 = value;
			}
		}

		public bool HasIndex2 => has_field.has_field(1);

		public long diamond_index1
		{
			get
			{
				return _diamond_index1;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_diamond_index1 = value;
			}
		}

		public bool HasDiamond_index1 => has_field.has_field(2);

		public long diamond_index2
		{
			get
			{
				return _diamond_index2;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_diamond_index2 = value;
			}
		}

		public bool HasDiamond_index2 => has_field.has_field(3);

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
					index1 = deserialize.read_integer();
					break;
				case 1:
					index2 = deserialize.read_integer();
					break;
				case 2:
					diamond_index1 = deserialize.read_integer();
					break;
				case 3:
					diamond_index2 = deserialize.read_integer();
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
				serialize.write_integer(index1, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(index2, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(diamond_index1, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(diamond_index2, 3);
			}
			return serialize.close();
		}
	}
}
