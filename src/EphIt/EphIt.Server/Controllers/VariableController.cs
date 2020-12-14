using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EphIt.BL.User;
using EphIt.Db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EphIt.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize("ScriptsRead")]
    public class VariableController : Controller
    {
        private IEphItUser _ephItUser;
        private EphItContext _dbContext;
        public VariableController(EphItContext dbContext, IEphItUser ephItUser)
        {
            _dbContext = dbContext;
            _ephItUser = ephItUser;
        }
        [HttpPost]
        [Route("/api/Variable/")]
        [Authorize("ScriptsEdit")]
        public Variable New(string name, string value)
        {
            var existingVariable = _dbContext.Variable
                .Where(v => v.Name.Equals(name) && v.Active == true)
                .FirstOrDefault();
            if(existingVariable != null)
            {
                existingVariable.Active = false;
            }
            Variable variable = new Variable();
            variable.Active = true;
            variable.Name = name;
            variable.Value = value;
            _dbContext.Variable.Add(variable);
            _dbContext.SaveChanges();
            return variable;
        }

        [HttpGet]
        [Route("/api/Variable/")]
        [Authorize("ScriptsEdit")]
        public Variable New(string name)
        {
            return _dbContext.Variable
                .Where(v => v.Name.Equals(name) && v.Active == true)
                .FirstOrDefault();
        }
    }
}
