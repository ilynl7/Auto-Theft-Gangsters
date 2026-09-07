using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class update_line_state
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private string _mapInfoId;

		private long _line_count;

		private List<long> _line_states;

		public string mapInfoId
		{
			get
			{
				return _mapInfoId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_mapInfoId = value;
			}
		}

		public bool HasMapInfoId => has_field.has_field(0);

		public long line_count
		{
			get
			{
				return _line_count;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_line_count = value;
			}
		}

		public bool HasLine_count => has_field.has_field(1);

		public List<long> line_states
		{
			get
			{
				return _line_states;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_line_states = value;
			}
		}

		public bool HasLine_states => has_field.has_field(2);

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
					mapInfoId = deserialize.read_string();
					break;
				case 1:
					line_count = deserialize.read_integer();
					break;
				case 2:
					line_states = deserialize.read_integer_list();
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
				serialize.write_string(mapInfoId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(line_count, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(line_states, 2);
			}
			return serialize.close();
		}
	}
}
