using Sproto;

namespace SprotoType;

public class donate_record : SprotoTypeBase
{
	private static int max_field_count = 2;

	private string _id;

	private long _DonateCount;

	public string id
	{
		get
		{
			return _id;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_id = value;
		}
	}

	public bool HasId => has_field.has_field(0);

	public long DonateCount
	{
		get
		{
			return _DonateCount;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_DonateCount = value;
		}
	}

	public bool HasDonateCount => has_field.has_field(1);

	public donate_record()
		: base(max_field_count)
	{
	}

	public donate_record(byte[] buffer)
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
				id = deserialize.read_string();
				break;
			case 1:
				DonateCount = deserialize.read_integer();
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
			serialize.write_string(id, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_integer(DonateCount, 1);
		}
		return serialize.close();
	}
}
