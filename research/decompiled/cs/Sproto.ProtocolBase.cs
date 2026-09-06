namespace Sproto;

public abstract class ProtocolBase
{
	private ProtocolFunctionDictionary _Protocol = new ProtocolFunctionDictionary();

	public ProtocolFunctionDictionary Protocol => _Protocol;
}
