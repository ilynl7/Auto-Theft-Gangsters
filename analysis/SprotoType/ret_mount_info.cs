using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_mount_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 1;

		private Dictionary<string, mount> _mount_info;

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
					mount_info = deserialize.read_map((mount v) => v.ID);
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
				serialize.write_obj(mount_info, 0);
			}
			return serialize.close();
		}
	}
}
