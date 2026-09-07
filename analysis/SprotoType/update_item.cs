using Sproto;

namespace SprotoType;

public class update_item
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private long _containertype;

		private long _indexId;

		private gameitem _gameitem;

		public long containertype
		{
			get
			{
				return _containertype;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_containertype = value;
			}
		}

		public bool HasContainertype => has_field.has_field(0);

		public long indexId
		{
			get
			{
				return _indexId;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_indexId = value;
			}
		}

		public bool HasIndexId => has_field.has_field(1);

		public gameitem gameitem
		{
			get
			{
				return _gameitem;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_gameitem = value;
			}
		}

		public bool HasGameitem => has_field.has_field(2);

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
					containertype = deserialize.read_integer();
					break;
				case 1:
					indexId = deserialize.read_integer();
					break;
				case 2:
					gameitem = deserialize.read_obj<gameitem>();
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
				serialize.write_integer(containertype, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(indexId, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_obj(gameitem, 2);
			}
			return serialize.close();
		}
	}
}
