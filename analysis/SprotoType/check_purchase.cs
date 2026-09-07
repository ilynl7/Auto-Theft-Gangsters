using Sproto;

namespace SprotoType;

public class check_purchase
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private string _productId;

		private string _token;

		private string _payload;

		private string _packageName;

		public string productId
		{
			get
			{
				return _productId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_productId = value;
			}
		}

		public bool HasProductId => has_field.has_field(0);

		public string token
		{
			get
			{
				return _token;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_token = value;
			}
		}

		public bool HasToken => has_field.has_field(1);

		public string payload
		{
			get
			{
				return _payload;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_payload = value;
			}
		}

		public bool HasPayload => has_field.has_field(2);

		public string packageName
		{
			get
			{
				return _packageName;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_packageName = value;
			}
		}

		public bool HasPackageName => has_field.has_field(3);

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
					productId = deserialize.read_string();
					break;
				case 1:
					token = deserialize.read_string();
					break;
				case 2:
					payload = deserialize.read_string();
					break;
				case 3:
					packageName = deserialize.read_string();
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
				serialize.write_string(productId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(token, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(payload, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_string(packageName, 3);
			}
			return serialize.close();
		}
	}
}
