using Sproto;

namespace SprotoType;

public class copy_swipe_out
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 2;

		private string _copyInfoId;

		private bool _reaminItem;

		public string copyInfoId
		{
			get
			{
				return _copyInfoId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_copyInfoId = value;
			}
		}

		public bool HasCopyInfoId => has_field.has_field(0);

		public bool reaminItem
		{
			get
			{
				return _reaminItem;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_reaminItem = value;
			}
		}

		public bool HasReaminItem => has_field.has_field(1);

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
					copyInfoId = deserialize.read_string();
					break;
				case 1:
					reaminItem = deserialize.read_boolean();
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
				serialize.write_string(copyInfoId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_boolean(reaminItem, 1);
			}
			return serialize.close();
		}
	}
}
