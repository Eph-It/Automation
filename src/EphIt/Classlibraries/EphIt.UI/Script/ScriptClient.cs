using System;
using Simple.OData.Client;

namespace EphIt.UI.Script
{
    public class ScriptClient : IScriptClient
    {
        private ODataClient _odataClient;
        public ScriptClient(ODataClient odataClient)
        {
            _odataClient = odataClient;
        }

        public void Get()
        {
        }
    }
}
