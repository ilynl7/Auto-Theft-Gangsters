public class ModelEffectData
{
	public ModelData modelData;

	public FxEffInfoData fxEffinfoData;

	public bool IsFirst;

	public string LayerStr;

	public int Index;

	public ModelEffectData(ModelData mData, FxEffInfoData fxData, bool isFirst, string layerStr = "", int index = -1)
	{
		modelData = mData;
		fxEffinfoData = fxData;
		IsFirst = isFirst;
		LayerStr = layerStr;
		Index = index;
	}
}
