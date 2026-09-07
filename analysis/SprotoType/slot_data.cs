using Sproto;

namespace SprotoType;

public class slot_data : SprotoTypeBase
{
	private static int max_field_count = 8;

	private string _ID;

	private string _RewardMap;

	private string _RewardEffect;

	private string _Desc;

	private long _Rank;

	private string _ShowRewardID;

	private long _PriceType;

	private long _PriceCost;

	public string ID
	{
		get
		{
			return _ID;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_ID = value;
		}
	}

	public bool HasID => has_field.has_field(0);

	public string RewardMap
	{
		get
		{
			return _RewardMap;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_RewardMap = value;
		}
	}

	public bool HasRewardMap => has_field.has_field(1);

	public string RewardEffect
	{
		get
		{
			return _RewardEffect;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_RewardEffect = value;
		}
	}

	public bool HasRewardEffect => has_field.has_field(2);

	public string Desc
	{
		get
		{
			return _Desc;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_Desc = value;
		}
	}

	public bool HasDesc => has_field.has_field(3);

	public long Rank
	{
		get
		{
			return _Rank;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_Rank = value;
		}
	}

	public bool HasRank => has_field.has_field(4);

	public string ShowRewardID
	{
		get
		{
			return _ShowRewardID;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_ShowRewardID = value;
		}
	}

	public bool HasShowRewardID => has_field.has_field(5);

	public long PriceType
	{
		get
		{
			return _PriceType;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_PriceType = value;
		}
	}

	public bool HasPriceType => has_field.has_field(6);

	public long PriceCost
	{
		get
		{
			return _PriceCost;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_PriceCost = value;
		}
	}

	public bool HasPriceCost => has_field.has_field(7);

	public slot_data()
		: base(max_field_count)
	{
	}

	public slot_data(byte[] buffer)
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
				ID = deserialize.read_string();
				break;
			case 1:
				RewardMap = deserialize.read_string();
				break;
			case 2:
				RewardEffect = deserialize.read_string();
				break;
			case 3:
				Desc = deserialize.read_string();
				break;
			case 4:
				Rank = deserialize.read_integer();
				break;
			case 5:
				ShowRewardID = deserialize.read_string();
				break;
			case 6:
				PriceType = deserialize.read_integer();
				break;
			case 7:
				PriceCost = deserialize.read_integer();
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
			serialize.write_string(ID, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_string(RewardMap, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_string(RewardEffect, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_string(Desc, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(Rank, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_string(ShowRewardID, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(PriceType, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(PriceCost, 7);
		}
		return serialize.close();
	}
}
