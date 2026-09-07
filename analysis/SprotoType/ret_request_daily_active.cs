using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_request_daily_active
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private Dictionary<string, daily_active> _daily_actives;

		private Dictionary<string, daily_reward> _daily_rewards;

		private long _score;

		public Dictionary<string, daily_active> daily_actives
		{
			get
			{
				return _daily_actives;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_daily_actives = value;
			}
		}

		public bool HasDaily_actives => has_field.has_field(0);

		public Dictionary<string, daily_reward> daily_rewards
		{
			get
			{
				return _daily_rewards;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_daily_rewards = value;
			}
		}

		public bool HasDaily_rewards => has_field.has_field(1);

		public long score
		{
			get
			{
				return _score;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_score = value;
			}
		}

		public bool HasScore => has_field.has_field(2);

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
					daily_actives = deserialize.read_map((daily_active v) => v.ID);
					break;
				case 1:
					daily_rewards = deserialize.read_map((daily_reward v) => v.ID);
					break;
				case 2:
					score = deserialize.read_integer();
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
				serialize.write_obj(daily_actives, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_obj(daily_rewards, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(score, 2);
			}
			return serialize.close();
		}
	}
}
