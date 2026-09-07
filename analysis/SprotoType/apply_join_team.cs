using Sproto;

namespace SprotoType;

public class apply_join_team
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private teammember _member;

		public teammember member
		{
			get
			{
				return _member;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_member = value;
			}
		}

		public bool HasMember => has_field.has_field(0);

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
					member = deserialize.read_obj<teammember>();
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
				serialize.write_obj(member, 0);
			}
			return serialize.close();
		}
	}
}
