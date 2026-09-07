using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_offline_chat
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private List<chat_item> _chat_list;

		public List<chat_item> chat_list
		{
			get
			{
				return _chat_list;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_chat_list = value;
			}
		}

		public bool HasChat_list => has_field.has_field(0);

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
					chat_list = deserialize.read_obj_list<chat_item>();
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
				serialize.write_obj(chat_list, 0);
			}
			return serialize.close();
		}
	}
}
