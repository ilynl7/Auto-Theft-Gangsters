using Sproto;

namespace SprotoType;

public class npc_create
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private npc_attribute _npc_attribute;

		public npc_attribute npc_attribute
		{
			get
			{
				return _npc_attribute;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_npc_attribute = value;
			}
		}

		public bool HasNpc_attribute => has_field.has_field(0);

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
					npc_attribute = deserialize.read_obj<npc_attribute>();
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
				serialize.write_obj(npc_attribute, 0);
			}
			return serialize.close();
		}
	}
}
