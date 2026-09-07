using Sproto;

namespace SprotoType;

public class require_domin_rewards
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private string _id;

		private long _index;

		private bool _cost;

		public string id
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

		public long index
		{
			get
			{
				return _index;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_index = value;
			}
		}

		public bool HasIndex => has_field.has_field(1);

		public bool cost
		{
			get
			{
				return _cost;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_cost = value;
			}
		}

		public bool HasCost => has_field.has_field(2);

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
					id = deserialize.read_string();
					break;
				case 1:
					index = deserialize.read_integer();
					break;
				case 2:
					cost = deserialize.read_boolean();
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
				serialize.write_string(id, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(index, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_boolean(cost, 2);
			}
			return serialize.close();
		}
	}
}
