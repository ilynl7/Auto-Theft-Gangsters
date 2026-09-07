public class EffInfoData
{
	public string ID = string.Empty;

	public string Name = string.Empty;

	public string HitAction = string.Empty;

	public int Target;

	public int ForceMove;

	public int MoveTime;

	public int MoveAngle;

	public int MoveDistance;

	public string BuffID = string.Empty;

	public int BuffDuration;

	public int Damage;

	public int DamageMulti;

	public int PvPDamage;

	public int PvPDamageMulti = 1;

	public int AreaType;

	public int Param1;

	public int Param2;

	public int Param3;

	public int Param4;

	public int Param5;

	public int DamageAdd;

	public int DamageMultiAdd;

	public int AddType1;

	public int AddValue1;

	public int AddType2;

	public int AddValue2;

	public int AddType3;

	public int AddValue3;

	public int AddType4;

	public int AddValue4;

	public int BuffProb;

	private XorFloat dmx = new XorFloat();

	private XorFloat movxt = new XorFloat();

	private XorFloat movx = new XorFloat();

	private XorFloat bufx = new XorFloat();

	private XorFloat parm1x = new XorFloat();

	private XorFloat parm2x = new XorFloat();

	private XorFloat parm3x = new XorFloat();

	private XorFloat parm4x = new XorFloat();

	private XorFloat parm5x = new XorFloat();

	private XorInt damagex = new XorInt();

	public float DamageMultiAdd_f => (float)DamageMultiAdd / 10000f;

	public float BuffProbFloat => (float)BuffProb / 10000f;

	public int Damagex => damagex.value;

	public float DamageMulti_100f => dmx.value;

	public float MoveTimeSecond => movxt.value;

	public float MoveDistanceMeter => movx.value;

	public float BuffDurationSecond => bufx.value;

	public float Param1Meter => parm1x.value;

	public float Param2Meter => parm2x.value;

	public float Param3Meter => parm3x.value;

	public float Param4Meter => parm4x.value;

	public float Param5Meter => parm5x.value;

	public void Init()
	{
		dmx.value = (float)DamageMulti / 10000f;
		movxt.value = (float)MoveTime / 1000f;
		movx.value = (float)MoveDistance / 100f;
		bufx.value = (float)BuffDuration / 1000f;
		parm1x.value = (float)Param1 / 100f;
		parm2x.value = (float)Param2 / 100f;
		parm3x.value = (float)Param3 / 100f;
		parm4x.value = (float)Param4 / 100f;
		parm5x.value = (float)Param5 / 100f;
		damagex.value = Damage;
	}

	public bool IsHaveDamage()
	{
		if (DamageMulti_100f != 0f || Damage != 0)
		{
			return true;
		}
		return false;
	}
}
