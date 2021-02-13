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
        [Authorize("ScriptsModify")]
        public VMVariable New(string name, string value)
        {
            var userId = _ephItUser.Register().UserId;

            var variable = _dbContext.Variable
                .Where(v => v.Name.Equals(name))
                .FirstOrDefault();
            if(variable == null)
            {
                variable = new Variable();
                variable.Name = name;
                variable.Created = DateTime.UtcNow;
                variable.CreatedByUserId = userId;
                variable.Value = value;
                _dbContext.Variable.Add(variable);
            }
            else
            {
                variable.ModifiedByUserId = userId;
                variable.Modified = DateTime.UtcNow;
                variable.Value = value;
                _dbContext.Variable.Update(variable);

            }
            _dbContext.SaveChanges();
            return new VMVariable(variable);
        }

        [HttpGet]
        [Route("/api/Variable/{name}")]
        [Authorize("ScriptsRead")]
        public VMVariable Get(string name)
        {
            return new VMVariable(_dbContext.Variable
                .Where(v => v.Name.Equals(name))
                .FirstOrDefault());
        }
    }
}
