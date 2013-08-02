using System.Collections.Generic;
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

	    public static IList<int> ToListOfIntegers(this string str)
	    {
		    var list = new List<int>();
		    var tokens = str.Split(new[] {','});

		    foreach (var token in tokens)
		    {
			    int value;
			    if(int.TryParse(token.Trim(), out value))
					list.Add(value);
		    }

		    return list;
	    }
    }
}