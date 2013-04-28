using RG_SRVLib.Interop;

namespace PimIntegration.Tasks.VismaGlobal
{
	public static class ExtensionMethods
	{
		public static BusinessComponentNavigate GetBusinessComponent(this GlobalServerComponent globalSrvComponent, GLOBAL_Components componentId)
		{
			var comp = (BusinessComponentNavigate)globalSrvComponent.bcBusinessComponent[(int)componentId];
			comp.bcEstablishData();
			comp.bcBindData();

			return comp;
		} 
	}
}