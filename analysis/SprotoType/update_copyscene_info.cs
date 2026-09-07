using Sproto;

namespace SprotoType;

public class update_copyscene_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private copyscene_info _copyscene;

		public copyscene_info copyscene
		{
			get
			{
				return _copyscene;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_copyscene = value;
			}
		}

		public bool HasCopyscene => has_field.has_field(0);

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
				if (num == 0)
				{
					copyscene = deserialize.read_obj<copyscene_info>();
				}
				else
				{
					deserialize.read_unknow_data();
				}
			}
		}

		public override int encode(SprotoStream stream)
		{
			serialize.open(stream);
			if (has_field.has_field(0))
			{
				serialize.write_obj(copyscene, 0);
			}
			return serialize.close();
		}
	}
}
