using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_mount_equip
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private Dictionary<string, mount> _mount_info;

		private string _ID;

		public Dictionary<string, mount> mount_info
		{
			get
			{
				return _mount_info;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_mount_info = value;
			}
		}

		public bool HasMount_info => has_field.has_field(0);

		public string ID
		{
			get
			{
				return _ID;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_ID = value;
			}
		}

		public bool HasID => has_field.has_field(1);

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
					mount_info = deserialize.read_map((mount v) => v.ID);
					break;
				case 1:
					ID = deserialize.read_string();
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
				serialize.write_obj(mount_info, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(ID, 1);
			}
			return serialize.close();
		}
	}
}
