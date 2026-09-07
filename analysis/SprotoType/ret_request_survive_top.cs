using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_request_survive_top
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private List<score_info> _score_infos;

		private long _my_rank;

		private long _my_score;

		private long _end_time;

		public List<score_info> score_infos
		{
			get
			{
				return _score_infos;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_score_infos = value;
			}
		}

		public bool HasScore_infos => has_field.has_field(0);

		public long my_rank
		{
			get
			{
				return _my_rank;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_my_rank = value;
			}
		}

		public bool HasMy_rank => has_field.has_field(1);

		public long my_score
		{
			get
			{
				return _my_score;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_my_score = value;
			}
		}

		public bool HasMy_score => has_field.has_field(2);

		public long end_time
		{
			get
			{
				return _end_time;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_end_time = value;
			}
		}

		public bool HasEnd_time => has_field.has_field(3);

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
					score_infos = deserialize.read_obj_list<score_info>();
					break;
				case 1:
					my_rank = deserialize.read_integer();
					break;
				case 2:
					my_score = deserialize.read_integer();
					break;
				case 3:
					end_time = deserialize.read_integer();
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
				serialize.write_obj(score_infos, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(my_rank, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(my_score, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(end_time, 3);
			}
			return serialize.close();
		}
	}
}
