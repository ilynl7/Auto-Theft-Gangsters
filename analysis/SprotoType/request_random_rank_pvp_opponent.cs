using Sproto;

namespace SprotoType;

public class request_random_rank_pvp_opponent
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count;

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
				int num2 = num;
				deserialize.read_unknow_data();
			}
		}

		public override int encode(SprotoStream stream)
		{
			serialize.open(stream);
			return serialize.close();
		}
	}
}
