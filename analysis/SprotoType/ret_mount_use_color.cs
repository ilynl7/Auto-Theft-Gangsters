using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_mount_use_color
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private Dictionary<string, mount> _mount_info;

		private string _mountId;

		private string _colorId;

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

		public string mountId
		{
			get
			{
				return _mountId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_mountId = value;
			}
		}

		public bool HasMountId => has_field.has_field(1);

		public string colorId
		{
			get
			{
				return _colorId;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_colorId = value;
			}
		}

		public bool HasColorId => has_field.has_field(2);

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
					mountId = deserialize.read_string();
					break;
				case 2:
					colorId = deserialize.read_string();
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
				serialize.write_string(mountId, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(colorId, 2);
			}
			return serialize.close();
		}
	}
}
