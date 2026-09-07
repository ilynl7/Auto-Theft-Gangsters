using Sproto;

namespace SprotoType;

public class syn_rank_pvp_data
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 8;

		private long _combValue;

		private long _times;

		private long _rankPos;

		private long _bestRankPos;

		private long _rewards;

		private long _preRankPos;

		private long _winCount;

		private long _winRewards;

		public long combValue
		{
			get
			{
				return _combValue;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_combValue = value;
			}
		}

		public bool HasCombValue => has_field.has_field(0);

		public long times
		{
			get
			{
				return _times;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_times = value;
			}
		}

		public bool HasTimes => has_field.has_field(1);

		public long rankPos
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

		public long bestRankPos
		{
			get
			{
				return _bestRankPos;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_bestRankPos = value;
			}
		}

		public bool HasBestRankPos => has_field.has_field(3);

		public long rewards
		{
			get
			{
				return _rewards;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_rewards = value;
			}
		}

		public bool HasRewards => has_field.has_field(4);

		public long preRankPos
		{
			get
			{
				return _preRankPos;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_preRankPos = value;
			}
		}

		public bool HasPreRankPos => has_field.has_field(5);

		public long winCount
		{
			get
			{
				return _winCount;
			}
			set
			{
				has_field.set_field(6, is_has: true);
				_winCount = value;
			}
		}

		public bool HasWinCount => has_field.has_field(6);

		public long winRewards
		{
			get
			{
				return _winRewards;
			}
			set
			{
				has_field.set_field(7, is_has: true);
				_winRewards = value;
			}
		}

		public bool HasWinRewards => has_field.has_field(7);

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
					combValue = deserialize.read_integer();
					break;
				case 1:
					times = deserialize.read_integer();
					break;
				case 2:
					rankPos = deserialize.read_integer();
					break;
				case 3:
					bestRankPos = deserialize.read_integer();
					break;
				case 4:
					rewards = deserialize.read_integer();
					break;
				case 5:
					preRankPos = deserialize.read_integer();
					break;
				case 6:
					winCount = deserialize.read_integer();
					break;
				case 7:
					winRewards = deserialize.read_integer();
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
				serialize.write_integer(combValue, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(times, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(rankPos, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(bestRankPos, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(rewards, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(preRankPos, 5);
			}
			if (has_field.has_field(6))
			{
				serialize.write_integer(winCount, 6);
			}
			if (has_field.has_field(7))
			{
				serialize.write_integer(winRewards, 7);
			}
			return serialize.close();
		}
	}
}
