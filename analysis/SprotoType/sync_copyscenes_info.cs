using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class sync_copyscenes_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<string, copyscene_info> _copyscenes;

		public Dictionary<string, copyscene_info> copyscenes
		{
			get
			{
				return _copyscenes;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_copyscenes = value;
			}
		}

		public bool HasCopyscenes => has_field.has_field(0);

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
					copyscenes = deserialize.read_map((copyscene_info v) => v.ID);
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
				serialize.write_obj(copyscenes, 0);
			}
			return serialize.close();
		}
	}
}
