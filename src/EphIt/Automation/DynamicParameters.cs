using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Automation
{
    class DynamicParameters
    {
        public static RuntimeDefinedParameterDictionary dynParams(ref RuntimeDefinedParameterDictionary _staticStorage)
        {
            RuntimeDefinedParameterDictionary p = new RuntimeDefinedParameterDictionary();
            var attributes = new Collection<Attribute>
            {
                new ParameterAttribute
                {
                    Mandatory = true,
                    ValueFromPipeline = false,
                    ValueFromPipelineByPropertyName = false,
                    HelpMessage = "Server"
                }
            };
            p.Add("Server", new RuntimeDefinedParameter("Server", typeof(string), attributes));

            attributes = new Collection<Attribute>
            {
                new ParameterAttribute
                {
                    Mandatory = true,
                    ValueFromPipeline = false,
                    ValueFromPipelineByPropertyName = false,
                    HelpMessage = "Port"
                }
            };
            p.Add("Port", new RuntimeDefinedParameter("Port", typeof(int), attributes));
            _staticStorage = p;
            return p;
        }
    }
}
