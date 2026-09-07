public class ModelData
{
	public string ID = string.Empty;

	public string Name = string.Empty;

	public string ModelPath = string.Empty;

	public int ModelType;

	public string EffectId = string.Empty;

	public MODEL_TYPE MODELTYPE => (MODEL_TYPE)ModelType;
}
