using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_request_random_rank_pvp_opponent
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private long _opponentNum;

		private List<character_look> _characters;

		private List<long> _rankPos;

		public long opponentNum
		{
			get
			{
				return _opponentNum;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_opponentNum = value;
			}
		}

		public bool HasOpponentNum => has_field.has_field(0);

		public List<character_look> characters
		{
			get
			{
				return _characters;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_characters = value;
			}
		}

		public bool HasCharacters => has_field.has_field(1);

		public List<long> rankPos
		{
			get
			{
				return _rankPos;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_rankPos = value;
			}
		}

		public bool HasRankPos => has_field.has_field(2);

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
					opponentNum = deserialize.read_integer();
					break;
				case 1:
					characters = deserialize.read_obj_list<character_look>();
					break;
				case 2:
					rankPos = deserialize.read_integer_list();
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
				serialize.write_integer(opponentNum, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_obj(characters, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(rankPos, 2);
			}
			return serialize.close();
		}
	}
}
