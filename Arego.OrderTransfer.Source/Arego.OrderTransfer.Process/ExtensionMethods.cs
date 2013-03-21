using RG_SRVLib.Interop;

namespace Arego.OrderTransfer.Process
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

		public static SalesOrderServerComponent GetSalesOrderServerComponent(this GlobalServerComponent globalSrvComponent)
		{
			var comp = (SalesOrderServerComponent)globalSrvComponent.bcBusinessComponent[(int)GLOBAL_Components.BC_SalesOrder];
			comp.bcEstablishData();
			comp.bcBindData();

			return comp;
		}
    }
}